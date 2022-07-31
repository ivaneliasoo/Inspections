<template>
  <div>
    <!-- <alert-dialog
      v-model="componentState.dialogRemove"
      title="Remove Reports"
      message="This operation will remove this report and all data related"
      :code="componentState.selectedItem.id"
      :description="componentState.selectedItem.name"
      @yes="deleteReport()"
      @no="componentState.dialogRemove = false"
    /> -->
    <v-row>
      <new-report-dialog v-model="componentState.dialog" @report-created="goToNewReport($event)" />
    </v-row>
    <v-data-table
      :class="$device.isTablet ? 'tablet-text' : ''"
      :items="reports"
      item-key="id"
      :search="componentState.filter"
      dense
      :headers="headers"
      :loading="componentState.loading"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="componentState.filter" />
          <v-spacer />
          <v-btn
            v-if="!route.query.closed"
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
              :loading="componentState.printing"
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
              :loading="componentState.printing"
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
                router.push({ name: 'Reports-id', params: { id: item.id } })
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
                componentState.dialogRemove = true;
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
import { useContext, useRouter, defineComponent, reactive, computed, useFetch, useRoute } from '@nuxtjs/composition-api'
import { Report } from '~/types'
import { useReportsStore } from '~/composables/useReportsStore'

export default defineComponent({
  layout: 'default',
  setup () {
    const headers: any[] = [
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

    const reportsStore = useReportsStore()

    const { $axios: axios } = useContext()

    const route = useRoute()
    const router = useRouter()

    const componentState = reactive({
      dialog: false,
      loading: false,
      printing: false,
      dialogRemove: false,
      selectedItem: { id: 0 },
      filter: '',
      showOnlyMyReports: false,
      hostName: axios!.defaults!.baseURL!.replace('/api', '')
    })

    const selectItem = (item: Report): void => {
      componentState.selectedItem = item
    }

    const formatDate = (date: string): string => {
      return moment(date).format('YYYY-MM-DD HH:mm')
    }

    const deleteReport = () => {
      reportsStore.deleteReport(componentState.selectedItem.id)
        .then(() => {
          componentState.dialogRemove = false
        })
    }

    const generatePdf = async (item: Report, printPhotos: boolean = false) => {
      try {
        componentState.printing = true
        const file = await axios.$get(
        `reports/${item.id}/export?printPhotos=${printPhotos}&reportConfigurationId=${item.reportConfigurationId}`,
        { responseType: 'blob' }
        )
        downloadFile(
          file,
          printPhotos
            ? `compunded_photo_record_${item.name}`
            : `report_${item.name}`
        )
      } catch (e) {
      } finally {
        componentState.printing = false
      }
    }

    const downloadFile = (blob: Blob, name: any): void => {
      const link = document.createElement('a')
      link.target = '_blank'
      link.href = window.URL.createObjectURL(blob)
      link.download = `${name}`
      link.click()
    }
    const goToNewReport = (event: any) => {
      router.push(`/reports/${event}`)
    }

    const createReport = () => {
      componentState.dialog = false; componentState.dialog = true
    }
    const reports = computed(() => reportsStore.reportList)

    useFetch(async () => {
      componentState.loading = true
      // TODO: Save filters state on store
      await reportsStore.getReports(
        {
          filter: '',
          closed: route.value.query.closed,
          orderBy: 'date',
          myreports: componentState.showOnlyMyReports,
          descending: true
        })

      componentState.loading = false
    })

    return {
      headers,
      reports,
      selectItem,
      formatDate,
      deleteReport,
      generatePdf,
      downloadFile,
      goToNewReport,
      createReport,
      componentState,
      router,
      route
    }
  }
})

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
