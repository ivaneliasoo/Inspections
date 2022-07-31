<template>
  <v-card class="mx-auto" flat>
    <v-row justify="center" align="end">
      <!-- <v-col cols="6" class="text-right">
        Tap Camera Icon To Add Photos to report
      </v-col> -->
      <v-col :cols="12">
        <v-file-input
          ref="fileInputElement"
          class="input-file"
          name="fileInputElement"
          color="primary accent-4"
          counter
          label="Upload File"
          multiple
          hide-input
          messages="Add Photos to Report"
          placeholder="Select your files"
          prepend-icon="mdi-camera"
          outlined
          accept="image/*"
          :show-size="1000"
          @change="showPreview($event); uploadFiles()"
          @click:clear="filesUrls = []"
        />
      </v-col>
      <!-- <v-col cols="4" /> -->
    </v-row>
    <v-divider />
    <v-row>
      <v-skeleton-loader
        v-if="loadingPhotos"
        class="mx-auto"
        min-width="400"
        type="card"
      />
      <v-skeleton-loader
        v-if="loadingPhotos"
        class="mx-auto"
        min-width="400"
        type="card"
      />
      <v-skeleton-loader
        v-if="loadingPhotos"
        class="mx-auto"
        min-width="400"
        type="card"
      />
      <PhotoRecordManager
        v-if="files.length === 0 && !loadingPhotos"
        v-model="photoRecords"
        :report-id="reportId"
      />
      <PhotoRecordPreviewer
        v-if="!loadingPhotos"
        v-model="filesUrls"
        :files="files"
        :progress="percentCompleted"
      />
    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import reduce from 'image-blob-reduce'
import { Prop } from 'nuxt-property-decorator'
import PhotoRecordPreviewer from '@/components/PhotoRecordPreviewer.vue'
import PhotoRecordManager from '@/components/PhotoRecordManager.vue'

@Component({
  components: {
    PhotoRecordPreviewer,
    PhotoRecordManager,
  },
})
export default class PhotoRecords extends Vue {
  @Prop({ required: true, type: Number }) reportId!: number
  files: File[] = []
  filesUrls: { url: string; id: number; label: string }[] = []
  loadingPhotos: boolean = false
  photoRecords: any[] = []

  selectedPhotoComponent: string = 'PhotoRecordManager'
  dialogUploading: boolean = false
  percentCompleted: number = 0

  testurl = ''
  testurlproc = ''

  async fetch () {
    try {
      this.loadingPhotos = true
      const result: any = await this.$reportsApi.apiReportsIdPhotorecordGet(
        this.reportId
      )
      if (result.data) {
        this.photoRecords = result.data as any[]
      }
    } catch (error) {
    } finally {
      this.loadingPhotos = false
    }
  }

  async uploadFiles () {
    const Pthis = this
    const formData = new FormData()
    this.dialogUploading = true
    const config = {
      onUploadProgress (progressEvent: any) {
        Pthis.percentCompleted = Math.round(
          (progressEvent.loaded * 100) / progressEvent.total
        )
      },
    }

    for (let i = 0; i < this.files.length; i++) {
      const file = this.files[i]
      const blob = await reduce().toBlob(file, { max: 1000 })
      const newFile = new File([blob], file.name)
      this.testurlproc = URL.createObjectURL(newFile)
      formData.append(
        'files',
        newFile,
        `${file.name}|${this.filesUrls[i].label}`
      )
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

    const result: any = await this.$reportsApi.apiReportsIdPhotorecordGet(
      this.reportId
    )
    if (result.data) {
      this.photoRecords = result.data as any[]
    }

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
    if (this.filesUrls.length > 0) {
      return this.filesUrls
    }

    return this.photoRecords
  }
}
</script>

<style scoped>
:deep(.input-file) {
  justify-content: center;
}
:deep(button.v-icon.notranslate.v-icon--link.mdi.mdi-camera.theme--light) {
    font-size: 58px;
}
</style>
