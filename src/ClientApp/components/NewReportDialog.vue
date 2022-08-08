<template>
  <message-dialog v-if="configurations" v-model="localValue" :actions="[]">
    <template #title="{}"> New Report </template>
    <div
      v-if="!componentState.creatingReport"
      class="tw-fixed tw-z-50 tw-bg-opacity-80 tw-bg-white"
    >
      <h2>Please, Select a Configuration to Create a New Report</h2>
    </div>
    <v-row
      v-if="componentState.creatingReport"
      class="fill-height"
      align-content="center"
      justify="center"
    >
      <v-col>
        <v-col class="subtitle-1 text-center" cols="12">
          Creating a New report. Please Wait
        </v-col>
        <v-col cols="12">
          <v-progress-linear color="indigo" indeterminate rounded height="6" />
        </v-col>
      </v-col>
    </v-row>

    <div v-if="configurations && configurations.length > 0">
      <v-row
        v-for="(item, index) in configurations"
        :key="`reportconfig${index}`"
        justify="center"
        align="center"
      >
        <v-col v-if="item">
          <v-card
            class="tw-mx-5"
            ripple
            @click="
              componentState.configuration = item.id
              createReport()
            "
          >
            <v-card-title>{{ item.title }}</v-card-title>
            <v-card-subtitle>
              Creates a new Report With {{ item.title }}
              {{ item.formName }} Configuration
            </v-card-subtitle>
          </v-card>
        </v-col>
      </v-row>
    </div>
  </message-dialog>
</template>

<script lang="ts">
import {
  defineComponent,
  reactive,
  computed,
  useFetch,
  useRoute,
  useContext,
} from '@nuxtjs/composition-api'
import { useConfigurationStore } from '~/composables/useConfigurationStore'
import { useReportsStore } from '~/composables/useReportsStore'
import { useUsersStore } from '~/composables/useUsersStore'

import { ReportConfiguration } from '~/types'

export default defineComponent({
  props: {
    value: {
      type: Boolean,
      default: false,
    },
  },
  setup(props, { emit }) {
    const componentState = reactive({
      creatingReport: false,
      search: '',
      configuration: 0,
    })

    const configurationStore = useConfigurationStore()
    const reportsStore = useReportsStore()
    const usersStore = useUsersStore()
    useFetch(() => configurationStore.getConfigurations(''))
    const route = useRoute()
    const { $auth } = useContext()

    const selectedConfiguration = computed(() => {
      return { reportType: 0, configurationId: componentState.configuration }
    })

    const localValue = computed({
      get: () => {
        return props.value
      },
      set: (value: boolean) => {
        emit('input', value)
      },
    })

    const createReport = async () => {
      try {
        componentState.creatingReport = true
        const reportId = (await reportsStore
          .createReport(selectedConfiguration.value)
          .then((resp) => {
            reportsStore.getReports({
              filter: '',
              closed: route.value.query.closed,
              orderBy: 'date',
              myreports: true,
              descending: true,
            })
            localValue.value = false
            emit('report-created', resp)
          })) as number
        await usersStore.setUserLastEditedReport({
          userName: $auth.user.userName as string,
          lastEditedReport: reportId,
        })
      } catch (error) {
        // eslint-disable-next-line no-console
        console.log({ error })
      } finally {
        componentState.creatingReport = false
      }
    }

    const configurations = computed((): ReportConfiguration[] => {
      const all = configurationStore.configurations.filter(
        (c) => c.formName !== 'Incidents Report'
      )
      const incident = configurationStore.configurations.filter(
        (c) => c.formName === 'Incidents Report'
      )
      all.unshift(incident[0])
      return all || []
    })

    return {
      componentState,
      createReport,
      configurations,
      localValue,
    }
  },
})
</script>

<style scoped>
:deep(.v-card) {
  cursor: pointer;
}
</style>
