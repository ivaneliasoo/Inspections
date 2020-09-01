
<template>
  <v-row>
    <new-report-dialog v-model="dialog" @report-created="goToNewReport($event)" />
    <OptionsCards :options="cardOptions" />
  </v-row>
</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { CardOption } from '~/types'

@Component({
  layout: 'default'
})
export default class IndexPage extends Vue{
  dialog: boolean = false
  self = this
  cardOptions: CardOption[] = [
    {
      name:'new',
      text: 'New Report',
      helpText: 'Creates a configured and empty report',
      icon: 'mdi-plus',
      color: 'accent',
      path: '',
      action: () => this.createReport()
    },
    {
      name: 'edit',
      text: 'Edit Report',
      helpText: 'Click one of the Options bellow',
      icon: 'mdi-pencil',
      color: 'accent',
      path: '/reports'
    },
    {
      name: 'view',
      text: 'View or Export Report',
      helpText: 'Allows to View CLOSED Reports and Photo Records and Export in PDF formats',
      icon: 'mdi-file-chart',
      color: 'accent',
      path: `/reports?closed=true`
    }
  ]

   goToNewReport(event: any){
     this.$router.push(`/reports/${event}`)
   }

   createReport() {
     this.dialog=false; this.dialog = true
   }
}
</script>