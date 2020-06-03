<template>
  <div>
    {{ name }}
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from '@vue/composition-api'

export default defineComponent({
  setup(props, context) {
    const name = computed(() => context.root.$accessor.inspections.name)

    return {
      name
    }
  }
})
</script>
