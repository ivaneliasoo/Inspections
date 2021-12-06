<template>
  <message-dialog v-model="value" :actions="[]">
      <template v-slot:title="{}">
        New Report
      </template>
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
      <v-row>
        <v-select
          v-model="configuration"
          :items="configurations"
          :label="!creatingReport ? 'Please, Select a Configuration to Create a New Report':'Selected Configuration'"
          item-value="id"
          item-text="title"
          :readonly="creatingReport"
          @change="createReport()"
        >
        <template #item="{ item }">
           <div two-line>
              <v-list-item-content>
                <v-list-item-title>  {{ item.title }}</v-list-item-title>
                <v-list-item-subtitle>Creates a new Report With {{ item.title }} {{item.formName}} Configuration</v-list-item-subtitle>
              </v-list-item-content>
            </div>
          </template>
          <template #selection="{ item }">
            {{ item.title }} - {{ item.formName }}
          </template>
        </v-select>
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
    this.creatingReport =true
    const reportId = await this.$store.dispatch('reportstrore/createReport', this.selectedConfiguration, { root: true })
      .then((resp) => {
        this.$store.dispatch('reportstrore/getReports', { filter: '', closed: this.$route.query.closed, orderBy: 'date', myreports: true, descending: true }, { root: true })
        this.$emit('input', false)
        this.$emit('report-created', resp)
      })
    await this.$store.dispatch('users/setUserLastEditedReport', { userName: this.$auth.user.userName, lastEditedReport: reportId }, { root: true })
    this.creatingReport = false
  }

  get configurations (): ReportConfiguration[] {
    let arrayConfigurations :ReportConfiguration[];
    arrayConfigurations = (this.$store.state.configurations as ReportConfigurationState).configurations;

    if(arrayConfigurations.length == 1){
       this.creatingReport =true
       this.configuration = arrayConfigurations[0].id;
       this.createReport();
    }

    return (arrayConfigurations)
  }

  get selectedConfiguration() {
    return { reportType: 0, configurationId: this.configuration }
  }
}
</script>

<style scoped>
::v-deep .v-card {
  cursor: pointer;
}
</style>
