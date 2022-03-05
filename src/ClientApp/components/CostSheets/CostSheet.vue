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
                    &nbsp;
                  </td>
                </tbody>

                <tbody class="cs-section">
                  <tr>
                    <td class="title-7"></td>
                    <td colspan="9" class="text-h5 text-left title-7">
                      Summary
                    </td>
                    <td colspan="2" rowspan="4" class="title-7">
                      <img src="~/static/Logo.jpeg" />
                    </td>
                    <td class="title-7"></td>
                  </tr>
                  <tr>
                    <td class="title-7"></td>
                    <td colspan="2" class="text-body-2 text-left title-7">
                      Set Material Mark Up:
                    </td>
                    <td class="text-caption font-weight-bold title-7">
                      <NumberField
                        v-model="costSheet.materialMarkup" format="percent" @change="updateSheet">
                      </NumberField>              
                    </td>
                    <td colspan="9" class="title-7">
                    </td>
                  </tr>
                  <tr>
                    <td class="title-7"></td>
                    <td colspan="2" class="text-body-2 text-left title-7">
                      Set Labour Daily Rate (Day):
                    </td>
                    <td class="text-caption font-weight-bold title-7">
                      <NumberField
                        v-model="costSheet.labourDailyRate" format="currency" @change="updateSheet">
                      </NumberField>                            
                    </td>
                    <td colspan="9" class="title-7">
                    </td>
                  </tr>
                  <tr>
                    <td class="title-7"></td>
                    <td colspan="2" class="text-body-2 text-left title-7">
                      Set Labour multiplier (Night):
                    </td>
                    <td class="text-caption font-weight-bold title-7">
                      <NumberField
                        v-model="costSheet.labourNightMultiplier" suffix="x" @change="updateSheet">
                      </NumberField>                            
                    </td>
                    <td colspan="9" class="title-7">
                    </td>
                  </tr>
                  <tr>
                    <td class="title-7"></td>
                    <td colspan="2" class="text-body-2 text-left title-7">
                      Final Overall Markup:
                    </td>
                    <td class="text-caption font-weight-bold title-7">
                      <NumberField
                        v-model="costSheet.finalMarkup" format="percent" @change="updateSheet">
                      </NumberField>                            
                    </td>
                    <td colspan="9" class="title-7">
                    </td>
                  </tr>

                  <tr>
                    <td colspan="13" class="text-caption title-7">
                      &nbsp;
                    </td>
                  </tr>

                  <tr>
                    <td class="title-7"></td>
                    <Summary
                      :cost-sheet="costSheet"
                      :trigger-update="updateSummary">
                    </Summary>
                    <td rowspan="5" class="title-7"></td>
                    <td colspan="4">
                      <table>
                        <tbody>
                          <tr>
                            <td colspan="3" class="text-body-2 text-left title-7">
                              Translate to number of days to complete:
                            </td>
                            <td class="text-caption font-weight-bold title-7">
                              <NumberField
                                :value="costSheet.numberOfDaysComplete()" :decimals="2" suffix=" " :readOnly="true">
                              </NumberField>                            
                            </td>
                          </tr>
                          <tr>
                            <td colspan="3" class="text-body-2 text-left title-7">
                              Translate to number of nights to complete:
                            </td>
                            <td class="text-caption font-weight-bold title-7">
                              <NumberField
                                :value="costSheet.numberOfNightsComplete()" :decimals="2" suffix=" " :readOnly="true">
                              </NumberField>                            
                            </td>
                          </tr>
                          <tr>
                            <td colspan="5" class="title-7" style="font-size: 6px">
                              &nbsp;
                            </td>
                          </tr>
                          <tr>
                            <td class="text-body-2 text-left title-7" style="background-color: #31b3c5; border: 1px solid black;">
                              Super
                            </td>
                            <td colspan="2" class="text-body-2 text-left title-7">
                              &nbsp; When GP &gt;20%
                            </td>
                            <td class="title-7"></td>
                          </tr>
                          <tr>
                            <td class="text-body-2 text-left title-7" style="background-color: #9eedca; border: 1px solid black;">
                              Healthy
                            </td>
                            <td colspan="2" class="text-body-2 text-left title-7">
                              &nbsp; When 10% &gt; GP &gt; 19.9%
                            </td>
                            <td class="title-7"></td>
                          </tr>
                          <tr>
                            <td class="text-body-2 text-left title-7" style="background-color: #c04225; border: 1px solid black;">
                              Low
                            </td>
                            <td colspan="2" class="text-body-2 text-left title-7">
                              &nbsp; When GP &lt;10%
                            </td>
                            <td class="title-7"></td>
                          </tr>
                        </tbody>
                      </table>
                    </td>
                  </tr>
                  <tr>
                    <td colspan="13" class="title-7" style="font-size: 6px">
                      &nbsp;
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
  min-width: 110px;
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

.title-7 {
  background-color: #eae4ef;
  padding-left: 2px;
  padding-right: 2px;
  padding-top: 1px;
  padding-bottom: 1px;
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
      updateSummary: 0,
      timer: null
    }),
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
        const newSection = new Section({materialMarkup: this.costSheet.materialMarkup});
        const newSecIndex = (position == "above") ? index : index + 1;
        this.costSheet.sections.splice(newSecIndex, 0, newSection);
        this.costSheet.renumberSections();
        this.costSheet.updated = true;
        this.$forceUpdate();
      },
      delSection(index) {
        this.costSheet.sections.splice(index, 1);
        this.costSheet.renumberSections();
        this.costSheet.updated = true;
        this.$forceUpdate();
      },
      updateSheet() {
        this.costSheet.updated = true;
        this.$forceUpdate();
        this.updateSection++;
        this.updateSummary++;
      }
    }
  }
</script>