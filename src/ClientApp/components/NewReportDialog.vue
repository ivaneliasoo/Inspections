<template>
  <message-dialog v-model="localValue" :actions="[]">
      <template v-slot:title="{}">
        New Report
      </template>
      <div v-if="!creatingReport" class="tw-fixed tw-z-50 tw-bg-opacity-80 tw-bg-white">
        <h2 >Please, Select a Configuration to Create a New Report</h2>
      </div>
      <v-row
        v-if="creatingReport"
        class="fill-height"
        align-content="center"
        justify="center">
        <v-col>
            <v-col
              class="subtitle-1 text-center"
              cols="12"
            >
              Creating a New report. Please Wait
            </v-col>
            <v-col cols="12">
              <v-progress-linear
                color="indigo"
                indeterminate
                rounded
                height="6"
              ></v-progress-linear>
            </v-col>
        </v-col>
      </v-row>

        <v-row justify="center" align="center" v-for="item in configurations"
              :key="item.id">
          <v-col>
            <v-card class="tw-mx-5" ripple @click="configuration = item.id; createReport()">
              <v-card-title>{{ item.title }}</v-card-title>
              <v-card-subtitle>Creates a new Report With {{ item.title }} {{item.formName}} Configuration</v-card-subtitle>
            </v-card>
          </v-col>
        </v-row>
    </message-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'nuxt-property-decorator'
import { ReportConfigurationState } from 'store/configurations'

import { CreateReport, ReportConfiguration, CardOption} from '~/types'
import { ValidationObserver, ValidationProvider } from 'vee-validate'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class NewReportDialog extends Vue {
  @Prop() value!: boolean
  creatingReport: boolean = false
  search: string = ''
  configuration: number = 0
  async fetch() {
    await this.$store.dispatch('configurations/getConfigurations', '', { root: true })
  }

  async createReport () {
    try {
      this.creatingReport =true
    const reportId = await this.$store.dispatch('reportstrore/createReport', this.selectedConfiguration, { root: true })
      .then((resp) => {
        this.$store.dispatch('reportstrore/getReports', { filter: '', closed: this.$route.query.closed, orderBy: 'date', myreports: true, descending: true }, { root: true })
        this.localValue = false
        this.$emit('report-created', resp)
      })
    await this.$store.dispatch('users/setUserLastEditedReport', { userName: this.$auth.user.userName, lastEditedReport: reportId }, { root: true })
    } catch (error) {
      console.log({ error })
    } finally {
      this.creatingReport = false
    }

  }

  get configurations (): ReportConfiguration[] {
    return (this.$store.state.configurations as ReportConfigurationState).configurations
  }

  get selectedConfiguration() {
    return { reportType: 0, configurationId: this.configuration }
  }

  get localValue() {
    return this.value
  }
  set localValue(value: boolean) {
    this.$emit('input', value)
  }

}
</script>

<style scoped>
::v-deep .v-card {
  cursor: pointer;
}
</style>
