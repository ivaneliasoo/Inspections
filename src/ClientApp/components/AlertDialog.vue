<template>
  <v-dialog
    v-model="dialog"
    persistent
    :fullscreen="$vuetify.breakpoint.smAndDown"
    :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
  >
    <v-card>
      <v-toolbar dark color="indigo">
        <v-btn icon @click.native="$emit('input', false)">
          <v-icon>mdi-arrow-left</v-icon>
        </v-btn>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
      </v-toolbar>
      <!-- <v-card-title class="indigo title elevation-2">
        {{ title }}
      </v-card-title> -->
      <v-card-text>
        <v-row align-center justify-center row fill-height>
          <v-col class="text-md-center" cols="2">
            <v-icon color="warning">
              mdi-exclamation-thick
            </v-icon>
          </v-col>
          <v-col cols="10">
            {{ message }}
            <b>{{ code }} - {{ description }}</b>. Are you Sure?.
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="error" text @click="$emit('yes'); $emit('input', false)">
          Yes
        </v-btn>
        <v-btn color="default" text @click="$emit('input', false)">
          No
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script lang="ts">
import { Vue, Component, Prop, Model } from 'nuxt-property-decorator'

@Component
export default class AlertDialog extends Vue {
  @Model('input', { type: Boolean, required: true }) visible: Boolean = false
  // @Prop({ required: true, default: false }) show: Boolean | undefined
  @Prop({ required: true, default: '' }) message: String | undefined
  @Prop({ required: true, default: '' }) title: String | undefined
  @Prop({ required: true, default: '' }) code: String | undefined
  @Prop({ required: true, default: '' }) description: String | undefined

  get dialog () {
    return this.visible
  }
}
</script>
<style>
.title {
    color: white;
}
</style>
