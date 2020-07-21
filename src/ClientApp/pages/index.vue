
<template>
  <v-row>
    <new-report-dialog v-model="dialog" @report-created="goToNewReport($event)" />
    <v-col cols="12" lg="3" md="3" v-for="(option, index) in cardOptions" :key="index">
      <v-card min-height="250" min-width="200" max-height="250" class="pt-10" @click="onCardClick(option)">
        <v-btn fab :class="option.color" dark>
          <v-icon>
            {{ option.icon }}
          </v-icon>
        </v-btn>
        <v-card-title class="text-center">
          <v-row>
            <v-col class="text-center">
              {{ option.text }}
            </v-col>
          </v-row>
        </v-card-title>
        <v-card-subtitle>
          <v-row>
            <v-col class="text-center">
              {{ option.helpText }}
            </v-col>
          </v-row>
        </v-card-subtitle>
      </v-card>
    </v-col>
  </v-row>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import NewReportDialog from '@/components/NewReportDialog.vue'


@Component({
  layout: 'default',
  components: {
    NewReportDialog
  }
})
export default class IndexPage extends Vue{
  dialog: boolean = false
  cardOptions: any[] = [
    {
      text: 'New Report',
      helpText: 'Creates a configured and empty report',
      icon: 'mdi-plus',
      color: 'accent',
      path: '',
      action(self: any) { self.dialog=false; self.dialog = true } 
    },
    {
      text: 'Edit Report',
      helpText: 'Edit or remove a previously created report from a list and optionally allows to go directly to the last created or edited report by current user',
      icon: 'mdi-pencil',
      color: 'accent',
      path: () => { 
        if (this.$auth.user.lastEditedReport)
          if(confirm('Do you wanna edit the last edited report?'))
            return `/reports/${this.$auth.user.lastEditedReport}` 
          else return `/reports`
        else
          return `/reports`
      }
    },
    {
      text: 'View or Export Report',
      helpText: 'Allows to View CLOSED Reports and Photo Records and Export in PDF formats',
      icon: 'mdi-file-chart',
      color: 'accent',
      path: `/reports?closed=true`
    },
    {
      text: 'Reports Settings',
      helpText: 'Allows to Configure reports check lists, signatures, titles and names',
      icon: 'mdi-cog-outline',
      color: 'accent',
      path: '/configurations'
    }
  ]

  onCardClick(option: any) {
    let path = typeof option.path === 'function' ? option.path(): option.path
    if(path)
      this.$router.push(path)
    else
      option.action(this)
  }
   goToNewReport(event: any){
     this.$router.push(`/reports/${event}`)
   }
}
</script>