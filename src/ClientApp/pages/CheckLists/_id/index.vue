<template>
  <div>
    <alert-dialog
      v-if="selectedItemData"
      v-model="dialogRemove"
      title="Remove Selected Item"
      message="the selected item is about to be removed."
      :code="selectedItemData.id"
      :description="selectedItemData.text"
      @yes="removeItem"
    />
    <message-dialog v-model="dialogNew" 
    :actions="['yes', 'cancel']"
    yes-text="Save"
    @yes="addItem"
    >
      <template v-slot:title="">
          Create CheckList Item
      </template>
      <ValidationObserver tag="form" ref="obsNew">
          <v-row>
            <v-col>
              <ValidationProvider rules="required" v-slot="{errors}">
                <v-text-field v-model="newItemData.text" label="Text" :error-messages="errors">
                  <template v-slot:append>
                    <v-chip
                      small
                      v-if="newItemData !== null "
                    >{{ newItemData.textParams.length }} params detected</v-chip>
                  </template>
                </v-text-field>
              </ValidationProvider>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-switch v-model="newItemData.checked" label="Checked by Default" />
            </v-col>
            <v-col>
              <v-switch v-model="newItemData.required" label="Is Required" />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12">
              <span class="font-weight-black">Text Params</span>
              <v-data-table dense :headers="headers" :items="newItemData.textParams"></v-data-table>
            </v-col>
          </v-row>
      </ValidationObserver>
    </message-dialog>
    <ValidationObserver tag="form" v-slot="{ valid, dirty }">
      <v-row align="center" justify="space-between">
        <v-col cols="12" md="6">
          <ValidationProvider rules="required" v-slot="{errors}">
            <v-text-field
              v-model="currentCheckList.text"
              label="Checklist Name"
              :error-messages="errors"
            >
              <template v-slot:append>
                <v-chip small>2 params detected</v-chip>
              </template>
            </v-text-field>
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="5">
          <ValidationProvider>
            <v-text-field v-model="currentCheckList.annotation" label="Annotation" />
          </ValidationProvider>
        </v-col>
        <v-col cols="1">
          <v-btn :disabled="!valid || !dirty" icon text color="success" @click="saveCheckList">
            <v-icon>mdi-check</v-icon>
          </v-btn>
        </v-col>
      </v-row>
      <v-row v-if="$vuetify.breakpoint.smAndDown">
        <v-col>
          <v-autocomplete
            v-model="selectedItemData"
            :items="checkList.checks"
            return-object
            clearable
            label="Select a checklist Item"
            @change="selectedItem = checkList.checks.indexOf(selectedItemData)"
          ></v-autocomplete>
        </v-col>
      </v-row>
    </ValidationObserver>
    <v-row>
      <v-col cols="5" class="text-left" v-if="!$vuetify.breakpoint.smAndDown">
        <v-list nav dense>
          <v-subheader class="text-h6">Checklist Items</v-subheader>
          <v-subheader>
            Select an item to view/edit
            <v-spacer />
            <v-btn class="mx-2" x-small 
            fab dark color="primary"
            @click="dialogNew = true">
              <v-icon dark>mdi-plus</v-icon>
            </v-btn>
          </v-subheader>
          <v-list-item-group v-model="selectedItem" color="primary">
            <v-list-item v-for="(item, i) in checkList.checks" :key="i">
              <v-list-item-content>
                <v-list-item-title>
                  <v-chip v-if="item.required" x-small>required</v-chip>
                  {{ item.text }}
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-col>
      <v-divider vertical v-if="!$vuetify.breakpoint.smAndDown" />
      <v-col>
        <v-card height="100%" v-if="selectedItemData" outlined>
          <v-card-title class="text-wrap">{{ selectedItemData.text }}</v-card-title>
          <v-card-subtitle v-if="selectedItemData !== null " class="text-left">
            Total Params: {{ selectedItemData.textParams.length }}
            <v-chip x-small v-if="selectedItemData.required">required</v-chip>

            <v-btn
              class="mx-2"
              title="delete this item"
              icon
              text
              color="error"
              @click="dialogRemove = true"
            >
              <v-icon dark>mdi-minus</v-icon>
            </v-btn>
          </v-card-subtitle>
          <ValidationObserver tag="form" v-slot="{ valid }">
            <v-card-text>
              <v-row>
                <v-col>
                  <ValidationProvider rules="required" v-slot="{errors}">
                    <v-text-field
                      v-model="selectedItemData.text"
                      label="Text"
                      :error-messages="errors"
                    >
                      <template v-slot:append>
                        <v-chip
                          small
                          v-if="selectedItemData !== null "
                        >{{ selectedItemData.textParams.length }} params detected</v-chip>
                      </template>
                    </v-text-field>
                  </ValidationProvider>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-switch v-model="selectedItemData.checked" label="Checked by Default" />
                </v-col>
                <v-col>
                  <v-switch v-model="selectedItemData.required" label="Is Required" />
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="12">
                  <span class="font-weight-black">Text Params</span>
                  <v-data-table dense :headers="headers" :items="selectedItemData.textParams"></v-data-table>
                </v-col>
              </v-row>
            </v-card-text>
            <v-card-actions class="text-right">
              <v-btn :disabled="!valid" color="success" text @click="saveItem">
                <v-icon>mdi-check</v-icon>Save
              </v-btn>
            </v-card-actions>
          </ValidationObserver>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>
