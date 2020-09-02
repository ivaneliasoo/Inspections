<template>
  <div id="report">
    <alert-dialog
      v-model="dialogClose"
      title="Close Report"
      message="your are about to close this report."
      :code="currentReport.id"
      :description="currentReport.name"
      :loading="savingNewReport"
      @yes="currentReport.isClosed = true; saveReportChanges(); dialogClose = false; currentReport.isClosed = true;"
      @no="currentReport.isClosed = true"
    />
    <v-row>
      <v-col>
        <ValidationObserver ref="obs" tag="form" v-slot="{ valid }">
          <v-row align="center" justify="space-between">
            <v-col cols="8" >
              <ValidationProvider rules="required" immediate  v-slot="{ errors }">
              <DatePickerBase type="number" label="License"
                :disabled="currentReport.isClosed"
                v-model="currentReport.date"
                :error-messages="errors"
               :max="new Date().toISOString()" />
              </ValidationProvider>
            </v-col>
            <v-col cols="4" class="text-right">
              <h4>{{ currentReport.title }}</h4>
              <h6>{{ currentReport.formName }}</h6>
            </v-col>
          </v-row>
          <v-row align="center" justify="space-between">
            <v-col cols="12" md="9">
              <ValidationProvider rules="required" immediate  v-slot="{ errors }">
                <v-autocomplete
                  :readonly="currentReport.isClosed"
                  v-model="currentReport.address"
                  :error-messages="errors"
                  :items="addresses"
                  :loading="searchingAddresses"
                  :search-input.sync="search"
                  @keypress="isDirty = true"
                  hide-selected
                  item-text="formatedAddress"
                  item-value="formatedAddress"
                  label="Inspection Address"
                  placeholder="Start typing to Search"
                  prepend-icon="mdi-crosshairs-gps"
                />
              </ValidationProvider>
            </v-col>
            <v-col cols="12" md="3" v-if="currentReport.license">
              <ValidationProvider rules="required" immediate  v-slot="{ errors }">
              <v-text-field 
                :readonly="currentReport.isClosed"
                v-model="currentReport.license.number"
                :error-messages="errors"
               label="License" />
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
              v-if="valid && !currentReport.isClosed"
              color="success"
              fab
              fixed
              dark
              bottom
              right
              :loading="savingNewReport"
              class="v-btn--example2"
              @click="saveReportChanges().then(CanCloseReport ? dialogClose = true: undefined)"
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
          <v-tabs-slider></v-tabs-slider>
          <v-tab href="#checklists" class="primary--text">
            Report Details
            <v-icon>mdi-message-bulleted</v-icon>
          </v-tab>

          <v-tab v-if="false" href="#notes" class="primary--text">
            <v-icon>mdi-message-bulleted</v-icon>
          </v-tab>

          <v-tab href="#photos" class="primary--text">
            Photo Record
            <v-icon>mdi-folder-multiple-image</v-icon>
          </v-tab>
          <v-tab v-if="false" href="#signatures" class="primary--text">
            <v-icon>mdi-signature-freehand</v-icon>
          </v-tab>
        </v-tabs>
        <v-tabs-items v-model="tabs">
          <v-tab-item key="checklists" value="checklists">
            <v-expansion-panels
          multiple
          focusable
          :value="openedPanels"
        >
          <v-expansion-panel>
            <v-expansion-panel-header>
              <span>Check List</span>
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
                          <v-row justify="start" align="center">
                            <v-col cols="10" md="6" class="font-weight-black">
                              {{ checkListIndex + 1 }} .- {{ item.text }} {{ item.annotation }}
                              <span v-if="item.checks.filter(c => c.required && c.checked===0).length == 0">
                                <v-chip x-small color="success">Completed</v-chip>
                              </span>
                            </v-col>
                            <v-col cols="2" md="6" :class="$vuetify.breakpoint.mdAndDown ? 'ml-n5':'ml-n8'">
                              <v-checkbox
                                :disabled="currentReport.isClosed"
                                color="primary"
                                v-model="item.checked"
                                :indeterminate="item.checks.length !== item.checks.filter(c => c.checked).length && item.checks.filter(c => c.checked).length > 0"
                                @click.stop=" item.checked=!item.checked; checkItemChecks(item.id, item.checked)"
                              />
                            </v-col>
                          </v-row>
                          <v-row>
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
                                              <v-col cols="1"><span>{{ checkListIndex + 1 }}.{{ checkListItemIndex + 1}} .- </span></v-col>
                                              <v-col cols="10">
                                                
                                                <span v-if="!checkItem.editable">{{ checkItem.text }}</span>
                                                <v-chip v-if="!checkItem.editable && checkItem.required" x-small>required</v-chip>
                                                <v-text-field v-else v-model="checkItem.text" @blur="saveCheckItem(checkItem)">
                                                  <template v-slot:append="">
                                                    <v-chip x-small v-if="checkItem.required">required</v-chip>
                                                  </template>
                                                </v-text-field>
                                              </v-col>
                                            </v-row>
                                          </v-col>
                                          <v-col cols="2" md="1">
                                            <v-checkbox
                                              :disabled="currentReport.isClosed"
                                              color="primary"
                                              v-model="checkItem.checked"
                                              :indeterminate="checkItem.checked===2"
                                              @click.stop="checkItem.checked < 2 ? checkItem.checked++:checkItem.checked=0; saveCheckItem(checkItem)"
                                            />
                                          </v-col>
                                          <v-col cols="10" md="4">
                                            <v-text-field
                                              :readonly="currentReport.isClosed"
                                              v-model="checkItem.remarks"
                                              :label="currentReport.remarksLabelText"
                                              @blur="saveCheckItem(checkItem)"
                                             />

                                          </v-col>
                                        </v-row>
                                      </v-col>
                                      <v-col cols="2">
                                        <v-btn color="amber" v-if="showUpdateCheck.findIndex(l => l.currentIndex===checkListItemIndex && l.parentIndex === checkListIndex)>=0"
                                          icon
                                          @click="showUpdateCheck.splice(showUpdateCheck.findIndex(l => l.currentIndex===checkListItemIndex && l.parentIndex === checkListIndex),1)">
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
            <v-expansion-panel-header>
              Notes
            </v-expansion-panel-header>
            <v-expansion-panel-content>
              <v-row justify="end" align="end" class="text-right">
                <v-col cols="12">
                  <v-btn class="mx-2" x-small
                            fab dark color="primary"
                            title="Add note"
                            @click="addNote">
                            <v-icon dark>mdi-plus</v-icon>
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
                            :disabled="note.needsCheck || currentReport.isClosed"
                            color="error"
                            @click="removeNote(note.id)"
                            >
                            <v-icon dark>mdi-minus</v-icon>
                            </v-btn>
                      </v-col>
                    <v-col cols="8">
                      <v-text-field v-model="note.text" :readonly="currentReport.isClosed" label="Note" @blur="saveNote(note)" />
                    </v-col>
                    <v-col cols="2">
                        <v-checkbox v-model="note.checked" :disabled="currentReport.isClosed" @change="saveNote(note)" :label="note.needsCheck ? '(Check Required)':''" />
                    </v-col>
                    <v-col>
                      
                    </v-col>
                  </v-row>
                </div>
                <v-row class="mt-10">
                    
                </v-row>
            </v-expansion-panel-content>
          </v-expansion-panel>
        </v-expansion-panels>
            <v-row>
              <v-col>
                <h2 class="text-left">Signatures</h2>
                <SignaturesForm v-model="currentReport.signatures" :is-closed="currentReport.isClosed"/>
              </v-col>
            </v-row>
          </v-tab-item>
          <v-tab-item key="photos" value="photos">
            <PhotoRecords v-model="currentReport" @uploaded="loadReport" />
          </v-tab-item>
        </v-tabs-items>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        
      </v-col>
    </v-row>
    
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch, mixins } from "nuxt-property-decorator";
import InnerPageMixin from '@/mixins/innerpage'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { AddressesState } from '@/store/addresses'
import { Report, 
EMALicenseType, 
Note, 
Signature, 
ResponsableType, 
PhotoRecord, 
EMALicense, 
DeleteNoteCommand, 
AddNoteCommand, 
UpdateReportCommand, 
EditNoteCommand, 
EditPhotoRecordCommand, 
EditSignatureCommand, CheckList, CheckListItem, UpdateCheckListItemCommand, DeletePhotoRecordCommand, CheckValue,
AddressDTO } from "@/types";

