<template>
  <div>
    <ValidationObserver v-slot="{ valid }" tag="form">
      <v-row>
        <alert-dialog
          v-model="dialogRemove"
          title="Remove Print Section"
          message="This operation will remove this Print Section. If Proceed, you no longer get it suggested again"
          :code="printSection.id"
          :description="printSection.description"
          @yes="deletePrintSection(printSection.id);"
        />
        <v-col cols="12" md="8" sm="12">
          <v-row>
            <v-toolbar flat color="white">
              <v-spacer />
              <v-switch v-model="ckEditorMode" inset label="Editor Mode" />
            </v-toolbar>
          </v-row>
          <v-card>
            <v-sheet v-if="ckEditorMode" height="450" style="overflow-y: scroll">
              <draggable>
                <VueEditor id="myeditor" v-model="dataCKEditor" />
              </draggable>
            </v-sheet>
            <v-sheet v-if="!ckEditorMode" height="450" fluid>
              <v-row class="pl-4">
                <v-col
                  cols="12"
                  sm="6"
                  md="4"
                >
                  <ValidationProvider
                    v-slot="{ errors }"
                    rules="required"
                    immediate
                  >
                    <v-text-field
                      v-model="printSection.code"
                      type="text"
                      label="Code"
                      :error-messages="errors"
                    />
                  </ValidationProvider>
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
                  <ValidationProvider
                    v-slot="{ errors }"
                    rules="required"
                    immediate
                  >
                    <v-text-field
                      v-model="printSection.description"
                      type="text"
                      label="Description"
                      :error-messages="errors"
                    />
                  </ValidationProvider>
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
            </v-sheet>
            <v-card-actions>
              <v-spacer />
              <v-btn
                v-if="printSection.code != ''"
                color="success"
                text
                :disabled="!valid"
                @click="upsertPrintSection();"
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
        <v-col cols="12" md="4" sm="12">
          <v-card height="568" style="overflow-y: scroll">
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
              <v-btn icon @click="isNew=true;reset(); dataCKEditor='';ckEditorMode = false">
                <v-icon>mdi-plus</v-icon>
              </v-btn>
            </v-toolbar>
            <v-list two-line>
              <v-list-item-group
                active-class="indigo--text"
              >
                <draggable v-model="printSections">
                  <template v-for="(item, index) in printSections">
                    <v-list-item :key="item.code + index">
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
                      <v-list-item-action>
                        <v-list-item-action-text v-text="'Delete'" />
                        <v-btn icon color="red" :disabled="item.isMainReport" @click=" dialogRemove = true;printSection.id=item.id">
                          <v-icon>mdi-delete</v-icon>
                        </v-btn>
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
    </ValidationObserver>
  </div>
</template>

<script lang="ts">
import { ref, defineComponent, reactive, computed, useStore, useFetch, watch } from '@nuxtjs/composition-api'
import draggable from 'vuedraggable'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import ClassicEditor from '~/assets/ckeditor5.0.0/ckeditor'
import { PrintSectionState } from '~/store/printsection'
import { PrintSectionDTO } from '@/types/PrintSections/ViewModels/PrintSectionDTO'
import useGoBack from '~/composables/useGoBack'

export default defineComponent({
  name: 'PrintSectionDetail',
  components: {
    draggable,
    ValidationObserver,
    ValidationProvider
  },
  layout: undefined,
  setup () {
    useGoBack()
    const store = useStore()
    const dialogRemove = ref<boolean>(false)
    const loading = ref<boolean>(false)
    const ckEditorMode = ref<boolean>(true)
    const isNew = ref<boolean>(false)
    const selectedTab = ref<string>('basicTab')
    const filter = ref<String>('')
    const dataCKEditor = ref<String>('')
    const printSection = ref({ id: 0, code: '', description: '', content: '', isMainReport: false, status: 0 } as PrintSectionDTO)
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

    const deletePrintSection = (id :number) => {
      store
        .dispatch('printsection/deletePrintSection', id, { root: true })
        .then(() => {
          reset()
        })
    }

    const reset = () => {
      dataCKEditor.value = ''
      printSection.value = { id: 0, code: '', description: '', content: '', isMainReport: false, status: 0 }
    }

    const upsertPrintSection = async () => {
      loading.value = true
      printSection.value.content = dataCKEditor.value.toString()
      if (printSection.value.id > 0) {
        await store.dispatch('printsection/updatePrintSection', printSection.value, { root: true })
        selectPrintSection(printSection.value)
      } else {
        await store.dispatch('printsection/createPrintSection', printSection.value, { root: true })
        await store.dispatch('printsection/getPrintSections', filter.value, { root: true })
        const lastElement = printSections.value[printSections.value.length - 1]
        selectPrintSection(lastElement)
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
      ckEditorMode,
      reset,
      selectedTab,
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
