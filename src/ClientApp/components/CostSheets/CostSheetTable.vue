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
          <!-- <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols=11>
              <div class="text-h6" style="text-align: bottomz; display: inline">
                  Cost Sheets
              </div>
          </v-col> -->
      </v-row>

      <div v-show="costSheetTableVisible">
        <v-row>
          <v-col cols="9" class="my-0 py-0 text-left text-h5">
            Costing Sheets
          </v-col>
          <v-col cols="3" class="my-0 py-0 text-center">
            <v-btn small @click="openNewProjectDialog">
              New cost sheet
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12">
            <div style="height: 80vh; width: 800px; overflow-y: auto;">
              <table
                id="cost-sheets"
                class="costsheet-table">
                  <thead>
                      <tr>
                          <th class="font-weight-bold" style="width:34%;">Project</th>
                          <th class="font-weight-bold" style="width:34%;">Location</th>
                          <th class="font-weight-bold" style="width:20%;">Date created</th>
                          <th class="font-weight-bold" style="width:12%;">Actions</th>
                      </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(sheet, index) in costSheets" :key="index" style="height: 0px;">
                      <td class="text-caption text-left" style="width:34%;">
                        {{sheet.project}}
                      </td>
                      <td class="text-caption text-left" style="width:34%;">
                        {{sheet.location}}
                      </td>
                      <td class="text-caption text-center" style="width:20%;">
                        {{sheet.dateCreated}}
                      </td>
                      <td class="text-caption text-center" style="width:12%;">
                        <v-btn icon small @click="editCostSheet(index)">
                          <v-icon>
                            mdi-pencil
                          </v-icon>
                        </v-btn>
                        <v-btn icon small @click="deleteCostSheet(index)">
                          <v-icon>
                            mdi-delete
                          </v-icon>
                        </v-btn>
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
          ref="costSheet"
          @go-back="goBack"
          @save-sheet="saveSheet"
          :costSheet="selectedSheet"
          :triggerUpdate="updateSheet">
        </CostSheet>
      </div>
    </v-container>

    <v-dialog v-model="newSheetDialog" max-width="600px">
      <v-card>
        <v-card-title class="mb-0 pb-0">
          <span class="mx-2 headline">New Cost Sheet</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  label="Project"
                  :rules="[rules.required]"
                  v-model="newSheet.project">
                </v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  label="Location"
                  v-model="newSheet.location">
                </v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  suffix="%"
                  label="Material markup"
                  :rules="[rules.number]"
                  v-model="newSheet.materialMarkup">
                </v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  prefix="$"
                  label="Labour team daily rate"
                  :rules="[rules.number]"
                  v-model="newSheet.labourDailyRate">
                </v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  label="Labour team multiplier (night)"
                  :rules="[rules.number]"
                  v-model="newSheet.labourNightMultiplier">
                </v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  suffix="%"
                  label="Final overall markup"
                  :rules="[rules.number]"
                  v-model="newSheet.finalMarkup">
                </v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="newCostSheet">
            Ok
          </v-btn>
          <v-btn @click="newSheetDialog=false">
            Cancel
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="alert" max-width="600px">
      <v-card>
        <v-card-title class="text-body-1"></v-card-title>
        <v-card-text>
          <p class="header3" style="white-space: pre-wrap;">{{ userMessage }}</p>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="closeMessage">OK</v-btn>
          <v-spacer></v-spacer>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="confirm" max-width="600px">
      <v-card>
        <v-card-title class="text-body-1"></v-card-title>
        <v-card-text>
          <p class="header3" style="white-space: pre-wrap;">{{ userMessage }}</p>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="confirmOk">OK</v-btn>
          <v-btn @click="confirmCancel">Cancel</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

  </div>
</template>

<style scoped>
html {
  overflow-y: auto;
}

.costsheet-table, td, th {
  width: 100%;
  border: 1px solid black;
}

.costsheet-table td, .costsheet-table th {
  padding: 5px 5px 5px 5px;
}
</style>

