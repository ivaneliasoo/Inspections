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
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols=2>
          <v-btn icon @click="$emit('go-back')">
            <v-icon>
              mdi-arrow-left
            </v-icon>
          </v-btn>
          <v-btn text>
            Save
          </v-btn>
        </v-col>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols=4>
          {{costSheet.project}}
        </v-col>
        <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols=6>
            <div class="text-h6" style="text-align: bottomz; display: inline">
                Cost Sheet
            </div>
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
                          <th colspan="2" class="text-h5 title-5">Labour</th>
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

                          <th class="text-caption font-weight-bold title-6">Labour Cost to do the work per unit</th>
                          <th class="text-caption font-weight-bold title-5">Labour Cost</th>

                          <th class="text-caption font-weight-bold title-3">Price to do the work</th>
                          <th class="text-caption font-weight-bold title-3">Summation</th>
                          <th class="text-caption font-weight-bold title-3">Final Mark Up</th>
                          <th class="text-caption font-weight-bold title-4">To Quote price</th>
                      </tr>
                      <tr>
                          <th colspan="2" class="title-2"></th>

                          <th colspan="4" class="title-3"></th>
                          <th class="text-caption font-weight-bold title-4">
                            <input
                              class="text-caption"
                              type="text"
                              v-model="costSheet.materialMarkup"
                            />
                          </th>

                          <th class="text-caption font-weight-bold title-6"></th>
                          <th class="text-caption font-weight-bold title-5"></th>

                          <th class="text-caption font-weight-bold title-3"></th>
                          <th class="text-caption font-weight-bold title-3"></th>
                          <th class="text-caption font-weight-bold title-3">
                            <input
                              class="text-caption"
                              type="text"
                              v-model="costSheet.finalMarkup"
                            />
                          </th>
                          <th class="text-caption font-weight-bold title-4"></th>
                      </tr>
                  </thead>
                  <Section v-for="(section, index) in costSheet.sections" :key="index" style="height: 0px;"
                    :section="section"
                    :materialMarkup="costSheet.materialMarkup"
                    :finalMarkup="costSheet.finalMarkup">
                  </Section>
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
  /* display: table; */
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
      costSheet: Object
    },
    data: () => ({
      version: 'v0.101',
      costSheetVisible: true,
    }),
    computed: {
      toQuotePrice() {
        return 0;
      },
      summation() {
        return 0;
      }
    },
    created() {
    },
    updated() {
    },
    methods: {
    }
  }
</script>