<template>
  <v-dialog
    v-model="dialog"
    persistent
    scrollable
    :fullscreen="$vuetify.breakpoint.smAndDown"
    :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
  >
    <v-card>
      <v-toolbar dark color="indigo">
        <v-btn icon @click.native="$emit('input', false)">
          <v-icon>mdi-arrow-left</v-icon>
        </v-btn>
        <v-toolbar-title><slot name="title" /></v-toolbar-title>
      </v-toolbar>
      <v-card-text class="mt-4">
        <slot name="subtitle" />
        <v-row align="center" justify="space-around">
          <v-col cols="12">
            <slot />
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn
          v-show="showYes"
          color="success"
          :loading="loading"
          text
          @click="$emit('yes')"
        >
          {{ yesText }}
        </v-btn>
        <v-btn v-show="showNo" color="error" text @click="$emit('no')">
          {{ noText }}
        </v-btn>
        <v-btn
          v-show="showCancel"
          color="default"
          text
          @click="$emit('cancel')"
        >
          Cancel
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script lang="ts">
import { defineComponent, computed, ref } from '@nuxtjs/composition-api'

export default defineComponent({
  model: {
    prop: 'visible',
    event: 'input',
  },
  props: {
    visible: {
      type: Boolean,
      required: true,
    },
    yesText: {
      type: String,
      default: 'Ok',
    },
    noText: {
      type: String,
      default: 'No',
    },
    actions: {
      type: Array,
      default: () => {
        return ['yes', 'no', 'cancel']
      },
    },
    required: {
      type: Boolean,
      default: false,
    },
  },
  setup(props) {
    const loading = ref(false)
    const dialog = computed(() => {
      return props.visible
    })

    const showYes = computed(() => {
      return props.actions.includes('yes')
    })

    const showNo = computed(() => {
      return props.actions.includes('no')
    })

    const showCancel = computed(() => {
      return props.actions.includes('cancel')
    })

    return {
      dialog,
      showYes,
      showNo,
      showCancel,
      loading,
    }
  },
})
</script>
<style>
.title {
  color: white;
}
</style>
