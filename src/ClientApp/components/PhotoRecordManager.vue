<template>
  <v-container fluid>
    <v-data-iterator
      key="id"
      :items="photos"
      :footer-props="{ itemsPerPageAllText: 'todos',
                       showFirstLastPage: true,
                       itemsPerPageText: 'Photos by page',
                       itemsPerPageOptions: [8, 16]}"
      :items-per-page.sync="itemsPerPage"
      no-results-text="No photos"
    >
      <template #default="props">
        <v-row>
          <v-col
            v-for="photo, index in props.items"
            :key="index"
            cols="12"
            sm="6"
            md="4"
            lg="3"
          >
            <v-card>
              <span class="tw-text-xs">{{ photo.timestamp }}</span>
              <v-img
                :src="`${photo.thumbnailUrl}`"
                class="white--text align-end"
                gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
                height="200px"
              >
                <v-card-title class="text-uppercase">
                  {{ photo.label }}
                </v-card-title>
              </v-img>
              <v-card-actions>
                <v-text-field
                  v-if="showLabelEdit.findIndex(l => l===index)>=0"
                  v-model="photo.label"
                  label="Photo Label"
                />
                <v-btn
                  v-if="showLabelEdit.findIndex(l => l===index)>=0"
                  icon
                  @click="savePhoto(photo); showLabelEdit.splice(showLabelEdit.findIndex(l => l===index),1)"
                >
                  <v-icon>mdi-check</v-icon>
                </v-btn>
                <v-spacer />
                <v-btn
                  v-if="showLabelEdit.findIndex(l => l===index)===-1"
                  icon
                  @click="editPhotoLabel(index)"
                >
                  <v-icon>mdi-pencil</v-icon>
                </v-btn>
                <v-btn icon @click="currentPhoto=index; showCarousel=true">
                  <v-icon>mdi-eye</v-icon>
                </v-btn>
                <v-btn icon @click="removePhoto(photo.id)">
                  <v-icon>mdi-delete</v-icon>
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </template>
    </v-data-iterator>
    <v-dialog v-model="showCarousel">
      <v-carousel v-model="currentPhoto" height="80%">
        <v-carousel-item v-for="(photo, index) in photos" :key="index" :src="`${photo.photoUrl}`" />
      </v-carousel>
    </v-dialog>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue, Model, Prop } from 'nuxt-property-decorator'
import { DeletePhotoRecordCommand, PhotoRecord, EditPhotoRecordCommand } from '~/types'

@Component
export default class PhotoRecordManager extends Vue {
  @Model('input') photos: PhotoRecord[] | undefined;

  @Prop({ required: true, type: Number }) reportId!: number
  showCarousel: boolean = false
  currentPhoto: number = 0;
  showLabelEdit: number[] = [];
  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api', '')

  itemsPerPage: number = 8

  removePhoto (id: number) {
    const delPhoto: DeletePhotoRecordCommand = {
      reportId: this.reportId!,
      id
    }
    this.$axios
      .delete(`reports/${delPhoto.reportId}/photorecord/${delPhoto.id}`)
      .then(() => {
        this.photos = this.photos!.filter(
          p => p.id !== id
        )
      })
  }

  editPhotoLabel (currentIndex: number) {
    const index = this.showLabelEdit.findIndex(l => l === index)
    if (index >= 0) {
      this.showLabelEdit.splice(index, 1)
      return
    }
    this.showLabelEdit.push(currentIndex)
  }

  savePhoto (photo: PhotoRecord) {
    const data:EditPhotoRecordCommand = {
      reportId: parseInt(this.$route.params.id),
      label: photo.label,
      id: photo.id
    }
    this.$axios.put(`reports/${data.reportId}/photorecord/${data.id}`, data)
  }
}
</script>

<style scoped>
span {
  font-size: 0.8em;
  color: #000;
  top: 0;
}
</style>
