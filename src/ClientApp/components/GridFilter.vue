<template>
  <v-text-field
    id="txtFilter"
    v-model="filterText"
    name="txtFilter"
    :label="placeholderText"
    single-line
    clearable
    hide-details
    append-icon="mdi-magnify"
    placeholder="enter search text here.."
    @keydown="keydown"
    @click:append="$emit('filter', filterText)"
    @click:clear="
      filterText = '';
      $emit('filter', filterText);
      $emit('clear', filterText);
    "
  />
</template>

<script lang="ts">
import { computed, defineComponent } from '@nuxtjs/composition-api'

export default defineComponent({
  props: {
    filter: {
      type: String,
      default: '',
    },
    placeholder: {
      type: String,
      default: 'enter text',
    },
  },
  setup (props, { emit }) {
    const placeholderText = computed(() => props.placeholder)

    const filterText = computed({
      get: () => props.filter,
      set: (value) => {
        emit('update:filter', value)
      },
    })

    const keydown = (event: any) => {
      if (event.code === 'NumpadEnter' || event.code === 'Enter') {
        emit('filter', props.filter)
      }
    }

    return {
      filterText,
      placeholderText,
      keydown,
    }
  },
})
</script>

<style>
</style>
