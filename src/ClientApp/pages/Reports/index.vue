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
    <v-data-table
      :class="$device.isTablet ? 'tablet-text':''"
      :items="reportList"
      item-key="id"
      :search="filter"
      dense
      :headers="headers"
      :loading="loading"
    >
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Inspection Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="dialog = true"
          >
            <v-icon dark>
              mdi-plus
            </v-icon>
          </v-btn>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-tooltip top>
          <template v-slot:activator="{ on }">
            <v-icon
              :disabled="!item.isClosed || !item.photoRecords.length>0"
              color="primary"
              class="mr-2"
              v-on="on"
              @click="printHelper.printCompoundedPhotoRecord(item.id)"
            >
              mdi-camera
            </v-icon>
          </template>
          <span>Print Inspection Report With Photos</span>
        </v-tooltip>
        <v-tooltip top>
          <template v-slot:activator="{ on }">
            <v-icon
              :disabled="!item.isClosed"
              color="primary"
              class="mr-2"
              v-on="on"
              @click="printHelper.print(item.id)"
            >
              mdi-printer
            </v-icon>
          </template>
          <span>Print Inspections Report without Photos</span>
        </v-tooltip>
        <v-tooltip top>
          <template v-slot:activator="{ on }">
            <v-icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="$router.push({ name: 'Reports-id', params: { id: item.id} })"
            >
              mdi-pencil
            </v-icon>
          </template>
          <span>Edit</span>
        </v-tooltip>
        <v-tooltip top>
          <template v-slot:activator="{ on }">
            <v-icon
              :disabled="item.isClosed"
              color="error"
              v-on="on"
              @click="selectItem(item); dialogRemove = true"
            >
              mdi-delete
            </v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template v-slot:activator="{ on }">
            <v-icon
              v-if="$auth.user.isAdmin"
              :disabled="item.isClosed"
              color="primary"
              v-on="on"
              @click="$router.push(`/checklists?&configurationonly=false&reportid=${item.id}`)"
            >
              mdi-format-list-checks
            </v-icon>
          </template>
          <span>Edit Checklist Configuration for this report</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template v-slot:activator="{ on }">
            <v-icon
              v-if="$auth.user.isAdmin"
              :disabled="item.isClosed"
              color="primary"
              v-on="on"
              @click="$router.push(`/signatures?&configurationonly=false&reportid=${item.id}`)"
            >
              mdi-draw
            </v-icon>
          </template>
          <span>Edit Signatures Configuration for this report</span>
        </v-tooltip>
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
import { Component, mixins } from 'nuxt-property-decorator'

import { ReportsState } from 'store/reportstrore'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import InnerPageMixin from '@/mixins/innerpage'
import { Report } from '~/types'
import { PrintHelper } from '@/Helpers'
import CreateReportDialog from '@/components/NewReportDialog.vue'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider,
    CreateReportDialog
  },
  head: {
    title: 'Reports List'
  }
})
export default class ReportsPage extends mixins(InnerPageMixin) {
  loading: boolean = false
  printHelper!: PrintHelper
  dialogRemove: Boolean = false
  dialog: Boolean = false
  selectedItem: Report = {} as Report
  filter: String = ''
  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api', '')
    headers: any[] = [
      {
        text: 'Id',
        value: 'id',
        sortable: true,
        align: 'center',
      },
      {
        text: 'Date',
        value: 'date',
        sortable: true,
        align: 'center',
      },
      {
        text: 'Report Name',
        value: 'name',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Completed With Signatures',
        value: 'isClosed',
        sortable: true,
        align: 'center',
      },
      {
        text: '',
        value: 'actions',
        sortable: false,
        align: 'center',
      }
    ];

    get reportList (): Report[] {
      return (this.$store.state.reportstrore as ReportsState)
        .reportList
    }

    async fetch () {
      this.loading = true
      await this.$store.dispatch('reportstrore/getReports', { filter: '', closed: this.$route.query.closed }, { root: true })

      this.loading = false
    }

    mounted () {
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
