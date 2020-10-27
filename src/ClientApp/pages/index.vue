
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
          <v-data-table :headers="licensesHeader" :item-class="() => 'expiring-row'" @dblclick:row="goToLicenses" item-key="licenseId" dense :items="expiring">
            <template v-slot:item.validityStart="{ item }">
              {{ parseDate(item.validityStart) }}
            </template>
            <template v-slot:item.validityEnd="{ item }">
              {{ parseDate(item.validityEnd) }}
            </template>
          </v-data-table>
        </v-card>
      </v-col>
      <v-col cols="12" md="6" sm="12">
        <v-card>
          <v-card-title class="justify-center">
            <h3>
              Expired Licenses
            </h3>
          </v-card-title>
          <v-data-table :headers="licensesHeader" :item-class="() => 'expired-row'" item-key="licenseId" dense :items="expired" @dblclick:row="goToLicenses" >
            <template v-slot:item.validityStart="{ item }">
              {{ parseDate(item.validityStart) }}
            </template>
            <template v-slot:item.validityEnd="{ item }">
              {{ parseDate(item.validityEnd) }}
            </template>
          </v-data-table>
        </v-card>
      </v-col>
  </v-row>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { CardOption } from '~/types'
import { LicensesState } from "store/licenses"
import { DateTime } from 'luxon'

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
  licensesHeader = [
     {
      text: "ID",
      value: "licenseId",
      sortable: true,
      align: "left",
    },
    {
      text: "License",
      value: "number",
      sortable: true,
      align: "left",
    },
    {
      text: "Valid From",
      value: "validityStart",
      sortable: true,
      align: "left",
    },
    {
      text: "Valid To",
      value: "validityEnd",
      sortable: true,
      align: "left",
    }
  ]
  async asyncData({ store }: any) {
    await store.dispatch('licenses/getLicensesDashboard', null, { root: true })
  }

   goToNewReport(event: any){
     this.$router.push(`/reports/${event}`)
   }

   createReport() {
     this.dialog=false; this.dialog = true
   }

   get expiring() {
     return (this.$store.state.licenses as LicensesState).dashboard.expiring
   }

   get expired() {
     return (this.$store.state.licenses as LicensesState).dashboard.expired
   }

   parseDate(date: string) {
     return DateTime.fromISO(date).toLocaleString()
   }

   goToLicenses(event, row) {
    this.$router.push(`/licenses?id=${row.item.licenseId}`)
   }
}
</script>

<style lang="scss" scoped>
::v-deep .expired-row {
  color: #C62828 !important;
  background-color: rgb(255, 242, 242) !important;
}
::v-deep .expiring-row {
  color: #FF8F00 !important;
  background-color: rgb(255, 242, 242) !important;
}
</style>