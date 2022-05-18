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
    <message-dialog
      v-model="dialogNew"
      :actions="['yes', 'cancel']"
      yes-text="Save"
      @yes="addItem"
    >
      <template #title="">
        Create CheckList Item
      </template>
      <ValidationObserver ref="obsNew" tag="form">
        <v-row>
          <v-col>
            <ValidationProvider v-slot="{ errors }" rules="required">
              <v-text-field
                v-model="newItemData.text"
                label="Text"
                :error-messages="errors"
              />
            </ValidationProvider>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-switch
              v-model="newItemData.checked"
              label="Checked by Default"
            />
          </v-col>
          <v-col>
            <v-switch v-model="newItemData.required" label="Is Required" />
          </v-col>
          <v-col>
            <v-switch
              v-model="newItemData.editable"
              label="Editable in the Report"
            />
          </v-col>
        </v-row>
      </ValidationObserver>
    </message-dialog>
    <ValidationObserver v-slot="{ valid }" tag="form">
      <v-row align="center" justify="space-between">
        <v-col cols="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              v-model="currentCheckList.text"
              label="Checklist Name"
              :error-messages="errors"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="4">
          <v-text-field
            v-model="currentCheckList.annotation"
            label="Annotation"
          />
        </v-col>
        <v-col cols="1">
          <v-checkbox
            v-model="currentCheckList.isConfiguration"
            label="Use in Config"
          />
        </v-col>
        <v-col cols="1">
          <v-btn
            :disabled="!valid"
            icon
            text
            color="success"
            @click="saveCheckList"
          >
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
          />
        </v-col>
      </v-row>
    </ValidationObserver>
    <v-row>
      <v-col v-if="!$vuetify.breakpoint.smAndDown" cols="5" class="text-left">
        <v-list nav dense>
          <v-subheader class="text-h6">
            Checklist Items
          </v-subheader>
          <v-subheader>
            Select an item to view/edit
            <v-spacer />
            <v-btn
              class="mx-2"
              x-small
              fab
              dark
              color="primary"
              @click="dialogNew = true"
            >
              <v-icon dark>
                mdi-plus
              </v-icon>
            </v-btn>
          </v-subheader>
          <v-list-item-group v-model="selectedItem" color="primary">
            <v-list-item v-for="(item, i) in checkList.checks" :key="i">
              <v-list-item-content>
                <v-list-item-title>
                  <v-chip v-if="item.required" x-small>
                    required
                  </v-chip>
                  {{ item.text }}
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-col>
      <v-divider v-if="!$vuetify.breakpoint.smAndDown" vertical />
      <v-col>
        <v-card v-if="selectedItemData" height="100%" outlined>
          <v-card-title class="text-wrap">
            {{ selectedItemData.text }}
          </v-card-title>
          <v-card-subtitle v-if="selectedItemData !== null" class="text-left">
            <v-chip v-if="selectedItemData.required" x-small>
              required
            </v-chip>

            <v-btn
              class="mx-2"
              title="delete this item"
              icon
              text
              color="error"
              @click="dialogRemove = true"
            >
              <v-icon dark>
                mdi-minus
              </v-icon>
            </v-btn>
          </v-card-subtitle>
          <ValidationObserver v-slot="{ invalid }" tag="form">
            <v-card-text>
              <v-row>
                <v-col>
                  <ValidationProvider v-slot="{ errors }" rules="required">
                    <v-text-field
                      v-model="selectedItemData.text"
                      label="Text"
                      :error-messages="errors"
                    />
                  </ValidationProvider>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-switch
                    v-model="selectedItemData.checked"
                    label="Checked by Default"
                  />
                </v-col>
                <v-col>
                  <v-switch
                    v-model="selectedItemData.required"
                    label="Is Required"
                  />
                </v-col>
                <v-col>
                  <v-switch
                    v-model="newItemData.editable"
                    label="Editable in the Report"
                  />
                </v-col>
              </v-row>
            </v-card-text>
            <v-card-actions class="text-right">
              <v-btn :disabled="invalid" color="success" text @click="saveItem">
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
import {
  defineComponent,
  ref,
  useRouter,
  useStore,
  useRoute,
  useFetch,
  watch,
  computed
} from '@nuxtjs/composition-api'
import { ValidationProvider, ValidationObserver } from 'vee-validate'
import {
  CheckListItem,
  CheckList,
  UpdateCheckListCommand,
  DeleteCheckListItem,
  UpdateCheckListItemCommand,
  CheckListParam,
  AddCheckListItemCommand,
  AddCheckListCommand
} from '~/types'
import useGoBack from '~/composables/useGoBack'
import { CheckListsState } from '~/store/checklists'

