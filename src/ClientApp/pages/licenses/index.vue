<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Licenses"
      message="This operation will remove this License. If Proceed, you no longer get it Avaliable again"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteLicense();"
    />
    <v-data-table
      :items="Licenses"
      item-key="id"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
      :class="$device.isTablet ? 'tablet-text':''"
    >
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Licenses</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="dialog = true; isNew = true; item = { licenseId: 0, validity: {start: '', end: '' } }"
          >
            <v-icon dark>mdi-plus</v-icon>
          </v-btn>
          <v-dialog
            v-model="dialog"
            persistent
            scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
          >
            <ValidationObserver tag="form" v-slot="{ valid, reset }">
              <v-card>
                <v-card-title>
                  <span class="headline">Edit License</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="4">
                        <ValidationProvider rules="required" immediate v-slot="{ errors }">
                          <v-text-field
                            v-model="item.licenseId"
                            readonly
                            autocomplete="nope"
                            name="LicenseId"
                            :error-messages="errors"
                            label="Id"
                          />
                        </ValidationProvider>
                      </v-col>
                    
                     <v-col cols="12" md="8">
                        <ValidationProvider rules="required" immediate v-slot="{ errors }">
                          <v-text-field
                            v-model="item.number"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="licenseNumber"
                            label="Number"
                          />
                        </ValidationProvider>
                      </v-col>
                      </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col>
                        <ValidationProvider rules="required" immediate vid="validFrom" v-slot="{ errors }">
                          <DatePickerBase type="number" label="License"
                              v-model="item.validityStart"
                              :error-messages="errors"
                              max="" />
                        </ValidationProvider>
                      </v-col>
                       <v-col cols="12" md="6">
                        <ValidationProvider rules="required|precedesDate:validFrom" immediate v-slot="{ errors }">
                          <DatePickerBase type="number" label="License"
                            v-model="item.validityEnd"
                            :error-messages="errors"
                            max="" />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn
                    color="success"
                    text
                    :loading="loading"
                    :disabled="!valid"
                    @click="upsertLicense()"
                  >Save</v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="reset(); item = { id: 0 }; dialog = false"
                  >Cancel</v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template v-slot:activator="{ on }">
            <v-icon
              v-on="on"
              color="primary"
              class="mr-2"
              @click="selectItem(item); isNew = false; dialog = true"
            >mdi-pencil</v-icon>
          </template>
          <span>Edit</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template v-slot:activator="{ on }">
            <v-icon
              v-on="on"
              color="error"
              @click="selectItem(item); dialogRemove = true"
            >mdi-delete</v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
      </template>
       <template v-slot:item.validityStart="{ item }">
              {{ parseDate(item.validityStart) }}
            </template>
            <template v-slot:item.validityEnd="{ item }">
              {{ parseDate(item.validityEnd) }}
        </template>
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { Vue, Component, mixins, Watch } from "nuxt-property-decorator";
import { ValidationObserver, ValidationProvider, extend } from "vee-validate";
import InnerPageMixin from "@/mixins/innerpage";
import { LicensesState } from "store/licenses";
import { LicenseDTO } from "@/types/Licenses";
import { DateTime } from "luxon";

// Regla para validadr fechas rangos de fechas
extend('precedesDate', {
  validate: (dateTo, { dateFrom }: any) => {
    return dateTo >= dateFrom
  },
  message: 'end validity date cant be a date before than start validity date',
  params: [{ name: 'dateFrom', isTarget: true }]
})

@Component({
  components: {
    ValidationObserver,
    ValidationProvider,
  },
})
export default class LicensesAdmin extends mixins(InnerPageMixin) {
  dialog: boolean = false;
  dialogRemove: boolean = false;
  loading: boolean = false;
  startVisible: boolean = false;
  endVisible: boolean = false;
  
  filter: any = {
    filterText: "",
  };
  headers: any[] = [
    {
      text: "ID",
      value: "licenseId",
      sortable: true,
      align: "left",
    },
    {
      text: "License",
      value: "number",
      sortable: true,
      align: "left",
    },
    {
      text: "Valid From",
      value: "validityStart",
      sortable: true,
      align: "left",
    },
    {
      text: "Valid To",
      value: "validityEnd",
      sortable: true,
      align: "left",
    },
    {
      text: "",
      value: "actions",
      sortable: false,
      align: "left",
    },
  ];
  selectedItem: LicenseDTO = {} as LicenseDTO;
  item: LicenseDTO = { licenseId: 0, validityStart: '', validityEnd: '' } as LicenseDTO;
  isNew: boolean = false;

  get Licenses(): LicenseDTO[] {
    return (this.$store.state.licenses as LicensesState).licensesList;
  }

  selectItem(item: LicenseDTO): void {
    this.selectedItem = item;
    this.$store
      .dispatch("licenses/getLicenseById", this.selectedItem.licenseId, {
        root: true,
      })
      .then((resp) => (this.item = resp));
  }

  async fetch() {
    if (!this.$auth.user.isAdmin)
      this.$nuxt.error({ statusCode: 403, message: "Forbbiden" });
    await this.$store.dispatch("licenses/getLicenses", {}, { root: true });

    if(this.$route.query.id) {

      this.$store
      .dispatch("licenses/getLicenseById", this.$route.query.id, {
        root: true,
      })
      .then((resp) => (this.item = resp));
      this.isNew = false; 
      this. dialog = true
    }
  }

  deleteLicense() {
    this.$store
      .dispatch("licenses/deleteLicense", this.selectedItem.licenseId, { root: true })
      .then(() => {
        this.dialog = false;
      });
  }

  async upsertLicense() {
    this.loading = true;
    if (!this.isNew)
      await this.$store.dispatch("licenses/updateLicense", this.item, { root: true });
    else {
      await this.$store.dispatch("licenses/createLicense", this.item, { root: true });
      await this.$store.dispatch("licenses/getLicenses", {}, { root: true });
    }
    this.dialog = false;
    this.isNew = true;
    this.loading = false;
  }
  
  parseDate(date: string) {
    return DateTime.fromISO(date).toLocaleString()
  }
}
</script>