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
    <new-report-dialog v-model="dialog" />
    <v-data-table :items="reportList" item-key="id" :search="filter" dense :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Inspection Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-btn class="mx-2" x-small 
            fab dark color="primary"
            @click="dialog = true">
              <v-icon dark>mdi-plus</v-icon>
          </v-btn>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
          :disabled="!item.isClosed || !item.photoRecords.length>0"
          small
          color="primary"
          class="mr-2"
          @click="printHelper.printCompoundedPhotoRecord(item.id)"
        >
          mdi-camera
        </v-icon>
        <v-icon
          :disabled="!item.isClosed"
          small
          color="primary"
          class="mr-2"
          @click="printHelper.print(item.id)"
        >
          mdi-printer
        </v-icon>
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="$router.push({ name: 'Reports-id', params: { id: item.id} })"
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          :disabled="item.isClosed"
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
import { Vue, Component, mixins } from 'nuxt-property-decorator'
import InnerPageMixin from '@/mixins/innerpage'

import { ReportsState } from 'store/reportstrore'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { Report, CreateReport, ReportConfiguration, EMALicenseType, CheckList, Signature, ResponsableType, CheckValue } from '~/types'
import { PrintHelper } from '@/Helpers'
import CreateReportDialog from '@/components/NewReportDialog.vue'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider,
    CreateReportDialog
  }
})
export default class ReportsPage extends mixins(InnerPageMixin) {
  printHelper!: PrintHelper
  dialogRemove: Boolean = false
  dialog: Boolean = false
  selectedItem: Report = {} as Report
  filter: String = ''
  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api','')
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

    fetch ({ store, query }: any) {
      store.dispatch('reportstrore/getReports', { filter: '', closed: query.closed }, { root: true })
    }

    mounted() {
      this.printHelper = new PrintHelper(this.$store)
    }

    selectItem (item: Report): void{
      this.selectedItem = item
    }

    formatDate (date:string): string {
      return moment(date).format('YYYY-MM-DD HH:mm')
    }

    deleteReport () {
      this.$store.dispatch('reportstrore/deleteReport', this.selectedItem.id, { root: true })
        .then(() => {
          this.dialogRemove = false
        })
    }
}
</script>

<style>

</style>