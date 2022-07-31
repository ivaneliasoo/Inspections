<template>
  <div>
    <alert-dialog
      v-model="state.dialogRemove"
      title="Remove Reports"
      message="This operation will remove this report and all data related"
      :code="selectedItem.id"
      :description="selectedItem.name"
      @yes="deleteReport()"
      @no="state.dialogRemove = false"
    />
    <new-report-dialog v-model="state.dialog" />
    <v-data-table
      :class="$device.isTablet ? 'tablet-text' : ''"
      :items="reports"
      item-key="id"
      :search="state.filter"
      dense
      :headers="headers"
      :loading="state.loading"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Inspection Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="state.filter" />
          <v-switch
            v-model="state.showOnlyMyReports"
            class="tw-mx-4"
            messages="Show only my reports"
            dense
            @change="$fetch()"
          />
          <v-spacer />
          <v-btn
            v-if="!route.query.closed"
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="state.dialog = true"
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
              :loading="state.printing"
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
              :loading="state.printing"
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
                state.dialogRemove = true;
              "
            >
              mdi-delete
            </v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
        <v-tooltip v-if="isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              v-if="isAdmin"
              :disabled="item.isClosed"
              color="primary"
              v-on="on"
              @click="
                router.push(
                  `/checklists?&configurationonly=false&reportid=${item.id}`
                )
              "
            >
              mdi-format-list-checks
            </v-icon>
          </template>
          <span>Edit Checklist Configuration for this report</span>
        </v-tooltip>
        <v-tooltip v-if="isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              v-if="isAdmin"
              :disabled="item.isClosed"
              color="primary"
              v-on="on"
              @click="
                router.push(
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
import { defineComponent, reactive, useContext, ref, useFetch, useRoute, computed, useRouter } from '@nuxtjs/composition-api'
import { useReportsStore } from '~/composables/useReportsStore'
import { Report } from '~/types'
import useDateTime from '~/composables/useDateTime'

export default defineComponent({
  setup () {
    const headers: any[] = [
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

    const reportsStore = useReportsStore()
    const { $auth, $axios } = useContext()
    const route = useRoute()
    const router = useRouter()

    const { formatDate } = useDateTime()

    const selectedItem = ref<Report>({ id: 0 } as Report)

    const state = reactive({
      loading: false,
      printing: false,
      dialogRemove: false,
      dialog: false,
      filter: '',
      showOnlyMyReports: false,
      hostName: $axios!.defaults!.baseURL!.replace('/api', '')
    })

    useFetch(async () => {
      state.loading = true
      // TODO: Save filters state on store
      await reportsStore.getReports(
        {
          filter: '',
          closed: route.value.query.closed,
          orderBy: 'date',
          myreports: state.showOnlyMyReports,
          descending: true
        })

      state.loading = false
    })

    const reports = computed((): Report[] => reportsStore.reportList)

    const selectItem = (item: Report): void => {
      selectedItem.value = item
    }

    const deleteReport = () => {
      reportsStore.deleteReport(selectedItem.value.id)
        .then(() => {
          state.dialogRemove = false
        })
    }

    const generatePdf = async (item: Report, printPhotos: boolean = false) => {
      try {
        state.printing = true
        const file = await $axios.$get(
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
        state.printing = false
      }
    }

    const isAdmin = computed(() => $auth.user.isAdmin)

    const downloadFile = (blob: Blob, name: any): void => {
      const link = document.createElement('a')
      link.target = '_blank'
      link.href = window.URL.createObjectURL(blob)
      link.download = `${name}`
      link.click()
    }

    return {
      state,
      reports,
      selectItem,
      formatDate,
      deleteReport,
      generatePdf,
      selectedItem,
      headers,
      router,
      route,
      isAdmin,
    }
  },
  head: {
    title: 'Reports List'
  }

})
</script>

<style>
</style>
