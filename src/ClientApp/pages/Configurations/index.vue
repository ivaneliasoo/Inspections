<template>
  <div>
    <alert-dialog
      v-model="componentState.dialogRemove"
      title="Remove Configuration"
      message="This operation will remove this Configuration. You'll not be able to proceed if this configration has been used in any reports"
      :code="componentState.selectedItem.id"
      :description="componentState.selectedItem.title"
      @yes="deleteConfig()"
      @no="componentState.dialogRemove = false"
    />
    <v-data-table
      :items="configs"
      item-key="id"
      :headers="headers"
      :class="$device.isTablet ? 'tablet-text' : ''"
      :loading="componentState.loading"
      dense
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Manage Reports Configurations</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="componentState.filter" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="
              router.push({ name: 'Configurations-id', params: { id: -1 } })
            "
          >
            <v-icon dark> mdi-plus </v-icon>
          </v-btn>
        </v-toolbar>
      </template>
      <template #[`item.actions`]="{ item }">
        <v-icon
          color="primary"
          class="mr-2"
          @click="
            router.push({ name: 'Configurations-id', params: { id: item.id } })
          "
        >
          mdi-pencil
        </v-icon>
        <v-icon
          color="error"
          @click="
            componentState.selectedItem = item
            componentState.dialogRemove = true
          "
        >
          mdi-delete
        </v-icon>
        <v-icon color="indigo" @click="$router.push(`/printsections`)">
          mdi-page-previous-outline
        </v-icon>
      </template>
      <template #[`item.type`]="{ item }">
        {{ item.type === 0 ? 'Inspection' : 'Unkown' }}
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import {
  defineComponent,
  reactive,
  computed,
  useContext,
  useFetch,
  useRouter,
} from '@nuxtjs/composition-api'
import { useConfigurationStore } from '~/composables/useConfigurationStore'
import GridFilter from '@/components/GridFilter.vue'
import { ReportConfiguration } from '~/types'
import useGoBack from '~/composables/useGoBack'

export default defineComponent({
  components: {
    GridFilter,
  },
  setup() {
    useGoBack()

    const configStore = useConfigurationStore()
    const router = useRouter()
    const { $auth, error } = useContext()

    const headers = [
      {
        text: 'Id',
        value: 'id',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
      {
        text: 'Type',
        value: 'type',
        sortable: true,
        align: 'left',
        class: 'secundary',
      },
      {
        text: 'Title',
        value: 'title',
        sortable: true,
        align: 'left',
        class: 'secundary',
      },
      {
        text: 'Form Name',
        value: 'formName',
        sortable: true,
        align: 'left',
        class: 'secundary',
      },
      {
        text: 'Remarks Label Text',
        value: 'remarksLabelText',
        sortable: true,
        align: 'left',
        class: 'secundary',
      },
      {
        text: 'Defined CheckLists Qty',
        value: 'definedCheckLists',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
      {
        text: 'Defined Signatures Qty',
        value: 'definedSignatures',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
      {
        text: 'Used by (Reports)',
        value: 'usedByReports',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
      {
        text: 'Print Section',
        value: 'printSection',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
      {
        text: '',
        value: 'actions',
        sortable: true,
        align: 'center',
        class: 'secundary',
      },
    ]

    const componentState = reactive({
      dialogRemove: false,
      selectedItem: {} as ReportConfiguration,
      filter: '',
      loading: false,
    })

    useFetch(async () => {
      if (!$auth.user.isAdmin) {
        error({ statusCode: 403, message: 'Forbbiden' })
      }
      componentState.loading = true
      await configStore.getConfigurations('')
      componentState.loading = false
    })

    const deleteConfig = async () => {
      await configStore.deleteConfiguration(componentState.selectedItem.id)
    }

    const configs = computed((): ReportConfiguration[] => {
      return configStore.configurations || []
    })

    return {
      headers,
      configs,
      deleteConfig,
      componentState,
      router,
    }
  },
})
</script>
