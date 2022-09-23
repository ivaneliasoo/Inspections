<template>
  <v-container fluid>
    <v-data-iterator
      key="id"
      :items="photos"
      :footer-props="{
        itemsPerPageAllText: 'todos',
        showFirstLastPage: true,
        itemsPerPageText: 'Photos by page',
        itemsPerPageOptions: [8, 16],
      }"
      :items-per-page.sync="state.itemsPerPage"
      no-results-text="No photos"
    >
      <template #default="props">
        <v-row>
          <v-col
            v-for="(photo, index) in props.items"
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
                  v-if="state.showLabelEdit.findIndex((l) => l === index) >= 0"
                  v-model="photo.label"
                  label="Photo Label"
                />
                <v-btn
                  v-if="state.showLabelEdit.findIndex((l) => l === index) >= 0"
                  icon
                  @click="
                    savePhoto(photo)
                    state.showLabelEdit.splice(
                      state.showLabelEdit.findIndex((l) => l === index),
                      1
                    )
                  "
                >
                  <v-icon>mdi-check</v-icon>
                </v-btn>
                <v-spacer />
                <v-btn
                  v-if="
                    state.showLabelEdit.findIndex((l) => l === index) === -1
                  "
                  icon
                  @click="editPhotoLabel(index)"
                >
                  <v-icon>mdi-pencil</v-icon>
                </v-btn>
                <v-btn
                  icon
                  @click="
                    state.currentPhoto = index
                    state.showCarousel = true
                  "
                >
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
    <v-dialog v-model="state.showCarousel">
      <v-carousel v-model="state.currentPhoto" height="80%">
        <v-carousel-item
          v-for="(photo, index) in state.localPhotos"
          :key="index"
          :src="`${photo.photoUrl}`"
        />
      </v-carousel>
    </v-dialog>
  </v-container>
</template>

<script lang="ts">
import {
  useContext,
  useRoute,
  defineComponent,
  reactive,
} from '@nuxtjs/composition-api'
import {
  DeletePhotoRecordCommand,
  PhotoRecord,
  EditPhotoRecordCommand,
} from '~/types'

export default defineComponent({
  model: {
    prop: 'photos',
    event: 'input',
  },
  props: {
    photos: {
      type: Array as () => PhotoRecord[] | undefined,
      required: true,
    },
    reportId: {
      type: Number,
      required: true,
    },
  },
  setup(props) {
    const { $axios } = useContext()
    const route = useRoute()
    const state = reactive({
      showCarousel: false,
      currentPhoto: 0,
      showLabelEdit: [],
      hostName: $axios!.defaults!.baseURL!.replace('/api', ''),
      itemsPerPage: 8,
      localPhotos: [],
    })

    const removePhoto = (id: number) => {
      const delPhoto: DeletePhotoRecordCommand = {
        reportId: props.reportId!,
        id,
      }
      $axios
        .delete(`reports/${delPhoto.reportId}/photorecord/${delPhoto.id}`)
        .then(() => {
          // eslint-disable-next-line vue/no-mutating-props
          state.localPhotos = props.photos!.filter((p) => p.id !== id)
        })
    }

    const editPhotoLabel = (currentIndex: number) => {
      const index = state.showLabelEdit.findIndex((l) => l === index)
      if (index >= 0) {
        state.showLabelEdit.splice(index, 1)
        return
      }
      state.showLabelEdit.push(currentIndex)
    }

    const savePhoto = (photo: PhotoRecord) => {
      const data: EditPhotoRecordCommand = {
        reportId: parseInt(route.value.params.id),
        label: photo.label,
        id: photo.id,
      }
      $axios.put(`reports/${data.reportId}/photorecord/${data.id}`, data)
    }

    return {
      state,
      removePhoto,
      editPhotoLabel,
      savePhoto,
    }
  },
})
</script>

<style scoped>
span {
  font-size: 0.8em;
  color: #000;
  top: 0;
}
</style>
