<template>
  <div>
    <v-container fluid class="my-n2 py-n2 mx-4 px-0 fill-height">
      <div v-if="costSheetTable">
        <CostSheetTable
          @show-cost-sheets="showCostSheets"
          @show-templates="showTemplates"
          @edit-sheet="editCostSheet"
          @new-sheet="newCostSheet"
          @open-select-template="openSelectTemplate"
          @new-from-template="openSelectTemplate"
          @delete-sheet="deleteCostSheet"
          :costSheets="costSheets">
        </CostSheetTable>
      </div>
      <div v-if="templateTable">
        <TemplateTable
          @show-cost-sheets="showCostSheets"
          @show-templates="showTemplates"
          @open-template="openTemplate"
          @new-template="newTemplate"
          @delete-template="deleteTemplate"
          :templates="templates">
        </TemplateTable>
      </div>
      <div v-if="costSheetVisible">
        <CostSheet
          ref="costSheet"
          @go-back="goBack"
          @save-sheet="saveSheet"
          @open-select-template="openSelectTemplate"
          :costSheet="selectedSheet"
          :template="selectedTemplate"
          :triggerUpdate="updateSheet">
        </CostSheet>
      </div>
    </v-container>

    <SelectTemplate
      :selectTemplateDialog="selectTemplateDialog"
      :templates="templates"
      :action="selectTemplateAction"
      @select-template="openTemplate"
      @close="closeSelectTemplate">
    </SelectTemplate>

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
    name: 'CostSheetMain',
    data: () => ({
      version: 'v1.101',
      costSheetVisible: false,
      costSheetTable: true,
      templateTable: false,
      selectTemplateDialog: false,
      selectTemplateAction: 'loadTemplate',
      currentPanel: "costSheetTable",
      costSheets: [],
      templates: [],
      deleteIndex: null,
      selectedSheet: new CostSheet(),
      selectedTemplate: null,
      newSheetDefaults: {
        id: 0,
        project: "",
        location: "",
        dateCreated: new Date(),
        materialMarkup: 10,
        labourDailyRate: 600,
        labourNightMultiplier: 2,
        finalMarkup: 10
      },
      updateSheet: 0,
      rules: {
        required: value => !!value || 'Required.',
        number: value => {
          const pattern = /^\d*\.?\d*$/
          return pattern.test(value) || 'Invalid number'
        }
      },
      alert: false,
      confirm: false,
      userMessage: "",
      confirmAction: null
    }),
    mounted() {
      this.init();
    },
    updated() {
    },
    methods: {
      init() {
        console.log("init")
        //this.costSheets = inMemorySheets;
        this.getCostSheets(true);
        this.getTemplates(false);
      },
      endpoint(resource, p) {
        const param = p ? "/" + p : ""
        //return `http://localhost:5000/api/costsheet${param}`
        return `/costsheet/${resource}${param}`;
      },
      showCostSheets() {
        this.templateTable = false;
        this.costSheetTable = true;
      },
      showTemplates() {
        this.templateTable = true;
        this.costSheetTable = false;
      },
      openSelectTemplate(action) {
        this.selectTemplateAction = action;
        this.selectTemplateDialog = true;
      },
      closeSelectTemplate() {
        this.selectTemplateDialog = false;
      },
      getCostSheets(show) {
        const self=this;
        this.$axios({
          url: this.endpoint("sheet"),
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
          this.costSheets = response.data;
          this.costSheets.sort((s1, s2) => s1.id - s2.id);
          if (show) {
            this.showCostSheets();
          }
        }).catch(function (error) {
          console.log(error);
        });
      },
      getTemplates(show) {
        //const self=this;
        this.$axios({
          url: this.endpoint("template"),
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
          this.templates = response.data;
          this.templates.sort((t1, t2) => t1.id - t2.id);
          if (show) {
            this.showTemplates();
          }
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
      showCostSheet() {
        this.selectedTemplate = null;
        this.costSheetTable = false;
        this.templateTable = false;
        this.costSheetVisible = true;
        this.currentPanel = "costSheetTable";
        this.updateSheet++;
      },
      editCostSheet(index) {
        this.$axios({
          url: this.endpoint("sheet", this.costSheets[index].id),
          method: "GET",
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse
        }).then(response => {
          this.selectedSheet = new CostSheet(response.data);
          this.showCostSheet();
        }).catch(function (error) {
          console.log(error);
        });
      },
      saveCostSheet(sheet) {
        console.log("Before");
        sheet.sections.forEach( (s) => console.log( s.secNumber, ", ", s.description ));
        var self = this;
        this.$axios({
          url: self.endpoint("sheet"),
          method: "PUT",
          data: sheet,
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse
        })
        .then(function (response) {
          console.log("Cost sheet updated");
          response.data.sections.forEach( (s) => console.log( s.secNumber, ", ", s.description ));
        })
        .catch(function (error) {
          console.log(error);
          self.showMessage("There was a server error when saving the cost sheet");
        });
      },
      saveSheet(sheet) {
        if (sheet.isTemplate) {
          this.saveTemplate(sheet);
        } else {
          this.saveCostSheet(sheet);
        }
      },
      createCostSheet(sheet) {
        var self = this;
        this.$axios({
          url: self.endpoint("sheet"),
          method: "POST",
          data: sheet,
          responseType: 'json',
          crossDomain: true,
          transformResponse: this.parseResponse
        })
        .then(function (response) {
          self.selectedSheet = new CostSheet(response.data);
          self.showCostSheet();
        })
        .catch(function (error) {
          console.log(error);
          self.showMessage("Server error when creating new cost sheet");
        });
      },
      newCostSheet() {
        const costSheet = new CostSheet(this.newSheetDefaults);
        costSheet.renumberSections();
        this.createCostSheet(costSheet);
      },
      deleteCostSheet(index) {
        this.showConfirmMessage(`Are you sure you want to delete project: ${this.costSheets[index].project}`,
          (result) => {
            if (result === "ok") {
              this.$axios({
                url: this.endpoint("sheet", this.costSheets[index].id),
                method: "DELETE",
                crossDomain: true
              }).then(response => {
                console.log("Cost sheet", this.costSheets[index].id, "deleted");
                this.costSheets.splice(index, 1);
              }).catch(function (error) {
                console.log(error);
              });
            }
          }
        )
      },
      goBack() {
        this.costSheetVisible = false;
        if (this.currentPanel === "costSheetTable") {
          console.log("currentPanel", this.currentPanel);
          this.getCostSheets(true);
        } else {
          this.getTemplates(true);
        }
      },
      createTemplate(template) {
        var self = this;
        this.$axios({
          url: self.endpoint("template"),
          method: "POST",
          data: template,
          responseType: 'json',
          crossDomain: true,
          transformResponse: this.parseResponse
        })
        .then(function (response) {
          self.showTemplate(response.data);
        })
        .catch(function (error) {
          console.log(error);
          self.showMessage("Server error when creating new cost sheet");
        });
      },
      newTemplate() {
        const template = new CostSheet(this.newSheetDefaults);
        template.isTemplate = true;
        template.renumberSections();
        this.createTemplate(template);
      },
      showTemplate(template) {
        this.selectedSheet = new CostSheet(template);
        this.selectedTemplate = null;
        this.templateTable = false;
        this.costSheetVisible = true;
        this.currentPanel = "templateTable";
        this.updateSheet++;
      },
      loadTemplateAsSheet(template) {
        // console.log("loadTemplateAsSheet", template.id)
        this.closeSelectTemplate();

        const costSheet = new CostSheet(template);

        costSheet.id = 0;
        costSheet.project = "";
        costSheet.location = "";
        costSheet.isTemplate = false;

        this.createCostSheet(costSheet);
      },
      loadTemplate(template) {
        this.selectedTemplate = new CostSheet(template);
        this.closeSelectTemplate();
      },
      saveTemplate(template) {
        var self = this;
        this.$axios({
          url: self.endpoint("template"),
          method: "PUT",
          data: template,
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse
        })
        .then(function (response) {
          console.log("Template updated");
        })
        .catch(function (error) {
          console.log(error);
          self.showMessage("There was a server error when saving the template");
        });
      },
      openTemplate(index, action) {
        console.log("openTemplate", index, action)
        this.$axios({
          url: this.endpoint("template", this.templates[index].id),
          method: "GET",
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse
        }).then(response => {
          if (action==="editTemplate") {
            console.log("openTemplate", action);
            this.showTemplate(response.data);
          } else if (action==="loadTemplate") {
            this.loadTemplate(response.data);
          } else {
            this.loadTemplateAsSheet(response.data);
          }
        }).catch(function (error) {
          console.log(error);
        });
      },
      deleteTemplate(index) {
        this.showConfirmMessage(`Are you sure you want to delete template: ${this.templates[index].project}`,
          (result) => {
            if (result === "ok") {
              this.$axios({
                url: this.endpoint("template", this.templates[index].id),
                method: "DELETE",
                crossDomain: true
              }).then(response => {
                console.log("Template", this.templates[index].id, "deleted");
                this.templates.splice(index, 1);
              }).catch(function (error) {
                console.log(error);
              })
            }
          }
        )
      },
      showMessage(msg) {
        this.userMessage = msg;
        this.alert = true;
      },
      closeMessage() {
        this.userMessage = "";
        this.alert = false;
      },
      showConfirmMessage(msg, action) {
        this.userMessage = msg;
        this.confirm = true;
        this.confirmAction = action;
      },
      confirmOk() {
        this.userMessage = "";
        this.confirm = false;
        if (this.confirmAction) {
          this.confirmAction("ok");
        }
      },
      confirmCancel() {
        this.userMessage = "";
        this.confirm = false;
        if (this.confirmAction) {
          this.confirmAction("cancel");
        }
      }
    }
  }
</script>