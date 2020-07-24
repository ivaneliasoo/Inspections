
<template>
  <v-row>
    <new-report-dialog v-model="dialog" @report-created="goToNewReport($event)" />
    <v-col cols="12" lg="3" md="3" v-for="(option, index) in cardOptions" :key="index">
      <v-card min-height="250" min-width="200" class="pt-10" :ripple="option.innerActions ? false:true" @click="option.innerActions ? 0:onCardClick(option)">
        <v-btn v-if="!option.innerActions" fab :class="option.color" dark>
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
        <v-card-actions>
          <v-row>
            <v-col cols="6" v-for="(action, index) in option.innerActions" :key="index">
              <v-btn :color="action.color" tile :block="$vuetify.breakpoint.smAndDown" @click.stop="action.action()">
                <v-icon>
                  {{ action.icon }}
                </v-icon>
                {{ action.text }}
              </v-btn>
            </v-col>
          </v-row>
        </v-card-actions>
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
      helpText: 'Click one of the Options bellow',
      icon: 'mdi-pencil',
      color: 'accent',
      path: false,
      innerActions: [
        {
          text: 'Edit Last',
          color: 'primary',
          action: () => {
            if (this.$auth.user.lastEditedReport)
              window.$nuxt.$router.push(`/reports/${this.$auth.user.lastEditedReport}`) 
          }
        },
        {
          text: 'List All',
          color: 'primary',
          action: () => {
            window.$nuxt.$router.push(`/reports`) 
          }
        }
      ]
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
      path: '/configurations?configurationonly=true'
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