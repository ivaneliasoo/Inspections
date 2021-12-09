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
          <v-tab-item>
            <v-card height="500">
              <ckeditor v-model="options.dataCKEditor" :editor="options.editor" />
            </v-card>
          </v-tab-item>
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
      <v-card height="555">
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
import { ref, defineComponent, reactive, computed, useStore, useFetch, useRoute, watch } from '@nuxtjs/composition-api'
import Vue from 'vue'
import CKEditor from '@ckeditor/ckeditor5-vue2';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { PrintSectionState } from 'store/printsection'
import { PrintSectionDTO } from '@/types/PrintSections/ViewModels/PrintSectionDTO'
import { PrintSection } from '~/types'
Vue.use(CKEditor)

export default defineComponent({
  setup () {
    const store = useStore()
    const route = useRoute()
    const dialogRemove = ref<boolean>(false)
    const loading = ref<boolean>(false)
    const filter: String = ''
    const selectedItem: PrintSection = {} as PrintSection
    const item = ref({ id: 0, code: '', description: '', content: '', isMainReport: true, status: 0 } as PrintSectionDTO)
    const options = reactive({
      dataCKEditor: '',
      editor: ClassicEditor
    })

    const printSections = computed((): PrintSectionDTO[] => {
      return ((store.state as any).licenses as PrintSectionState).printSectionsList
    })

    const getPrintSections = async (filter: string) => {
      await store.dispatch(
        'printsection/getPrintSections',
        { filter },
        { root: true }
      )
    }

    const deletePrintSection = () => {
      store
        .dispatch('printsection/deletePrintSection', selectedItem.id, { root: true })
        .then(() => {
        })
    }

    useFetch(async () => {
      await store.dispatch('printsection/getPrintSections', { filter }, { root: true })

      if (route.value.query.id) {
        store
          .dispatch('printsection/getPrintSectionsById', route.value.query.id, {
            root: true
          })
          .then(resp => (item.value = resp))
      }
    })

    const upsertPrintSection = async () => {
      loading.value = true
      if (item.value.id > 0) { await store.dispatch('printsection/updatePrintSection', selectedItem.id, { root: true }) } else {
        await store.dispatch('printsection/createPrintSection', item, { root: true })
        await store.dispatch('printsection/getPrintSections', filter, { root: true })
      }
      loading.value = false
    }

    watch(
      () => '',
      async (newValue: string) => {
        if (!newValue) {
          return
        }

        if (newValue.length >= 3) {
          await getPrintSections(newValue)
        }
      }
    )

    return {
      options,
      dialogRemove,
      loading,
      filter,
      selectedItem,
      item,
      printSections,
      deletePrintSection,
      upsertPrintSection,
      getPrintSections
    }
  }
})
</script>
<style>

</style>
