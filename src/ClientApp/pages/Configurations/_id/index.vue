<template>
  <div>
    <ValidationObserver ref="obs" v-slot="{ valid, dirty }" tag="form">
      <v-row align="center">
        <v-col cols="12" xs="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-select
              id="selReportType"
              v-model="componentState.newConfig.type"
              :error-messages="errors"
              disabled
              item-value="id"
              item-text="text"
              label="Report Type"
              :items="[{ id:0, text: 'Inspection' }]"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" xs="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtTitle"
              v-model="componentState.newConfig.title"
              :error-messages="errors"
              label="Report Title"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtFormName"
              v-model="componentState.newConfig.formName"
              :error-messages="errors"
              label="Report Form Name/Number"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-select
              id="printSectionId"
              v-model="componentState.newConfig.printSectionId"
              :error-messages="errors"
              item-value="id"
              item-text="code"
              label="Print Section"
              :items="printSections"
              append-outer-icon="mdi-page-previous-outline"
              @click:append-outer="$router.push(`/printsections`)"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="3">
          <v-text-field
            id="txtRemarksLabelText"
            v-model="componentState.newConfig.remarksLabelText"
            label="Remarks Label Text"
          />
        </v-col>
        <v-col cols="12" md="2">
          <ValidationProvider>
            <v-switch v-model="componentState.newConfig.useCheckList" label="Use CheckList" />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="2">
          <ValidationProvider>
            <v-switch v-model="componentState.newConfig.usePhotoRecord" label="Use PhotoRecord" />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="2">
          <ValidationProvider>
            <v-switch v-model="componentState.newConfig.useNotes" label="Use Notes" />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="1">
          <ValidationProvider>
            <v-switch v-model="componentState.newConfig.inactive" label="Inactive" />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" justify="space-between" md="2">
          <nuxt-link :to="`/Configurations/${componentState.newConfig.id}/FormsSettingsList`">
            <v-btn color="primary" outlined>
              Configure Forms
            </v-btn>
          </nuxt-link>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" md="6">
          <v-select
            id="checksDefinition"
            v-model="componentState.newConfig.checksDefinition"
            item-value="id"
            item-text="text"
            multiple
            small-chips
            deletable-chips
            label="Include CheckLists"
            :items="checks"
            append-outer-icon="mdi-format-list-checks"
            @click:append-outer="$router.push(`/checklists?configurationid=${componentState.newConfig.id}&configurationonly=true`)"
          />
        </v-col>
        <v-col cols="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-select
              id="signatureDefinitions"
              v-model="componentState.newConfig.signatureDefinitions"
              :error-messages="errors"
              item-value="id"
              item-text="title"
              multiple
              small-chips
              deletable-chips
              label="Include Signatures"
              :items="signatures"
              append-outer-icon="mdi-draw"
              @click:append-outer="$router.push(`/signatures?configurationid=${componentState.newConfig.id}&configurationonly=true`)"
            />
          </ValidationProvider>
        </v-col>
      </v-row>
      <h1 class="text-left">
        Print Options
      </h1>
      <v-row>
        <v-col>
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-select
              v-model="componentState.newConfig.display"
              :items="componentState.displayOptions"
              name="display"
              label="Checklists Display Orientation"
              :error-messages="errors"
              item-text="text"
              item-value="text"
            />
          </ValidationProvider>
        </v-col>
        <v-col>
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtTemplateName"
              v-model="componentState.newConfig.templateName"
              :error-messages="errors"
              label="Print Template Name"
            />
          </ValidationProvider>
        </v-col>
      </v-row>
      <v-fab-transition>
        <v-btn
          v-show="componentState.hasPendingChanges"
          :disabled="!dirty || !valid"
          color="success"
          fab
          fixed
          dark
          large
          bottom
          right
          class="v-btn--example2"
          @click="saveChanges"
        >
          <v-icon>mdi-content-save</v-icon>
        </v-btn>
      </v-fab-transition>
    </ValidationObserver>
  </div>
