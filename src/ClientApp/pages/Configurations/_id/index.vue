<template>
    <div>
        <ValidationObserver tag="form" ref="obs" v-slot="{ valid, dirty }">
        <v-row>
          <v-col cols="12" xs="12" md="6">
            <ValidationProvider rules="required" v-slot="{ errors }">
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
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-text-field
                id="txtTitle"
                v-model="newConfig.title"
                :error-messages="errors"
                label="Report Title"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-text-field
                id="txtFormName"
                v-model="newConfig.formName"
                :error-messages="errors"
                label="Report Form Name/Number"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="10">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-text-field
                id="txtRemarksLabelText"
                v-model="newConfig.remarksLabelText"
                :error-messages="errors"
                label="Remarks Label Text"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="2">
            <v-switch v-model="newConfig.inactive">
              Inactive
            </v-switch>
          </v-col>
        </v-row>
      <v-row>
          <v-col cols="12" md="6">
              <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select
                id="checksDefinition"
                v-model="newConfig.checksDefinition"
                :error-messages="errors"
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
            </ValidationProvider>
          </v-col>
          <v-col cols="12" md="6">
              <ValidationProvider rules="required" v-slot="{ errors }">
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
import InnerPageMixin from '@/mixins/innerpage'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { ReportConfiguration, ReportType, CheckList, FilterType, UpdateReportConfigurationCommand } from '@/types'
import { CheckListsState } from '@/store/checklists'
import { SignatureState } from '@/store/signatures'
import { SignatureDTO } from '@/types/Signatures/ViewModels/SignatureDTO'

@Component({
    components: {
        ValidationObserver,
        ValidationProvider
    }
})
export default class AddEditReportConiguration extends mixins(InnerPageMixin){
    defaultType: ReportType = ReportType.Inspection
    newConfig!: ReportConfiguration

    get checks (): CheckList[] {
        return (this.$store.state.checklists as CheckListsState)
        .checkLists
    }

    get signatures (): SignatureDTO[] {
        return (this.$store.state.signatures as SignatureState)
        .signaturesList
    }

    get hasPendingChanges() {
        return true
    }

    async saveChanges() {
      const self = this
        const command: UpdateReportConfigurationCommand = {
            id: parseInt(self.$route.params.id),
            type: this.newConfig.type,
            title: this.newConfig.title,
            formName: this.newConfig.formName,
            remarksLabelText: this.newConfig.formName,
            inactive: this.newConfig.inactive,
            checksDefinition: this.newConfig.checksDefinition.flatMap(check => check.id),
            signatureDefinitions: this.newConfig.signatureDefinitions.flatMap(sign => sign.id)
        }
        if(parseInt(self.$route.params.id) >0) {
          await this.$store.dispatch('configurations/updateConfiguration', command, { root: true })
          .then((resp) => {
              this.$store.dispatch('configurations/getConfigurationById', self.$route.params.id, { root: true })
          })
        } else {
          await this.$store.dispatch('configurations/createConfiguration', this.newConfig, { root: true })
          .then((resp) => {
            if(parseInt(self.$route.params.id) > 0)
              this.$store.dispatch('configurations/getConfigurationById', self.$route.params.id, { root: true })
            else
              this.$router.push({ name: 'Configurations-id', params: { id: resp } })
          })
        }
    }

    async asyncData({ store, params }: any) {
        const id: number = parseInt(params.id)
        const filter: FilterType = {
            filterText: '',
            inConfigurationOnly: true,
            reportId: undefined,
            reportConfigurationId: undefined
        }
        await store.dispatch('checklists/getChecklists', filter, { root: true })
        await store.dispatch('signatures/getSignatures', filter, { root: true })

        let newConfig: ReportConfiguration
        if(id > 0) {
            const result = await store.dispatch('configurations/getConfigurationById', id, { root: true })
            newConfig = Object.assign({}, result)
        }
        else
            newConfig= { type: 0  } as ReportConfiguration

        return { newConfig }
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
