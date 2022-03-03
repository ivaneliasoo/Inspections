<template>
  <td colspan="7">
    <table>
      <thead class="header">
        <th class="text-body2 text-left">Final Markup</th>
        <th class="text-body2" colspan="2">5.00%</th>
        <th class="text-body2" colspan="2">10%</th>
        <th class="text-body2" colspan="2">15%</th>	
      </thead>
      <tbody class="cs-section">
        <SummaryRow
          :costSheet="costSheet"
          title="Total Sales:"
          valueMethod="totalSales"
          percentMethod="totalSalesPercent"
          :lowerPercent="-5"
          :higherPercent="5"
          :triggerUpdate="updateRow">
        </SummaryRow>
        <SummaryRow
          :costSheet="costSheet"
          title="Material Cost:"
          valueMethod="materialCost"
          percentMethod="materialCostPercent"
          :lowerPercent="-5"
          :higherPercent="5"
          :triggerUpdate="updateRow">
        </SummaryRow>
        <SummaryRow
          :costSheet="costSheet"
          title="Labour Cost:"
          valueMethod="labourCost"
          percentMethod="labourCostPercent"
          :lowerPercent="-5"
          :higherPercent="5"
          :triggerUpdate="updateRow">
        </SummaryRow>
        <SummaryRow
          :costSheet="costSheet"
          title="Gross Profit:"
          valueMethod="grossProfit"
          percentMethod="grossProfitPercent"
          :lowerPercent="-5"
          :higherPercent="5"
          :highlight="true"
          :triggerUpdate="updateRow">
        </SummaryRow>
      </tbody>
    </table>
  </td>
</template>

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

<script>
import { Section, Item } 
    from '../../composables/costsheets/entity.js';

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
    toQuotePrice() {
      return 0;
    },
    summation() {
      return 0;
    }
  },
  watch: { 
    triggerUpdate(newVal, oldVal) {
      this.$forceUpdate();
    }
  },
  mounted() {
  },
  methods: {
  }
};
</script>
