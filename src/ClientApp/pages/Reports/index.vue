<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Reports"
      message="This operation will remove this report and all data related"
      :code="selectedItem.id"
      :description="selectedItem.name"
      @yes="deleteReport()"
      @no="dialogRemove = false"
    />
    <new-report-dialog v-model="dialog" />
    <v-data-table
      :class="$device.isTablet ? 'tablet-text' : ''"
      :items="reportList"
      item-key="id"
      :search="filter"
      dense
      :headers="headers"
      :loading="loading"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Inspection Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
          <v-switch
            v-model="showOnlyMyReports"
            class="tw-mx-4"
            messages="Show only my reports"
            dense
            @change="$fetch()"
          />
          <v-spacer />
          <v-btn
            v-if="!$route.query.closed"
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
      <template #item.actions="{ item }">
        <v-tooltip top>
          <template #activator="{ on }">
            <v-btn
              icon
              :loading="printing"
              :disabled="!item.isClosed || !(item.photosCount > 0)"
              color="primary"
              class="mr-2"
              v-on="on"
              @click="generatePdf(item, true)"
            >
              <v-icon>
                mdi-camera
              </v-icon>
            </v-btn>
          </template>
          <span>Print Inspection Report With Photos</span>
        </v-tooltip>
        <v-tooltip top>
          <template #activator="{ on }">
            <v-btn
              icon
              :loading="printing"
              :disabled="!item.isClosed"
              color="primary"
              class="mr-2"
              v-on="on"
              @click="generatePdf(item)"
            >
              <v-icon> mdi-printer </v-icon>
            </v-btn>
          </template>
          <span>Print Inspections Report without Photos</span>
        </v-tooltip>
        <v-tooltip top>
          <template #activator="{ on }">
            <v-btn
              icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="
                $router.push({ name: 'Reports-id', params: { id: item.id } })
              "
            >
              <v-icon> mdi-pencil </v-icon>
            </v-btn>
          </template>
          <span>Edit</span>
        </v-tooltip>
        <v-tooltip top>
          <template #activator="{ on }">
            <v-icon
              :disabled="item.isClosed"
              color="error"
              v-on="on"
              @click="
                selectItem(item);
                dialogRemove = true;
              "
            >
              mdi-delete
            </v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              v-if="$auth.user.isAdmin"
              :disabled="item.isClosed"
              color="primary"
              v-on="on"
              @click="
                $router.push(
                  `/checklists?&configurationonly=false&reportid=${item.id}`
                )
              "
            >
              mdi-format-list-checks
            </v-icon>
          </template>
          <span>Edit Checklist Configuration for this report</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              v-if="$auth.user.isAdmin"
              :disabled="item.isClosed"
              color="primary"
              v-on="on"
              @click="
                $router.push(
                  `/signatures?&configurationonly=false&reportid=${item.id}`
                )
              "
            >
              mdi-draw
            </v-icon>
          </template>
          <span>Edit Signatures Configuration for this report</span>
        </v-tooltip>
      </template>
      <template #item.date="{ item }">
        {{ formatDate(item.date) }}
      </template>
      <template #item.photoRecords="{ item }">
        {{ item.photoRecords.length }}
      </template>
      <template #item.notes="{ item }">
        {{ item.notes.length }}
      </template>
      <template #item.checkList="{ item }">
        {{ item.checkList.length }}
      </template>
      <template #item.signatures="{ item }">
        {{ item.signatures.length }}
      </template>
      <template #item.completed="{ item }">
        <v-simple-checkbox v-model="item.completed" disabled />
      </template>
      <template #item.isClosed="{ item }">
        <v-simple-checkbox v-model="item.isClosed" disabled />
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import moment from 'moment'
import { Component, mixins } from 'nuxt-property-decorator'

import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { ReportsState } from 'store/reportstrore'
import InnerPageMixin from '@/mixins/innerpage'
import { PrintHelper } from '@/Helpers'
import CreateReportDialog from '@/components/NewReportDialog.vue'
import { Report } from '~/types'

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
  printing: boolean = false
  printHelper!: PrintHelper
  dialogRemove: Boolean = false
  dialog: Boolean = false
  selectedItem: Report = {} as Report
  filter: String = ''
  showOnlyMyReports: Boolean = false
  hostName: string = this.$axios!.defaults!.baseURL!.replace('/api', '')
  headers: any[] = [
    {
      text: 'Id',
      value: 'id',
      sortable: true,
      align: 'center'
    },
    {
      text: 'Date',
      value: 'date',
      sortable: true,
      align: 'center'
    },
    {
      text: 'Report Name',
      value: 'name',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Completed With Signatures',
      value: 'isClosed',
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

  get reportList (): Report[] {
    return (this.$store.state.reportstrore as ReportsState).reportList || []
  }

  async fetch () {
    this.loading = true
    // TODO: Save filters state on store
    await this.$store.dispatch(
      'reportstrore/getReports',
      {
        filter: '',
        closed: this.$route.query.closed,
        orderBy: 'date',
        myreports: this.showOnlyMyReports,
        descending: true
      },
      { root: true }
    )

    this.loading = false
  }

  mounted () {
    this.printHelper = new PrintHelper(this.$store)
  }

  selectItem (item: Report): void {
    this.selectedItem = item
  }

  formatDate (date: string): string {
    return moment(date).format('YYYY-MM-DD HH:mm')
  }

  deleteReport () {
    this.$store
      .dispatch('reportstrore/deleteReport', this.selectedItem.id, {
        root: true
      })
      .then(() => {
        this.dialogRemove = false
      })
  }

  async generatePdf (item: Report, printPhotos: boolean = false) {
    try {
      this.printing = true
      const file = await this.$axios.$get(
        `reports/${item.id}/export?printPhotos=${printPhotos}`,
        { responseType: 'blob' }
      )
      this.downloadFile(
        file,
        printPhotos
          ? `compunded_photo_record_${item.name}`
          : `report_${item.name}`
      )
    } catch (e) {
    } finally {
      this.printing = false
    }
  }

  downloadFile (blob: Blob, name: any): void {
    const link = document.createElement('a')
    link.target = '_blank'
    link.href = window.URL.createObjectURL(blob)
    link.download = `${name}`
    link.click()
  }
}
</script>

<style>
</style>
