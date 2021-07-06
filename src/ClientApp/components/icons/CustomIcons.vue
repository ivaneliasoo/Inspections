<template>
  <div class="tw-flex-row">
    <svg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 20 20"
      :class="['tw-flex-col', 'tw-w-5', 'tw-h-5', 'tw-align-middle', color]"
    >
      <path fill-rule="evenodd" :d="icon" clip-rule="evenodd" />
    </svg>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  props: {
    name: {
      required: true,
      type: String,
      default: () => 'check'
    },
    color: {
      type: String,
      default: () => ''
    }
  },
  computed: {
    icon () {
      switch (this.name) {
        case 'check':
          return 'M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z'
        case 'dash':
          return 'M10 18a8 8 0 100-16 8 8 0 000 16zM7 9a1 1 0 000 2h6a1 1 0 100-2H7z'
        case 'cross':
          return 'M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z'
        case 'none':
          return 'M13.477 14.89A6 6 0 015.11 6.524l8.367 8.368zm1.414-1.414L6.524 5.11a6 6 0 018.367 8.367zM18 10a8 8 0 11-16 0 8 8 0 0116 0z'
        default:
          return ''
      }
    }
  }
})
</script>

<style scoped>
</style>
