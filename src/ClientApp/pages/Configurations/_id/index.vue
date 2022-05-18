<template>
  <div>
    <ValidationObserver ref="obs" v-slot="{ valid, dirty }" tag="form">
      <v-row align="center">
        <v-col cols="12" xs="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-select
              id="selReportType"
              v-model="newConfig.type"
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
              v-model="newConfig.title"
              :error-messages="errors"
              label="Report Title"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtFormName"
              v-model="newConfig.formName"
              :error-messages="errors"
              label="Report Form Name/Number"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-select
              id="printSectionId"
              v-model="newConfig.printSectionId"
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
        <v-col cols="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtRemarksLabelText"
              v-model="newConfig.remarksLabelText"
              :error-messages="errors"
              label="Remarks Label Text"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="2">
          <v-switch v-model="newConfig.inactive" label="Inactive" />
        </v-col>
        <v-col cols="12" justify="space-between" md="4">
          <nuxt-link :to="`/Configurations/${newConfig.id}/FormsSettingsList`">
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
            v-model="newConfig.checksDefinition"
            item-value="id"
            item-text="text"
            multiple
            small-chips
            deletable-chips
            label="Include CheckLists"
            :items="checks"
            append-outer-icon="mdi-format-list-checks"
            @click:append-outer="$router.push(`/checklists?configurationid=${newConfig.id}&configurationonly=true`)"
          />
        </v-col>
        <v-col cols="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-select
              id="signatureDefinitions"
              v-model="newConfig.signatureDefinitions"
              :error-messages="errors"
              item-value="id"
              item-text="title"
              multiple
              small-chips
              deletable-chips
              label="Include Signatures"
              :items="signatures"
              append-outer-icon="mdi-draw"
              @click:append-outer="$router.push(`/signatures?configurationid=${newConfig.id}&configurationonly=true`)"
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
              v-model="newConfig.display"
              :items="displayOptions"
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
              v-model="newConfig.templateName"
              :error-messages="errors"
              label="Print Template Name"
            />
          </ValidationProvider>
        </v-col>
      </v-row>
      <v-fab-transition>
        <v-btn
          v-show="hasPendingChanges"
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
import { Component, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import InnerPageMixin from '@/mixins/innerpage'
import { ReportConfiguration, ReportType, CheckList, FilterType, UpdateReportConfigurationCommand, ChecklistDisplayList, CheckListDisplay } from '@/types'
import { CheckListsState } from '@/store/checklists'
import { SignatureState } from '@/store/signatures'
import { SignatureDTO } from '@/types/Signatures/ViewModels/SignatureDTO'
import { PrintSectionDTO } from '~/types/PrintSections/ViewModels/PrintSectionDTO'
import { PrintSectionState } from '~/store/printsection'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class AddEditReportConiguration extends mixins(InnerPageMixin) {
  defaultType: ReportType = ReportType.Inspection
  newConfig!: ReportConfiguration

  display: CheckListDisplay = CheckListDisplay.Numbered
  displayOptions = ChecklistDisplayList

  get checks (): CheckList[] {
    return (this.$store.state.checklists as CheckListsState)
      .checkLists
  }

  get printSections (): PrintSectionDTO[] {
    return (this.$store.state.printsection as PrintSectionState)
      .printSectionsList.filter(c => c.isMainReport === true)
  }

  get signatures (): SignatureDTO[] {
    return (this.$store.state.signatures as SignatureState)
      .signaturesList
  }

  get hasPendingChanges () {
    return true
  }

  async saveChanges () {
    const self = this

    const command: UpdateReportConfigurationCommand = {
      id: parseInt(self.$route.params.id),
      type: this.newConfig.type,
      title: this.newConfig.title,
      formName: this.newConfig.formName,
      remarksLabelText: this.newConfig.remarksLabelText,
      inactive: this.newConfig.inactive,
      checksDefinition: this.newConfig.checksDefinition,
      signatureDefinitions: this.newConfig.signatureDefinitions,
      printSectionId: this.newConfig.printSectionId,
      display: this.newConfig.display,
      templateName: this.newConfig.templateName
    }

    if (parseInt(self.$route.params.id) > 0) {
      await this.$store.dispatch('configurations/updateConfiguration', command, { root: true })
        .then(() => {
          this.$store.dispatch('configurations/getConfigurationById', self.$route.params.id, { root: true })
        })
    } else {
      await this.$store.dispatch('configurations/createConfiguration', this.newConfig, { root: true })
        .then((resp) => {
          if (parseInt(self.$route.params.id) > 0) { this.$store.dispatch('configurations/getConfigurationById', self.$route.params.id, { root: true }) } else { this.$router.push({ name: 'Configurations-id', params: { id: resp } }) }
        })
    }
  }

  async asyncData ({ store, params }: any) {
    const id: number = parseInt(params.id)
    const filter: FilterType = {
      filterText: '',
      inConfigurationOnly: true,
      reportId: undefined,
      reportConfigurationId: undefined
    }

    await Promise.all([
      store.dispatch('checklists/getChecklists', filter, { root: true }),
      store.dispatch('signatures/getSignatures', filter, { root: true }),
      store.dispatch('printsection/getPrintSections', filter, { root: true })
    ])

    let newConfig: ReportConfiguration
    let display: number
    if (id > 0) {
      const result = await store.dispatch('configurations/getConfigurationById', id, { root: true })
      newConfig = Object.assign({}, result)
      display = parseInt(result.checkListMetadata.display)
    } else { newConfig = { type: 0 } as ReportConfiguration }
    return { newConfig, display }
  }
}
</script>

<style scoped>
.v-btn--example2 {
    bottom: 0;
    /* position: absolute; */
    margin: 0 46px 16px 16px;
  }
</style>
