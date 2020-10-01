<template>
  <v-container fluid>
    <v-row dense>
      <v-col
        v-for="(photo, index) in urls"
        :key="index"
        :cols="index===0 ? 12: $vuetify.breakpoint.smAndDown ? 6:3"
      >
        <v-card>
          <v-img
            :src="`${photo.url}`"
            class="white--text align-end"
            gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
            height="200px"
          >
            <v-card-title>{{ photo ? photo.label : '' }}</v-card-title>
          </v-img>
          <v-card-actions>
            <v-text-field
              v-model="photo.label"
              label="Photo Label"
              hint="write a description if needed"
            />
            <v-spacer></v-spacer>
            <v-btn icon @click="currentPhoto=index; showCarousel=true">
              <v-icon>mdi-eye</v-icon>
            </v-btn>
            <v-btn icon @click="removePhoto(index)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="showCarousel">
      <v-carousel v-model="currentPhoto" height="80%">
        <v-carousel-item v-for="(photo, index) in urls" :key="index" :src="photo.url">
          <span>{{ photo }} {{ index }}</span>
        </v-carousel-item>
      </v-carousel>
    </v-dialog>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue, Model, PropSync } from "vue-property-decorator";
import { Report, DeletePhotoRecordCommand, PhotoRecord, EditPhotoRecordCommand } from "~/types";

@Component
export default class PhotoRecordPreviewer extends Vue {
  @Model("input") urls: string[] | undefined;
  @PropSync('files', { required: true }) filesSync: File[] | undefined

  showCarousel: boolean = false
  currentPhoto: number = 0;
  showLabelEdit: number[] = [];

  async removePhoto(id: number) {
    this.filesSync!.splice(id,1)
    this.urls!.splice(id,1)
  }
}
</script>

<style scoped>
</style>