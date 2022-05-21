<template>
  <div id="costsheet-container">
    <v-container v-show="costSheetVisible" class="my-n8 py-n8 mx-4 px-0 fill-height">
      <v-row>
        <v-col class="mt-n4 pt-n4 mb-n3 pb-n3 text-left" cols="1">
          &nbsp;&nbsp;
          <v-btn icon small @click="goBack">
            <v-icon>
              mdi-arrow-left
            </v-icon>
          </v-btn>
        </v-col>
        <v-col class="mt-n4 pt-n4 mb-n3 pb-n3 text-left text-h6" cols="1">
          {{ fieldName }}
        </v-col>
        <v-col class="mt-n4 pt-n4 mb-n3 pb-n3 text-left" cols="4">
          <input
            class="text-h6 table-input"
            type="text"
            v-model="costSheet.project"
          />
        </v-col>
        <v-col class="mt-n4 pt-n4 mb-n3 pb-n3" cols="2">
          <div class="text-center text-h6">
              {{ title }}
          </div>
        </v-col>
        <v-col class="mt-n4 pt-n4 mb-n3 pb-n3 text-center" cols="3">
            <v-btn small @click="saveSheet">
              Save
            </v-btn>
            <v-btn v-if="!costSheet.isTemplate" small @click="openSelectTemplate">
              L.Template
            </v-btn>
            <v-btn small @click="createPDF">
              Gen.PDF
            </v-btn>
        </v-col>
      </v-row>
      <v-row>
        <v-col class="mt-0 pt-0 mb-n5 pb-n5 text-left" cols="1">
        </v-col>
        <v-col v-if="!costSheet.isTemplate" class="mt-0 pt-0 mb-n5 pb-n5 text-left text-body" cols="1">
          Location:
        </v-col>
        <v-col class="mt-0 pt-0 mb-n5 pb-n5 text-left text-body" cols="5">
          <input  v-if="!costSheet.isTemplate"
            class="table-input"
            type="text"
            v-model="costSheet.location"
          />
        </v-col>
        <v-col v-if="!costSheet.isTemplate && template" class="mt-0 pt-0 mb-n5 pb-n5 text-center text-body" cols="1">
          Template: 
        </v-col>
        <v-col v-if="!costSheet.isTemplate && template" class="mt-0 pt-0 mb-n5 pb-n5 text-center text-body" cols="4">
          <input size="15" type="text" readonly v-model="templateName">
        </v-col>
      </v-row>
      <v-row>
        <v-col class="my-0 py-0">
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
                        <th colspan="2" class="text-left text-caption title-2">
                          <v-menu right bottom fixed>
                            <template v-slot:activator="{ on, attrs }">
                              <v-btn icon x-small
                                v-bind="attrs"
                                v-on="on"
                              >
                                <v-icon>mdi-plus-box-outline</v-icon>
                              </v-btn>
                            </template>
                              <v-list dense class="menu">
                                <v-list-item>
                                  <v-list-item-title @click="addSection(0, 'above', -1)">
                                    Add section
                                  </v-list-item-title>
                                </v-list-item>
                                <v-list-item v-if="template!==null">
                                  <v-list-item-title @click="openSelectSection">
                                    Select from template
                                  </v-list-item-title>
                                </v-list-item>
                              </v-list>
                          </v-menu>
                        </th>
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
                  :template="template"
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
                    <td class="text-caption font-weight-bold title-7 cs-section" style="background-color: #d1eee1;">
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
                    <td class="text-caption font-weight-bold title-7 cs-section" style="background-color: #d1eee1;">
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
                    <td class="text-caption font-weight-bold title-7 cs-section" style="background-color: #d1eee1;">
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
                    <td class="text-caption font-weight-bold title-7 cs-section" style="background-color: #d1eee1;">
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

      <SelectSection v-if="template!==null"
        :selectSectionDialog="selectSectionDialog"
        :template="template"
        @select-section="selectSection"
        @close="closeSelectSection">
      </SelectSection>

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
import pdfMake from "pdfmake/build/pdfmake";
import pdfFonts from "pdfmake/build/vfs_fonts";

import pdfDocument from '../../composables/costsheets/pdf_util.js';
import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';

pdfMake.vfs = pdfFonts.pdfMake.vfs

  export default {
    props: {
      costSheet: Object,
      template: Object,
      triggerUpdate: Number
    },
    data: () => ({
      version: 'v0.101',
      costSheetVisible: true,
      selectSectionDialog: false,
      contextMenu: {
        visible: false,
        rowIndex: 0,
        xcoord: 0,
        ycoord: 0
      },
      updateSection: 0,
      updateSummary: 0,
      timer: null
    }),
    computed: {
      title() {
        return this.costSheet.isTemplate ? "Template" : "Costing Sheet"
      },
      fieldName() {
        return this.costSheet.isTemplate ? 'Name:' : 'Project:';
      },
      templateName() {
        return this.template ? this.template.project : "";
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
      showContextMenu(e, index) {
        e.preventDefault();
        this.contextMenu.xcoord = e.clientX
        this.contextMenu.ycoord = e.clientY
        this.contextMenu.rowIndex = index;
        this.contextMenu.visible = true;
      },
      createPDF () {
        var doc = pdfDocument(this.costSheet);
        const pdf = pdfMake.createPdf(doc);
        pdf.download(this.costSheet.project+".pdf");
      },
      openSelectTemplate() {
        this.$emit("open-select-template", "loadTemplate");
      },
      saveSheet() {
        this.$emit('save-sheet', this.costSheet);
      },
      goBack() {
        clearInterval(this.timer);
        this.timer = null;
        this.$emit('go-back');
      },
      openSelectSection() {
        this.selectSectionDialog = true;
      },
      selectSection(newSectionIndex) {
        this.closeSelectSection();
        this.addSection(0, 'above', newSectionIndex);
      },
      closeSelectSection() {
        this.selectSectionDialog = false;
      },
      addSection(index, position, newSectionIndex) {
        // console.log("addSection", position, newSectionIndex)
        let newSection;
        if (newSectionIndex === -1) {
          newSection = new Section({materialMarkup: this.costSheet.materialMarkup});
        } else {
          newSection = new Section(this.template.sections[newSectionIndex]);
        }
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