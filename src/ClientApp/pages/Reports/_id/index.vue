<template>
  <div id="report">
    <alert-dialog
      v-model="dialogClose"
      title="Close Report"
      message="you are about to close this report."
      :code="currentReport.id"
      :description="currentReport.name"
      :loading="savingNewReport"
      @yes="closeReport"
      @no="currentReport.isClosed = true"
    />
    <v-row>
      <v-col>
        <ValidationObserver ref="obs" v-slot="{ valid }" tag="form">
          <v-row align="center" justify="space-between">
            <v-col cols="8">
              <ValidationProvider v-slot="{ errors }" rules="required" immediate>
                <DatePickerBase
                  v-model="currentReport.date"
                  type="number"
                  label="License"
                  :error-messages="errors"
                  :max="new Date().toISOString()"
                />
              </ValidationProvider>
            </v-col>
            <v-col cols="4" class="text-right">
              <h4>{{ currentReport.title }}</h4>
              <h6>{{ currentReport.formName }}</h6>
            </v-col>
          </v-row>
          <v-row align="center" justify="space-between">
            <v-col cols="12" md="9">
              <ValidationProvider v-slot="{ errors }" rules="required" immediate>
                <v-autocomplete
                  v-model="currentReport.address"
                  :error-messages="errors"
                  :items="addresses"
                  :loading="searchingAddresses"
                  :search-input.sync="search"
                  hide-selected
                  item-text="formatedAddress"
                  item-value="formatedAddress"
                  label="Inspection Address"
                  placeholder="Start typing to Search"
                  prepend-icon="mdi-crosshairs-gps"
                  clearable
                  autocomplete="nope"
                  @keypress="isDirty = true"
                  @change="setLicenseFromAddress"
                />
              </ValidationProvider>
            </v-col>
            <v-col v-if="currentReport.license" cols="12" md="3">
              <ValidationProvider v-slot="{ errors }" rules="required" immediate>
                <v-text-field
                  v-model="currentReport.license.number"
                  readonly
                  :error-messages="errors"
                  label="License"
                />
              </ValidationProvider>
            </v-col>
          </v-row>
          <v-row align="center" justify="space-between">
            <v-col cols="6" md="2" class="text-right">
              <v-checkbox
                v-if="currentReport.isClosed"
                v-model="currentReport.isClosed"
                label="Completed With Signatures"
                :disabled="currentReport.isClosed || !CanCloseReport"
                @click="dialogClose = true"
              />
              <v-btn
                v-else
                :disabled="!valid || currentReport.isClosed || !CanCloseReport"
                color="success"
                class="v-btn--example2"
                @click="dialogClose = true; currentReport.isClosed=true;"
              >
                <v-icon>mdi-lock</v-icon>
                Complete Report
              </v-btn>
            </v-col>
          </v-row>
          <v-fab-transition>
            <v-btn
              v-if="tabs!=='photos'"
              color="success"
              fab
              fixed
              dark
              bottom
              right
              :loading="savingNewReport"
              class="v-btn--example2"
              @click="saveReportChanges().then(CanCloseReport && !currentReport.isClosed ? dialogClose = true: !CanCloseReport ? errorsDialog = true : errorsDialog = false)"
            >
              <v-icon>mdi-content-save</v-icon>
            </v-btn>
          </v-fab-transition>
        </ValidationObserver>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-tabs v-model="tabs" centered fixed-tabs icons-and-text>
          <v-tabs-slider />
          <v-tab href="#checklists" :class="!IsCompleted || HasNotesWithPendingChecks || !PrincipalSignatureHasAResponsable > 0 ? 'error--text':'primary--text'">
            Report Details
            <v-tooltip v-if="!IsCompleted || HasNotesWithPendingChecks || !PrincipalSignatureHasAResponsable" top>
              <template v-slot:activator="{ on }">
                <v-icon :color="!IsCompleted || HasNotesWithPendingChecks || !PrincipalSignatureHasAResponsable ? 'error':''" v-on="on">
                  mdi-message-bulleted
                </v-icon>
              </template>
              <span>You must complete all the required checks, notes and signatures in tab Report Details to proceeded uploading photos</span>
            </v-tooltip>
            <v-icon v-else>
              mdi-message-bulleted
            </v-icon>
          </v-tab>
          <v-tab href="#photos">
            Photo Record
            <v-icon>
              mdi-folder-multiple-image
            </v-icon>
          </v-tab>
        </v-tabs>
        <v-tabs-items v-model="tabs" touchless>
          <v-tab-item key="checklists" value="checklists">
            <v-expansion-panels
              multiple
              focusable
              :value="openedPanels"
            >
              <v-expansion-panel>
                <v-expansion-panel-header :style="!IsCompleted ? 'color: white;':''" :color="!IsCompleted ? 'red':''">
                  <span class="font-weight-black">Check List</span>
                  <v-row v-if="!IsCompleted" dense>
                    <v-col>
                      <v-icon dark>
                        mdi-alert-circle
                      </v-icon>
                      <span style="color: white;" class="font-weight-accent">
                        there are items that need to be completed or checked
                      </span>
                    </v-col>
                  </v-row>
                </v-expansion-panel-header>
                <v-expansion-panel-content>
                  <v-list subheader two-line flat dense>
                    <v-list-item
                      v-for="(item, checkListIndex) in currentReport.checkList"
                      :key="item.id"
                    >
                      <template v-slot:default>
                        <v-list-item-content class="text-left">
                          <v-list-item-title>
                            <v-row justify="start" align="center" dense>
                              <v-col cols="10" md="6" class="font-weight-black text-wrap">
                                <h3>
                                  {{ checkListIndex + 1 }} .- {{ item.text }} {{ item.annotation }}
                                </h3>
                                <span v-if="item.checks.filter(c => c.required && c.checked==3).length == 0">
                                  <v-chip x-small color="success">Completed</v-chip>
                                </span>
                              </v-col>
                              <v-col cols="2" md="6" :class="$vuetify.breakpoint.mdAndDown ? 'ml-n5':'ml-n6'">
                                <v-btn
                                  icon
                                  text
                                  @click.stop="item.checked < 3 ? item.checked++:item.checked=0;checkItemChecks(item.id, item.checked);"
                                >
                                  <v-icon :color="item.checks.length !== item.checks.filter(c => c.checked == 1).length && item.checks.filter(c => c.checked == 1).length > 0 ? getCheckIconColor(2): getCheckIconColor(item.checks[0].checked)">
                                    {{ `mdi-${item.checks.length !== item.checks.filter(c => c.checked == 1).length && item.checks.filter(c => c.checked == 1).length > 0 ? 'minus': getCheckIcon(item.checks[0].checked) }` }}
                                  </v-icon>
                                </v-btn>
                              </v-col>
                            </v-row>
                            <v-row dense>
                              <v-col cols="12">
                                <v-list-item
                                  v-for="(checkItem, checkListItemIndex) in item.checks"
                                  :key="checkItem.id"
                                >
                                  <template v-slot:default="{ }">
                                    <v-list-item-title :title="checkItem.text">
                                      <v-row dense align="center" justify="space-between">
                                        <v-col cols="10">
                                          <v-row dense align="center" justify="space-between" :class="{ ml_5: true }">
                                            <v-col
                                              cols="12"
                                              md="7"
                                              class="text-wrap"
                                            >
                                              <v-row justify="space-around" align="center" dense>
                                                <v-col cols="1" :class="['text-right', checkItem.checked == 3 && shouldShowRequired ? 'error--text' : '']">
                                                  <h3>{{ checkListIndex + 1 }}.{{ checkListItemIndex + 1 }} </h3>
                                                </v-col>
                                                <v-col cols="10" :class="['text-left', checkItem.checked == 3 && shouldShowRequired ? 'error--text' : '']">
                                                  <h3 v-if="!checkItem.editable">
                                                    .-{{ checkItem.text }} <v-chip v-if="!checkItem.editable && checkItem.checked == 3" class="text-uppercase" :color="shouldShowRequired ? 'error':''" x-small>
                                                      required
                                                    </v-chip>
                                                  </h3>
                                                  <v-text-field v-else v-model="checkItem.text" @blur="saveCheckItem(checkItem)">
                                                    <template v-slot:append="">
                                                      <v-chip v-if="checkItem.checked == 3" x-small class="text-uppercase" :color="shouldShowRequired ? 'error':''">
                                                        required
                                                      </v-chip>
                                                    </template>
                                                  </v-text-field>
                                                </v-col>
                                              </v-row>
                                            </v-col>
                                            <v-col cols="2" md="1">
                                              <v-btn
                                                icon
                                                text
                                                @click.stop="checkItem.checked < 2 ? checkItem.checked++:checkItem.checked=0;saveCheckItem(checkItem);">
                                                <v-icon :color="getCheckIconColor(checkItem.checked)">
                                                  {{ `mdi-${getCheckIcon(checkItem.checked)}` }}
                                                </v-icon>
                                              </v-btn>
                                            </v-col>
                                            <v-col cols="10" md="4">
                                              <v-text-field
                                                v-model="checkItem.remarks"
                                                :label="currentReport.remarksLabelText"
                                                @blur="saveCheckItem(checkItem)"
                                              />
                                            </v-col>
                                          </v-row>
                                        </v-col>
                                        <v-col cols="2">
                                          <v-btn
                                            v-if="showUpdateCheck.findIndex(l => l.currentIndex===checkListItemIndex && l.parentIndex === checkListIndex)>=0"
                                            color="amber"
                                            icon
                                            @click="showUpdateCheck.splice(showUpdateCheck.findIndex(l => l.currentIndex===checkListItemIndex && l.parentIndex === checkListIndex),1)"
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
              <v-expansion-panel>
                <v-expansion-panel-header :style="HasNotesWithPendingChecks ? 'color: white;':''" :color="HasNotesWithPendingChecks ? 'red':''">
                  <span class="font-weight-black">Notes</span>
                  <v-row v-if="HasNotesWithPendingChecks" dense>
                    <v-col>
                      <v-icon dark>
                        mdi-alert-circle
                      </v-icon>
                      <span style="color: white;" class="font-weight-accent">
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
                  <div v-for="(note, index) in currentReport.notes" :key="index">
                    <v-row dense align="center" justify="space-around">
                      <v-col cols="1">
                        <v-btn
                          class="mx-2"
                          title="delete this note"
                          icon
                          text
                          :disabled="note.needsCheck"
                          color="error"
                          @click="removeNote(note.id)"
                        >
                          <v-icon dark>
                            mdi-minus
                          </v-icon>
                        </v-btn>
                      </v-col>
                      <v-col cols="8">
                        <v-text-field v-model="note.text" label="Note" @blur="saveNote(note)" />
                      </v-col>
                      <v-col cols="2">
                        <v-checkbox v-model="note.checked" :label="note.needsCheck ? '(Check Required)':''" @change="saveNote(note)" />
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
                <SignaturesForm v-model="currentReport.signatures" :is-closed="false" />
              </v-col>
            </v-row>
          </v-tab-item>
          <v-tab-item key="photos" value="photos">
            <PhotoRecords v-model="currentReport" @uploaded="saveAndLoad()" />
          </v-tab-item>
        </v-tabs-items>
      </v-col>
    </v-row>
    <message-dialog v-model="errorsDialog" :actions="['no']" no-text="close" @no="errorsDialog = false">
      <template v-slot:title="{}">
        Report Errors
      </template>
      <h2>The Report has been saved! but we've found some errors: </h2><br>
      <span v-if="!IsCompleted" class="subtitle-1 error--text font-weight-black">- Please, Complete and check all the required item in the checklist</span><br>
      <span v-if="HasNotesWithPendingChecks" class="subtitle-1 error--text font-weight-black">- There are notes that you need to check. verify if it needs to be checked and filled</span><br>
      <span v-if="!PrincipalSignatureHasAResponsable" class="subtitle-1 error--text font-weight-black">- The Principal Signature must have an responsable name and type</span>
    </message-dialog>
    <v-dialog
      v-model="dialogPrinting"
      hide-overlay
      persistent
      width="300"
    >
      <v-card
        color="primary"
        dark
      >
        <v-card-text>
          Processing and Downloading pdf report
          <v-progress-linear
            indeterminate
            color="white"
            class="mb-0"
          />
        </v-card-text>
      </v-card>
    </v-dialog>
    <v-snackbar
      v-model="savedNotification"
      top
      centered
      color="success"
      :timeout="3000"
    >
      Changes has been saved

      <template v-slot:action="{ attrs }">
        <v-btn
          dark
          text
          v-bind="attrs"
          @click="savedNotification = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script lang="ts">
