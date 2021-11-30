<template>
  <v-col :cols="context.cols" :class="`formulate-input-element formulate-input-element--${context.class}`"
    :data-type="context.type">
    <v-text-field v-model="context.model" :type="context.clasification" :label="context.label" :placeholder="context.placeholder" error :error-messages="context.errors[0]" />
    <!-- <pre>{{context}}</pre> -->
  </v-col>
</template>

<script lang="ts">
import { defineComponent } from '@nuxtjs/composition-api'

export default defineComponent({
   props: {
    context: {
      type: Object,
      required: true
    },
  },
  setup() {

  },
})
</script>
<style lang="postcss" scoped>

</style>
