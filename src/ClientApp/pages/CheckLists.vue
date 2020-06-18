<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove CheckList"
      message="This operation will remove this CheckList and all items and params (if not used)"
      :code="selectedItem.id"
      :description="selectedItem.text"
    />
    <message-dialog v-model="dialog" :actions="['yes','cancel']">
      <template v-slot:title="{}">
        New CheckLists
      </template>
      <v-row>
      </v-row>
    </message-dialog>
    <message-dialog v-model="dialogItems" :actions="[]">
      <template v-slot:title="{}">
        {{ selectedItem.text }} Items
      </template>
      <template v-slot:subtitle="{}">
        Items &amp; Params
      </template>
      <v-row>
      </v-row>
    </message-dialog>
    <v-data-table :items="checks" item-key="id" dense :search="filter" :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>CheckLists</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-btn color="primary" dark class="mb-2" @click="dialog=true">
            New CheckList
          </v-btn>
          
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
          small
          color="amber"
          class="mr-2"
          @click="selectItem(item); dialogItems = true"
        >
          mdi-format-list-checks
        </v-icon>
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="selectItem(item); dialog = true"
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          color="error"
          @click="selectItem(item); dialogRemove = true"
        >
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { CheckListsState } from 'store/checklists'
import { CheckList } from '../types'
import AlertDialog from '@/components/AlertDialog.vue'
import MessageDialog from '@/components/MessageDialog.vue'
import GridFilter from '@/components/GridFilter.vue'

@Component({
  components: {
    AlertDialog,
    MessageDialog,
    GridFilter
  }
})
export default class CheckListsPage extends Vue {
  dialog: Boolean = false
  dialogRemove: boolean = false
  dialogItems: Boolean = false
  filter: String = ''
  selectedItem: CheckList = {} as CheckList

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
    },
    {
      text: '',
      value: 'actions',
      sortable: false,
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

  selectItem (item: CheckList): void{
    this.selectedItem = item
  }
}
</script>

<style>
</style>
