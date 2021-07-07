<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove CheckList"
      message="This operation will remove this CheckList and all items and params (if not used)"
      :code="selectedItem.id"
      :description="selectedItem.text"
      @yes="removeCheckList(selectedItem)"
    />
    <message-dialog v-model="dialogItems" :actions="[]">
      <template #title="{}">
        {{ selectedItem.text }} Items
      </template>
      <template #subtitle="{}">
        <v-list
          subheader
          two-line
          flat
        >
          <v-subheader>Items &amp; Params</v-subheader>
          <v-list-item v-for="item in checkItems" :key="item.id">
            <template #default="{ active, toggle }">
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
    <v-data-table
      :class="$device.isTablet ? 'tablet-text':''"
      :items="checks"
      item-key="id"
      dense
      :search="filter.filterText"
      :loading="loading"
      :headers="headers"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>CheckLists</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="$router.push({ name: 'CheckLists-id', params: { id: -1 } })"
          >
            <v-icon dark>
              mdi-plus
            </v-icon>
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
      <template #item.actions="{ item }">
        <v-icon
          color="primary"
          class="mr-2"
          @click="selectItem(item); $router.push({ name: 'CheckLists-id', params: { id: selectedItem.id }}); dialog = true"
        >
          mdi-pencil
        </v-icon>
        <v-icon
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
import { Vue, Component, Watch, mixins } from 'nuxt-property-decorator'
import { CheckListsState } from 'store/checklists'
import { ReportConfigurationState } from '@/store/configurations'
import { ReportsState } from '@/store/reportstrore'
import { CheckList, CheckListItem, FilterType, ReportConfiguration, Report } from '@/types'
import InnerPageMixin from '@/mixins/innerpage'

@Component
export default class CheckListsPage extends mixins(InnerPageMixin) {
  dialog: Boolean = false
  dialogRemove: boolean = false
  dialogItems: Boolean = false
  loading: boolean = false
  filter: FilterType = {
    filterText: '',
    inConfigurationOnly: true,
    reportId: 1001,
    reportConfigurationId: 1
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
      align: 'left',
      class: 'secundary'
    },
    {
      text: 'Annotation',
      value: 'annotation',
      sortable: true,
      align: 'left',
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

  asyncData ({ query }:any) {
    const filter: FilterType = {
      filterText: '',
      inConfigurationOnly: query.configurationonly === 'true' ? true : false ?? true,
      reportId: query.reportid ? parseInt(query.reportid) : undefined,
      reportConfigurationId: query.configurationid ? parseInt(query.configurationid) : undefined
    }
    return {
      filter
    }
  }

  async fetch () {
    this.loading = true

    Promise.all([await this.$store.dispatch('reportstrore/getReports', '', { root: true }),
      await this.$store.dispatch('configurations/getConfigurations', '', { root: true }),
      await this.$store.dispatch('checklists/getChecklists', this.filter, { root: true })])

    this.loading = false
  }

  selectItem (item: CheckList): void {
    this.selectedItem = item
    this.loading = true
    this.$store.dispatch('checklists/getCheckListItemsById', this.selectedItem.id, { root: false })
      .finally(() => this.loading = false)
  }

  @Watch('filter', { deep: true })
  onFilterChanged (value: FilterType, oldValue: FilterType) {
    this.loading = true
    this.$store.dispatch('checklists/getChecklists', value, { root: true })
      .finally(() => { this.loading = false })
  }

  async removeCheckList (item: CheckList) {
    this.loading = true
    await this.$store.dispatch('checklists/deleteCheckList', { idCheckList: item.id }, { root: true })
    this.loading = false
  }
}
</script>

<style>
</style>
