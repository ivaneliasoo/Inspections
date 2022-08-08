<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Reports"
      message="This operation will remove this signature and all data related"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteSignature()"
    />
    <v-data-table
      :class="$device.isTablet ? 'tablet-text' : ''"
      :items="signatures"
      item-key="id"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Signatures</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="
              item = {
                title: '',
                annotation: '',
                responsableType: 0,
                responsibleName: '',
                date: new Date().toISOString(),
                signature: '',
                designation: '',
                remarks: '',
                date: null,
                principal: true,
                reportId: 0,
                reportConfigurationId: filter.reportConfigurationId,
                order: 0,
              }
              dialog = true
            "
          >
            <v-icon dark> mdi-plus </v-icon>
          </v-btn>
          <v-dialog
            v-model="dialog"
            persistent
            scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
          >
            <ValidationObserver v-slot="{ valid, reset }" tag="form">
              <v-card>
                <v-card-title>
                  <span class="headline">Edit Signature</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="6">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                        >
                          <v-text-field
                            v-model="item.title"
                            name="title"
                            :readonly="
                              item.report ? item.report.isClosed : false
                            "
                            :error-messages="errors"
                            label="Title"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <v-text-field
                          v-model="item.annotation"
                          :readonly="item.report ? item.report.isClosed : false"
                          name="annotation"
                          label="Annotation"
                        />
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="4">
                        <ValidationProvider
                          v-slot="{ errors }"
                          :rules="`${
                            !item.reportConfigurationId ? 'required' : ''
                          }`"
                        >
                          <v-autocomplete
                            v-model="item.reportId"
                            :disabled="
                              filter.reportConfigurationId > 0 || item.id > 0
                            "
                            :error-messages="errors"
                            clearable
                            :items="reports"
                            item-text="name"
                            :readonly="
                              item.report ? item.report.isClosed : false
                            "
                            item-value="id"
                            label="Use in Report"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="4">
                        <ValidationProvider
                          v-slot="{ errors }"
                          :rules="`${!item.reportId ? 'required' : ''}`"
                        >
                          <v-autocomplete
                            v-model="item.reportConfigurationId"
                            :disabled="
                              item.reportId > 0 ||
                              item.id > 0 ||
                              filter.reportConfigurationId > 0
                            "
                            :error-messages="errors"
                            :readonly="
                              item.report ? item.report.isClosed : false
                            "
                            clearable
                            :items="configurations"
                            item-text="title"
                            item-value="id"
                            label="Use in Report Configuration"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="4">
                        <v-checkbox
                          v-model="item.principal"
                          :disabled="item.report ? item.report.isClosed : false"
                          name="principal"
                          label="Principal"
                        />
                      </v-col>
                    </v-row>
                    <v-row v-if="filter.reportConfigurationId > 0">
                      <v-col cols="6">
                        <v-select
                          id="defaultResponsibleType"
                          v-model="item.defaultResponsibleType"
                          name="defaultResponsibleType"
                          :items="responsibleTypesList"
                          item-text="text"
                          item-value="id"
                          label="Default Representation Type"
                        />
                      </v-col>
                      <v-col cols="6">
                        <v-checkbox
                          v-model="item.useLoggedInUserAsDefault"
                          name="useLoggedInUserAsDefault"
                          label="Use Logged In User as default responsible"
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
                    :disabled="
                      item.report ? item.report.isClosed : false && !valid
                    "
                    @click="upsertSignature()"
                  >
                    Save
                  </v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="
                      reset()
                      item = { principal: false }
                      dialog = false
                    "
                  >
                    Cancel
                  </v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
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
          <v-col cols="12" :md="filter.inConfigurationOnly ? 2 : 7">
            <v-switch
              v-model="filter.inConfigurationOnly"
              label="In Configuration"
            />
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
      <template #[`item.actions`]="{ item }">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="
            selectItem(item)
            dialog = true
          "
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          :disabled="item.report ? item.report.isClosed : false"
          color="error"
          @click="
            selectItem(item)
            dialogRemove = true
          "
        >
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import {
  useFetch,
  useRoute,
  watch,
  defineComponent,
  reactive,
  computed,
  ref,
} from '@nuxtjs/composition-api'
import { useSignaturesStore } from '~/composables/useSignaturesStore'
import { SignatureDTO } from '@/types/Signatures/ViewModels/SignatureDTO'
import { Report, ReportConfiguration, FilterType, Signature } from '~/types'
import { useConfigurationStore } from '~/composables/useConfigurationStore'
import { useReportsStore } from '~/composables/useReportsStore'
import { responsibleTypesString as responsibleTypes } from '@/types/Signatures'
import { ResponsibleType } from '~/services/api'

export default defineComponent({
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  setup() {
    const route = useRoute()
    const signaturesStore = useSignaturesStore()
    const configurationStore = useConfigurationStore()
    const reportsStore = useReportsStore()

    const responsibleTypesList = responsibleTypes

    const headers = [
      {
        text: 'Id',
        value: 'id',
        sortable: true,
        align: 'center',
      },
      {
        text: 'Title',
        value: 'title',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Annotation',
        value: 'annotation',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Principal',
        value: 'responsibleName',
        sortable: true,
        align: 'center',
      },
      {
        text: '',
        value: 'actions',
        sortable: false,
        align: 'center',
      },
    ]

    const filter = ref<FilterType>({
      filterText: '',
      inConfigurationOnly:
        route.value.query.configurationonly === 'true' ? true : false ?? true,
      reportId: route.value.query.reportid
        ? parseInt(route.value.query.reportid as string)
        : undefined,
      reportConfigurationId: route.value.query.configurationid
        ? parseInt(route.value.query.configurationid as string)
        : undefined,
    })

    const componentState = reactive({
      dialog: false,
      dialogRemove: false,
      loading: false,
      selectedItem: {} as Signature,
      item: {
        id: 0,
        title: '',
        annotation: '',
        responsableType: ResponsibleType.Inspector,
        responsibleName: '',
        designation: '',
        remarks: '',
        date: new Date().toISOString(),
        signature: '',
        principal: true,
        reportId: 0,
        reportConfigurationId: filter.value?.reportConfigurationId ?? 0,
        order: 0,
        defaultResponsibleType: '',
        useLoggedInUserAsDefault: false,
      },
    })

    useFetch(async () => {
      componentState.loading = true
      await Promise.all([
        reportsStore.getReports({
          filter: '',
          closed: route.value.query.closed,
          orderBy: 'date',
          myreports: false,
          descending: true,
        }),
        configurationStore.getConfigurations(''),
        signaturesStore.getSignatures(filter.value),
      ])
      componentState.loading = false
    })

    const selectItem = (item: Signature): void => {
      componentState.selectedItem = item
      signaturesStore
        .getSignatureById(componentState.selectedItem.id)
        .then((resp) => {
          componentState.item = resp
        })
    }

    const deleteSignature = () => {
      signaturesStore
        .deleteSignature(componentState.selectedItem.id)
        .then(() => {
          componentState.dialog = false
        })
    }

    const upsertSignature = async () => {
      if (componentState.item.id > 0) {
        await signaturesStore.updateSignature(componentState.item)
      } else {
        await signaturesStore.createSignature(componentState.item)
      }
      await signaturesStore.getSignatures(filter.value)
      componentState.dialog = false
    }

    watch(
      () => filter,
      async (value) => {
        await signaturesStore.getSignatures(value)
      },
      { deep: true }
    )

    const reports = computed((): Report[] => {
      return reportsStore.reportList
    })

    const configurations = computed((): ReportConfiguration[] => {
      return configurationStore.configurations
    })

    const signatures = computed((): SignatureDTO[] => {
      return signaturesStore.signaturesList || []
    })

    return {
      reports,
      configurations,
      signatures,
      headers,
      filter,
      componentState,
      selectItem,
      deleteSignature,
      upsertSignature,
      responsibleTypesList,
    }
  },
})
</script>
