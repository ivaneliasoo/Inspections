<template>
  <div>
    <v-container v-show="costSheetVisible" class="my-n2 py-n2 mx-4 px-0 fill-height">
      <v-row>
        <!-- <v-col class="text-left mt-0 pt-0 mb-n4 pb-n4" cols=1>
            <main-menu
                @show-job-projection-overview="showJobProjectionOverview"
                @show-job-schedule="showJobSchedule"
                @show-man-power="showManPower"
                @show-man-power-forecast="showManPowerForecast"
                @open-team-dialog="openTeamDialog"
                @edit-options="editOptions"
                @save="save"
            >
            </main-menu>
        </v-col> -->
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 text-left" cols="2">
          <v-btn icon @click="goBack">
            <v-icon>
              mdi-arrow-left
            </v-icon>
          </v-btn>
          &nbsp;
          <v-btn @click="saveSheet">
            Save
          </v-btn>
        </v-col>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 text-left text-h5" cols="1">
          Project:
        </v-col>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 text-left" cols="6">
          <input
            class="text-h5 table-input"
            type="text"
            v-model="costSheet.project"
          />
        </v-col>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols="3">
            <div class="text-center text-h5">
                Costing Sheet
            </div>
        </v-col>
      </v-row>
      <v-row>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 text-left" cols="2">
        </v-col>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 text-left text-body" cols="1">
          Location:
        </v-col>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 text-left text-body" cols="7">
          <input
            class="table-input"
            type="text"
            v-model="costSheet.location"
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <div class="container">
            <div class="table-container">
              <table id="cost-sheets" class="cost-sheet">
                <thead class="cs-section">
                    <tr style="border-bottom: 1px solid;">
                        <th colspan="2" class="text-h5 title-2">Descriptions(s)</th>
                        <th colspan="5" class="text-h5 title-4">Material(s)</th>
                        <th colspan="2" class="text-h5 title-2">Labour</th>
                        <th colspan="4" class="text-h5 title-4">Summary</th>
                    </tr>
                    <tr>
                        <th class="text-caption font-weight-bold title-1">S/N</th>
                        <th class="text-caption font-weight-bold title-2">Descriptions(s)</th>

                        <th class="text-caption font-weight-bold title-3">No. of Cables<br>(Apply to cables)</th>
                        <th class="text-caption font-weight-bold title-3">Unit cost</th>
                        <th class="text-caption font-weight-bold title-3">Unit(s) (m/nos/pcs/lot)</th>
                        <th class="text-caption font-weight-bold title-3">Material Cost</th>
                        <th class="text-caption font-weight-bold title-4">Mark up on Material</th>

                        <th class="text-caption font-weight-bold title-1">Labour Cost to do the work per unit</th>
                        <th class="text-caption font-weight-bold title-2">Labour Cost</th>

                        <th class="text-caption font-weight-bold title-3">Price to do the work</th>
                        <th class="text-caption font-weight-bold title-3">Summation</th>
                        <th class="text-caption font-weight-bold title-3">Final Mark Up</th>
                        <th class="text-caption font-weight-bold title-4">To Quote price</th>
                    </tr>
                    <tr>
                        <th colspan="2" class="title-2"></th>

                        <th colspan="4" class="title-3"></th>
                        <th class="text-caption font-weight-bold title-4">
                          <NumberField
                            v-model="costSheet.materialMarkup" format="percent" @change="updateSheet">
                          </NumberField>
                        </th>

                        <th class="text-caption font-weight-bold title-1"></th>
                        <th class="text-caption font-weight-bold title-2"></th>

                        <th class="text-caption font-weight-bold title-3"></th>
                        <th class="text-caption font-weight-bold title-3"></th>
                        <th class="text-caption font-weight-bold title-3">
                          <NumberField
                            v-model="costSheet.finalMarkup" format="percent" @change="updateSheet">
                          </NumberField>                            
                        </th>
                        <th class="text-caption font-weight-bold title-4"></th>
                    </tr>
                </thead>
                <Section v-for="(section, index) in costSheet.sections" :key="index" style="height: 0px;"
                  :section="section"
                  :index="index"
                  :trigger-update="updateSection"
                  @add-section=addSection
                  @del-section=delSection
                  @update-sheet=updateSheet>
                </Section>
                <tbody class="cs-section">
                  <tr>
                    <td colspan="2" class="title-4">
                    </td>
                    <td colspan="3" class="text-right title-3">
                      Material Cost
                    </td>
                    <td class="title-3">
                      <NumberField
                        :value="costSheet.materialCost()" format="currency" :readOnly="true">
                      </NumberField>
                    </td>
                    <td colspan="2" class="text-right title-3">
                      Labour Cost
                    </td>
                    <td class="title-3">
                      <NumberField
                        :value="costSheet.labourCost()" format="currency" :readOnly="true">
                      </NumberField>
                    </td>
                    <td colspan="3" class="text-right title-3">
                      Total Quote
                    </td>
                    <td class="text-right title-3">
                      <NumberField
                        :value="costSheet.toQuotePrice()" format="currency" :readOnly="true">
                      </NumberField>
                    </td>
                  </tr>
                </tbody>

                <tbody class="cs-section">
                  <td colspan="10" class="text-h5 text-left">
                    Summary
                  </td>
                </tbody>

                <tbody class="cs-section">
                  <tr>
                    <td colspan="2" class="text-left title-1">
                      Set Material Mark Up:
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        v-model="costSheet.materialMarkup" format="percent" @change="updateSheet">
                      </NumberField>              
                    </td>
                    <td colspan="10" class="title-1">
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" class="text-left title-1">
                      Set Labour Daily Rate (Day):
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        v-model="costSheet.labourDailyRate" format="currency" @change="updateSheet">
                      </NumberField>                            
                    </td>
                    <td colspan="10" class="title-1">
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" class="text-left title-1">
                      Final Overall Markup:
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        v-model="costSheet.finalMarkup" format="currency" @change="updateSheet">
                      </NumberField>                            
                    </td>
                    <td colspan="10" class="title-1">
                    </td>
                  </tr>

                  <tr>
                    <td colspan="13" class="title-1">
                    </td>
                  </tr>
                  <tr>
                    <td colspan="13" class="title-1">
                    </td>
                  </tr>

                  <tr>
                    <td colspan="2" class="text-left title-1">
                      Total Sales:
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.toQuotePrice()" format="currency" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td colspan="10" class="title-1">
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" class="text-left title-1">
                      COGS:
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.materialCost()" format="currency" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.materialCostPercent()" format="percent" :decimals="2" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td colspan="9" class="title-1">
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" class="text-left title-1">
                      Labour cost:
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.labourCost()" format="currency" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.labourCostPercent()" format="percent" :decimals="2" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td class="title-1">
                    </td>
                    <td colspan="3" class="text-left title-1">
                      Translate to number of days to complete:
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.numberOfDaysComplete()" :decimals="2" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td colspan="4" class="title-1">
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" class="text-left title-1">
                      Gross Profit:
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.grossProfit()" format="currency" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td class="text-caption font-weight-bold title-1">
                      <NumberField
                        :value="costSheet.grossProfitPercent()" format="percent" :decimals="2" :readOnly="true">
                      </NumberField>                            
                    </td>
                    <td colspan="10" class="title-1">
                    </td>
                  </tr>
                </tbody>

              </table>
            </div>
          </div>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<style scoped>
