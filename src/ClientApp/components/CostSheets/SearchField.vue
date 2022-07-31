<template>
  <div width="width">
    <input
      v-model="searchValue"
      class="text-body-1 text-left"
      type="search"
      :size="size"
      :placeholder="placeHolder"
      style="border: none; border-color: transparent; outline: none;"
      @input="search"
    >
    <v-icon @click="findPrevious">
      mdi-chevron-up
    </v-icon>
    <v-icon @click="findNext">
      mdi-chevron-down
    </v-icon>
  </div>
</template>

<script>
import { Section, Item, CostSheet }
  from '../../utils/costsheets/entity.js'
import { endpoint } from '../../utils/costsheets/util.js'
import { toIsoDate } from '~/utils/jp_util.js'

export default {
  props: {
    data: Array,
    fieldNames: Array,
    placeHolder: String,
    size: Number,
    compareFunc: String
  },
  data: () => ({
    // fieldName: "project",
    searchValue: '',
    searchIndex: -1,
    curRow: null
  }),
  methods: {
    startsWith (dataValue, searchValue) {
      return dataValue.startsWith(searchValue)
    },
    includes (dataValue, searchValue) {
      return dataValue.toLowerCase().includes(searchValue.toLowerCase())
    },
    search () {
      const compareFunc = this.compareFunc
      this.searchIndex = -1
      for (let i = 0; i < this.data.length; i++) {
        for (const field of this.fieldNames) {
          if (this[compareFunc](this.data[i][field], this.searchValue)) {
            this.scrollTo(i)
            this.searchIndex = i
            return
          }
        }
      };
    },
    findNext () {
      if (this.searchIndex === -1) {
        return
      }
      const compareFunc = this.compareFunc
      for (let i = this.searchIndex + 1; i < this.data.length; i++) {
        for (const field of this.fieldNames) {
          if (this[compareFunc](this.data[i][field], this.searchValue)) {
            this.scrollTo(i)
            this.searchIndex = i
            return
          }
          if (i >= this.data.length - 1) {
            i = 0
          }
        }
      };
    },
    findPrevious () {
      if (this.searchIndex === -1) {
        return
      }
      const compareFunc = this.compareFunc
      for (let i = this.searchIndex - 1; i >= 0; i--) {
        for (const field of this.fieldNames) {
          if (this[compareFunc](this.data[i][field], this.searchValue)) {
            this.scrollTo(i)
            this.searchIndex = i
            return
          }
          if (i <= 0) {
            i = this.data.length - 1
          }
        }
      };
    },
    scrollTo (line) {
      // console.log("scrollTo", line);
      this.$emit('scroll_to', line)
    }
  }
}
</script>

<style scoped>
</style>
