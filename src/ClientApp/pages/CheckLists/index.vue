<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove CheckList"
      message="This operation will remove this CheckList and all items and params (if not used)"
      :code="selectedItem.id"
      :description="selectedItem.text"
    />
    <message-dialog v-model="dialogItems" :actions="[]">
      <template v-slot:title="{}">
        {{ selectedItem.text }} Items
      </template>
      <template v-slot:subtitle="{}">
        <v-list
          subheader
          two-line
          flat
        >
          <v-subheader>Items &amp; Params</v-subheader>
          <v-list-item v-for="item in checkItems" :key="item.id">
            <template v-slot:default="{ active, toggle }">
              <v-list-item-action>
                <v-checkbox
                  color="primary"
                  @click="toggle"
                />
              </v-list-item-action>

              <v-list-item-content>
                <v-list-item-title>{{ item.text }}</v-list-item-title>
                <v-list-item-subtitle>Required {{ item.required }}</v-list-item-subtitle>
              </v-list-item-content>
            </template>
          </v-list-item>
        </v-list>
      </template>
    </message-dialog>
    <v-data-table :items="checks" item-key="id" dense :search="filter.filterText" :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>CheckLists</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn color="primary" dark class="mb-2" @click="$router.push({ name: 'CheckLists-id', params: { id: -1 }})">
            New CheckList
          </v-btn>
        </v-toolbar>
        <v-row justify="space-around" class="ml-2 mr-2">
          <v-col cols="12" md="5">
            <v-autocomplete
              v-model="filter.reportId"
              label="In Report"
              :items="reports"
              item-text="name"
              item-value="id"
              clearable
            />
          </v-col>
          <v-col cols="12" :md="filter.inConfigurationOnly ? 2:7">
            <v-switch v-model="filter.inConfigurationOnly" label="In Configuration" />
          </v-col>
          <v-col v-if="filter.inConfigurationOnly" cols="12" md="5">
            <v-autocomplete
              v-model="filter.reportConfigurationId"
              label="In Report Configuration"
              :items="configurations"
              item-text="title"
              item-value="id"
              clearable
            />
          </v-col>
        </v-row>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="selectItem(item); dialogItems = true"
        >
          mdi-format-list-checks
        </v-icon>
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="selectItem(item); $router.push({ name: 'CheckLists-id', params: { id: selectedItem.id }}); dialog = true"
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
import { Vue, Component, Watch } from 'nuxt-property-decorator'
import { CheckListsState } from 'store/checklists'
import { ReportConfigurationState } from '@/store/configurations'
import { ReportsState } from '@/store/reportstrore'
import { CheckList, CheckListItem, FilterType, ReportConfiguration, Report } from '@/types'
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
  filter: FilterType = {
    filterText: '',
    inConfigurationOnly: undefined,
    repotId: undefined,
    reportConfigurationId: undefined
  }

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

  get reports (): Report[] {
    return (this.$store.state.reportstrore as ReportsState)
      .reportList
  }

  get configurations (): ReportConfiguration[] {
    return (this.$store.state.configurations as ReportConfigurationState)
      .configurations
  }

  get checkItems (): CheckListItem[] {
    return (this.$store.state.checklists as CheckListsState)
      .currentCheckList.checks
  }

  async fetch () {
    await this.$store.dispatch('reportstrore/getReports', '', { root: true })
    await this.$store.dispatch('configurations/getConfigurations', '', { root: true })
    await this.$store.dispatch('checklists/getChecklists', {}, { root: true })
  }

  selectItem (item: CheckList): void{
    this.selectedItem = item
    this.$store.dispatch('checklists/getCheckListItemsById', this.selectedItem.id, { root: false })
  }

  @Watch('filter', { deep: true })
  onFilterChanged (value: FilterType, oldValue: FilterType) {
    this.$store.dispatch('checklists/getChecklists', value, { root: true })
  }
}
</script>

<style>
</style>
