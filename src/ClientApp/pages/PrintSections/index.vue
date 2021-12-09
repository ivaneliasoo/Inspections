<template>
  <v-row>
    <v-col cols="12" md="8" sm="12" class="mb-2">
      <h1 class="font-weight-bold mb-3">
        Print Section Detail
      </h1>

      <v-card>
        <v-tabs
          color="deep-indigo accent-4"
          left
        >
          <v-tab>Beginner Mode</v-tab>
          <v-tab>Advanced Mode</v-tab>
          <v-tab-item />
          <v-tab-item>
            <v-container fluid>
              <v-row class="pl-4">
                <v-col
                  cols="12"
                  sm="6"
                  md="4"
                >
                  <v-text-field
                    v-model="selectedItem.code"
                    type="text"
                    label="Code"
                  />
                </v-col>
              </v-row>
              <v-row class="pl-4">
                <v-col
                  cols="12"
                  sm="6"
                  md="12"
                >
                  <v-text-field
                    v-model="selectedItem.description"
                    type="text"
                    label="Description"
                  />
                </v-col>
              </v-row>
              <v-row class="pl-4">
                <v-col
                  cols="12"
                  sm="6"
                  md="12"
                >
                  <v-textarea
                    v-model="selectedItem.content"
                    type="text"
                    label="Content"
                  />
                </v-col>
              </v-row>
            </v-container>
          </v-tab-item>
        </v-tabs>
      </v-card>
    </v-col>
    <v-col cols="12" md="4" sm="12" class="mt-8">
      <v-card>
        <v-toolbar
          color="indigo"
          dark
        >
          <v-toolbar-title>Print Sections</v-toolbar-title>

          <v-spacer />
          <v-col cols="12" md="6" sm="12" class="mt-3">
            <v-text-field
              v-model="filter"
              type="text"
            />
          </v-col>
          <v-btn icon>
            <v-icon>mdi-magnify</v-icon>
          </v-btn>
          <v-btn icon>
            <v-icon>mdi-plus</v-icon>
          </v-btn>
        </v-toolbar>
        <v-list two-line>
          <v-list-item-group
            active-class="indigo--text"
          >
            <template v-for="(item, index) in printSections">
              <v-list-item :key="item.code">
                <v-list-item-content @click="selectedItem = item">
                  <v-list-item-subtitle
                    class="text--primary text-left"
                    v-text="item.code"
                  />
                  <v-list-item-subtitle class="text-left" v-text="item.description" />
                </v-list-item-content>
                <v-list-item-action>
                  <v-list-item-action-text v-text="'Is Main Report'" />
                  <v-icon
                    v-if="!item.isMainReport"
                    class="mr-6"
                    color="grey lighten-1"
                  >
                    mdi-key-star
                  </v-icon>
                  <v-icon
                    v-else
                    class="mr-6"
                    color="green darken-3"
                  >
                    mdi-key-star
                  </v-icon>
                </v-list-item-action>
                <v-list-item-action>
                  <v-list-item-action-text v-text="'Embed'" />
                  <v-icon
                    color="default darken-3"
                  >
                    mdi-gesture-tap
                  </v-icon>
                </v-list-item-action>
              </v-list-item>

              <v-divider
                v-if="index < printSections.length - 1"
                :key="item.id"
              />
            </template>
          </v-list-item-group>
        </v-list>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Watch, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { PrintSectionState } from 'store/printsection'
import InnerPageMixin from '@/mixins/innerpage'
import { PrintSectionDTO } from '@/types/PrintSections/ViewModels/PrintSectionDTO'
import { PrintSection } from '~/types'
import EnergyReport from '~/components/EnergyReport.vue'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider,
    EnergyReport
  }
})
export default class PrintSectionsPage extends mixins(InnerPageMixin) {
  dialog: boolean = false
  dialogRemove: boolean = false
  loading: boolean = false
  filter: String = ''

  selectedItem: PrintSection = {} as PrintSection
  item: any = { principal: false }

  get printSections (): PrintSectionDTO[] {
    return (this.$store.state.printsection as PrintSectionState)
      .printSectionsList
  }

  selectItem (item: PrintSection): void {
    this.selectedItem = item
    this.$store.dispatch('printsection/getPrintSectionById', this.selectedItem.id, { root: true })
      .then((resp) => { this.item = resp })
  }

  asyncData () {
    const filter: String = ''
    return {
      filter
    }
  }

  async fetch () {
    this.loading = true
    await Promise.all([this.$store.dispatch('printsection/getPrintSections', this.filter, { root: true })])
    this.loading = false
  }

  @Watch('filter', { deep: true })
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  async onFilterChanged (value: String) {
    await this.$store.dispatch('printsection/getPrintSections', value, { root: true })
  }

  deletePrintSection () {
    this.$store.dispatch('printsection/deletePrintSection', this.selectedItem.id, { root: true })
      .then(() => {
        this.dialog = false
      })
  }

  async upsertPrintSection () {
    if (this.item.id > 0) { await this.$store.dispatch('printsection/updatePrintSection', this.item, { root: true }) } else {
      await this.$store.dispatch('printsection/createPrintSection', this.item, { root: true })
      await this.$store.dispatch('printsection/getPrintSections', {}, { root: true })
    }
    this.dialog = false
  }
}
</script>

<style>

</style>
