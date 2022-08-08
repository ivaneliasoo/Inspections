<template>
  <v-dialog
    v-model="dialog"
    persistent
    :fullscreen="$vuetify.breakpoint.smAndDown"
    :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
  >
    <v-card>
      <v-toolbar dark color="indigo" dense>
        <v-btn icon @click.native="$emit('input', false)">
          <v-icon>mdi-arrow-left</v-icon>
        </v-btn>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <v-row align-center justify-center row fill-height>
          <v-col class="text-md-center" cols="2">
            <v-icon color="warning"> mdi-exclamation-thick </v-icon>
          </v-col>
          <v-col cols="10">
            {{ message }}
            <b>{{ code }} - {{ description }}</b
            >. Are you Sure?.
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn
          color="error"
          text
          @click="
            $emit('yes')
            $emit('input', false)
          "
        >
          Yes
        </v-btn>
        <v-btn color="default" text @click="$emit('input', false)"> No </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script lang="ts">
import { computed, defineComponent } from '@nuxtjs/composition-api'

export default defineComponent({
  model: {
    prop: 'visible',
    event: 'input',
  },
  props: {
    visible: {
      type: [Boolean, undefined],
      default: false,
    },
    message: {
      type: String,
      required: true,
    },
    title: {
      type: String,
      default: '',
    },
    code: {
      type: [String, Number],
      default: '',
    },
    description: {
      type: String,
      default: '',
    },
  },
  setup(props) {
    const dialog = computed(() => {
      return props.visible
    })

    return {
      dialog,
    }
  },
})
</script>
<style>
.title {
  color: white;
}
</style>
