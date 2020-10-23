
<template>
  <div>
    <v-row>
      <new-report-dialog v-model="dialog" @report-created="goToNewReport($event)" />
      <OptionsCards :options="cardOptions" />
    </v-row>
    <v-row>
      <v-col cols="12" md="6" sm="12">
        <v-card>
          <v-card-title class="justify-center">
            <h3>Licenses Expiring Soon</h3>
          </v-card-title>
          <v-data-table :headers="[]" />
        </v-card>
      </v-col>
      <v-col cols="12" md="6" sm="12">
        <v-card>
          <v-card-title class="justify-center">
            <h3>
              Expired Licenses
            </h3>
          </v-card-title>
          <v-data-table :headers="[]" />
        </v-card>
      </v-col>
  </v-row>
  </div>
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