<template>
  <v-card class="mx-auto" flat>
    <v-row justify="center" align="end">
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
          @change="
            showPreview($event)
            uploadFiles()
          "
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
import reduce from 'image-blob-reduce'
import {
  defineComponent,
  reactive,
  computed,
  useContext,
  useFetch,
  useRoute,
} from '@nuxtjs/composition-api'
import PhotoRecordManager from '@/components/PhotoRecordManager.vue'
import PhotoRecordPreviewer from '@/components/PhotoRecordPreviewer.vue'

export default defineComponent({
  components: {
    PhotoRecordPreviewer,
    PhotoRecordManager,
  },
  props: {
    reportId: {
      type: Number,
      required: true,
    },
  },
  setup(props, { emit }) {
    const state = reactive({
      files: [],
      filesUrls: [],
      loadingPhotos: false,
      photoRecords: [],
      selectedPhotoComponent: 'PhotoRecordManager',
      dialogUploading: false,
      percentCompleted: 0,

      testurl: '',
      testurlproc: '',
    })

    const { $reportsApi, $axios } = useContext()
    const route = useRoute()

    useFetch(async () => {
      try {
        state.loadingPhotos = true
        const result: any = await $reportsApi.apiReportsIdPhotorecordGet(
          props.reportId
        )
        if (result.data) {
          state.photoRecords = result.data as any[]
        }
      } catch (error) {
      } finally {
        state.loadingPhotos = false
      }
    })

    const uploadFiles = async () => {
      const formData = new FormData()
      state.dialogUploading = true
      const config = {
        onUploadProgress(progressEvent: any) {
          state.percentCompleted = Math.round(
            (progressEvent.loaded * 100) / progressEvent.total
          )
        },
      }

      for (let i = 0; i < state.files.length; i++) {
        const file = state.files[i]
        const blob = await reduce().toBlob(file, { max: 1000 })
        const newFile = new File([blob], file.name)
        state.testurlproc = URL.createObjectURL(newFile)
        formData.append(
          'files',
          newFile,
          `${file.name}|${state.filesUrls[i].label}`
        )
      }

      formData.append('label', 'archivos de Prueba')

      await $axios
        .post(`reports/${route.value.params.id}/photorecord`, formData, config)
        .then(() => {
          state.files = []
          state.filesUrls = []
          emit('uploaded')
          state.selectedPhotoComponent = 'PhotoRecordManager'
          state.percentCompleted = 0
          state.files = []
        })

      const result: any = await $reportsApi.apiReportsIdPhotorecordGet(
        props.reportId
      )
      if (result.data) {
        state.photoRecords = result.data as any[]
      }

      state.dialogUploading = false
    }

    const showPreview = (filesAdded: File[]) => {
      filesAdded.forEach((file, index) => {
        state.files.push(file)
        const url = URL.createObjectURL(file)
        state.filesUrls.push({ url, id: index, label: '' })
      })

      state.selectedPhotoComponent = 'PhotoRecordPreviewer'
    }

    const source = computed(() => {
      if (state.filesUrls.length > 0) {
        return state.filesUrls
      }

      return state.photoRecords
    })

    return {
      state,
      uploadFiles,
      showPreview,
      source,
    }
  },
})
</script>

<style scoped>
:deep(.input-file) {/* stylelint-disable-line */
  justify-content: center;
}

:deep(button.v-icon.notranslate.v-icon--link.mdi.mdi-camera.theme--light) {/* stylelint-disable-line */
  font-size: 58px;
}
</style>