html {
  overflow-y: auto;
}

/* https://stackoverflow.com/questions/41539803/html-table-wont-scroll-horizontal */
.container {
  width: 100%;
}

.table-container {
	overflow-x: auto;
}

.cost-sheet {
	width: 100%;
	max-width: 100%;
	margin-bottom: 10px;
}

td, th {
  min-width: 120px;
} 

/* .costsheet-table {
  display: block;
  max-width: 100%;
  white-space: nowrap;
} */

input {
  width: 100%;
}

.cs-section {
  border: 1px solid black;
}

.title-1 {
  background-color: #d5c9df;
  padding: 2px;
}

.title-2 {
  background-color: #d5c9df;
  border-right: 1px solid;
  padding: 2px;
}

.title-3 {
  background-color: #c0aed0;
  padding: 2px;
}

.title-4 {
  background-color: #c0aed0;
  border-right: 1px solid;
  padding: 2px;
}

.title-5 {
  background-color: #eae4ef;
  border-right: 1px solid;
  padding: 2px;
}

.title-6 {
  background-color: #eae4ef;
  padding: 2px;
}

</style>

<script>
import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';

  export default {
    props: {
      costSheet: Object,
      triggerUpdate: Number
    },
    data: () => ({
      version: 'v0.101',
      costSheetVisible: true,
      updateSection: 0,
      timer: null
    }),
    computed: {
      labourCostPercent() {
        return this.costSheet.toQuotePrice() / this.costSheet.labourCost();
      },
      grossProfitPercent() {
        return this.costSheet.toQuotePrice() / this.costSheet.grossProfit();
      }
    },
    watch: { 
      triggerUpdate(newVal, oldVal) {
        this.updateSheet();
      }
    },
    mounted() {
      this.timer = setInterval(this.saveSheet, 120 * 1000);
      //console.log("Auto save started");
    },
    methods: {
      saveSheet() {
        // console.log("Saving sheet");
        this.$emit('save-sheet', this.costSheet);
      },
      goBack() {
        clearInterval(this.timer);
        this.timer = null;
        // console.log("Auto save stopped");
        this.$emit('go-back');
      },
      addSection(index, position) {
        const newSection = new Section();
        newSection.id = 0;
        const newSecIndex = (position == "above") ? index : index + 1;
        this.costSheet.sections.splice(newSecIndex, 0, newSection);
        this.costSheet.updated = true;
        this.$forceUpdate();
      },
      delSection(index) {
        this.costSheet.sections.splice(index, 1);
        this.costSheet.updated = true;
        this.$forceUpdate();
      },
      updateSheet() {
        this.costSheet.updated = true;
        this.$forceUpdate();
        this.updateSection++;
      }
    }
  }
</script>