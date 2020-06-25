<template>
  <vue-dropzone
    id="dropzone"
    ref="dropzone"
    v-bind="$attrs"
    :options="fileUploadOptionsCommputed"
    width="100%"
    v-on="$listeners"
  />
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import vue2Dropzone from 'vue2-dropzone'
import 'vue2-dropzone/dist/vue2Dropzone.min.css'

@Component({
  layout: 'Default',
  components: { vueDropzone: vue2Dropzone }
})
export default class zFileupload extends Vue {
  @Prop() rutaApi:string | undefined;
  @Prop() filtro:string | undefined;
  @Prop() params:any | undefined;
  @Prop() maxArchivos:any | undefined;

  fileUploadOptions:object= {
    url: `${this.rutaApi}`,
    maxFilesize: 10, // MB
    dictRemoveFile: 'Remove',
    dictDefaultMessage: '<i aria-hidden="true" class="v-icon notranslate accent--text material-icons theme--light" style="font-size: 40px;">post_add</i><p class="mt-4 font-weight-bold" style="font-size: 1.10em;">Drop your files here</p>',
    dictResponseError: 'error uploading file',
    dictInvalidFileType: 'Invalid file type',
    dictMaxFilesExceeded: 'only one file at a time',
    acceptedFiles: this.filtro ?? '',
    dictCancelUploadConfirmation: 'are you sure you want to cancel upload process?',
    dictCancelUpload: 'Cancel',
    maxFiles: this.maxArchivos ?? 1,
    chunking: false,
    addRemoveLinks: true,
    params: this.params,
    headers: {
      Authorization: this.$auth.getToken(this.$auth.$state.strategy) ?? ''
    },
    complete: (file: any) => {
      const self = this
      setTimeout(() => {
        self.removeFile(file)
      }, 3000)
    }
  }

  get fileUploadOptionsCommputed() {
    return this.fileUploadOptions
  }

  removeFile(file) {
    const $refs:any = this.$refs
    const dropzone:any = $refs.dropzone
    dropzone.removeFile(file)

    this.$emit('completed')
  }
}
</script>

<style>
.vue-dropzone {
    background-color: #f1f8fc;
    border-radius: 10px;
    border-color: rgba(21, 89, 192, 0.192) !important

}
</style>
