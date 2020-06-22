<template>
  <div>
    <v-data-table :items="signatures" item-key="id" :search="filter.filterText" dense :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Signatures</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on">
                New Signature
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">New Signature</span>
              </v-card-title>
              <v-card-text>
                <v-container>
                  <v-row>
                  </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions>
                <v-spacer />
              </v-card-actions>
            </v-card>
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
      <template v-slot:item.actions="{}">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click=""
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          color="error"
          @click=""
        >
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'nuxt-property-decorator'
import { SignatureState } from 'store/signatures'
import { SignatureDTO } from '../types/Signatures/ViewModels/SignatureDTO'
import GridFilter from '@/components/GridFilter.vue'
import { Report, ReportConfiguration, FilterType } from '~/types'
import { ReportConfigurationState } from '~/store/configurations'
import { ReportsState } from '~/store/reportstrore'

@Component({
  components: {
    GridFilter
  }
})
export default class SignaturesPage extends Vue {
  dialog: boolean = false
  filter: FilterType = {
    filterText: '',
    inConfigurationOnly: undefined,
    repotId: undefined,
    reportConfigurationId: undefined
  }

  headers: any[] = [
    {
      text: 'Id',
      value: 'id',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Title',
      value: 'title',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Annotation',
      value: 'annotation',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Principal',
      value: 'responsableName',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: '',
      value: 'actions',
      sortable: false,
      align: 'center',
      class: 'secundary'
    }
  ];

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

  async fetch () {
    await this.$store.dispatch('reportstrore/getReports', '', { root: true })
    await this.$store.dispatch('configurations/getConfigurations', '', { root: true })
    await this.$store.dispatch('signatures/getSignatures', {}, { root: true })
  }

  @Watch('filter', { deep: true })
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  async onFilterChanged (value: FilterType, oldValue: FilterType) {
    await this.$store.dispatch('signatures/getSignatures', value, { root: true })
  }
}
</script>

<style>

</style>