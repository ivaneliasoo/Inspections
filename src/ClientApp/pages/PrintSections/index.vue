<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Reports"
      message="This operation will remove this print section and all data related"
      :code="selectedItem.id"
      :description="selectedItem.code"
      @yes="deletePrintSection();"
    />
    <v-data-table
      :class="$device.isTablet ? 'tablet-text':''"
      :items="printSections"
      item-key="id"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Print Sections</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="item = { principal: false }; dialog = true"
          >
            <v-icon dark>
              mdi-plus
            </v-icon>
          </v-btn>
          <v-dialog
            v-model="dialog"
            persistent
            scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
          >
            <ValidationObserver v-slot="{ reset }" tag="form">
              <v-card>
                <v-card-title>
                  <span class="headline">Edit Print Section</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="6">
                        <ValidationProvider v-slot="{ errors }" rules="required">
                          <v-text-field
                            v-model="item.code"
                            name="code"
                            :error-messages="errors"
                            label="Code"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <v-text-field
                          v-model="item.content"
                          name="content"
                          label="Content"
                        />
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="4">
                        <v-checkbox
                          v-model="item.isMainReport"
                          name="isMainReport"
                          label="Is Main Report"
                        />
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn
                    color="success"
                    text
                    @click="upsertPrintSection()"
                  >
                    Save
                  </v-btn>
                  <v-btn color="default" text @click="reset(); item = { isMainReport: false }; dialog = false">
                    Cancel
                  </v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template #item.actions="{ item }">
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
import { Component, Watch, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { PrintSectionState } from 'store/printsection'
import InnerPageMixin from '@/mixins/innerpage'
import { PrintSectionDTO } from '@/types/PrintSections/ViewModels/PrintSectionDTO'
import { FilterType, PrintSection } from '~/types'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class PrintSectionsPage extends mixins(InnerPageMixin) {
  dialog: boolean = false
  dialogRemove: boolean = false
  loading: boolean = false
  filter: FilterType = {
    filterText: '',
    inConfigurationOnly: undefined,
    reportId: undefined,
    reportConfigurationId: undefined
  }

  headers: any[] = [
    {
      text: 'Id',
      value: 'id',
      sortable: true,
      align: 'center'
    },
    {
      text: 'Code',
      value: 'code',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Principal',
      value: 'isMainReport',
      sortable: true,
      align: 'center'
    },
    {
      text: '',
      value: 'actions',
      sortable: false,
      align: 'center'
    }
  ]

  selectedItem: PrintSection = {} as PrintSection
  item: any = { principal: false }

  get printsections (): PrintSectionDTO[] {
    return (this.$store.state.printsections as PrintSectionState)
      .printSectionsList
  }

  selectItem (item: PrintSection): void {
    this.selectedItem = item
    this.$store.dispatch('printsections/getPrintSectionById', this.selectedItem.id, { root: true })
      .then((resp) => { this.item = resp })
  }

  asyncData () {
    const filter: FilterType = {
      filterText: '',
      inConfigurationOnly: undefined,
      reportId: undefined,
      reportConfigurationId: undefined
    }
    return {
      filter
    }
  }

  async fetch () {
    this.loading = true
    await Promise.all([this.$store.dispatch('printsections/getPrintSections', this.filter, { root: true })])
    this.loading = false
  }

  @Watch('filter', { deep: true })
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  async onFilterChanged (value: FilterType) {
    await this.$store.dispatch('printsections/getPrintSections', value, { root: true })
  }

  deletePrintSection () {
    this.$store.dispatch('printsections/deletePrintSection', this.selectedItem.id, { root: true })
      .then(() => {
        this.dialog = false
      })
  }

  async upsertPrintSection () {
    if (this.item.id > 0) { await this.$store.dispatch('printsections/updatePrintSection', this.item, { root: true }) } else {
      await this.$store.dispatch('printsections/createPrintSection', this.item, { root: true })
      await this.$store.dispatch('printsections/getPrintSections', {}, { root: true })
    }
    this.dialog = false
  }
}
</script>

<style>

</style>
