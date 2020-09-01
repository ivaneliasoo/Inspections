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
      <h2 v-if="!creatingReport">Please, Select a Configuration to Create a New Report</h2>
      <v-data-iterator
        v-if="!creatingReport"
        :items="configurations"
        :search="search"
        :disable-pagination="configurations.length<=3"
        :hide-default-footer="configurations.length<=3"
      >
        <template v-slot:default="props">
          <v-row>
            <v-col
              v-for="item in props.items"
              :key="item.id"
              cols="12"
              sm="6"
              md="4"
              lg="3"
            >
              <v-card ripple @click="configuration = item.id; createReport()">
                <v-card-title class="text-center">
                  <v-row>
                    <v-col class="text-center">
                      {{ item.title }}
                    </v-col>
                  </v-row>
                </v-card-title>
                <v-card-subtitle>
                  <v-row>
                    <v-col class="text-center">
                      Creates a new Report With {{ item.title }} Configuration
                    </v-col>
                  </v-row>
                </v-card-subtitle>
              </v-card>
            </v-col>
          </v-row>
        </template>
      </v-data-iterator>
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
        this.$store.dispatch('reportstrore/getReports', '', { root: true })
        this.$emit('input', false)
        this.$emit('report-created', resp)
      })
    await this.$store.dispatch('users/setUserLastEditedReport', { userName: this.$auth.user.userName, lastEditedReport: reportId }, { root: true })
    this.creatingReport = false
  }
  
  get configurations (): ReportConfiguration[] {
    return (this.$store.state.configurations as ReportConfigurationState).configurations
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