<template>
  <div
    class="tw-shadow-md tw-rounded-sm tw-m-1 tw-p-1 tw-flex tw-flex-row tw-justify-between tw-align-middle"
    @click="changeValue()"
  >
    <div class="tw-m-1 tw-p-1 tw-flex tw-flex-row tw-justify-start">
      <div class="tw-text-sm tw-flex-col tw-align-middle">
        {{ index }} {{ localItem.text }}
        <span class="tw-flex-col tw-text-red-600 tw-text-xs"> Required </span>
      </div>
    </div>
    <div class="tw-flex-row">
      <CustomIcons :name="checkProps.name" :color="checkProps.color" />
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import { CheckListItem } from '~/types'

export default Vue.extend({
  props: ['item', 'index'],
  computed: {
    localItem (): CheckListItem {
      return this.item
    },
    checkProps (): { name:string; color:string; } {
      switch (this.localItem.checked) {
        case 0:
          return { name: 'cross', color: 'tw-text-red-600' }
        case 1:
          return { name: 'check', color: 'tw-text-green-600' }
        case 2:
          return { name: 'dash', color: 'tw-text-yellow-600' }
        case 3:
          return { name: 'none', color: 'tw-text-blue-600' }
        default:
          return { name: '', color: '' }
      }
    }
  },
  methods: {
    changeValue () {
      this.localItem.checked = this.localItem.checked === 3 ? this.localItem.checked = 0 : this.localItem.checked + 1
      this.$emit('update:item', this.localItem)
    }
  }
})
</script>