<script>
import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';

  export default {
    name: 'CostSheetTable',
    data: () => ({
      version: 'v0.101',
      costSheetTableVisible: true,
      costSheetVisible: false,
      costSheets: [],
      deleteIndex: null,
      selectedSheet: new CostSheet(),
      newSheetDialog: false,
      newSheet: {},
      updateSheet: 0,
      rules: {
        required: value => !!value || 'Required.',
        number: value => {
          const pattern = /^\d*\.?\d*$/
          return pattern.test(value) || 'Invalid number'
        }
      },
      alert: false,
      userMessage: "",
      confirm: false,
      okAction: null,
      cancelAction: null
    }),
    computed: {

    },
    mounted() {
      this.init();
    },
    updated() {
    },
    methods: {
      init() {
        console.log("init")
        //this.costSheets = inMemorySheets;
        this.getCostSheets();
      },
      endpoint(p) {
        const param = p ? "/" + p : ""
        //return `http://localhost:5000/api/costsheet${param}`
        return `/costsheet${param}`;
      },
      getCostSheets() {
        this.$axios({
          url: this.endpoint(),
          method: "GET",
          crossDomain: true,
          responseType: 'json',
          transformResponse: (response) => {
            return JSON.parse(response, (key, value) =>{
              if (key == "dateCreated") {
                return new Intl.DateTimeFormat('en-SG').format(new Date(value))
              } 
              return value;
            })
          }
        }).then(response => {
          this.costSheets = response.data.sort((c1, c2) => c1.id - c2.id);
        }).catch(function (error) {
          console.log(error);
        });
      },
      parseResponse(response) {
        const costSheet = JSON.parse(response, (key, value) =>{
          if (key == "dateCreated") {
            return new Date(value);
          } else if (value && typeof value === "object") {
            if (value.hasOwnProperty("project")) {
              return new CostSheet(value);
            } else if (value.hasOwnProperty("secNumber")) {
              return new Section(value);
            } else if (value.hasOwnProperty("itemNumber")) {
              return new Item(value);
            }
          }
          return value;
        });
        return costSheet;
      },
      editCostSheet(index) {
        this.$axios({
          url: this.endpoint(this.costSheets[index].id),
          method: "GET",
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse
        }).then(response => {
          this.selectedSheet = response.data;
          this.costSheetTableVisible = false;
          this.costSheetVisible = true;
        }).catch(function (error) {
          console.log(error);
        });
      },
      saveSheet(sheet) {
        var self = this;
        this.$axios({
          url: self.endpoint(),
          method: "PUT",
          data: sheet,
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse
        })
        .then(function (response) {
          console.log("Cost sheet updated");
        })
        .catch(function (error) {
          console.log(error);
          self.showMessage("There was a server error when saving the cost sheet");
        });
      },
      createCostSheet(sheet) {
        var self = this;
        this.$axios({
          url: self.endpoint(),
          method: "POST",
          data: sheet,
          responseType: 'json',
          crossDomain: true,
          transformResponse: this.parseResponse
        })
        .then(function (response) {
          const costSheet = new CostSheet(response.data);
          const cs = { 
            project: costSheet.project,
            location: costSheet.location,
            dateCreated: costSheet.dateCreated.toISOString()
          }
          self.selectedSheet = costSheet;
          self.newSheetDialog = false;
          self.costSheetTableVisible = false;
          self.costSheetVisible = true;
          self.updateSheet++;
        })
        .catch(function (error) {
          console.log(error);
          self.showMessage("Server error when creating new cost sheet");
        });
      },
      openNewProjectDialog() {
        this.newSheet = new CostSheet();
        this.newSheetDialog = true;
      },
      newCostSheet() {
        if (!this.newSheet.project || this.newSheet.project.trim() === "") {
          this.showMessage("Please input the project name");
          return;
        }
        const materialMarkup = parseFloat(this.newSheet.materialMarkup);
        const finalMarkup = parseFloat(this.newSheet.finalMarkup);
        const costSheet = new CostSheet({
            id: 0,
            project: this.newSheet.project,
            location: this.newSheet.location,
            dateCreated: new Date(),
            materialMarkup: materialMarkup,
            labourDailyRate: parseFloat(this.newSheet.labourDailyRate),
            labourNightMultiplier: parseFloat(this.newSheet.labourNightMultiplier),
            finalMarkup: finalMarkup
        });
        costSheet.renumberSections();
        this.createCostSheet(costSheet);
      },
      async deleteCostSheet(index) {
        this.showConfirmMessage(`Are you sure you want to delete project: ${this.costSheets[index].project}`)
          .then( result => {
            this.$axios({
              url: this.endpoint(this.costSheets[index].id),
              method: "DELETE",
              crossDomain: true
            }).then(response => {
              console.log("Cost sheet", this.costSheets[index].id, "deleted");
              this.costSheets.splice(index, 1);
            }).catch(function (error) {
              console.log(error);
            });
          })
      },
      goBack() {
        this.getCostSheets();
        this.costSheetVisible = false;
        this.costSheetTableVisible = true;
      },
      showMessage(msg) {
        this.userMessage = msg;
        this.alert = true;
      },
      closeMessage() {
        this.userMessage = "";
        this.alert = false;
      },
      showConfirmMessage(msg) {
        this.userMessage = msg;
        this.confirm = true;
        return new Promise((resolve, reject) => {
          this.okAction = resolve;
          this.cancelAction = reject;
        });
      },
      confirmOk() {
        this.userMessage = "";
        this.confirm = false;
        if (this.okAction) {
          this.okAction("ok");
        }
      },
      confirmCancel() {
        this.userMessage = "";
        this.confirm = false;
        if (this.cancelAction) {
          this.cancelAction("cancel");
        }
      }
    }
  }
</script>