import { debounce } from '@/Helpers/index'

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

  search: string = ''
  isDirty: boolean = false
  searchingAddresses: boolean = false
  openedPanels: number[] = [0,1]
  
  savingNewReport: boolean = false
  currentPhoto: number = 0
  showLabelEdit:number[] = []
  showUpdateCheck: { parentIndex:number, currentIndex:number }[] =[]
  tabs: any = 0;
  dialogClose: boolean = false;
  currentReport: Report = {} as Report;
  emaTypes: any = Object.keys(EMALicenseType)
    .map((key: any) => {
      if (!isNaN(Number(key.toString()))) return;

      return { id: EMALicenseType[key], text: key };
    })
    .filter(i => i !== undefined)

  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api','')
  signaturesChanges: boolean = false

  get hasPendingChanges() {
    return this.$store.state.showFabSaveButton
  }
  get localAddress(): AddressDTO[] {
    return [{ formatedAddress: this.currentReport.address }]
  }
  get addresses(): AddressDTO[] {
    const dbAddressses = (this.$store.state.addresses as AddressesState).addressList;
    
    if(dbAddressses.length >0)
      return dbAddressses
    
    return this.localAddress
  }

  async addNote() {
      const newNote: AddNoteCommand = {
          reportId: this.currentReport.id,
          text: "",
          checked: false,
          needsCheck: false
      }
    this.$axios.$post(`reports/${this.$route.params.id}/note`, newNote).then((resp) =>{
      this.currentReport.notes.push({id: resp, ...newNote})
    })
    .catch(err => alert(err))
  }

  async removeNote(id: number) {
      const delNote:DeleteNoteCommand = {
        reportId: this.currentReport.id,
        id
      }
      this.$axios.delete(`reports/${delNote.reportId}/note/${delNote.id}`).then(() =>{
      this.currentReport.notes =  this.currentReport.notes.filter(n=>n.id !== id)
    })

  }

  editCheck(parentIndex: number, currentIndex:number) {
    const index = this.showUpdateCheck.findIndex(l => l.parentIndex === parentIndex
                                                  && l.currentIndex === currentIndex )
    if(index>=0) {
      this.showUpdateCheck.splice(index, 1)
      return
    }
    this.showUpdateCheck.push({parentIndex, currentIndex})
  }

  saveNote(note: Note) {
    const data: EditNoteCommand = {
      reportId: parseInt(this.$route.params.id),
      id: note.id,
      text: note.text,
      checked: note.checked
    }
    this.$axios.put(`reports/${data.reportId}/note/${note.id}`,  data)
  }

  saveCheckItem(checkItem: CheckListItem) {
    const command: UpdateCheckListItemCommand = {
      id: checkItem.id,
      checkListId: checkItem.checkListId,
      text: checkItem.text,
      required: checkItem.required,
      checked: parseInt(checkItem.checked as any),
      editable: checkItem.editable,
      remarks: checkItem.remarks
    }
    this.$axios.put(`checklists/${command.checkListId}/items/${checkItem.id}`,  command)
  }

  async saveReportChanges() {
    this.savingNewReport = true
    const update: UpdateReportCommand = {
      id:this.currentReport.id,
      name:this.currentReport.name,
      address:this.currentReport.address ?? this.search,
      date:this.currentReport.date,
      licenseType:this.currentReport.license.licenseType,
      licenseNumber:this.currentReport.license.number,
      isClosed:this.currentReport.isClosed,
    }
    await this.$axios.put(`reports/${this.$route.params.id}`, update).then(() =>{
      this.$store.dispatch('hasPendingChanges', false)
      this.currentReport.signatures.forEach(signature => {
          const command: EditSignatureCommand =  {
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
    })
    this.savingNewReport=false
  }

  @Watch('currentReport', { deep: false })
  onReportChanged(value: Report, oldValue: Report) {
    if(value !== oldValue && oldValue !== undefined)
      this.$store.dispatch('hasPendingChanges', true)
  }

  async fetch() {
    await this.loadReport()
  }

  async loadReport() {
     const result = await this.$store.dispatch(
      "reportstrore/getReportById",
      this.$route.params.id,
      { root: true }
    );

    result.signatures.map((signature: Signature) => {
      if(signature.responsable === null || signature.responsable === undefined)
        signature.responsable = { type: 0, name: ''}
      else
        signature.responsable = { 
          type:  ResponsableType[signature.responsable.type]  as unknown as ResponsableType,
          name: signature.responsable.name
        }
    });

    this.currentReport = Object.assign({}, result);
    if(!this.currentReport.license)
      this.currentReport.license = { } as EMALicense
    else  
      this.currentReport.license.licenseType = EMALicenseType[this.currentReport.license.licenseType] as unknown as EMALicenseType

    this.search = this.currentReport.address
    
    await this.$store.dispatch('users/setUserLastEditedReport', { userName: this.$auth.user.userName, lastEditedReport: this.$route.params.id }, { root: true })
    await this.$auth.fetchUser()
  }

  get IsValidForm() {
    return this.$refs.obs.flags.valid
  }

  get IsCompleted() {
    if(this.currentReport.checkList){
      return this.currentReport.checkList.filter(cl => cl.checks.findIndex(c => c.required && (c.checked as any) === 0)>=0).length === 0
    }

    return false
  }

  get PrincipalSignatureHasAResponsable() {
    return this.currentReport.signatures.findIndex(s=>s.responsable.type !== undefined && s.responsable.name !== '' && s.principal) >= 0
  }

  get HasNotesWithPendingChecks() {
    return this.currentReport.notes.findIndex(n => n.needsCheck && !n.checked) >= 0
  }

  get CanCloseReport() {
     return this.IsCompleted && !this.HasNotesWithPendingChecks && this.PrincipalSignatureHasAResponsable && this.IsValidForm
  }

  checkItemChecks(checkListId: number, value: boolean): void {
    const checkList = this.currentReport.checkList.find(c => c.id === checkListId)
    checkList?.checks.forEach(check => {
      if(value) check.checked = CheckValue.True
      else check.checked = CheckValue.False; 
      this.saveCheckItem(check)
    })
  }

  checkListCheckedValue(item: any) {
    return item.checks.length === item.checks.filter((c:any) => c.checked).length
  }

  mounted() {
    return this.$refs.obs.validate()
  }

  async getSuggestedAddresses(filter: string) {
    await this.$store.dispatch("addresses/getAddresses", { filter }, { root: true });
    this.searchingAddresses = false
    this.isDirty=false
  }

  @Watch('search')
  onSearchChanged(value: string) {
    if(!value || !this.isDirty) return

    if(this.searchingAddresses) return 

    if(value.length >= 3){
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