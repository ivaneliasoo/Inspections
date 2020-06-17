<template>
  <div>
    <v-data-table :items="checks" item-key="id" dense :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>CheckLists</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <v-spacer />
          <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on">New CheckList</v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">New CheckLists</span>
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
                    </v-col>-->
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
                </v-btn>-->
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
import { CheckListsState } from 'store/checklists'
import { CheckList } from '../types'

@Component
export default class CheckListsPage extends Vue {
  dialog: boolean = false;
  headers: any[] = [
    {
      text: 'Id',
      value: 'id',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Text',
      value: 'text',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Annotation',
      value: 'annotation',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Child Items',
      value: 'totalItems',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Parameters',
      value: 'totalParams',
      sortable: true,
      align: 'center',
      class: 'secundary'
    }
  ];

  get checks (): CheckList[] {
    return (this.$store.state.checklists as CheckListsState)
      .checkLists
  }

  fetch ({ store }: any) {
    store.dispatch('checklists/getChecklists', '', { root: true })
  }
}
</script>

<style>
</style>