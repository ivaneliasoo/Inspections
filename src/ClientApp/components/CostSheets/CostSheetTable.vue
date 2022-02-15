<template>
  <div>
    <v-container fluid class="my-n2 py-n2 mx-4 px-0 fill-height">
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
          <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols=11>
              <div class="text-h6" style="text-align: bottomz; display: inline">
                  Cost Sheets
              </div>
          </v-col>
      </v-row>

      <div v-show="costSheetTableVisible">
        <v-row>
          <v-col align="left" class="my-0 py-0">
            <v-btn small @click="newProject">
              New Project
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div style="height: 80vh; overflow-y: scroll">
              <table
                id="cost-sheets"
                class="costsheet-table">
                  <colgroup>
                    <col style="width:40%;">
                    <col style="width:24%;">
                    <col style="width:24%;">
                    <col style="width:12%;">
                  </colgroup>
                  <thead>
                      <tr>
                          <th class="font-weight-bold">Project</th>
                          <th class="font-weight-bold">Location</th>
                          <th class="font-weight-bold">Date created</th>
                          <th class="font-weight-bold">Actions</th>
                      </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(sheet, index) in costSheets" :key="index" style="height: 0px;">
                      <td class="text-caption text-left">
                        {{sheet.project}}
                      </td>
                      <td class="text-caption text-left">
                        {{sheet.location}}
                      </td>
                      <td class="text-caption text-left">
                        {{sheet.dateCreated}}
                      </td>
                      <td class="text-caption text-center">
                        <v-icon dense @click="editCostSheet(index)">mdi-pencil</v-icon>
                        <v-icon dense @click="deleteCostSheet(index)">mdi-delete</v-icon>
                      </td>
                    </tr>
                  </tbody>
              </table>
            </div>
          </v-col>
        </v-row>
      </div>
      <div v-if="costSheetVisible">
        <CostSheet
          @go-back="goBack" 
          :costSheet="selectedSheet">
        </CostSheet>
      </div>
    </v-container>
  </div>
</template>

<style scoped>
html {
  overflow-y: auto;
}

.costsheet-table, td, th {
  border: 1px solid black;
}

.costsheet-table td, .costsheet-table th {
  padding: 5px 5px 5px 5px;
}
</style>

<script>
import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';

var inMemorySheets = [
  new CostSheet({
    id: 1,
    project: "A&A works for new Server Room",
    location: "25 Serangoon North Ave 5",
    dateCreated: "01-04-2022",
    materialMarkup: 10.0,
    finalMarkup: 10.0,
    sections: [
      new Section({
        id: 1,
        secNumber: "1",
        description: "Switchboards and distribution Equipment",
        materialMarkup: 10,
        finalMarkup: 10,
        items: [
          new Item({
            id: 1,
            itemNumber: "1.1",
            description: "EMSB-2A",
            noCables: "",
            unitCost: 320056.0,
            units: 1,
            materialMarkup: 14,
            laborCostUnit: 600
          })
        ]
      }),
      new Section({
        id: 2,
        secNumber: "2",
        description: "Switchboards and distribution Equipment",
        materialMarkup: 10,
        finalMarkup: 10,
        items: [
          new Item({
            id: 2,
            itemNumber: "2.1",
            description: "EMSB-2B",
            noCables: "",
            unitCost: 320056.0,
            units: 1,
            materialMarkup: 14,
            laborCostUnit: 600
          })
        ]
      }),
      new Section({
        id: 3,
        secNumber: "3",
        description: "Switchboards and distribution Equipment",
        materialMarkup: 10,
        finalMarkup: 10,
        items: [
          new Item({
            id: 1,
            itemNumber: "3.1",
            description: "GSB-R-1",
            noCables: "",
            unitCost: 148750.0,
            units: 1,
            materialMarkup: 14,
            laborCostUnit: 600
          }),
          new Item({
            id: 2,
            itemNumber: "3.2",
            description: "Junction Box",
            noCables: "",
            unitCost: 8600.0,
            units: 2,
            materialMarkup: 10,
            laborCostUnit: 600
          }),
          new Item({
            id: 3,
            itemNumber: "3.3",
            description: "GSB Feed-in via Busduct modification to suit",
            noCables: "",
            unitCost: 3000.0,
            units: 1,
            materialMarkup: 10,
            laborCostUnit: 600
          })
        ]
      })
    ]
  }),
  new CostSheet({
    id: 2,
    project: "Sample project",
    location: "25 Serangoon North Ave 5",
    dateCreated: "02-05-2022",
    materialMarkup: 10.0,
    finalMarkup: 10.0,
    sections: [
      new Section({
        id: 1,
        secNumber: "1",
        description: "Switchboards and distribution Equipment",
        materialMarkup: 10,
        finalMarkup: 10,
        items: [
          new Item({
            id: 1,
            itemNumber: "1.1",
            description: "EMSB-2A",
            noCables: "",
            unitCost: 320056.0,
            units: 1,
            materialMarkup: 14,
            laborCostUnit: 600
          })
        ]
      }),
      new Section({
        id: 2,
        secNumber: "2",
        description: "Switchboards and distribution Equipment",
        materialMarkup: 10,
        finalMarkup: 10,
        items: [
          new Item({
            id: 1,
            itemNumber: "2.1",
            description: "EMSB-2B",
            noCables: "",
            unitCost: 320056.0,
            units: 1,
            materialMarkup: 14,
            laborCostUnit: 600
          }),
          new Item({
            id: 3,
            itemNumber: "2.2",
            description: "GSB Feed-in via Busduct modification to suit",
            noCables: "",
            unitCost: 3000.0,
            units: 1,
            materialMarkup: 10,
            laborCostUnit: 600
          })
        ]
      }),
      new Section({
        id: 3,
        secNumber: "3",
        description: "Switchboards and distribution Equipment",
        materialMarkup: 10,
        finalMarkup: 10,
        items: [
          new Item({
            id: 1,
            itemNumber: "3.1",
            description: "GSB-R-1",
            noCables: "",
            unitCost: 148750.0,
            units: 1,
            materialMarkup: 14,
            laborCostUnit: 600
          }),
          new Item({
            id: 2,
            itemNumber: "3.2",
            description: "Junction Box",
            noCables: "",
            unitCost: 8600.0,
            units: 2,
            materialMarkup: 10,
            laborCostUnit: 600
          })
        ]
      })
    ]
  }),
];

  export default {
    name: 'CostSheetTable',
    data: () => ({
      version: 'v0.101',
      costSheetTableVisible: true,
      costSheetVisible: false,
      costSheets: [],
      selectedSheet: new CostSheet()
    }),
    computed: {

    },
    created() {
      this.init();
    },
    updated() {
    },
    methods: {
      init() {
        console.log("init")
        this.costSheets = inMemorySheets;
        //console.log(this.costSheets);
      },
      newProject() {
        this.costSheet = new CostSheet();
        this.costSheetTableVisible = false;
        this.costSheetVisible = true;
      },
      getCostSheet() {
        console.log("selectedSheet", this.selectedSheet)
        return this.selectedSheet == -1 ? new CostSheet() : this.costSheets[this.selectedSheet];
      },
      editCostSheet(index) {
        console.log("Editing cost sheet", index);
        this.selectedSheet = this.costSheets[index];
        console.log("Editing cost sheet", JSON.stringify(this.selectedSheet));
        this.costSheetTableVisible = false;
        this.costSheetVisible = true;
      },
      deleteCostSheet(index) {

      },
      goBack() {
        this.costSheetVisible = false;
        this.costSheetTableVisible = true;
      }
    }
  }
</script>