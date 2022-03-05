<template>
  <div id="report">
    <alert-dialog
      v-model="pageOptions.dialogClose"
      title="Close Report"
      message="you are about to close this report."
      :code="currentReport.id"
      :description="currentReport.name"
      :loading="pageOptions.savingNewReport"
      @yes="closeReport"
      @no="currentReport.isClosed = true"
    />
    <v-row v-if="pageOptions.loadingReport">
      <v-col cols="12">
        <v-skeleton-loader
          type="list-item-avatar, divider, list-item-three-line, card-heading, image, actions"
        />
        <v-skeleton-loader type="list-item-avatar-three-line, image, article" />
      </v-col>
    </v-row>
    <ValidationObserver ref="obs" v-slot="{ valid }" tag="form">
      <v-row dense justify="space-between">
        <v-col align-self="start" class="text-left">
          <h4><strong>{{ currentReport.title }}</strong></h4>
          <h6><strong>{{ currentReport.formName }}</strong></h6>
        </v-col>
        <v-col class="text-right">
          <span v-if="currentReport.isClosed" class="tw-text-green-700">
            <v-icon class="mt-n1" color="#047857">
              mdi-check-circle-outline
            </v-icon>
            Completed With Signatures
          </span>
          <v-btn
            v-else
            :disabled="!valid || (!currentReport.isClosed && !CanCloseReport)"
            color="success"
            class="v-btn--example2"
            @click="
              pageOptions.dialogClose = true;
              currentReport.isClosed = true;
            "
          >
            <v-icon>mdi-lock</v-icon>
            Complete Report
          </v-btn>
        </v-col>
      </v-row>
      <v-row dense align="center" justify="space-between">
        <v-col cols="12" md="3">
          <ValidationProvider
            v-slot="{ errors }"
            rules="required"
            immediate
          >
            <DatePickerBase
              v-model="currentReport.date"
              name="date"
              type="number"
              :error-messages="errors"
              :max="new Date().toISOString()"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="9">
          <ValidationProvider
            v-slot="{ errors }"
            rules="required"
            immediate
          >
            <v-autocomplete
              v-model="currentReport.address"
              :error-messages="errors"
              :items="addresses"
              :loading="pageOptions.searchingAddresses"
              :search-input.sync="pageOptions.search"
              hide-selected
              item-text="formatedAddress"
              item-value="formatedAddress"
              label="Inspection Address"
              placeholder="Start typing to Search"
              prepend-icon="mdi-crosshairs-gps"
              clearable
              autocomplete="nope"
              name="address"
              @keypress="pageOptions.isDirty = true"
              @change="setLicenseFromAddress"
            />
          </ValidationProvider>
        </v-col>
      </v-row>
      <v-row dense>
        <v-col>
          <span class="tw-text-base tw-mx-4"><strong>Name: </strong> {{ currentReport.licenseName }}</span>
          <span class="tw-text-base tw-mx-4"><strong>License: </strong> {{ currentReport.licenseNumber }}</span>
          <span class="tw-text-base tw-mx-4"><strong>Approved Load: </strong> {{ currentReport.licenseAmp }} A /{{ currentReport.licenseVolt }} V</span>
          <span class="tw-text-base tw-mx-4"><strong>kVA: </strong> {{ currentReport.licenseKVA }}</span>
        </v-col>
      </v-row>
    </ValidationObserver>
    <v-row>
      <v-col cols="12">
        <v-tabs v-model="pageOptions.tabs" centered fixed-tabs icons-and-text>
          <v-tabs-slider />
          <v-tab
            href="#checklists"
            :class="
              hasUntouchedChecks ||
                HasNotesWithPendingChecks ||
                !PrincipalSignatureHasAResponsable
                ? 'error--text'
                : 'primary--text'
            "
          >
            Report Details
            <v-tooltip
              v-if="
                !hasUntouchedChecks ||
                  HasNotesWithPendingChecks ||
                  !PrincipalSignatureHasAResponsable
              "
              top
            >
              <template #activator="{ on }">
                <v-icon
                  :color="
                    hasUntouchedChecks ||
                      HasNotesWithPendingChecks ||
                      !PrincipalSignatureHasAResponsable
                      ? 'error'
                      : ''
                  "
                  v-on="on"
                >
                  mdi-message-bulleted
                </v-icon>
              </template>
              <span>You must complete all the required checks, notes and signatures
                in tab Report Details to proceeded uploading photos</span>
            </v-tooltip>
            <v-icon v-else>
              mdi-message-bulleted
            </v-icon>
          </v-tab>
          <v-tab href="#photos">
            Photo Record
            <v-icon> mdi-folder-multiple-image </v-icon>
          </v-tab>
          <v-tab v-for="form in forms.filter(f => f.enabled)" :key="`df-${form.id}`" eager>
            {{ form.name }}
            <v-icon> {{ form.icon }} </v-icon>
          </v-tab>
        </v-tabs>
        <v-tabs-items v-model="pageOptions.tabs" touchless>
          <v-tab-item key="checklists" value="checklists">
            <v-expansion-panels
              multiple
              focusable
              :value="pageOptions.openedPanels"
            >
              <v-expansion-panel>
                <v-expansion-panel-header
                  :style="hasUntouchedChecks ? 'color: white;' : ''"
                  :color="hasUntouchedChecks ? 'red' : ''"
                >
                  <span class="font-weight-black">Check List States: Not Acceptable (
                    <v-icon
                      :color="hasUntouchedChecks ? 'white' : getCheckIconColor(0)"
                    >
                      mdi-{{ getCheckIcon(0) }}
                    </v-icon>
                    ) Acceptable (
                    <v-icon
                      :color="hasUntouchedChecks ? 'white' : getCheckIconColor(1)"
                    >
                      mdi-{{ getCheckIcon(1) }}
                    </v-icon>
                    ) Not Aplicable (
                    <span
                      :color="hasUntouchedChecks ? 'white' : getCheckIconColor(2)"
                    >
                      {{ getCheckIcon(2) }}
                    </span>
                    )
                  </span>
                  <v-row v-if="hasUntouchedChecks" dense>
                    <v-col>
                      <v-icon dark>
                        mdi-alert-circle
                      </v-icon>
                      <span style="color: white" class="font-weight-accent">
                        there are items that need to be completed or checked
                      </span>
                    </v-col>
                  </v-row>
                </v-expansion-panel-header>
                <v-expansion-panel-content>
                  <v-list subheader two-line flat dense>
                    <v-list-item
                      v-for="(item, checkListIndex) in currentReport.checkLists"
                      :key="item.id"
                    >
                      <template #default>
                        <v-list-item-content class="text-left">
                          <v-list-item-title>
                            <v-row justify="start" align="center" dense>
                              <v-col
                                cols="10"
                                md="6"
                                class="font-weight-black text-wrap"
                              >
                                <h3>
                                  {{ checkListIndex + 1 }} .- {{ item.text }}
                                </h3>
                                <span
                                  v-if="
                                    item.checks.filter(
                                      (c) => c.required && !c.touched
                                    ).length == 0
                                  "
                                >
                                  <v-chip
                                    x-small
                                    color="success"
                                  >Completed</v-chip>
                                </span>
                              </v-col>
                              <v-col
                                cols="2"
                                md="6"
                                :class="
                                  $vuetify.breakpoint.mdAndDown
                                    ? 'ml-n5'
                                    : 'ml-n6'
                                "
                              >
                                <v-btn
                                  icon
                                  text
                                  @click.stop="
                                    item.checked < 2
                                      ? item.checked++
                                      : (item.checked = 0);
                                    checkItemChecks(item.id, item.checked);
                                  "
                                >
                                  <v-icon
                                    :color="
                                      item.checks.length !==
                                        item.checks.filter(
                                          (c) => c.checked === 1
                                        ).length &&
                                        item.checks.filter((c) => c.checked === 1)
                                          .length > 0
                                        ? getCheckIconColor(2)
                                        : getCheckIconColor(
                                          item.checks[0].checked
                                        )
                                    "
                                  >
                                    {{
                                      `mdi-${
                                        item.checks.length !==
                                        item.checks.filter(
                                          (c) => c.checked === 1
                                        ).length &&
                                        item.checks.filter(
                                          (c) => c.checked === 1
                                        ).length > 0
                                          ? "minus"
                                          : item.checks.length ===
                                            item.checks.filter(
                                              (c) => c.checked === 2
                                            ).length
                                            ? "minus"
                                            : item.checks.length ===
                                              item.checks.filter(
                                                (c) => c.checked === 0
                                              ).length
                                              ? "close"
                                              : getCheckIcon(1)
                                      }`
                                    }}
                                  </v-icon>
                                </v-btn>
                              </v-col>
                            </v-row>
                            <v-row dense>
                              <v-col cols="12">
                                <v-list-item
                                  v-for="(
                                    checkItem, checkListItemIndex
                                  ) in item.checks"
                                  :key="checkItem.id"
                                >
                                  <template #default="{}">
                                    <v-list-item-title :title="checkItem.text">
                                      <v-row
                                        dense
                                        align="center"
                                        justify="space-between"
                                      >
                                        <v-col cols="10">
                                          <v-row
                                            style="cursor: pointer"
                                            dense
                                            align="center"
                                            justify="space-between"
                                            :class="{ ml_5: true }"
                                            @click.stop="
                                              checkItem.checked < 2
                                                ? checkItem.checked++
                                                : (checkItem.checked = 0);
                                              saveCheckItem(checkItem);
                                            "
                                          >
                                            <v-col
                                              cols="12"
                                              md="7"
                                              class="text-wrap"
                                            >
                                              <v-row
                                                justify="space-around"
                                                align="center"
                                                dense
                                              >
                                                <v-col
                                                  cols="1"
                                                  :class="[
                                                    'text-right',
                                                    checkItem.checked == 3 &&
                                                      pageOptions.shouldShowRequired
                                                      ? 'error--text'
                                                      : '',
                                                  ]"
                                                >
                                                  <h3>
                                                    {{ checkListIndex + 1 }}.{{
                                                      checkListItemIndex + 1
                                                    }}
                                                  </h3>
                                                </v-col>
                                                <v-col
                                                  cols="10"
                                                  :class="[
                                                    'text-left',
                                                    checkItem.checked == 3 &&
                                                      pageOptions.shouldShowRequired
                                                      ? 'error--text'
                                                      : '',
                                                  ]"
                                                >
                                                  <h3
                                                    v-if="!checkItem.editable"
                                                  >
                                                    .-{{ checkItem.text }}
                                                    <v-chip
                                                      v-if="
                                                        !checkItem.editable &&
                                                          checkItem.checked == 3
                                                      "
                                                      class="text-uppercase"
                                                      :color="
                                                        pageOptions.shouldShowRequired
                                                          ? 'error'
                                                          : ''
                                                      "
                                                      x-small
                                                    >
                                                      required
                                                    </v-chip>
                                                  </h3>
                                                  <v-text-field
                                                    v-else
                                                    v-model="checkItem.text"
                                                    @blur="
                                                      saveCheckItem(checkItem)
                                                    "
                                                    @click.stop.prevent=""
                                                  >
                                                    <template #append="">
                                                      <v-chip
                                                        v-if="
                                                          checkItem.checked == 3
                                                        "
                                                        x-small
                                                        class="text-uppercase"
                                                        :color="
                                                          pageOptions.shouldShowRequired
                                                            ? 'error'
                                                            : ''
                                                        "
                                                      >
                                                        required
                                                      </v-chip>
                                                    </template>
                                                  </v-text-field>
                                                </v-col>
                                              </v-row>
                                            </v-col>
                                            <v-col cols="2" md="1">
                                              <v-btn icon text>
                                                <v-icon
                                                  v-if="checkItem.checked != 2"
                                                  :color="
                                                    getCheckIconColor(
                                                      checkItem.checked
                                                    )
                                                  "
                                                >
                                                  {{
                                                    `mdi-${getCheckIcon(
                                                      checkItem.checked
                                                    )}`
                                                  }}
                                                </v-icon>
                                                <span v-else>
                                                  {{
                                                    getCheckIcon(
                                                      checkItem.checked
                                                    )
                                                  }}
                                                </span>
                                              </v-btn>
                                            </v-col>
                                            <v-col cols="10" md="4">
                                              <v-text-field
                                                v-model="checkItem.remarks"
                                                :label="
                                                  currentReport.remarksLabelText
                                                "
                                                @blur="saveCheckItem(checkItem)"
                                                @click.stop.prevent=""
                                              />
                                            </v-col>
                                          </v-row>
                                        </v-col>
                                        <v-col cols="2">
                                          <v-btn
                                            v-if="
                                              pageOptions.showUpdateCheck.findIndex(
                                                (l) =>
                                                  l.currentIndex ===
                                                  checkListItemIndex &&
                                                  l.parentIndex ===
                                                  checkListIndex
                                              ) >= 0
                                            "
                                            color="amber"
                                            icon
                                            @click="
                                              pageOptions.showUpdateCheck.splice(
                                                pageOptions.showUpdateCheck.findIndex(
                                                  (l) =>
                                                    l.currentIndex ===
                                                    checkListItemIndex &&
                                                    l.parentIndex ===
                                                    checkListIndex
                                                ),
                                                1
                                              )
                                            "
                                          >
                                            <v-icon>mdi-check</v-icon>
                                          </v-btn>
                                        </v-col>
                                      </v-row>
                                    </v-list-item-title>
                                  </template>
                                </v-list-item>
                              </v-col>
                            </v-row>
                          </v-list-item-title>
                        </v-list-item-content>
                      </template>
                    </v-list-item>
                  </v-list>
                </v-expansion-panel-content>
              </v-expansion-panel>
              <!-- End of Checklists -->
              <!-- Signatures -->
              <v-expansion-panel>
                <v-expansion-panel-header
                  :style="HasNotesWithPendingChecks ? 'color: white;' : ''"
                  :color="HasNotesWithPendingChecks ? 'red' : ''"
                >
                  <span class="font-weight-black">Notes</span>
                  <v-row v-if="HasNotesWithPendingChecks" dense>
                    <v-col>
                      <v-icon dark>
                        mdi-alert-circle
                      </v-icon>
                      <span style="color: white" class="font-weight-accent">
                        there are items that need to be completed or checked
                      </span>
                    </v-col>
                  </v-row>
                </v-expansion-panel-header>
                <v-expansion-panel-content>
                  <v-row justify="end" align="end" class="text-right">
                    <v-col cols="12">
                      <v-btn
                        class="mx-2"
                        x-small
                        fab
                        dark
                        color="primary"
                        title="Add note"
                        @click="addNote"
                      >
                        <v-icon dark>
                          mdi-plus
                        </v-icon>
                      </v-btn>
                    </v-col>
                  </v-row>
                  <div
                    v-for="(note, index) in currentReport.notes"
                    :key="index"
                  >
                    <v-row dense align="center" justify="space-around">
                      <v-col cols="1">
                        <v-btn
                          class="mx-2"
                          title="delete this note"
                          icon
                          text
                          :disabled="note.needsCheck"
                          color="error"
                          @click="removeNote(note)"
                        >
                          <v-icon dark>
                            mdi-minus
                          </v-icon>
                        </v-btn>
                      </v-col>
                      <v-col cols="8">
                        <v-text-field
                          v-model="note.text"
                          label="Note"
                          @blur="saveNote(note)"
                        />
                      </v-col>
                      <v-col cols="2">
                        <v-checkbox
                          v-model="note.checked"
                          :label="note.needsCheck ? '(Check Required)' : ''"
                          @change="saveNote(note)"
                        />
                      </v-col>
                      <v-col />
                    </v-row>
                  </div>
                  <v-row class="mt-10" />
                </v-expansion-panel-content>
              </v-expansion-panel>
            </v-expansion-panels>
            <v-row>
              <v-col>
                <h2 class="text-left">
                  Signatures
                </h2>
                <ValidationObserver ref="obsSignatures">
                  <SignaturesForm
                    v-model="signatures"
                    :can-edit="currentReport.isClosed"
                  />
                </ValidationObserver>
              </v-col>
            </v-row>
          </v-tab-item>
          <v-tab-item key="photos" value="photos">
            <PhotoRecords
              :report-id="currentReport.id"
              @uploaded="saveAndLoad()"
            />
          </v-tab-item>
          <v-tab-item v-for="form in forms.filter(f => f.enabled)" :key="`ti-${form.id}`">
            <CustomForm :schema="form.fields.fieldsDefinitions" />
          </v-tab-item>
        </v-tabs-items>
      </v-col>
    </v-row>
    <message-dialog
      v-model="pageOptions.errorsDialog"
      :actions="['no']"
      no-text="close"
      @no="pageOptions.errorsDialog = false"
    >
      <template #title="{}">
        Report Errors
      </template>
      <h2>The Report has been saved! but we've found some errors:</h2>
      <br>
      <span
        v-if="!hasUntouchedChecks"
        class="subtitle-1 error--text font-weight-black"
      >
        - Please, Complete and check all the required item in the
        checklist</span><br>
      <span
        v-if="HasNotesWithPendingChecks"
        class="subtitle-1 error--text font-weight-black"
      >
        - There are notes that you need to check. verify if it needs to be
        checked and filled</span><br>
      <span
        v-if="!PrincipalSignatureHasAResponsable"
        class="subtitle-1 error--text font-weight-black"
      >- The Principal Signature must have an responsable name and type</span>
    </message-dialog>
    <v-dialog
      v-model="pageOptions.dialogPrinting"
      hide-overlay
      persistent
      width="300"
    >
      <v-card color="primary" dark>
        <v-card-text>
          Processing and Downloading pdf report
          <v-progress-linear indeterminate color="white" class="mb-0" />
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import {
  defineComponent,
  watch,
  computed,
  useContext,
  onMounted,
  ref,
  reactive,
  useRoute,
  useFetch
} from '@nuxtjs/composition-api'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { debouncedWatch } from '@vueuse/core'
import {
  AddNoteCommand,
  CheckListItemQueryResult,
  ReportQueryResult,
  EditSignatureCommand,
  UpdateReportCommand,
  CheckValue,
  EditNoteCommand,
  UpdateCheckListItemCommand,
  CheckListQueryResult,
  SignatureQueryResult,
  NoteQueryResult,
  AddressDto,
  FormDefinitionResponse
} from '@/services/api'
import { useNotifications } from '~/composables/use-notifications'
import useGoBack from '~/composables/useGoBack'
import { AddressesState } from '@/store/addresses'

