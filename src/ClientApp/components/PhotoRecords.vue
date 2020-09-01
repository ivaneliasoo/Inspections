<template>
  <v-card flat>
    <v-row justify="space-around" align="center">
      <v-col :cols="files.length>0 ? 10:12">
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
          accept="image/*"
          :show-size="1000"
          @change="showPreview($event)"
          @click:clear="filesUrls = []"
        >
          <template v-slot:selection="{ index, text }">
            <v-chip v-if="index < 2" color="primary accent-4" dark label small>{{ text }}</v-chip>

            <span
              v-else-if="index === 2"
              class="overline grey--text text--darken-3 mx-2"
            >+{{ files.length - 2 }} File(s)</span>
          </template>
        </v-file-input>
      </v-col>
      <v-col v-if="files.length>0" cols="2">
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn
              color="indigo"
              v-on="on"
              dark
              fab
              elevation="2"
              :disabled="!files.length>0 || currentReport.isClosed"
              @click="uploadFiles"
            >
              <v-icon>mdi-upload</v-icon>
            </v-btn>
          </template>
          <span>Upload Selected Files</span>
        </v-tooltip>
      </v-col>
    </v-row>
    <v-divider />
    <v-row>
      <PhotoRecordManager v-if="files.length===0" v-model="currentReport" />
      <PhotoRecordPreviewer v-else v-model="filesUrls" :files="files" />
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

  uploadFiles() {
    let formData = new FormData();

    this.files.forEach((file: File, index) => {
      formData.append("files", file, `${file.name}|${this.filesUrls[index].label}`);
    });

    formData.append("label", "archivos de Prueba");

    const self = this;

    this.$axios
      .post(`reports/${this.$route.params.id}/photorecord`, formData)
      .then(() => {
        this.files = [];
        this.filesUrls = [];
        this.$emit('uploaded')
        this.selectedPhotoComponent = "PhotoRecordManager"
      });
  }

  showPreview() {
    this.files.forEach((file, index)=> {
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