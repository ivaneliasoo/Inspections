<template>
  <div id="report">
    <alert-dialog
      v-model="dialogClose"
      title="Close Report"
      message="your are about to close this report."
      :code="currentReport.id"
      :description="currentReport.name"
      @yes="currentReport.isClosed = true; saveReportChanges(); dialogClose = false; currentReport.isClosed = true;"
      @no="currentReport.isClosed = true"
    />
    <v-row>
      <v-col>
        <ValidationObserver ref="obs" tag="form" v-slot="{ valid, dirty }">
          <v-row align="center" justify="space-between">
            <v-col cols="6">
              <ValidationProvider rules="required" v-slot="{ errors }">
                <v-text-field
                :readonly="currentReport.isClosed"
                v-model="currentReport.name"
                label="Report Name"
                :error-messages="errors"
                />
              </ValidationProvider>
            </v-col>
            <v-col cols="6" class="text-right">
              <h4>{{ currentReport.title }}</h4>
              <h6>{{ currentReport.formName }}</h6>
            </v-col>
          </v-row>
          <v-row>
            <v-col></v-col>
          </v-row>
          <v-row>
            <v-col>
              <ValidationProvider rules="required" v-slot="{ errors }">
              <v-textarea label="Address" rows="2 "
                :readonly="currentReport.isClosed"
                v-model="currentReport.address"
                :error-messages="errors"
               />
              </ValidationProvider>
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" md="2" v-if="currentReport.license">
              <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select :items="emaTypes"
                :readonly="currentReport.isClosed"
                v-model="currentReport.license.licenseType"
                :error-messages="errors"
               label="EMA License Type" />
              </ValidationProvider>
            </v-col>
            <v-col cols="12" md="3" v-if="currentReport.license">
              <ValidationProvider rules="required" v-slot="{ errors }">
              <v-text-field type="number"
                :readonly="currentReport.isClosed"
                v-model="currentReport.license.number"
                :error-messages="errors"
               label="License" />
              </ValidationProvider>
            </v-col>
            <v-col cols="12" md="2">
              <ValidationProvider rules="required" v-slot="{ errors }">
              <DatePickerBase type="number" label="License"
                :disabled="currentReport.isClosed"
                v-model="currentReport.date"
                :error-messages="errors"
               :max="new Date().toISOString()" />
              </ValidationProvider>
            </v-col>
            <v-col cols="6" md="2">
              <v-checkbox v-model="IsCompleted" label="Completed" disabled />
            </v-col>
            <v-col cols="6" md="2">
              <v-checkbox
                v-model="currentReport.isClosed"
                label="Close Report"
                :disabled="currentReport.isClosed || !CanCloseReport"
                @click="dialogClose = true"
              />
            </v-col>
          </v-row>
           <v-fab-transition>
      <v-btn
        :disabled="!dirty || !valid || currentReport.isClosed"
        color="success"
        fab
        fixed
        dark
        small
        bottom
        right
        class="v-btn--example2"
        @click="saveReportChanges"
      >
        <v-icon>mdi-content-save</v-icon>
      </v-btn>
    </v-fab-transition>
        </ValidationObserver>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-tabs v-model="tabs" fixed-tabs>
          <v-tabs-slider></v-tabs-slider>
          <v-tab href="#checklists" class="primary--text">
            <v-icon>mdi-check</v-icon>
          </v-tab>

          <v-tab href="#notes" class="primary--text">
            <v-icon>mdi-message-bulleted</v-icon>
          </v-tab>

          <v-tab href="#photos" class="primary--text">
            <v-icon>mdi-folder-multiple-image</v-icon>
          </v-tab>
          <v-tab href="#signatures" class="primary--text">
            <v-icon>mdi-signature-freehand</v-icon>
          </v-tab>
        </v-tabs>
        <v-tabs-items v-model="tabs">
          <v-tab-item key="checklists" value="checklists">
            <v-card flat>
              <v-card-text>
                <v-list subheader two-line flat dense>
                  <v-subheader>Items &amp; Params</v-subheader>
                  <v-list-item
                    v-for="(item, checkListIndex) in currentReport.checkList"
                    :key="item.id"
                  >
                    <template v-slot:default>
                      <v-list-item-content class="text-left">
                        <v-list-item-title>
                          <v-row>

                            <v-col cols="9">{{ checkListIndex + 1 }} .- {{ item.text }}</v-col>
                            <v-col cols="3">
                              <span v-if="item.checks.filter(c => c.required && c.checked===0).length == 0">
                                <v-chip x-small color="success">Completed</v-chip>
                              </span>
                            </v-col>
                          </v-row>
                          <v-row>
                            <v-col cols="12">
                              <v-list-item
                                v-for="(checkItem, checkListItemIndex) in item.checks"
                                :key="checkItem.id"
                              >
                                <template v-slot:default="{ active, toggle }">
                                  <v-list-item-title :title="checkItem.text">
                                    <v-row dense align="center" justify="space-between">
                                      <v-col cols="10">
                                        <v-row dense align="center" justify="space-between">
                                          <v-col
                                            cols="12"
                                            md="7"
                                            class="text-wrap"
                                          >{{ checkListIndex + 1 }}.{{ checkListItemIndex + 1}} .- {{ checkItem.text }}</v-col>
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
              </v-card-text>
            </v-card>
          </v-tab-item>
          <v-tab-item key="notes" value="notes">
            <v-card flat>
              <v-card-text>
                  <v-row>
                      <v-btn class="mx-2" x-small
                        fab dark color="primary"
                        title="Add note"
                        @click="addNote">
                        <v-icon dark>mdi-plus</v-icon>
                        </v-btn>
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
                  </v-row>
                </div>

              </v-card-text>
            </v-card>
          </v-tab-item>
          <v-tab-item key="photos" value="photos">
            <v-card flat>
              <v-row justify="space-around" align="center">
                <v-col :cols="files.length>0 ? 10:12" v-if="!hasPendingChanges">
                  <v-file-input
                    v-model="files"
                    :disabled="currentReport.isClosed"
                    color="primary accent-4"
                    counter
                    label="Upload File"
                    multiple
                    placeholder="Select your files"
                    prepend-icon="mdi-paperclip"
                    outlined
                    :show-size="1000"
                  >
                    <template v-slot:selection="{ index, text }">
                      <v-chip
                        v-if="index < 2"
                        color="primary accent-4"
                        dark
                        label
                        small
                      >
                        {{ text }}
                      </v-chip>

                      <span
                        v-else-if="index === 2"
                        class="overline grey--text text--darken-3 mx-2"
                      >
                        +{{ files.length - 2 }} File(s)
                      </span>
                    </template>
                  </v-file-input>
                </v-col>
                <v-col v-if="files.length>0 && !hasPendingChanges" cols="2">
                  <v-tooltip bottom>
                    <template v-slot:activator="{ on }">
                    <v-btn color="indigo" v-on="on" dark fab elevation="2" :disabled="!files.length>0 || currentReport.isClosed" @click="uploadFiles">
                        <v-icon >
                          mdi-upload
                        </v-icon>
                    </v-btn>
                    </template>
                    <span>Upload Selected Files </span>
                  </v-tooltip>
                </v-col>
                <v-col v-else>
                  <span class="error--text">
                    {{ hasPendingChanges ? 'You cant Upload Photos becouse You Have Pending Changes. To Continue please save your pending changes':''}}
                  </span>
                </v-col>
              </v-row>
              <v-divider />
              <v-row>
                <v-container fluid>
                  <v-row dense>
                      <v-col
                      v-for="(photo, index) in currentReport.photoRecords"
                      :key="index"
                      :cols="index===0 ? 12: $vuetify.breakpoint.smAndDown ? 6:3"
                      >
                      <v-card>
                          <v-img
                          :src="`${hostName}${photo.fileName}`"
                          class="white--text align-end"
                          gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
                          height="200px"
                          >
                          <v-card-title>
                            {{ photo.label }}
                          </v-card-title>
                          </v-img>
                          <v-card-actions>
                            <v-text-field
                                v-if="showLabelEdit.findIndex(l => l===index)>=0"
                                v-model="photo.label"
                                label="Photo Label"
                              />
                            <v-btn v-if="showLabelEdit.findIndex(l => l===index)>=0" icon @click="savePhoto(photo); showLabelEdit.splice(showLabelEdit.findIndex(l => l===index),1)">
                              <v-icon>mdi-check</v-icon>
                            </v-btn>
                          <v-spacer></v-spacer>
                          <v-btn v-if="showLabelEdit.findIndex(l => l===index)===-1" icon @click="editPhotoLabel(index) || !currentReport.isClosed">
                              <v-icon>mdi-pencil</v-icon>
                          </v-btn>
                          <v-btn icon @click="currentPhoto=index; showCarousel=true">
                              <v-icon>mdi-eye</v-icon>
                          </v-btn>
                          <v-btn v-if="!currentReport.isClosed" icon @click="removePhoto(photo.id)">
                              <v-icon>mdi-delete</v-icon>
                          </v-btn>
                          </v-card-actions>
                      </v-card>
                      </v-col>
                  </v-row>
                  </v-container>
                </v-row>
            </v-card>
          </v-tab-item>
          <v-tab-item key="signatures" value="signatures">
            <v-card flat>
              <v-card-text>
                  <div v-for="(signature, index) in currentReport.signatures" :key="index">
                  <v-row>
                      <v-col class="text-left">
                          <h3>{{ signature.title }}</h3>
                          <h5>{{ signature.annotation }}</h5>
                          <v-chip v-if="signature.principal">Principal Sign</v-chip>
                      </v-col>
                  </v-row>
                  <v-row align="center" justify="space-around">
                    <v-col cols="6" md="3">
                        <v-select 
                        v-model="signature.responsable.type" 
                        :readonly="currentReport.isClosed"
                        :items="responsableTypes" 
                        label="Responsable Type" />
                    </v-col>
                    <v-col cols="6" md="3">
                      <v-text-field v-model="signature.responsable.name" :readonly="currentReport.isClosed" label="Responsable" />
                    </v-col>
                    <v-col cols="12" md="4">
                      <v-text-field v-model="signature.designation" :readonly="currentReport.isClosed" label="Designation" />
                    </v-col>
                    <v-col cols="12" md="2">
                      <DatePickerBase v-model="signature.date" :disabled="currentReport.isClosed" label="Date" max="" />
                    </v-col>
                  </v-row>
                  <v-row>
                      <v-col>
                          <v-text-field v-model="signature.remarks" :readonly="currentReport.isClosed" label="Remarks (if any)" />
                      </v-col>
                  </v-row>
                </div>
              </v-card-text>
            </v-card>
          </v-tab-item>
        </v-tabs-items>
      </v-col>
    </v-row>
    <v-dialog v-if="currentPhoto" v-model="showCarousel">
      <v-carousel v-model="currentPhoto" height="80%">
        <v-carousel-item v-for="(photo, index) in currentReport.photoRecords" :key="index" :src="`${hostName}${photo.fileName}`">
        </v-carousel-item>
      </v-carousel>
    </v-dialog>

  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch } from "nuxt-property-decorator";
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { Report, EMALicenseType, Note, Signature, ResponsableType, PhotoRecord, EMALicense, DeleteNoteCommand, AddNoteCommand, UpdateReportCommand, EditNoteCommand, EditPhotoRecordCommand, EditSignatureCommand, CheckList, CheckListItem, UpdateCheckListItemCommand, DeletePhotoRecordCommand } from "../../../types";

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class EditReport extends Vue {
  $refs!: {
    obs: InstanceType<typeof ValidationObserver>
  }
  showCarousel: boolean = false
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

  responsableTypes: any = Object.keys(ResponsableType)
    .map((key: any) => {
      if (!isNaN(Number(key.toString()))) return;

      return { id: ResponsableType[key], text: key };
    })
    .filter(i => i !== undefined)

  files: File[] = []
  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api','')
  signaturesChanges: boolean = false


  get hasPendingChanges() {
    return this.$store.state.showFabSaveButton
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

 async removePhoto(id: number) {
      const delPhoto:DeletePhotoRecordCommand = {
        reportId: this.currentReport.id,
        id
      }
      this.$axios.delete(`reports/${delPhoto.reportId}/photorecord/${delPhoto.id}`).then(() =>{
      this.currentReport.photoRecords =  this.currentReport.photoRecords.filter(p=>p.id !== id)
    })

  }

  uploadFiles() {
    let formData = new FormData

    this.files.forEach((file: File) => {
      formData.append("files", file, file.name)
    });

    formData.append('label', 'archivos de Prueba')

    const self = this

    this.$axios.post(`reports/${this.$route.params.id}/photorecord`, formData).then(() =>{
       this.files = []
      //  this.$store.dispatch('reportstrore/getReportById',
      // this.$route.params.id,
      // { root: true })
      self.$fetch()
    })
  }

  editPhotoLabel(currentIndex:number) {
    const index = this.showLabelEdit.findIndex(l => l===index)
    if(index>=0) {
      this.showLabelEdit.splice(index, 1)
      return
    }
    this.showLabelEdit.push(currentIndex)
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

  savePhoto(photo: PhotoRecord) {
    const data:EditPhotoRecordCommand = {
      reportId: parseInt(this.$route.params.id),
      label: photo.label,
      id: photo.id
    }
    this.$axios.put(`reports/${data.reportId}/photorecord/${data.id}`, data )
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
      remarks: checkItem.remarks
    }
    this.$axios.put(`checklists/${command.checkListId}/items/${checkItem.id}`,  command)
  }

  async saveReportChanges() {
    const update: UpdateReportCommand = {
      id:this.currentReport.id,
      name:this.currentReport.name,
      address:this.currentReport.address,
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
            principal: signature.principal
          }
          this.$axios.$put(`signatures/${signature.id}`, command)
      })
    })
  }

  @Watch('currentReport', { deep: false })
  onReportChanged(value: Report, oldValue: Report) {
    if(value !== oldValue && oldValue !== undefined)
      this.$store.dispatch('hasPendingChanges', true)
  }

  async fetch() {
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
          type:  ResponsableType[signature.responsable.type]  as unknown as ResponsableType,// this.responsableTypes.find((et: any) => et.text == signature.responsable.type)?.id, 
          name: signature.responsable.name
        }
    });

    this.currentReport = Object.assign({}, result);
    if(!this.currentReport.license)
      this.currentReport.license = { } as EMALicense
    else  
      this.currentReport.license.licenseType = EMALicenseType[this.currentReport.license.licenseType] as unknown as EMALicenseType //this.emaTypes.find((et: any) => et.text == this.currentReport.license.licenseType)?.id
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
}
</script>

<style scoped>
.v-btn--example2 {
    bottom: 0;
    /* position: absolute; */
    margin: 0 46px 16px 16px;
  }
</style>