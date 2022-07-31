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
    <v-row>
      <new-report-dialog v-model="dialog" @report-created="goToNewReport($event)" />
      <!-- <OptionsCards :options="cardOptions" /> -->
    </v-row>
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
          <v-toolbar-title>Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
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
      <template #[`item.actions`]="{ item }">
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
      </template>
      <template #[`item.date`]="{ item }">
        {{ formatDate(item.date) }}
      </template>
      <template #[`item.photoRecords`]="{ item }">
        {{ item.photoRecords.length }}
      </template>
      <template #[`item.notes`]="{ item }">
        {{ item.notes.length }}
      </template>
      <template #[`item.checkList`]="{ item }">
        {{ item.checkList.length }}
      </template>
      <template #[`item.signatures`]="{ item }">
        {{ item.signatures.length }}
      </template>
      <template #[`item.completed`]="{ item }">
        <v-simple-checkbox v-model="item.completed" disabled />
      </template>
      <template #[`item.isClosed`]="{ item }">
        <v-simple-checkbox v-model="item.isClosed" disabled />
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Component } from 'nuxt-property-decorator'
import { CardOption, Report } from '~/types'
import { PrintHelper } from '~/Helpers'
import { ReportsState } from '~/store/reportstrore'

@Component({
  layout: 'default'
})
export default class IndexPage extends Vue {
  dialog: boolean = false
  self = this

  loading: boolean = false
  printing: boolean = false
  printHelper!: PrintHelper
  dialogRemove: Boolean = false
  selectedItem: Report = {} as Report
  filter: String = ''
  showOnlyMyReports: Boolean = false
  hostName: string = this.$axios!.defaults!.baseURL!.replace('/api', '')

  cardOptions: CardOption[] = [
    {
      name: 'new',
      text: 'New Report',
      helpText: 'Creates a configured and empty report',
      icon: 'mdi-plus',
      color: 'accent',
      path: '',
      action: () => this.createReport()
    },
  ]

  goToNewReport (event: any) {
    this.$router.push(`/reports/${event}`)
  }

  createReport () {
    this.dialog = false; this.dialog = true
  }

  headers: any[] = [
    {
      text: 'Date',
      value: 'date',
      sortable: true,
      align: 'center'
    },
    {
      text: 'Location',
      value: 'address',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Report Type',
      value: 'title',
      sortable: true,
      align: 'left'
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
    // this.loading = true
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
        `reports/${item.id}/export?printPhotos=${printPhotos}&reportConfigurationId=${item.reportConfigurationId}`,
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

<style lang="scss" scoped>
:deep(.expired-row) {
  color: #C62828 !important;
  background-color: rgb(255, 242, 242) !important;
}
:deep(.expiring-row) {
  color: #FF8F00 !important;
  background-color: rgb(255, 242, 242) !important;
}
</style>