export default defineComponent({
  name: 'ReportForm',
  components: {
    ValidationObserver,
    ValidationProvider
  },
  setup () {
    useGoBack()
    const obs = ref<InstanceType<typeof ValidationObserver> | null>(null)
    const obsSignatures = ref<InstanceType<typeof ValidationObserver> | null>(null)

    const { notify } = useNotifications()
    const route = useRoute()
    const { store, $auth, $axios, $reportsApi, $formsApi } = useContext()
    const id = computed(() => route.value.params.id)

    onMounted(() => {
      if (obs.value && obsSignatures.value) {
        obs.value!.validate()
        obsSignatures.value!.validate()
      }
    })

    const pageOptions = reactive({
      loadingReport: false,
      errorsDialog: false,
      dialogPrinting: false,
      shouldShowRequired: false,
      isDirty: false,
      searchingAddresses: false,
      openedPanels: [0, 1],
      savingNewReport: true,
      currentPhoto: 0,
      showLabelEdit: [],
      showUpdateCheck: [] as { parentIndex: number; currentIndex: number }[],
      tabs: '',
      dialogClose: false,
      signaturesChanges: false,
      search: ''
    })

    const currentReport = ref<ReportQueryResult>({} as ReportQueryResult)
    const signatures = computed(() => (currentReport.value.signatures || []))

    const forms = ref<FormDefinitionResponse[]>([])

    const { fetch } = useFetch(async () => {
      try {
        currentReport.value = await store.dispatch(
          'reportstrore/getReportById',
          id.value,
          {
            root: true
          }
        )

        Promise.all([
          getSuggestedAddresses(''),
          store.dispatch(
            'users/setUserLastEditedReport',
            {
              userName: $auth.user.userName,
              lastEditedReport: id.value
            },
            { root: true }
          ),
          $auth.fetchUser(),
          $formsApi.getFormsDefinitionsByReportId(parseInt(id.value.toString()))
            .then((resp) => { forms.value = resp.data })
        ])
      } catch (error) {
        notify({ error })
      }
    })

    const loadReport = () => fetch()

    const getSuggestedAddresses = async (filter: string) => {
      await store.dispatch(
        'addresses/getAddresses',
        { filter },
        { root: true }
      )
    }

    const localAddress = computed((): AddressDto[] => {
      return [
        {
          formatedAddress: currentReport.value.address
        } as AddressDto
      ]
    })

    const addresses = computed((): AddressDto[] => {
      const dbAddressses = (store.state.addresses as AddressesState)
        .addressList

      if (dbAddressses.length > 0) {
        return dbAddressses
      }

      return localAddress.value
    })

    const IsValidForm = computed(() => {
      if (obs.value && obs.value!.flags) {
        return obs.value!.flags.valid
      }
      return false
    })

    const formHasChanged = computed(() => {
      return obs.value!.flags.dirty
    })

    const formHasSignaturesChanged = computed(() => {
      return obsSignatures.value?.flags.dirty
    })

    const hasUntouchedChecks = computed(() => {
      if (currentReport.value.checkLists) {
        const result = currentReport.value.checkLists.filter(
          (cl: CheckListQueryResult) =>
              cl.checks!.findIndex(c => !c.touched) >= 0
        )
        return result.length > 0
      }

      return false
    })

    const PrincipalSignatureHasAResponsable = computed(() => {
      if (!currentReport) {
        return true
      }
      if (!currentReport!.value.signatures) {
        return true
      }
      return (
        currentReport.value.signatures.findIndex(
          (s: SignatureQueryResult) =>
            s.responsibleType !== undefined &&
            s.responsibleName !== '' &&
            s.principal
        ) >= 0
      )
    })

    const HasNotesWithPendingChecks = computed(() => {
      if (!currentReport) {
        return false
      }
      if (!currentReport.value.notes) {
        return false
      }
      return (
        currentReport.value.notes.findIndex(
          (n: NoteQueryResult) => n.needsCheck && !n.checked
        ) >= 0
      )
    })

    const CanCloseReport = computed(() => {
      return !hasUntouchedChecks.value && !HasNotesWithPendingChecks.value && PrincipalSignatureHasAResponsable.value && IsValidForm.value
    })

    const isMultiLine = computed(() => {
      return currentReport.value.licenseVolt! >= 400
    })

    watch(
      () => pageOptions.search!,
      async (newValue: string) => {
        if (!newValue || !pageOptions.isDirty) {
          return
        }

        if (pageOptions.searchingAddresses) {
          return
        }

        if (newValue.length >= 3) {
          await getSuggestedAddresses(newValue)
        }
      }
    )

    debouncedWatch(
      currentReport,
      async (newValue: ReportQueryResult) => {
        if (!IsValidForm.value || !formHasChanged.value) {
          return
        }
        try {
          if (pageOptions.loadingReport) {
            return
          }
          await saveReportChanges(newValue)
        } catch (error) {
          notify({
            title: 'Reports',
            message: 'Error while saving the report',
            type: 'error',
            error
          })
        } finally {
          pageOptions.savingNewReport = false
          pageOptions.shouldShowRequired = true
        }
      },
      {
        debounce: 1000,
        deep: true
      }
    )

    debouncedWatch(
      signatures,
      (newValue: any) => {
        if (!formHasSignaturesChanged.value) {
          return
        }
        try {
          if (pageOptions.loadingReport) {
            return
          }
          newValue!.forEach(async (signature: any) => {
            const command: EditSignatureCommand = {
              id: signature.id,
              title: signature.title,
              annotation: signature.annotation,
              responsibleType: signature.responsibleType!,
              responsibleName: signature.responsibleName,
              designation: signature.designation,
              remarks: signature.remarks,
              date: signature.date,
              principal: signature.principal,
              drawnSign: signature.drawnSign
            }
            await $axios.$put(`signatures/${signature.id}`, command)
          })
          // notify({
          //   title: 'Signatures',
          //   message: 'Changes has been saved',
          //   type: 'success'
          // })
        } catch (error) {
          notify({
            title: 'Reports',
            message: 'Error while saving the report',
            type: 'error',
            error
          })
        } finally {
          pageOptions.savingNewReport = false
          pageOptions.shouldShowRequired = true
        }
      },
      {
        debounce: 1000,
        deep: true
      }
    )

    const addNote = async () => {
      const newNote: AddNoteCommand = {
        reportId: currentReport.value.id,
        text: '',
        checked: false,
        needsCheck: false
      }
      await $axios
        .$post(`reports/${route.value.params.id}/note`, newNote)
        .then((resp) => {
          currentReport.value.notes!.push({ id: resp, ...newNote })
        })
        .catch(err => notify({ type: 'error', message: err.message }))
    }

    const removeNote = async (note: NoteQueryResult) => {
      const delNote = {
        reportId: id,
        id: note.id ?? 0
      }
      await $axios
        .delete(`reports/${delNote.reportId}/note/${delNote.id}`)
        .then(() => {
          currentReport.value.notes = currentReport.value.notes!.filter(
            n => n.id !== delNote.id
          )
        })
    }

    const getCheckIcon = (value: CheckValue) => {
      switch (value) {
        case CheckValue.NotAcceptableFalse:
          return 'close'
        case CheckValue.Acceptable:
          return 'check'
        case CheckValue.NotApplicable:
          return 'N.A.'
        case CheckValue.None:
          return 'new-box'
        default:
          return ''
      }
    }

    const getCheckIconColor = (value: CheckValue) => {
      switch (value) {
        case CheckValue.NotAcceptableFalse:
          return 'error'
        case CheckValue.Acceptable:
          return 'success'
        case CheckValue.NotApplicable:
          return 'black'
        default:
          return 'info'
      }
    }

    const saveNote = (note: NoteQueryResult) => {
      const data: EditNoteCommand = {
        reportId: parseInt(route.value.params.id),
        id: note.id!,
        text: note.text!,
        checked: note.checked!
      }
      $axios.put(`reports/${data.reportId}/note/${note.id}`, data)
    }

    const saveCheckItem = async (checkItem: CheckListItemQueryResult) => {
      const command: UpdateCheckListItemCommand = {
        id: checkItem.id!,
        checkListId: checkItem.checkListId!,
        text: checkItem.text!,
        required: checkItem.required!,
        checked: checkItem.checked as unknown as CheckValue,
        editable: checkItem.editable!,
        remarks: checkItem.remarks!
      }
      await $axios.put(
        `checklists/${command.checkListId}/items/${checkItem.id}`,
        command
      )
    }

    const saveReportChanges = async (currentReport: ReportQueryResult) => {
      pageOptions.savingNewReport = true
      const update: UpdateReportCommand = {
        id: currentReport.id,
        name: currentReport.name,
        address: currentReport.address ?? pageOptions.search,
        date: currentReport.date,
        licenseNumber: currentReport.licenseNumber,
        isClosed: currentReport.isClosed
      }
      await $axios.put(`reports/${route.value.params.id}`, update)
      store.dispatch('hasPendingChanges', false)
    }

    const checkItemChecks = (checkListId: number, value: CheckValue): void => {
      try {
        if ($reportsApi) {
          $reportsApi.bulkUpdateChecks(
            currentReport.value.id!,
            checkListId,
            parseInt(value.toString())
          )
          const checkList = currentReport.value.checkLists!.find(
            c => c.id === checkListId
          )
          if (!checkList) {
            return
          }
          checkList.checks!.forEach((check: any) => {
            check.checked = value
            check.touched = true
          })
        }
      } catch (error) {
        notify({
          title: 'Report Details',
          defaultMessage: 'Error Updating Checklist',
          error
        })
      }
    }

    const setLicenseFromAddress = () => {
      if (!currentReport.value.address) {
        currentReport.value.licenseNumber = ''
        return
      }
      const addressData = addresses.value.filter(
        a => a.formatedAddress === currentReport.value.address
      )
      if (addressData) {
        currentReport.value.licenseNumber = addressData[0].number
        currentReport.value.licenseVolt = addressData[0].volt
        currentReport.value.licenseAmp = addressData[0].amp
        currentReport.value.licenseValidity = addressData[0].validity
        currentReport.value.licenseName = addressData[0].name
        currentReport.value.licenseKVA = addressData[0].kva
      }
    }

    const closeReport = async () => {
      currentReport.value.isClosed = true
      await saveReportChanges(currentReport.value)
      pageOptions.dialogClose = false
      currentReport.value.isClosed = true
      pageOptions.dialogPrinting = true
      await generatePdf(currentReport.value)
      pageOptions.dialogPrinting = false
    }

    const saveAndLoad = async () => {
      await saveReportChanges(currentReport.value)
      await loadReport()
    }

    const generatePdf = async (item: any, printPhotos: boolean = false) => {
      const file = await $axios.$get(
        `reports/${item.id}/export?printPhotos=${printPhotos}`,
        { responseType: 'blob' }
      )
      downloadFile(
        file,
        printPhotos
          ? `compunded_photo_record_${item.name}`
          : `report_${item.name}`
      )
    }

    const downloadFile = (blob: Blob, name: any): void => {
      const link = document.createElement('a')
      link.target = '_blank'
      link.href = window.URL.createObjectURL(blob)
      link.download = `${name}`
      link.click()
    }

    return {
      currentReport,
      pageOptions,
      addresses,
      CanCloseReport,
      HasNotesWithPendingChecks,
      hasUntouchedChecks,
      PrincipalSignatureHasAResponsable,
      isMultiLine,
      IsValidForm,
      saveAndLoad,
      closeReport,
      setLicenseFromAddress,
      checkItemChecks,
      saveCheckItem,
      saveNote,
      getCheckIconColor,
      getCheckIcon,
      removeNote,
      addNote,
      saveReportChanges,
      obs,
      obsSignatures,
      signatures,
      forms
    }
  }
})
</script>

<style scoped>
.v-btn--example2 {
  bottom: 0;
  /* position: absolute; */
  margin: 0 62px 16px 16px;
}
</style>
