<template>
  <div>
    <alert-dialog
      v-model="dialogClose"
      title="Close Report"
      message="your are about to close this report."
      :code="currentReport.id"
      :description="currentReport.name"
      @yes="currentReport.isClosed = true; dialogClose = false"
    />
    <v-row align="center" justify="space-between">
      <v-col cols="6">
        <v-text-field v-model="currentReport.name" label="Report Name" />
      </v-col>
      <v-col cols="6" class="text-right">
        <h4>{{ currentReport.formName }}</h4>
      </v-col>
    </v-row>
    <v-row>
      <v-col></v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-textarea label="Address" rows="3" />
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" md="2">
        <v-select :items="emaTypes" label="EMA License Type" />
      </v-col>
      <v-col cols="12" md="3">
        <v-text-field type="number" label="License" />
      </v-col>
      <v-col cols="12" md="2">
        <DatePickerBase type="number" label="License" :min="new Date().toISOString()" max />
      </v-col>
      <v-col cols="6" md="2">
        <v-checkbox label="Completed" disabled />
      </v-col>
      <v-col cols="6" md="2">
        <v-checkbox
          label="Close Report"
          :disabled="currentReport.isClosed"
          @click="dialogClose = true"
        />
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
                <v-list subheader two-line flat>
                  <v-subheader>Items &amp; Params</v-subheader>
                  <v-list-item
                    v-for="(item, checkListIndex) in currentReport.checkList"
                    :key="item.id"
                  >
                    <template v-slot:default>
                      <v-list-item-content class="text-left">
                        <v-list-item-title>
                          <v-row>
                            <v-col cols="10">{{ checkListIndex + 1 }} .- {{ item.text }}</v-col>
                            <v-col cols="2">
                              <span v-if="item.completed">
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
                                    <v-row align="center" justify="space-between">
                                      <v-col
                                        cols="12"
                                        md="7"
                                      >{{ checkListIndex + 1 }}.{{ checkListItemIndex + 1}} .- {{ checkItem.text }}</v-col>
                                      <v-col cols="2" md="1">
                                        <v-simple-checkbox
                                          color="primary"
                                          @click="toggle"
                                          v-model="checkItem.checked"
                                        />
                                      </v-col>
                                      <v-col cols="10" md="4">
                                        <v-text-field :label="currentReport.remarksLabelText" />
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
                  <v-row align="center" justify="space-around">
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
                            <v-icon dark>mdi-minus</v-icon>
                            </v-btn>
                      </v-col>
                    <v-col cols="8">
                      <v-text-field v-model="note.text" label="Note" />
                    </v-col>
                    <v-col cols="2">
                        <v-checkbox v-model="note.checked" :label="note.needsCheck ? '(Check Required)':''" />
                    </v-col>
                  </v-row>
                </div>
                
              </v-card-text>
            </v-card>
          </v-tab-item>
          <v-tab-item key="photos" value="photos">
            <v-card flat>
              <v-row justify="space-around" align="center">
                <v-col :cols="files.length>0 ? 10:12">
                  <v-file-input
                    v-model="files"
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
                <v-col v-if="files.length>0" cols="2">
                  <v-tooltip bottom>
                    <template v-slot:activator="{ on }">
                    <v-btn color="indigo" v-on="on" dark fab elevation="2" :disabled="!files.length>0" @click="uploadFiles">
                        <v-icon >
                          mdi-upload
                        </v-icon>
                    </v-btn>
                    </template>
                    <span>Upload Selected Files</span>
                  </v-tooltip>
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
                                  <v-btn v-if="showLabelEdit.findIndex(l => l===index)>=0" icon @click="showLabelEdit.splice(showLabelEdit.findIndex(l => l===index),1)">
                                    <v-icon>mdi-check</v-icon>
                                  </v-btn>
                                <v-spacer></v-spacer>
                                <v-btn v-if="showLabelEdit.findIndex(l => l===index)===-1" icon @click="editPhotoLabel(index)">
                                    <v-icon>mdi-pencil</v-icon>
                                </v-btn>
                                <v-btn icon @click="currentPhoto=index; showCarousel=true">
                                    <v-icon>mdi-eye</v-icon>
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
                        <v-select :items="responsableTypes" label="Responsable Type" />
                    </v-col>
                    <v-col cols="6" md="3">
                      <v-text-field v-model="signature.name" label="Responsable" />
                    </v-col>
                    <v-col cols="12" md="4">
                      <v-text-field v-model="signature.designation" label="Designation" />
                    </v-col>
                    <v-col cols="12" md="2">
                      <DatePickerBase v-model="signature.date" label="Date" />
                    </v-col>
                  </v-row>
                  <v-row>
                      <v-col>
                          <v-text-field v-model="signature.remarksLabelText" label="Remarks" />
                      </v-col>
                  </v-row>
                </div>
              </v-card-text>
            </v-card>
          </v-tab-item>
        </v-tabs-items>
      </v-col>
    </v-row>
    <v-dialog v-model="showCarousel">
      <v-carousel v-model="currentPhoto" height="80%">
        <v-carousel-item v-for="(photo, index) in currentReport.photoRecords" :key="index" :src="`${hostName}${photo.fileName}`">
        </v-carousel-item>
      </v-carousel>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "nuxt-property-decorator";
import { Report, EMALicenseType, Note, Signature, ResponsableType, PhotoRecord } from "../../../types";

@Component
export default class EditReport extends Vue {
  showCarousel: boolean = false
  currentphoto: number = 0
  showLabelEdit:number[] = []
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
  photoRecords: PhotoRecord[] = []
  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api','')

  async addNote() {
      const newNote: Note = {
          id: 0,
          reportId: this.currentReport.id,
          text: "",
          checked: false,
          needsCheck: false
      }
      this.currentReport.notes.push(newNote)
  }

  async removeNote(id: number) {
      //TODO Call Remove in api
      this.currentReport.notes =  this.currentReport.notes.filter(n=>n.id !== id)
  }

  uploadFiles() {
    let formData = new FormData

    this.files.forEach((file: File) => {
      formData.append("files", file, file.name)
    });

    formData.append('label', 'archivos de Prueba')

    this.$axios.post(`reports/${this.$route.params.id}/photorecord`, formData).then(() =>{
       this.files = []
       this.fetch()
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

  async fetch() {
    const result = await this.$store.dispatch(
      "reportstrore/getReportById",
      this.$route.params.id,
      { root: true }
    );
    this.currentReport = Object.assign({}, result);
  }
}
</script>