<template>
  <td colspan="7">
    <table id="summary">
      <tbody class="header">
        <th class="text-body2 text-left">
          Final Markup
        </th>
        <th class="text-body2" colspan="2">
          5.00%
        </th>
        <th class="text-body2" colspan="2">
          10%
        </th>
        <th class="text-body2" colspan="2">
          15%
        </th>
      </tbody>
      <tbody class="cs-section">
        <SummaryRow
          :cost-sheet="costSheet"
          title="Total Sales:"
          value-method="totalSales"
          percent-method="totalSalesPercent"
          :lower-percent="-5"
          :higher-percent="5"
          :trigger-update="updateRow"
        />
        <SummaryRow
          :cost-sheet="costSheet"
          title="Material Cost:"
          value-method="materialCost"
          percent-method="materialCostPercent"
          :lower-percent="-5"
          :higher-percent="5"
          :trigger-update="updateRow"
        />
        <SummaryRow
          :cost-sheet="costSheet"
          title="Labour Cost:"
          value-method="labourCost"
          percent-method="labourCostPercent"
          :lower-percent="-5"
          :higher-percent="5"
          :trigger-update="updateRow"
        />
        <SummaryRow
          :cost-sheet="costSheet"
          title="Gross Profit:"
          value-method="grossProfit"
          percent-method="grossProfitPercent"
          :lower-percent="-5"
          :higher-percent="5"
          :highlight="true"
          :trigger-update="updateRow"
        />
      </tbody>
    </table>
  </td>
</template>

<script>
import { Section, Item }
  from '../../utils/costsheets/entity.js'

export default {
  props: {
    costSheet: Object,
    triggerUpdate: Number
  },
  data: () => ({
    selected: null,
    updateRow: 0
  }),
  computed: {
    toQuotePrice () {
      return 0
    },
    summation () {
      return 0
    }
  },
  watch: {
    triggerUpdate (newVal, oldVal) {
      this.$forceUpdate()
    }
  },
  mounted () {
  },
  methods: {
  }
}
</script>

<style scoped>
input {
  width: 100%;
}

.header {
  border: 1px solid black;
  background-color: #7f7f7f;
  color: white;
}

.cs-section {
  border: 1px solid black;
}

th {
  background-color: #7f7f7f;
  padding: 1px;
}

td {
  background-color: #eae4ef;
  padding: 1px;
}

</style>
