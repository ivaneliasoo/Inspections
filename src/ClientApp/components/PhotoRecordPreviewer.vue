<template>
  <v-container fluid>
    <v-row dense>
      <v-col
        v-for="(photo, index) in urls"
        :key="index"
        cols="12"
        sm="6"
        md="4"
        lg="3"
      >
        <v-card>
          <!-- <v-img
            :src="`${photo.url}`"
            class="white--text align-end"
            gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
            height="200px"
          > -->
          <v-img
            :src="`${$axios.defaults.baseURL}/reports/${photo.id}/photo`"
            class="white--text align-end"
            gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
            height="200px"
          >
            <v-card-title class="text-uppercase">
              {{ photo ? photo.label : '' }}
            </v-card-title>
          </v-img>
          <v-card-actions>
            <v-text-field
              v-model="photo.label"
              label="Photo Label"
              hint="write a description if needed"
            />
            <v-spacer />
            <!-- <v-btn icon @click="currentPhoto=index; showCarousel=true">
              <v-icon>mdi-eye</v-icon>
            </v-btn> -->
            <v-btn icon @click="removePhoto(index)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </v-card-actions>
          <v-progress-linear
            color="light-blue"
            height="10"
            :value="progress"
            striped
          />
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="state.showCarousel">
      <v-carousel v-model="state.currentPhoto" height="80%">
        <v-carousel-item
          v-for="(photo, index) in urls"
          :key="index"
          :src="photo.url"
        >
          <span>{{ photo }} {{ index }}</span>
        </v-carousel-item>
      </v-carousel>
    </v-dialog>
  </v-container>
</template>

<script lang="ts">
import { defineComponent, computed, reactive } from '@nuxtjs/composition-api'

export default defineComponent({
  model: {
    prop: 'urls',
    event: 'input'
  },
  props: {
    urls: {
      type: Array,
      required: true
    },
    files: {
      type: Array as () => File[],
      required: true
    },
    progress: {
      type: Number,
      required: true
    }
  },
  setup(props, { emit }) {
    const state = reactive({
      showCarousel: false,
      currentPhoto: 0
    })

    const filesSync = computed({
      get: () => props.files,
      set: (files) => {
        emit('input', files)
      }
    })

    const removePhoto = (id: number) => {
      filesSync!.value.splice(id, 1)
      props.urls!.splice(id, 1)
    }

    return {
      filesSync,
      removePhoto,
      state
    }
  }
})
</script>
