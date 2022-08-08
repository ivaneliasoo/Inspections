<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove CheckList"
      message="This operation will remove this CheckList and all items and params (if not used)"
      :code="selectedItem.id"
      :description="selectedItem.text"
      @yes="removeCheckList(selectedItem)"
    />
    <message-dialog v-model="dialogItems" :actions="[]">
      <template #title="{}"> {{ selectedItem.text }} Items </template>
      <template #subtitle="{}">
        <v-list subheader two-line flat>
          <v-subheader>Items &amp; Params</v-subheader>
          <v-list-item v-for="item in checkItems" :key="item.id">
            <template #default="{ toggle }">
              <v-list-item-action>
                <v-checkbox color="primary" @click="toggle" />
              </v-list-item-action>

              <v-list-item-content>
                <v-list-item-title>{{ item.text }}</v-list-item-title>
                <v-list-item-subtitle>
                  Required {{ item.required }}
                </v-list-item-subtitle>
              </v-list-item-content>
            </template>
          </v-list-item>
        </v-list>
      </template>
    </message-dialog>
    <v-data-table
      :class="$device.isTablet ? 'tablet-text' : ''"
      :items="checks"
      item-key="id"
      dense
      :search="filter.filterText"
      :loading="loading"
      :headers="headers"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>CheckLists</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="$router.push({ name: 'CheckLists-id', params: { id: -1 } })"
          >
            <v-icon dark> mdi-plus </v-icon>
          </v-btn>
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
          <v-col cols="12" :md="filter.inConfigurationOnly ? 2 : 7">
            <v-switch
              v-model="filter.inConfigurationOnly"
              label="In Configuration"
            />
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
      <template #[`item.actions`]="{ item }">
        <v-icon
          color="primary"
          class="mr-2"
          @click="
            selectItem(item)
            router.push({
              name: 'CheckLists-id',
              params: { id: selectedItem.id },
            })
            dialog = true
          "
        >
          mdi-pencil
        </v-icon>
        <v-icon
          color="error"
          @click="
            selectItem(item)
            dialogRemove = true
          "
        >
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import {
  defineComponent,
  reactive,
  useRoute,
  ref,
  watch,
  useFetch,
  useRouter,
} from '@nuxtjs/composition-api'
import { useChecklistStore } from '~/composables/useChecklistStore'
import { useConfigurationStore } from '~/composables/useConfigurationStore'
import { useReportsStore } from '~/composables/useReportsStore'
import {
  CheckList,
  CheckListItem,
  FilterType,
  ReportConfiguration,
  Report,
} from '@/types'
import useGoBack from '~/composables/useGoBack'

export default defineComponent({
  setup() {
    useGoBack()
    const checklistsStore = useChecklistStore()
    const configurationsStore = useConfigurationStore()
    const reportsStore = useReportsStore()
    const route = useRoute()
    const router = useRouter()

    const headers = [
      {
        text: 'Id',
        value: 'id',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
      {
        text: 'Text',
        value: 'text',
        sortable: true,
        align: 'left',
        class: 'secundary',
      },
      {
        text: 'Annotation',
        value: 'annotation',
        sortable: true,
        align: 'left',
        class: 'secundary',
      },
      {
        text: 'Child Items',
        value: 'totalItems',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
      {
        text: '',
        value: 'actions',
        sortable: false,
        align: 'center',
        class: 'secundary',
      },
    ]

    const filter: FilterType = {
      filterText: '',
      inConfigurationOnly:
        route.value.query.configurationonly === 'true' ? true : false ?? true,
      reportId: route.value.query.reportid
        ? parseInt(route.value.query.reportid as string)
        : 1001,
      reportConfigurationId: route.value.query.configurationid
        ? parseInt(route.value.query.configurationid as string)
        : 1,
    }

    const selectedItem = ref<CheckList>({} as CheckList)

    const state = reactive({
      dialog: false,
      dialogRemove: false,
      dialogItems: false,
      loading: false,
    })

    useFetch(async () => {
      state.loading = true

      await Promise.all([
        reportsStore.getReports({
          filter: '',
          closed: route.value.query.closed,
          orderBy: 'date',
          myreports: false,
          descending: true,
        }),
        configurationsStore.getConfigurations(''),
        checklistsStore.getChecklists(filter),
      ])

      state.loading = false
    })

    const selectItem = (item: CheckList): void => {
      try {
        selectedItem.value = item
        state.loading = true
        checklistsStore.getCheckListItemsById(selectedItem.value.id)
      } catch (error) {
        state.loading = false
      }
    }

    const removeCheckList = async (item: CheckList) => {
      state.loading = true
      await checklistsStore.deleteCheckList({ idCheckList: item.id })
      state.loading = false
    }

    watch(
      () => filter,
      (value) => {
        state.loading = true
        checklistsStore.getChecklists(value).finally(() => {
          state.loading = false
        })
      },
      { deep: true }
    )

    const checks = (): CheckList[] => {
      return checklistsStore.$state.checkLists
    }

    const reports = (): Report[] => {
      return reportsStore.reportList
    }

    const configurations = (): ReportConfiguration[] => {
      return configurationsStore.configurations
    }

    const checkItems = (): CheckListItem[] => {
      return checklistsStore.$state.currentCheckList.checks
    }

    return {
      checks,
      checkItems,
      headers,
      configurations,
      reports,
      filter,
      selectedItem,
      selectItem,
      removeCheckList,
      state,
      route,
      router,
    }
  },
})
</script>
