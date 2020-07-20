<template>
  <message-dialog v-model="value" :actions="['yes','cancel']" @yes="createReport" @cancel="$emit('input',false)">
      <template v-slot:title="{}">
        New Report
      </template>
      <ValidationObserver tag="form" ref="obs">
        <v-row>
          <v-col cols="12" xs="12" md="6">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select
                id="selReportType"
                v-model="newReport.reportType"
                :error-messages="errors"
                item-value="id"
                item-text="text"
                label="Report Type"
                :items="[{ id:0, text: 'Inspection' }]"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12" xs="12" md="6">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select
                id="selConfigurations"
                v-model="newReport.configurationId"
                :error-messages="errors"
                label="Select a Saved Configuration"
                item-value="id"
                item-text="title"
                :items="configurations"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-text-field
                id="txtName"
                v-model="newReport.name"
                :error-messages="errors"
                label="Report Name"
              />
            </ValidationProvider>
          </v-col>
        </v-row>
      </ValidationObserver>
    </message-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'nuxt-property-decorator'
import { ReportConfigurationState } from 'store/configurations'

import { CreateReport, ReportConfiguration} from '~/types'
import { ValidationObserver, ValidationProvider } from 'vee-validate'

@Component({
  components: {
    ValidationObserver, 
    ValidationProvider
  }
})
export default class NewReportDialog extends Vue {
  @Prop() value!: boolean
  $refs!: {
    obs: InstanceType<typeof ValidationObserver>
  }
  newReport:CreateReport = {} as CreateReport

  async fetch() {
    await this.$store.dispatch('configurations/getConfigurations', '', { root: true })
  }
  
  async createReport () {
      if(await this.$refs.obs.validate() === true)
      this.$store.dispatch('reportstrore/createReport', this.newReport, { root: true })
        .then((resp) => {
          this.$store.dispatch('reportstrore/getReports', '', { root: true })
          this.$emit('input', false)
          console.log(resp)
          this.$emit('report-created', resp)
        })
    }
  
  get configurations (): ReportConfiguration[] {
    return (this.$store.state.configurations as ReportConfigurationState).configurations
  }
}
</script>