<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Reports"
      message="This operation will remove this signature and all data related"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteSignature();"
    />
    <v-data-table
      :class="$device.isTablet ? 'tablet-text':''"
      :items="signatures"
      item-key="id"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Signatures</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="item = { principal: false }; dialog = true"
          >
            <v-icon dark>
              mdi-plus
            </v-icon>
          </v-btn>
          <v-dialog
            v-model="dialog"
            persistent
            scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
          >
            <ValidationObserver v-slot="{ valid, reset }" tag="form">
              <v-card>
                <v-card-title>
                  <span class="headline">Edit Signature</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="6">
                        <ValidationProvider v-slot="{ errors }" rules="required">
                          <v-text-field
                            v-model="item.title"
                            name="title"
                            :readonly="item.report ? item.report.isClosed:false"
                            :error-messages="errors"
                            label="Title"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <v-text-field
                          v-model="item.annotation"
                          :readonly="item.report ? item.report.isClosed:false"
                          name="annotation"
                          label="Annotation"
                        />
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="4">
                        <ValidationProvider v-slot="{ errors }" :rules="`${!item.reportConfigurationId ? 'required' : ''}`">
                          <v-autocomplete
                            v-model="item.reportId"
                            :disabled="item.reportConfigurationId > 0 || item.id>0"
                            :error-messages="errors"
                            clearable
                            :items="reports"
                            item-text="name"
                            :readonly="item.report ? item.report.isClosed:false"
                            item-value="id"
                            label="Use in Report"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="4">
                        <ValidationProvider v-slot="{ errors }" :rules="`${!item.reportId ? 'required' : ''}`">
                          <v-autocomplete
                            v-model="item.reportConfigurationId"
                            :disabled="item.reportId > 0 || item.id>0"
                            :error-messages="errors"
                            :readonly="item.report ? item.report.isClosed:false"
                            clearable
                            :items="configurations"
                            item-text="title"
                            item-value="id"
                            label="Use in Report Configuration"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="4">
                        <v-checkbox
                          v-model="item.principal"
                          :disabled="item.report ? item.report.isClosed:false"
                          name="principal"
                          label="Principal"
                        />
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn
                    color="success"
                    text
                    :disabled="item.report ? item.report.isClosed : false && !valid"
                    @click="upsertSignature()"
                  >
                    Save
                  </v-btn>
                  <v-btn color="default" text @click="reset(); item = { principal: false }; dialog = false">
                    Cancel
                  </v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
        <v-row justify="space-around" class="ml-2 mr-2">
          <v-col cols="12" md="5">
            <v-autocomplete
              v-model="filter.reportId"
              label="In Report"
              :items="reports"
              item-text="name"
              item-value="id"
              clearable
            />
          </v-col>
          <v-col cols="12" :md="filter.inConfigurationOnly ? 2:7">
            <v-switch v-model="filter.inConfigurationOnly" label="In Configuration" />
          </v-col>
          <v-col v-if="filter.inConfigurationOnly" cols="12" md="5">
            <v-autocomplete
              v-model="filter.reportConfigurationId"
              label="In Report Configuration"
              :items="configurations"
              item-text="title"
              item-value="id"
              clearable
            />
          </v-col>
        </v-row>
      </template>
      <template #item.actions="{ item }">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="selectItem(item); dialog = true"
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          :disabled="item.report ? item.report.isClosed:false"
          color="error"
          @click="selectItem(item); dialogRemove = true"
        >
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import { Component, Watch, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { SignatureState } from 'store/signatures'
import InnerPageMixin from '@/mixins/innerpage'
import { SignatureDTO } from '@/types/Signatures/ViewModels/SignatureDTO'
import { Report, ReportConfiguration, FilterType, Signature } from '~/types'
import { ReportConfigurationState } from '~/store/configurations'
import { ReportsState } from '~/store/reportstrore'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class SignaturesPage extends mixins(InnerPageMixin) {
  dialog: boolean = false
  dialogRemove: boolean = false
  loading: boolean = false
  filter: FilterType = {
    filterText: '',
    inConfigurationOnly: undefined,
    reportId: undefined,
    reportConfigurationId: undefined
  }

  headers: any[] = [
    {
      text: 'Id',
      value: 'id',
      sortable: true,
      align: 'center'
    },
    {
      text: 'Title',
      value: 'title',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Annotation',
      value: 'annotation',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Principal',
      value: 'responsibleName',
      sortable: true,
      align: 'center'
    },
    {
      text: '',
      value: 'actions',
      sortable: false,
      align: 'center'
    }
  ]

  selectedItem: Signature = {} as Signature
  item: any = { principal: false }

  get reports (): Report[] {
    return (this.$store.state.reportstrore as ReportsState)
      .reportList
  }

  get configurations (): ReportConfiguration[] {
    return (this.$store.state.configurations as ReportConfigurationState)
      .configurations
  }

  get signatures (): SignatureDTO[] {
    return (this.$store.state.signatures as SignatureState)
      .signaturesList
  }

  selectItem (item: Signature): void {
    this.selectedItem = item
    this.$store.dispatch('signatures/getSignatureById', this.selectedItem.id, { root: true })
      .then((resp) => { this.item = resp })
  }

  asyncData ({ query }:any) {
    const filter: FilterType = {
      filterText: '',
      inConfigurationOnly: query.configurationonly === 'true' ? true : false ?? true,
      reportId: query.reportid ? parseInt(query.reportid) : undefined,
      reportConfigurationId: query.configurationid ? parseInt(query.configurationid) : undefined
    }
    return {
      filter
    }
  }

  async fetch () {
    this.loading = true
    await Promise.all([this.$store.dispatch('reportstrore/getReports', { filter: '', closed: this.$route.query.closed, orderBy: 'date', myreports: false, descending: true }, { root: true }),
      this.$store.dispatch('configurations/getConfigurations', '', { root: true }),
      this.$store.dispatch('signatures/getSignatures', this.filter, { root: true })])
    this.loading = false
  }

  @Watch('filter', { deep: true })
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  async onFilterChanged (value: FilterType) {
    await this.$store.dispatch('signatures/getSignatures', value, { root: true })
  }

  deleteSignature () {
    this.$store.dispatch('signatures/deleteSignature', this.selectedItem.id, { root: true })
      .then(() => {
        this.dialog = false
      })
  }

  async upsertSignature () {
    if (this.item.id > 0) { await this.$store.dispatch('signatures/updateSignature', this.item, { root: true }) } else {
      await this.$store.dispatch('signatures/createSignature', this.item, { root: true })
      await this.$store.dispatch('signatures/getSignatures', {}, { root: true })
    }
    this.dialog = false
  }
}
</script>

<style>

</style>
