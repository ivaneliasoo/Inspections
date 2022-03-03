<template>
  <tr>
    <td style="width: 200px" class="text-body-2 text-left">
      {{title}}
    </td>
    <td class="text-caption font-weight-bold title-7">
      <NumberField
        :value="costSheet[valueMethod](lowerPercent)" format="currency" :readOnly="true">
      </NumberField>
    </td>
    <td class="text-caption font-weight-bold title-7" :style="style">
      <NumberField
        :value="costSheet[percentMethod](lowerPercent)" format="percent" :readOnly="true">
      </NumberField>
    </td>
    <td class="text-caption font-weight-bold title-7">
      <NumberField
        :value="costSheet[valueMethod]()" format="currency" :bold="true" :readOnly="true">
      </NumberField>
    </td>
    <td class="text-caption font-weight-bold title-7" :style="style">
      <NumberField
        :value="costSheet[percentMethod]()" format="percent" :bold="true" :readOnly="true">
      </NumberField>
    </td>
    <td class="text-caption font-weight-bold title-7">
      <NumberField
        :value="costSheet[valueMethod](higherPercent)" format="currency" :readOnly="true">
      </NumberField>
    </td>
    <td class="text-caption font-weight-bold title-7" :style="style">
      <NumberField
        :value="costSheet[percentMethod](higherPercent)" format="percent" :readOnly="true">
      </NumberField>
    </td>
  </tr>
</template>

<style scoped>
input {
  width: 100%;
}

td {
  border: 1px solid black;
}
</style>

<script>
import { Section, Item } 
    from '../../composables/costsheets/entity.js';

export default {
  props: {
    costSheet: Object,
    title: String,
    valueMethod: String,
    percentMethod: String,
    lowerPercent: Number,
    higherPercent: Number,
    highlight: Boolean,
    triggerUpdate: Number
  },
  data: () => ({
    selected: null,
    contextMenuVisible: false,
    rowIndex: 0,
    xcoord: 0,
    ycoord: 0,
    inactive: true
  }),
  watch: { 
    triggerUpdate(newVal, oldVal) {
      this.$forceUpdate();
    }
  },
  computed: {
    style() {
      if (this.highlight) {
        return "background-color: #9eedca;"
      }
      return "";
    }
  }
};
</script>
