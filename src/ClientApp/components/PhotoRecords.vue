<template>
  <v-card flat>
    <v-row justify="space-around" align="end">
      <v-col cols="6" class="text-right">
        Tap Camera Icon To Add Photos to report
      </v-col>
      <v-col :cols="2">
        <v-file-input
          :disabled="currentReport.isClosed"
          color="primary accent-4"
          counter
          ref="fileInputElement"
          label="Upload File"
          multiple
          hide-input
          messages="Add Photos to Report"
          placeholder="Select your files"
          prepend-icon="mdi-camera-plus"
          outlined
          accept="image/*"
          :show-size="1000"
          @change="showPreview($event)"
          @click:clear="filesUrls = []"
        />
      </v-col>
      <v-col cols="4"></v-col>
      <!-- <v-col v-if="files.length>0" cols="2">
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn
              color="indigo"
              v-on="on"
              dark
              fab
              :small="$device.isMobile"
              elevation="2"
              :disabled="!files.length>0 || currentReport.isClosed || dialogUploading"
              @click="uploadFiles"
            >
              <v-icon>mdi-upload</v-icon>
            </v-btn>
          </template>
          <span>Upload Selected Files</span>
        </v-tooltip>
      </v-col> -->
    </v-row>
    <v-divider />
    <v-row>
            <v-btn
              color="indigo"
              dark
              block
              x-large
              :small="$device.isMobile"
              elevation="2"
              :disabled="!files.length>0 || currentReport.isClosed || dialogUploading"
              @click="uploadFiles"
            >
              Save Photos
              <v-icon>mdi-upload</v-icon>
            </v-btn>
      <PhotoRecordManager v-if="files.length===0" v-model="currentReport" />
      <PhotoRecordPreviewer v-else v-model="filesUrls" :files="files" :progress="percentCompleted"/>

    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue, Model } from "vue-property-decorator";
import { Report } from "~/types";
import PhotoRecordPreviewer from '@/components/PhotoRecordPreviewer.vue'
import PhotoRecordManager from '@/components/PhotoRecordManager.vue'

@Component({
  components: {
    PhotoRecordPreviewer,
    PhotoRecordManager
  }
})
export default class PhotoRecords extends Vue {
  @Model("input") currentReport: Report | undefined;
  files: File[] = [];
  filesUrls: { url: string, id: number, label: string }[] = [];

  selectedPhotoComponent: string = 'PhotoRecordManager'
  dialogUploading: boolean = false
  percentCompleted: number= 0

  async uploadFiles() {
    const Pthis =  this
    let formData = new FormData();
    this.dialogUploading=true
    var config = {
            onUploadProgress: function(progressEvent: any) {
              console.log(progressEvent)
              Pthis.percentCompleted = Math.round( (progressEvent.loaded * 100) / progressEvent.total );
            }
          };

    this.files.forEach((file: File, index) => {
      formData.append("files", file, `${file.name}|${this.filesUrls[index].label}`);
    });

    formData.append("label", "archivos de Prueba");

    const self = this;

    await this.$axios
      .post(`reports/${this.$route.params.id}/photorecord`, formData, config)
      .then(() => {
        this.files = [];
        this.filesUrls = [];
        this.$emit('uploaded')
        this.selectedPhotoComponent = "PhotoRecordManager"
        this.percentCompleted = 0
        this.files = []
      });

    this.dialogUploading=false
  }

  showPreview(filesAdded: File[]) {
    filesAdded.forEach((file, index)=> {
      this.files.push(file)
      var url = URL.createObjectURL(file)
      this.filesUrls.push({ url, id: index, label: ''})
    })

    this.selectedPhotoComponent = "PhotoRecordPreviewer"
  }

  get source() {
    if(this.filesUrls.length>0)
      return this.filesUrls
    
    return this.currentReport!.photoRecords
  }
}
</script>

<style scoped>
</style>