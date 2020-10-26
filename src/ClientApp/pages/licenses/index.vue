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
                        <v-menu
                          ref="startVisible"
                          v-model="startVisible"
                          :close-on-content-click="false"
                          transition="scale-transition"
                          offset-y
                          max-width="290px"
                          min-width="290px"
                        >
                          <template v-slot:activator="{ on, attrs }">
                            <ValidationProvider rules="required" immediate vid="validFrom" v-slot="{ errors }">
                              <v-text-field
                                v-model="validFromFormatted"
                                label="Valid From"
                                hint="MM/DD/YYYY format"
                                persistent-hint
                                prepend-icon="mdi-calendar"
                                v-bind="attrs"
                                :error-messages="errors[0]"
                                @blur="validFrom = parseDate(validFromFormatted)"
                                v-on="on"
                              ></v-text-field>
                            </ValidationProvider>
                          </template>
                          <v-date-picker
                            v-model="validFrom"
                            no-title
                            @input="startVisible = false"
                          ></v-date-picker>
                        </v-menu>
                      </v-col>
                       <v-col cols="12" md="6">
                        <v-menu
                          ref="endVisible"
                          v-model="endVisible"
                          :close-on-content-click="false"
                          transition="scale-transition"
                          offset-y
                          max-width="290px"
                          min-width="290px"
                        >
                          <template v-slot:activator="{ on, attrs }">
                            <ValidationProvider rules="required|precedesDate:validFrom" immediate v-slot="{ errors }">
                              <v-text-field
                                v-model="validToFormatted"
                                label="Valid To"
                                hint="MM/DD/YYYY format"
                                persistent-hint
                                prepend-icon="mdi-calendar"
                                :error-messages="errors[0]"
                                @blur="validTo = parseDate(validToFormatted)"
                                v-bind="attrs"
                                v-on="on"
                              ></v-text-field>
                            </ValidationProvider>
                          </template>
                          <v-date-picker
                            v-model="validTo"
                            no-title
                            @input="endVisible = false"
                          ></v-date-picker>
                        </v-menu>
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
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { Vue, Component, mixins, Watch } from "nuxt-property-decorator";
import { ValidationObserver, ValidationProvider, extend } from "vee-validate";
import InnerPageMixin from "@/mixins/innerpage";
import { LicensesState } from "store/licenses";
import { LicenseDTO } from "@/types/Licenses";

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
      value: "validity.start",
      sortable: true,
      align: "left",
    },
    {
      text: "Valid To",
      value: "validity.end",
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
  item: LicenseDTO = { licenseId: 0, validity: {start: '', end: '' } } as LicenseDTO;
  isNew: boolean = false;

  validFrom= new Date().toISOString().substr(0, 10)
  validFromFormatted= this.formatDate(new Date().toISOString().substr(0, 10))

  validTo= new Date().toISOString().substr(0, 10)
  validToFormatted= this.formatDate(new Date().toISOString().substr(0, 10))

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
  
  formatDate (date: any) {
    if (!date) return null

    const [year, month, day] = date.split('-')
    return `${month}/${day}/${year}`
  }
  parseDate (date: any) {
    if (!date) return null

    const [month, day, year] = date.split('/')
    return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
  }

  @Watch('validFrom')
  onValidFromChanged() {
    this.validFromFormatted = this.formatDate(this.validFrom)
    this.item.validity.start = this.validFrom
  }

   @Watch('validTo')
  onValidToChanged() {
    this.validToFormatted = this.formatDate(this.validTo)
    this.item.validity.end = this.validTo
  }
}
</script>