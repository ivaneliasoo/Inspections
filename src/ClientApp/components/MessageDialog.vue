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
        <v-btn v-show="showYes" color="success" :loading="loading" text @click="$emit('yes')">
          {{ yesText }}
        </v-btn>
        <v-btn v-show="showNo" color="error" text @click="$emit('no')">
          {{ noText }}
        </v-btn>
        <v-btn v-show="showCancel" color="default" text @click="$emit('cancel')">
          Cancel
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script lang="ts">
import { Vue, Component, Prop, Model } from 'nuxt-property-decorator'

@Component
export default class MessageDialog extends Vue {
  @Model('input', { type: Boolean, required: true }) visible: Boolean = false
  @Prop({ default: () => { return ['yes', 'no', 'cancel'] } }) actions!: Array<string>
  @Prop({ default: 'Ok' }) yesText!: string
  @Prop({ default: 'No' }) noText!: string

  @Prop({ required: false }) loading!: boolean

  get dialog (): Boolean {
    return this.visible
  }

  get showYes () {
    return this.actions.includes('yes')
  }

  get showNo () {
    return this.actions.includes('no')
  }

  get showCancel () {
    return this.actions.includes('cancel')
  }
}
</script>
<style>
.title {
  color: white;
}
</style>
