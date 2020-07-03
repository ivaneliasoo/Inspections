<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Reports"
      message="This operation will remove this report and all data related"
      :code="selectedItem.id"
      :description="selectedItem.name"
      @yes="deleteReport();"
      @no="dialogRemove=false"
    />
    <message-dialog v-model="dialog" :actions="['yes','cancel']" @yes="createReport" @cancel="dialog=false">
      <template v-slot:title="{}">
        New Report
      </template>
      <ValidationObserver tag="form" ref="obs">
        <v-row>
          <v-col cols="12" xs="12" md="6">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select
                id="selReportType"
                v-model="newReport.reportType"
                :error-messages="errors"
                item-value="id"
                item-text="text"
                label="Report Type"
                :items="[{ id:0, text: 'Inspection' }]"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12" xs="12" md="6">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select
                id="selConfigurations"
                v-model="newReport.configurationId"
                :error-messages="errors"
                label="Select a Saved Configuration"
                item-value="id"
                item-text="title"
                :items="configurations"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-text-field
                id="txtName"
                v-model="newReport.name"
                :error-messages="errors"
                label="Report Name"
              />
            </ValidationProvider>
          </v-col>
        </v-row>
      </ValidationObserver>
    </message-dialog>
    <v-data-table :items="reportList" item-key="id" :search="filter" dense :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Inspection Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-btn color="primary" dark class="mb-2" @click="dialog=true">
            New Report
          </v-btn>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click=""
        >
          mdi-printer
        </v-icon>
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="$router.push({ name: 'Reports-id', params: { id: item.id} })"
        >
          mdi-file-chart
        </v-icon>
        <v-icon
          small
          color="error"
          @click="selectItem(item); dialogRemove = true"
        >
          mdi-delete
        </v-icon>
      </template>
      <template v-slot:item.date="{ item }">
        {{ formatDate(item.date) }}
      </template>
      <template v-slot:item.photoRecords="{ item }">
        {{ item.photoRecords.length }}
      </template>
      <template v-slot:item.notes="{ item }">
        {{ item.notes.length }}
      </template>
      <template v-slot:item.checkList="{ item }">
        {{ item.checkList.length }}
      </template>
      <template v-slot:item.signatures="{ item }">
        {{ item.signatures.length }}
      </template>
      <template v-slot:item.completed="{ item }">
        <v-simple-checkbox v-model="item.completed" disabled />
      </template>
      <template v-slot:item.isClosed="{ item }">
        <v-simple-checkbox v-model="item.isClosed" disabled />
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Component } from 'nuxt-property-decorator'
import { ReportConfigurationState } from 'store/configurations'
import { ReportsState } from 'store/reportstrore'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { Report, CreateReport, ReportConfiguration } from '~/types'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class ReportsPage extends Vue {
  $refs!: {
    obs: InstanceType<typeof ValidationObserver>
  }
  dialog: Boolean = false
  dialogRemove: Boolean = false
  selectedItem: Report = {} as Report
  newReport:CreateReport = {} as CreateReport
  filter: String = ''
    headers: any[] = [
      {
        text: 'Id',
        value: 'id',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Report Name',
        value: 'name',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Date',
        value: 'date',
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
        text: 'Notes',
        value: 'notes',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Checks',
        value: 'checkList',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Signatures',
        value: 'signatures',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Photos',
        value: 'photoRecords',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Completed',
        value: 'completed',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Closed',
        value: 'isClosed',
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

    get reportList (): Report[] {
      return (this.$store.state.reportstrore as ReportsState)
        .reportList
    }

    fetch ({ store }: any) {
      store.dispatch('configurations/getConfigurations', '', { root: true })
      store.dispatch('reportstrore/getReports', '', { root: true })
    }

    get configurations (): ReportConfiguration[] {
      return (this.$store.state.configurations as ReportConfigurationState).configurations
    }

    selectItem (item: Report): void{
      this.selectedItem = item
    }

    formatDate (date:string): string {
      return moment(date).format('YYYY-MM-DD HH:mm')
    }

    // only if i've more time then expected
    get idValid () {
      return this.$refs.obs.flags.valid
    }

    async createReport () {
      if(await this.$refs.obs.validate() === true)
      this.$store.dispatch('reportstrore/createReport', this.newReport, { root: true })
        .then(() => {
          this.$store.dispatch('reportstrore/getReports', '', { root: true })
          this.dialog = false
        })
    }

    deleteReport () {
      this.$store.dispatch('reportstrore/deleteReport', this.selectedItem.id, { root: true })
        .then(() => {
          this.dialog = false
        })
    }
}
</script>

<style>

</style>