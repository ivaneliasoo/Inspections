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
              <v-switch v-model="editorMode" inset label="Editor Mode" />
            </v-toolbar>
          </v-row>
          <v-card>
            <v-sheet v-if="editorMode" height="450" style="overflow-y: scroll">
              <draggable>
                <VueEditor id="myeditor" v-model="editorContent" />
              </draggable>
            </v-sheet>
            <v-sheet v-if="!editorMode" height="450" fluid>
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
                    v-model="editorContent"
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
              <v-btn icon @click="isNew=true;reset(); editorContent='';editorMode = false">
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
                      <v-list-item-content @click="selectPrintSection(item); editorContent = item.content;isNew = false">
                        <v-list-item-subtitle
                          class="text--primary text-left font-weight-bold"
                        >
                          #{{ item.code }}
                        </v-list-item-subtitle>
                        <v-list-item-subtitle class="text-left">
                          {{ item.description }}
                        </v-list-item-subtitle>
                      </v-list-item-content>
                      <v-list-item-action>
                        <v-list-item-action-text>
                          {{ 'Is Main Report' }}
                        </v-list-item-action-text>
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
                        <v-list-item-action-text>
                          {{ 'Delete' }}
                        </v-list-item-action-text>
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
// eslint-disable-next-line import/named
import { ref, defineComponent, computed, useFetch, watch } from '@nuxtjs/composition-api'
import draggable from 'vuedraggable'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { usePrintSectionsStore } from '~/composables/usePrintSectionsStore'
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
    const printSectionsStore = usePrintSectionsStore()
    const dialogRemove = ref<boolean>(false)
    const loading = ref<boolean>(false)
    const editorMode = ref<boolean>(true)
    const isNew = ref<boolean>(false)
    const selectedTab = ref<string>('basicTab')
    const filter = ref<String>('')
    const editorContent = ref<String>('')
    const printSection = ref({ id: 0, code: '', description: '', content: '', isMainReport: false, status: 0 } as PrintSectionDTO)

    const selectPrintSection = (item: PrintSectionDTO): void => {
      printSectionsStore.getPrintSectionById(item.id)
        .then(resp => (printSection.value = resp))
    }

    const printSections = computed((): PrintSectionDTO[] => {
      return printSectionsStore.$state.printSectionsList
    })

    const getPrintSections = async (filter: string) => {
      await printSectionsStore.getPrintSections({ filter })
    }

    const deletePrintSection = (id :number) => {
      printSectionsStore.deletePrintSection(id)
        .then(() => {
          reset()
        })
    }

    const reset = () => {
      editorContent.value = ''
      printSection.value = { id: 0, code: '', description: '', content: '', isMainReport: false, status: 0 }
    }

    const upsertPrintSection = async () => {
      loading.value = true
      printSection.value.content = editorContent.value.toString()
      if (printSection.value.id > 0) {
        await printSectionsStore.updatePrintSection(printSection.value)
        selectPrintSection(printSection.value)
      } else {
        await printSectionsStore.createPrintSection(printSection.value)
        await printSectionsStore.getPrintSections(filter.value)
        const lastElement = printSections.value[printSections.value.length - 1]
        selectPrintSection(lastElement)
      }

      loading.value = false
      isNew.value = false
    }

    useFetch(async () => {
      await printSectionsStore.getPrintSections(filter.value)
    })

    watch(filter, () => {
      if (!filter) {
        return
      }

      getPrintSections(filter.value.toString())
    }, { immediate: true, deep: true })

    return {
      dialogRemove,
      loading,
      isNew,
      filter,
      editorMode,
      reset,
      selectedTab,
      selectPrintSection,
      printSection,
      printSections,
      editorContent,
      deletePrintSection,
      upsertPrintSection,
      getPrintSections
    }
  }
})
</script>
<style>

</style>
