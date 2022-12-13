<template>
  <div id="app">
    <v-row align="center" dense class="mb-10" justify="center">
      <v-col cols="12" class="text-center">
        <v-btn class="mx-2" fab dark small color="primary" @click="save">
          <v-icon dark> mdi-content-save </v-icon>
        </v-btn>
        <v-btn class="mx-2" fab dark small color="secundary" @click="undo">
          <v-icon dark> mdi-undo </v-icon>
        </v-btn>
        <v-btn class="mx-2" fab dark small color="secundary" @click="loadSaved">
          <v-icon dark> mdi-refresh </v-icon>
        </v-btn>
        <v-btn
          class="mx-2"
          fab
          dark
          small
          color="error"
          @click="$emit('close')"
        >
          <v-icon dark> mdi-close </v-icon>
        </v-btn>
      </v-col>
    </v-row>
    <v-row align="center" dense class="mb-10" justify="center">
      <v-col cols="6">
        <VueSignaturePad
          id="pad"
          ref="signaturePad"
          class="elevation-5"
          :width="$device.isMobile ? '300px' : '500px'"
          height="250px"
          :custom-style="{ border: 'black 3px solid' }"
        />
      </v-col>
    </v-row>
  </div>
</template>
<script>
import Vue from 'vue'
import VueSignaturePad from 'vue-signature-pad'
Vue.use(VueSignaturePad)

export default {
  name: 'SignaturePad',
  props: {
    savedData: {
      type: String,
      default: () => undefined
    }
  },
  mounted() {
    if (this.savedData) {
      this.$refs.signaturePad.fromDataURL(this.savedData)
    }
  },
  updated() {
    if (this.savedData) {
      this.$refs.signaturePad.fromDataURL(this.savedData)
    }
  },
  methods: {
    undo() {
      this.$refs.signaturePad.undoSignature()
    },
    save() {
      const { data } = this.$refs.signaturePad.saveSignature()
      this.$emit('input', data)
    },
    loadSaved() {
      this.$refs.signaturePad.fromDataURL(this.savedData)
    }
  }
}
</script>