</template>
<script lang="ts">
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { defineComponent, reactive, useRoute, useRouter, computed } from '@nuxtjs/composition-api'
import { ReportConfiguration, ReportType, CheckList, FilterType, UpdateReportConfigurationCommand, ChecklistDisplayList, CheckListDisplay } from '@/types'
import { useChecklistStore } from '~/composables/useChecklistStore'
import { useSignaturesStore } from '~/composables/useSignaturesStore'
import { SignatureDTO } from '@/types/Signatures/ViewModels/SignatureDTO'
import { PrintSectionDTO } from '~/types/PrintSections/ViewModels/PrintSectionDTO'
import { usePrintSectionsStore } from '~/composables/usePrintSectionsStore'
import useGoBack from '~/composables/useGoBack'
import { useConfigurationStore } from '~/composables/useConfigurationStore'

export default defineComponent({
  components: {
    ValidationObserver,
    ValidationProvider
  },
  setup () {
    useGoBack()
    const componentState = reactive({
      defaultType: ReportType.Inspection,
      newConfig: {} as ReportConfiguration,
      display: CheckListDisplay.Numbered,
      displayOptions: ChecklistDisplayList,
    })

    const checklistsStore = useChecklistStore()
    const signaturesStore = useSignaturesStore()
    const printSectionsStore = usePrintSectionsStore()
    const configurationStore = useConfigurationStore()
    const route = useRoute()
    const router = useRouter()

    const id: number = parseInt(route.value.params.id)
    const filter: FilterType = {
      filterText: '',
      inConfigurationOnly: true,
      reportId: undefined,
      reportConfigurationId: undefined
    }

    Promise.all([
      checklistsStore.getChecklists(filter),
      signaturesStore.getSignatures(filter),
      printSectionsStore.getPrintSections(filter)
    ])

    if (id > 0) {
      configurationStore.getConfigurationById(id).then((result) => {
        componentState.newConfig = Object.assign({}, result)
        componentState.display = parseInt(result.checkListMetadata.display)
      })
    } else { componentState.newConfig = { type: 0 } as ReportConfiguration }

    const checks = computed((): CheckList[] => {
      return checklistsStore.checkLists
    })

    const printSections = computed((): PrintSectionDTO[] => {
      return printSectionsStore.printSectionsList.filter(c => c.isMainReport === true)
    })

    const signatures = computed((): SignatureDTO[] => {
      return signaturesStore.signaturesList
    })

    const hasPendingChanges = () => {
      return true
    }
    const saveChanges = async () => {
      const command: UpdateReportConfigurationCommand = {
        id: parseInt(route.value.params.id),
        type: componentState.newConfig.type,
        title: componentState.newConfig.title,
        formName: componentState.newConfig.formName,
        remarksLabelText: componentState.newConfig.remarksLabelText,
        inactive: componentState.newConfig.inactive,
        checksDefinition: componentState.newConfig.checksDefinition,
        signatureDefinitions: componentState.newConfig.signatureDefinitions,
        printSectionId: componentState.newConfig.printSectionId,
        display: componentState.newConfig.display,
        templateName: componentState.newConfig.templateName,
        useNotes: componentState.newConfig.useNotes,
        useCheckList: componentState.newConfig.useCheckList,
        usePhotoRecord: componentState.newConfig.usePhotoRecord
      }

      if (parseInt(route.value.params.id) > 0) {
        await configurationStore.updateConfiguration(command)
          .then(() => {
            configurationStore.getConfigurationById(route.value.params.id)
          })
      } else {
        await configurationStore.createConfiguration(componentState.newConfig)
          .then((resp) => {
            if (parseInt(route.value.params.id) > 0) { configurationStore.getConfigurationById(route.value.params.id) } else { router.push({ name: 'Configurations-id', params: { id: resp } }) }
          })
      }
    }

    return {
      componentState,
      checks,
      signatures,
      printSections,
      hasPendingChanges,
      saveChanges,

    }
  },
})
</script>

<style scoped>
.v-btn--example2 {
    bottom: 0;
    /* position: absolute; */
    margin: 0 46px 16px 16px;
  }
</style>
