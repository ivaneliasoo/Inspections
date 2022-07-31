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
    @click:clear="filterText=''; $emit('filter', filterText); $emit('clear', filterText)"
  />
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'nuxt-property-decorator'

@Component
export default class GridFilter extends Vue {
  @Prop({
    type: String,
    default: ''
  })
    filter!: String

  @Prop({
    type: String,
    default: 'enter text'
  })
    placeholder!: String

  placeholderText: String = this.placeholder

  get filterText () {
    return this.filter
  }

  set filterText (value) {
    this.$emit('update:filter', value)
  }

  keydown (event: any) {
    if (event.code === 'NumpadEnter' || event.code === 'Enter') {
      this.$emit('filter', this.filter)
    }
  }
}
</script>

<style>
</style>
