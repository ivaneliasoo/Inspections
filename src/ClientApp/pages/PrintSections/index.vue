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
          <v-tab v-if="!isNew">Beginner Mode</v-tab>
          <v-tab>Advanced Mode</v-tab>
          <v-tab-item v-if="!isNew">
            <v-card height="450" style="overflow-y: scroll">
              <draggable>
                <ckeditor v-model="dataCKEditor" :editor="options.editor" />
              </draggable>
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
                    v-model="printSection.code"
                    type="text"
                    label="Code"
                  />
                </v-col>
                <v-col cols="2">
                  <v-switch v-model="printSection.isMainReport" label="Is Main Report" />
                </v-col>
              </v-row>
              <v-row class="pl-4">
                <v-col
                  cols="12"
                  sm="6"
                  md="12"
                >
                  <v-text-field
                    v-model="printSection.description"
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
                    v-model="dataCKEditor"
                    type="text"
                    label="Content"
                  />
                </v-col>
              </v-row>
            </v-container>
          </v-tab-item>
        </v-tabs>
        <v-card-actions>
          <v-spacer />
          <v-btn
            color="success"
            text
            @click="upsertPrintSection()"
          >
            Save
          </v-btn>
          <v-btn
            color="default"
            text
          >
            Cancel
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-col>
    <v-col cols="12" md="4" sm="12" class="mt-8">
      <v-card height="555" style="overflow-y: scroll">
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
          <v-btn icon @click="getPrintSections(filter)">
            <v-icon>mdi-magnify</v-icon>
          </v-btn>
          <v-btn icon @click="isNew=true;reset(); dataCKEditor=''">
            <v-icon>mdi-plus</v-icon>
          </v-btn>
        </v-toolbar>
        <v-list two-line>
          <v-list-item-group
            active-class="indigo--text"
          >
            <draggable v-model="printSections">
              <template v-for="(item, index) in printSections">
                <v-list-item :key="item.code">
                  <v-list-item-content @click="selectPrintSection(item); dataCKEditor = item.content;isNew = false">
                    <v-list-item-subtitle
                      class="text--primary text-left font-weight-bold"
                      v-text="'#' + item.code"
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
                </v-list-item>

                <v-divider
                  v-if="index < printSections.length - 1"
                  :key="item.id"
                />
              </template>
            </draggable>
          </v-list-item-group>
        </v-list>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { ref, defineComponent, reactive, computed, useStore, useFetch, watch } from '@nuxtjs/composition-api'
import Vue from 'vue'
import CKEditor from '@ckeditor/ckeditor5-vue2'
import draggable from 'vuedraggable'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { PrintSectionState } from '~/store/printsection'
import { PrintSectionDTO } from '@/types/PrintSections/ViewModels/PrintSectionDTO'
Vue.use(CKEditor)
export default defineComponent({
  components: {
    draggable
  },
  setup () {
    const store = useStore()
    const dialogRemove = ref<boolean>(false)
    const loading = ref<boolean>(false)
    const isNew = ref<boolean>(false)
    const filter = ref<String>('')
    const dataCKEditor = ref<String>('')
    const printSection = ref({ id: 0, code: '', description: '', content: '', isMainReport: true, status: 0 } as PrintSectionDTO)
    const options = reactive({
      editor: ClassicEditor
    })

    const selectPrintSection = (item: PrintSectionDTO): void => {
      store.dispatch('printsection/getPrintSectionById', item.id, { root: true })
        .then(resp => (printSection.value = resp))
    }

    const printSections = computed((): PrintSectionDTO[] => {
      return ((store.state as any).printsection as PrintSectionState).printSectionsList
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
        .dispatch('printsection/deletePrintSection', printSection.value.id, { root: true })
        .then(() => {
        })
    }

    const reset = () => {
      printSection.value = { id: 0, code: '', description: '', content: '', isMainReport: true, status: 0 }
    }

    const upsertPrintSection = async () => {
      loading.value = true
      printSection.value.content = dataCKEditor.value.toString()
      if (printSection.value.id > 0) { await store.dispatch('printsection/updatePrintSection', printSection.value, { root: true }) } else {
        await store.dispatch('printsection/createPrintSection', printSection.value, { root: true })
        await store.dispatch('printsection/getPrintSections', filter.value, { root: true })
      }
      loading.value = false
      isNew.value = false
    }

    useFetch(async () => {
      await store.dispatch('printsection/getPrintSections', filter.value, { root: true })
    })

    watch(filter, () => {
      if (!filter) {
        return
      }

      getPrintSections(filter.value.toString())
    }, { immediate: true, deep: true })

    return {
      options,
      dialogRemove,
      loading,
      isNew,
      filter,
      reset,
      selectPrintSection,
      printSection,
      printSections,
      dataCKEditor,
      deletePrintSection,
      upsertPrintSection,
      getPrintSections
    }
  }
})
</script>
<style>

</style>
