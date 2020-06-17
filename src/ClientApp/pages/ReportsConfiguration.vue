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
                    <!-- <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.name" label="Dessert name" />
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.calories" label="Calories" />
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.fat" label="Fat (g)" />
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.carbs" label="Carbs (g)" />
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.protein" label="Protein (g)" />
                    </v-col> -->
                  </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions>
                <v-spacer />
                <!-- <v-btn color="blue darken-1" text @click="close">
                  Cancel
                </v-btn>
                <v-btn color="blue darken-1" text @click="save">
                  Save
                </v-btn> -->
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
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