export default defineComponent({
  name: 'AddEditCheckList',
  components: {
    ValidationObserver,
    ValidationProvider
  },
  setup () {
    useGoBack()
    const router = useRouter()
    const route = useRoute()
    const store = useStore()

    const obsNew = ref<InstanceType<typeof ValidationObserver>>(null)

    const dialogRemove = ref(false)
    const dialogNew = ref(false)
    const selectedItem = ref(-1)
    const selectedItemData = ref<CheckListItem | null>(null)
    const newItemData = ref<CheckListItem | null>({
      textParams: [] as CheckListParam[]
    } as CheckListItem)
    const currentCheckList = ref<CheckList>({} as CheckList)

    const headers = ref([
      {
        text: 'Key',
        value: 'key',
        sortable: true,
        align: 'center'
      },
      {
        text: 'Value',
        value: 'value',
        sortable: true,
        align: 'center'
      },
      {
        text: 'Type',
        value: 'type',
        sortable: true,
        align: 'center'
      }
    ])

    const { fetch } = useFetch(async () => {
      if (parseInt(route.value.params.id) > 0) {
        const result = await store.dispatch(
          'checklists/getCheckListItemsById',
          route.value.params.id,
          { root: false }
        )
        currentCheckList.value = Object.assign({}, result)
      }
    })

    const checkList = computed(() => {
      return ((store.state as any).checklists as CheckListsState).currentCheckList
    })

    watch(() => selectedItem.value,
      (item, _) => {
        selectedItemData.value = Object.assign({}, checkList.value.checks[item])
      })

    const saveItem = async () => {
      const command: UpdateCheckListItemCommand = {
        id: selectedItemData.value!.id,
        checkListId: selectedItemData.value!.checkListId,
        text: selectedItemData.value!.text,
        checked: selectedItemData.value!.checked ? 1 : 0,
        editable: selectedItemData.value!.editable,
        required: selectedItemData.value!.required,
        remarks: selectedItemData.value!.remarks
      }
      await store.dispatch('checklists/updateCheckListItem', command, {
        root: false
      })
    }

    const addItem = async () => {
      const command: AddCheckListItemCommand = {
        idCheckList: parseInt(route.value.params.id),
        text: newItemData.value!.text,
        checked: newItemData.value!.checked ? 1 : 0,
        editable: newItemData.value!.editable,
        required: newItemData.value!.required,
        remarks: newItemData.value!.remarks,
        checklistParams: []
      }

      const isValid = await obsNew.value?.validate()
      alert(isValid)

      if (!isValid) {
        return
      }

      await store.dispatch('checklists/createCheckListItem', command, {
        root: false
      })
      fetch()
      dialogNew.value = false
    }

    const removeItem = async () => {
      const command: DeleteCheckListItem = {
        idCheckListItem: selectedItemData.value!.id,
        idCheckList: parseInt(route.value.params.id)
      }
      selectedItem.value -= 1
      await store.dispatch('checklists/deleteCheckListItem', command, {
        root: false
      })
    }

    const saveCheckList = async () => {
      const command: UpdateCheckListCommand = {
        idCheckList: parseInt(route.value.params.id),
        text: currentCheckList.value.text,
        annotation: currentCheckList.value.annotation,
        isConfiguration: currentCheckList.value.isConfiguration
      }

      const addCommand: AddCheckListCommand = {
        text: currentCheckList.value.text,
        annotation: currentCheckList.value.annotation,
        isConfiguration: currentCheckList.value.isConfiguration,
        textParams: [],
        items: []
      }

      try {
        if (command.idCheckList > 0) {
          await store.dispatch('checklists/updateCheckList', command, {
            root: false
          })
        } else {
          await store.dispatch('checklists/createCheckList', addCommand, {
            root: false
          })
        }
      } catch (error) {
      } finally {
        router.push({ name: 'CheckLists' })
      }
    }

    return {
      obsNew,
      dialogRemove,
      dialogNew,
      headers,
      checkList,
      selectedItem,
      selectedItemData,
      newItemData,
      currentCheckList,
      saveItem,
      addItem,
      removeItem,
      saveCheckList
    }
  }
})
</script>
