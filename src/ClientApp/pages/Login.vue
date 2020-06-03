<template>
  <div>
    <h1>{{ title }}</h1>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api'
export default defineComponent({
  layout: 'gest',
  setup() {
    const title = ref('Login')

    return {
      title
    }
  }
})
</script>

<style>

</style>