import { Component, Watch, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import InnerPageMixin from '@/mixins/innerpage'
import { PrintHelper } from '@/Helpers'
import { AddressesState } from '@/store/addresses'
import {
  Report,
  EMALicenseType,
  Note,
  Signature,
  ResponsableType,
  EMALicense,
  DeleteNoteCommand,
  AddNoteCommand,
  UpdateReportCommand,
  EditNoteCommand,
  EditSignatureCommand, CheckListItem, UpdateCheckListItemCommand, CheckValue,
  AddressDTO
} from '@/types'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class EditReport extends mixins(InnerPageMixin) {
  $refs!: {
    obs: InstanceType<typeof ValidationObserver>
  }

  errorsDialog: boolean = false
  dialogPrinting: boolean = false
  savedNotification: boolean = false
  shouldShowRequired: boolean = false

  search: string = ''
  isDirty: boolean = false
  searchingAddresses: boolean = false
  openedPanels: number[] = [0, 1]
  printHelper!: PrintHelper
  savingNewReport: boolean = false
  currentPhoto: number = 0
  showLabelEdit:number[] = []
  showUpdateCheck: { parentIndex:number, currentIndex:number }[] =[]
  tabs: any = 0;
  dialogClose: boolean = false;
  currentReport: Report = {} as Report;
  emaTypes: any = Object.keys(EMALicenseType)
    .map((key: any) => {
      if (!isNaN(Number(key.toString()))) { return }

      return { id: EMALicenseType[key], text: key }
    })
    .filter(i => i !== undefined)

  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api', '')
  signaturesChanges: boolean = false

  get hasPendingChanges () {
    return this.$store.state.showFabSaveButton
  }

  get localAddress (): AddressDTO[] {
    return [{ formatedAddress: this.currentReport.address } as AddressDTO]
  }

  get addresses (): AddressDTO[] {
    const dbAddressses = (this.$store.state.addresses as AddressesState).addressList

    if (dbAddressses.length > 0) { return dbAddressses }

    return this.localAddress
  }

  async addNote () {
    const newNote: AddNoteCommand = {
      reportId: this.currentReport.id,
      text: '',
      checked: false,
      needsCheck: false
    }
    await this.$axios.$post(`reports/${this.$route.params.id}/note`, newNote).then((resp) => {
      this.currentReport.notes.push({ id: resp, ...newNote })
    })
      .catch(err => alert(err))
  }

  async removeNote (id: number) {
    const delNote:DeleteNoteCommand = {
      reportId: this.currentReport.id,
      id
    }
    await this.$axios.delete(`reports/${delNote.reportId}/note/${delNote.id}`).then(() => {
      this.currentReport.notes = this.currentReport.notes.filter(n => n.id !== id)
    })
  }

  editCheck (parentIndex: number, currentIndex:number) {
    const index = this.showUpdateCheck.findIndex(l => l.parentIndex === parentIndex &&
                                                  l.currentIndex === currentIndex)
    if (index >= 0) {
      this.showUpdateCheck.splice(index, 1)
      return
    }
    this.showUpdateCheck.push({ parentIndex, currentIndex })
  }

  getCheckIcon (value: CheckValue) {
    switch (value) {
      case CheckValue.NotAcceptable:
        return 'close'
      case CheckValue.Acceptable:
        return 'check'
      case CheckValue.NotApplicable:
        return 'minus'
      default:
        return ''
    }
  }

  getCheckIconColor (value: CheckValue) {
    switch (value) {
      case CheckValue.NotAcceptable:
        return 'error'
      case CheckValue.Acceptable:
        return 'success'
      case CheckValue.NotApplicable:
        return 'warning'
      default:
        return 'indigo'
    }
  }

  getCheckName (id: number) {
    return CheckValue[id]
  }

  saveNote (note: Note) {
    const data: EditNoteCommand = {
      reportId: parseInt(this.$route.params.id),
      id: note.id,
      text: note.text,
      checked: note.checked
    }
    this.$axios.put(`reports/${data.reportId}/note/${note.id}`, data)
  }

  saveCheckItem (checkItem: CheckListItem) {
    const command: UpdateCheckListItemCommand = {
      id: checkItem.id,
      checkListId: checkItem.checkListId,
      text: checkItem.text,
      required: checkItem.required,
      checked: parseInt(checkItem.checked as any),
      editable: checkItem.editable,
      remarks: checkItem.remarks
    }
    this.$axios.put(`checklists/${command.checkListId}/items/${checkItem.id}`, command)
  }

  async saveReportChanges () {
    this.savingNewReport = true
    const update: UpdateReportCommand = {
      id: this.currentReport.id,
      name: this.currentReport.name,
      address: this.currentReport.address ?? this.search,
      date: this.currentReport.date,
      licenseType: this.currentReport.license.licenseType,
      licenseNumber: this.currentReport.license.number,
      isClosed: this.currentReport.isClosed
    }
    await this.$axios.put(`reports/${this.$route.params.id}`, update).then(() => {
      this.$store.dispatch('hasPendingChanges', false)
      this.currentReport.signatures.forEach((signature) => {
        const command: EditSignatureCommand = {
          id: signature.id,
          title: signature.title,
          annotation: signature.annotation,
          responsableType: signature.responsable.type,
          responsableName: signature.responsable.name,
          designation: signature.designation,
          remarks: signature.remarks,
          date: signature.date,
          principal: signature.principal,
          drawedSign: signature.drawedSign
        }
        this.$axios.$put(`signatures/${signature.id}`, command)
      })
      this.savedNotification = true
    })
    this.savingNewReport = false
    this.shouldShowRequired = true
  }

  @Watch('currentReport', { deep: false })
  onReportChanged (value: Report, oldValue: Report) {
    if (value !== oldValue && oldValue !== undefined) { this.$store.dispatch('hasPendingChanges', true) }
  }

  async fetch () {
    await this.loadReport()
    await this.getSuggestedAddresses('')
  }

  async loadReport () {
    const result = await this.$store.dispatch(
      'reportstrore/getReportById',
      this.$route.params.id,
      { root: true }
    )

    result.signatures.map((signature: Signature) => {
      if (signature.responsable === null || signature.responsable === undefined) { signature.responsable = { type: 0, name: '' } } else {
        signature.responsable = {
          type: ResponsableType[signature.responsable.type] as unknown as ResponsableType,
          name: signature.responsable.name
        }
      }
    })

    this.currentReport = Object.assign({}, result)
    if (!this.currentReport.license) { this.currentReport.license = { } as EMALicense } else { this.currentReport.license.licenseType = EMALicenseType[this.currentReport.license.licenseType] as unknown as EMALicenseType }

    this.search = this.currentReport.address

    await this.$store.dispatch('users/setUserLastEditedReport', { userName: this.$auth.user.userName, lastEditedReport: this.$route.params.id }, { root: true })
    await this.$auth.fetchUser()
  }

  get IsValidForm () {
    return this.$refs.obs.flags.valid
  }

  get IsCompleted () {
    if (this.currentReport.checkList) {
      return this.currentReport.checkList.filter(cl => cl.checks.findIndex(c => c.checked == 3) >= 0).length === 0
    }

    return false
  }

  get PrincipalSignatureHasAResponsable () {
    if (!this.currentReport) { return true }
    if (!this.currentReport!.signatures) { return true }
    return this.currentReport.signatures.findIndex(s => s.responsable.type !== undefined && s.responsable.name !== '' && s.principal) >= 0
  }

  get HasNotesWithPendingChecks () {
    if (!this.currentReport) { return false }
    if (!this.currentReport!.notes) { return false }
    return this.currentReport.notes.findIndex(n => n.needsCheck && !n.checked) >= 0
  }

  get CanCloseReport () {
    return this.IsCompleted && !this.HasNotesWithPendingChecks && this.PrincipalSignatureHasAResponsable && this.IsValidForm
  }

  checkItemChecks (checkListId: number, value: CheckValue): void {
    const checkList = this.currentReport.checkList.find(c => c.id === checkListId)
    if (!checkList) { return }
    checkList.checks.forEach((check) => {
      check.checked = value
      this.saveCheckItem(check)
    })
  }

  checkListCheckedValue (item: any) {
    return item.checks.length === item.checks.filter((c:any) => c.checked).length
  }

  setLicenseFromAddress () {
    if (!this.currentReport.address) {
      this.currentReport!.license.number = ''
      return
    }
    const addressData = this.addresses.filter(a => a.formatedAddress === this.currentReport.address)
    if (addressData) { this.currentReport!.license.number = addressData[0].number ?? '' }
  }

  mounted () {
    this.printHelper = new PrintHelper(this.$store)
    return this.$refs.obs.validate()
  }

  async getSuggestedAddresses (filter: string) {
    await this.$store.dispatch('addresses/getAddresses', { filter }, { root: true })
    this.searchingAddresses = false
    this.isDirty = false
  }

  async closeReport () {
    this.currentReport.isClosed = true
    await this.saveReportChanges()
    this.dialogClose = false
    this.currentReport.isClosed = true
    this.dialogPrinting = true
    await this.printHelper.print(this.currentReport.id)
    this.dialogPrinting = false
    this.$router.push('/reports')
  }

  async saveAndLoad () {
    await this.saveReportChanges()
    await this.loadReport()
  }

  @Watch('search')
  onSearchChanged (value: string) {
    if (!value || !this.isDirty) { return }

    if (this.searchingAddresses) { return }

    if (value.length >= 3) {
      this.searchingAddresses = true
      const self = this
      self.getSuggestedAddresses(value)
    }
  }
}
</script>

<style scoped>
.v-btn--example2 {
    bottom: 0;
    /* position: absolute; */
    margin: 0 62px 16px 16px;
  }
</style>
