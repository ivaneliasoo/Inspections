<template>
  <v-card flat>
    <v-row justify="space-around" align="end">
      <v-col cols="6" class="text-right">
        Tap Camera Icon To Add Photos to report
      </v-col>
      <v-col :cols="2">
        <v-file-input
          ref="fileInputElement"
          color="primary accent-4"
          counter
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
      <v-col cols="4" />
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
        :disabled="!files.length>0 || dialogUploading"
        @click="uploadFiles"
      >
        Save Photos
        <v-icon>mdi-upload</v-icon>
      </v-btn>
      <PhotoRecordManager v-if="files.length===0" v-model="currentReport" />
      <PhotoRecordPreviewer v-else v-model="filesUrls" :files="files" :progress="percentCompleted" />
    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue, Model } from 'vue-property-decorator'
import { Report } from '~/types'
import reduce from 'image-blob-reduce'
import PhotoRecordPreviewer from '@/components/PhotoRecordPreviewer.vue'
import PhotoRecordManager from '@/components/PhotoRecordManager.vue'

@Component({
  components: {
    PhotoRecordPreviewer,
    PhotoRecordManager
  }
})
export default class PhotoRecords extends Vue {
  @Model('input') currentReport: Report | undefined;
  files: File[] = [];
  filesUrls: { url: string, id: number, label: string }[] = [];

  selectedPhotoComponent: string = 'PhotoRecordManager'
  dialogUploading: boolean = false
  percentCompleted: number= 0

  testurl = ''
  testurlproc = ''

  async uploadFiles () {
    const Pthis = this
    const filesProcessed = 0
    const formData = new FormData()
    this.dialogUploading = true
    const config = {
      onUploadProgress (progressEvent: any) {
        Pthis.percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total)
      }
    }

    for (let i = 0; i < this.files.length; i++) {
      const file = this.files[i]
      const blob = await reduce().toBlob(file, { max: 1000 })
      const newFile = new File([blob], file.name)
      this.testurlproc = URL.createObjectURL(newFile)
      formData.append('files', newFile, `${file.name}|${this.filesUrls[i].label}`)
    }

    formData.append('label', 'archivos de Prueba')

    await this.$axios
      .post(`reports/${this.$route.params.id}/photorecord`, formData, config)
      .then(() => {
        this.files = []
        this.filesUrls = []
        this.$emit('uploaded')
        this.selectedPhotoComponent = 'PhotoRecordManager'
        this.percentCompleted = 0
        this.files = []
      })

    this.dialogUploading = false
  }

  showPreview (filesAdded: File[]) {
    filesAdded.forEach((file, index) => {
      this.files.push(file)
      const url = URL.createObjectURL(file)
      this.filesUrls.push({ url, id: index, label: '' })
    })

    this.selectedPhotoComponent = 'PhotoRecordPreviewer'
  }

  get source () {
    if (this.filesUrls.length > 0) { return this.filesUrls }

    return this.currentReport!.photoRecords
  }
}
</script>

<style scoped>
</style>
