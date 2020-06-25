<template>
  <div>
    <v-data-table
      :items="configs"
      item-key="id"
      :headers="headers"
      dense
    >
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Manage Reports Configurations</v-toolbar-title>
          <v-divider
            class="mx-4"
            inset
            vertical
          />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                color="primary"
                dark
                class="mb-2"
                v-bind="attrs"
                v-on="on"
              >
                New Report Configuration
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">New Report Configuration</span>
              </v-card-title>
              <v-card-text>
                <v-container>
                  <v-row>
                  </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions>
                <v-spacer />
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{}">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click=""
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          color="error"
          @click=""
        >
          mdi-delete
        </v-icon>
      </template>
      <template v-slot:item.type="{ item }">
        {{ item.type === 0 ? 'Inspection':'Unkown' }}
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { ReportConfigurationState } from 'store/configurations'
import { ReportConfiguration } from '~/types'

@Component
export default class ReportsConfigurationPage extends Vue {
  dialog: boolean =false
  filter: string = ''
  headers: any[] = [
    {
      text: 'Id',
      value: 'id',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Type',
      value: 'type',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Title',
      value: 'title',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Form Name',
      value: 'formName',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Remarks Label Text',
      value: 'remarksLabelText',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Defined CheckLists Qty',
      value: 'definedCheckLists',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Defined Signatures Qty',
      value: 'definedSignatures',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Used by (Reports)',
      value: 'usedByReports',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: '',
      value: 'actions',
      sortable: true,
      align: 'center',
      class: 'secundary'
    }
  ]

  get configs (): ReportConfiguration[] {
    return (this.$store.state.configurations as ReportConfigurationState).configurations
  }

  fetch ({ store }: any) {
    store.dispatch('configurations/getConfigurations', '', { root: true })
  }
}
</script>

<style>

</style>