<script lang="ts">
import { Vue, Component, Watch } from "nuxt-property-decorator";
import {
  CheckListItem,
  CheckList,
  UpdateCheckListCommand,
  DeleteCheckListItem,
  UpdateCheckListItemCommand,
  CheckListParam,
  AddCheckListItemCommand
} from "~/types";
import { CheckListsState } from "~/store/checklists";
import { ValidationProvider, ValidationObserver } from "vee-validate";

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class AddEditCheckList extends Vue {
  $refs!: {
      obsNew: InstanceType<typeof ValidationObserver>
  }
  dialogRemove: boolean = false
  dialogNew: boolean = false
  selectedItem: number = -1
  selectedItemData: CheckListItem | null = null
  newItemData: CheckListItem | null = { textParams: [] as CheckListParam[]} as CheckListItem
  currentCheckList: CheckList = {} as CheckList
  

  headers: any = [
    {
      text: "Key",
      value: "key",
      sortable: true,
      align: "center"
    },
    {
      text: "Value",
      value: "value",
      sortable: true,
      align: "center"
    },
    {
      text: "Type",
      value: "type",
      sortable: true,
      align: "center"
    }
  ];

  @Watch("selectedItem")
  onSelectedItemChanged(value: number) {
    this.selectedItemData = Object.assign({}, this.checkList.checks[value]);
  }

  get checkList(): CheckList {
    return (this.$store.state.checklists as CheckListsState).currentCheckList;
  }

  async fetch() {
    const result = await this.$store.dispatch(
      "checklists/getCheckListItemsById",
      this.$route.params.id,
      { root: false }
    );
    this.currentCheckList = Object.assign({}, result);
  }

  async saveItem() {
    const command: UpdateCheckListItemCommand = {
      id: this.selectedItemData!.id,
      checkListId: this.selectedItemData!.checkListId,
      text: this.selectedItemData!.text,
      checked: this.selectedItemData!.checked,
      required: this.selectedItemData!.required,
      remarks: this.selectedItemData!.remarks
    };
    await this.$store.dispatch("checklists/updateCheckListItem", command, {
      root: false
    });
  }

  async addItem() {
    const command: AddCheckListItemCommand = {
      idCheckList: parseInt(this.$route.params.id),
      text: this.newItemData!.text,
      checked: this.newItemData!.checked,
      required: this.newItemData!.required,
      remarks: this.newItemData!.remarks,
      checklistParams: []
    };

    const isValid = await this.$refs.obsNew.validate()
    
    if(!isValid) return;

    await this.$store.dispatch("checklists/createCheckListItem", command, {
      root: false
    });
    await this.$fetch()
    this.dialogNew = false
  }

  async removeItem() {
    const command: DeleteCheckListItem = {
      idCheckListItem: this.selectedItemData!.id,
      idCheckList: parseInt(this.$route.params.id)
    };
    this.selectedItem -= 1;
    await this.$store.dispatch("checklists/deleteCheckListItem", command, {
      root: false
    });
  }

  async saveCheckList() {
    const command: UpdateCheckListCommand = {
      idCheckList: parseInt(this.$route.params.id),
      text: this.currentCheckList.text,
      annotation: this.currentCheckList.annotation
    };
    await this.$store.dispatch("checklists/updateCheckList", command, {
      root: false
    });
  }
}
</script>