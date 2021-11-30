<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <h1 class="display-2 font-weight-bold mb-3">
          Energy Report
        </h1>

        <v-card height="550" width="890" class="pa-md-4 mx-auto">
          <v-row>
            <v-col cols="4">
              <v-navigation-drawer permanent>
                <v-list-item class="text-left">
                  <v-list-item-content>
                    <v-list-item-title class="title">
                      Actions
                    </v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
                <v-list dense flat class="text-left">
                  <v-list-item @click="openReportDialog">
                    <v-list-item-icon>
                      <v-icon>mdi-file-pdf-box</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Report</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="newTemplate">
                    <v-list-item-icon>
                      <v-icon>mdi-new-box</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>New template</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="loadTemplate">
                    <v-list-item-icon>
                      <v-icon>mdi-open-in-new</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Load template</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="openPage2">
                    <v-list-item-icon>
                      <v-icon>mdi-pencil</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Report Summary</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="openCalcColumns">
                    <v-list-item-icon>
                      <v-icon>mdi-pencil</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Calculated Columns</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="doReport">
                    <v-list-item-icon>
                      <v-icon>mdi-view-dashboard</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Generate Report</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="openCurrentTableDialog">
                    <v-list-item-icon>
                      <v-icon>mdi-view-dashboard</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Generate Current Table</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                </v-list>
              </v-navigation-drawer>
            </v-col>

            <v-col cols="8">
              <v-row justify="center">
                <v-col class="my-0 py-0">
                  <v-toolbar flat>
                    <div class="text-body-2">
                      <v-icon small @click="editTemplateName" v-show="template.description">
                        mdi-pencil
                      </v-icon>
                      {{template.description}}
                    </div>
                    <v-spacer />
                    <v-btn small @click="newItem">
                      New Section
                    </v-btn>
                  </v-toolbar>
                </v-col>
              </v-row>
              <v-row justify="center">
                <v-col class="my-n2 py-n2">
                  <v-simple-table class="simple-table" height="400px"
                      style="max-width: 100%; overflow-x: hidden; overflow-y: auto"
                      dense fixed-header>
                    <colgroup>
                      <col style="width:8%;">
                      <col style="width:76%;">
                      <col style="width:16%;">
                    </colgroup>
                    <thead class="text-caption">
                      <tr>
                        <th>
                          Num
                        </th>
                        <th>
                          Title
                        </th>
                        <th>
                          Actions
                        </th>
                      </tr>
                    </thead>
                    <tbody class="text-caption text-left">
                      <tr v-for="(category, index) in template.categories" :key="index"
                          draggable="true"
                          @dragover="dragoverHandler($event)"
                          @dragstart="dragstartHandler($event, index)"
                          @drop="dropHandler($event, index)"
                      >
                        <td style="border: 1px solid lightgray;">
                          {{index+1}}
                        </td>
                        <td style="border: 1px solid lightgray;">
                          {{category.title}}
                        </td>
                        <td class="mx-4 px-4" style="border: 1px solid lightgray;">
                          <v-icon small class="mx-1" @click="editItem(category)">
                            mdi-pencil
                          </v-icon>
                          <v-label>
                            {{ category.disabled ? "x" : "" }}
                          </v-label>
                        </td>
                      </tr>
                    </tbody>
                  </v-simple-table>
                </v-col>
              </v-row>
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="10"> </v-col>
            <v-col cols="2">
              <p justify="end" style="font-size: small;">
                Version {{ version }}
              </p>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog v-model="dialog" max-width="1000px">
      <v-card>
        <v-card-title>
          <span class="mx-2 headline">{{ formTitle }}</span>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="10">
              <v-text-field
                class="ml-2 mr-4 mt-4 pt-0"
                v-model="editedItem.title"
                label="Title"
              ></v-text-field>
            </v-col>
            <v-col cols="2">
              <v-checkbox
                class="py-0 mt-4"
                label="Disabled"
                v-model="editedItem.disabled"
              ></v-checkbox>
            </v-col>
          </v-row>
          <v-tabs class="mx-1 pt-0" v-model="tabs">
            <v-tab> Text </v-tab>
            <v-tab> Charts </v-tab>
            <v-tab-item>
              <v-container>
                <v-row class="pt-4">
                  <v-col cols="1">
                    <v-checkbox
                      class="ml-3"
                      v-model="editedItem.text.incParam"
                    ></v-checkbox>
                  </v-col>
                  <v-col cols="5">
                    <v-text-field
                      flat
                      label="Parameter name"
                      value=""
                      v-model="editedItem.text.paramName"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="5">
                    <v-text-field
                      flat
                      label="Value"
                      value=""
                      v-model="editedItem.text.paramValue"
                    ></v-text-field>
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col cols="1">
                    <v-checkbox
                      class="ml-3"
                      v-model="editedItem.text.incParamDef"
                    ></v-checkbox>
                  </v-col>
                  <v-col cols="11">
                    <v-textarea
                      dense
                      flat
                      label="Parameter Definition"
                      rows="2"
                      clearable
                      clear-icon="mdi-close-circle"
                      v-model="editedItem.text.paramDef"
                    >
                    </v-textarea>
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col cols="1">
                    <v-checkbox
                      class="ml-3"
                      v-model="editedItem.text.incLim"
                    ></v-checkbox>
                  </v-col>
                  <v-col cols="11">
                    <v-textarea
                      dense
                      flat
                      label="Limitations"
                      rows="2"
                      clearable
                      clear-icon="mdi-close-circle"
                      v-model="editedItem.text.limitations"
                    >
                    </v-textarea>
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col class="my-0 py-0" cols="1">
                    <v-checkbox
                      class="ml-3 my-0 py-0"
                      v-model="editedItem.text.includeRequirements"
                    ></v-checkbox>
                  </v-col>
                  <v-col class="my-0 py-0" cols="4">
                    <v-label>Include requierements table</v-label>
                  </v-col>
                </v-row>

                <v-simple-table
                  class="my-0 py-0 simple-table"
                  style="max-width: 100%; overflow-x: hidden;"
                  dense fixed-header>
                    <thead>
                      <tr class="my-0 py-0">
                        <th class="text-left"></th>
                        <th class="text-left" width="9%" style="border: 2px solid lightgray;">% Time</th>
                        <th class="text-left" width="12%" style="border: 2px solid lightgray;">Min value</th>
                        <th class="text-left" width="12%" style="border: 2px solid lightgray;">Max value</th>
                        <th class="text-left" width="22%" style="border: 2px solid lightgray;">
                          <v-text-field
                            flat solo hide-details
                            v-model="editedItem.text.reqHeader1"
                          ></v-text-field>
                        </th>
                        <th class="text-left" width="22%" style="border: 2px solid lightgray;">
                          <v-text-field
                            flat solo hide-details
                            v-model="editedItem.text.reqHeader2"
                          ></v-text-field>
                        </th>
                        <th class="text-left" width="22%" style="border: 2px solid lightgray;">
                          <v-text-field
                            flat solo hide-details
                            v-model="editedItem.text.reqHeader3"
                          ></v-text-field>
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr
                        v-for="item in editedItem.text.requirements"
                        :key="item.num"
                      >
                        <td style="border: 2px solid lightgray;">
                          <v-checkbox
                            flat solo hide-details
                            class="ma-0 pa-0"
                            v-model="item.include"></v-checkbox>
                        </td>
                        <td style="border: 2px solid lightgray;">
                          <v-text-field
                            flat solo hide-details
                            v-model="item.timePercent"
                          ></v-text-field>
                        </td>
                        <td style="border: 2px solid lightgray;">
                          <v-text-field
                            flat solo hide-details
                            v-model="item.ymin"></v-text-field>
                        </td>
                        <td style="border: 2px solid lightgray;">
                          <v-text-field
                            flat solo hide-details
                            v-model="item.ymax"></v-text-field>
                        </td>
                        <td style="border: 2px solid lightgray;">
                          <v-select
                            dense flat solo hide-details
                            class="ma-0 pa-0"
                            :items="csvColumns"
                            v-model="item.value1"
                          >
                          </v-select>
                        </td>
                        <td style="border: 2px solid lightgray;">
                          <v-select
                            dense flat solo hide-details
                            class="ma-0 pa-0"
                            :items="csvColumns"
                            v-model="item.value2"
                          >
                          </v-select>
                        </td>
                        <td style="border: 2px solid lightgray;">
                          <v-select
                            dense flat solo hide-details
                            class="ma-0 pa-0"
                            :items="csvColumns"
                            v-model="item.value3"
                          >
                          </v-select>
                        </td>
                      </tr>
                    </tbody>
                </v-simple-table>
              </v-container>
            </v-tab-item>

            <v-tab-item>
              <v-container>
                <v-row class="py-0 my-0">
                  <v-col cols="2">
                    <v-checkbox
                      label="Daily totals"
                      v-model="editedItem.dailyTotals"
                      hide-details
                    ></v-checkbox>
                  </v-col>
                  <v-col cols="4">
                    <v-select
                      :items="csvColumns"
                      dense
                      v-model="editedItem.histogram"
                      class="mt-6"
                      label="Histogram column"
                    >
                    </v-select>
                  </v-col>
                  <v-col cols="4">
                    <v-select
                      :items="colors"
                      dense
                      v-model="editedItem.histoColor"
                      class="mt-6"
                      label="Histogram color"
                      item-text="name"
                      item-value="hex"
                      v-on:change="updateHistoColor"
                    >
                    </v-select>
                  </v-col>
                  <v-col cols="2">
                    <v-btn class="mt-3" :color="editedItem.histoColor"></v-btn>
                  </v-col>
                </v-row>
                <v-row class="py-0 my-0">
                  <v-col cols="2">
                    <v-checkbox
                      label="Set limits"
                      v-model="editedItem.chartLimits.enable"
                      hide-details
                    ></v-checkbox>
                  </v-col>
                  <v-col cols="2">
                    <v-text-field
                      type="number"
                      v-model="editedItem.chartLimits.ymin"
                      :disabled="!editedItem.chartLimits.enable"
                      label="Min value"
                      >>
                    </v-text-field>
                  </v-col>
                  <v-col cols="2">
                    <v-text-field
                      type="number"
                      v-model="editedItem.chartLimits.ymax"
                      :disabled="!editedItem.chartLimits.enable"
                      label="Max value"
                      >>
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field label="Units" v-model="editedItem.yAxisName">
                    </v-text-field>
                  </v-col>
                  <v-col cols="2">
                    <v-text-field
                      type="number"
                      label="Scale factor"
                      v-model="editedItem.factor"
                    >
                    </v-text-field>
                  </v-col>
                </v-row>
                <v-row class="py-0 my-0">
                  <v-col cols="2">
                    <v-checkbox
                      class="py-0 my-0"
                      label="Separate charts"
                      v-model="editedItem.separateCharts"
                      hide-details
                    ></v-checkbox>
                  </v-col>
                  <v-col class="mt-1" cols="4">
                    <v-text-field
                      dense
                      label="Chart title"
                      v-model="editedItem.chartTitle"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col class="mt-1" cols="4">
                    <v-text-field
                      dense
                      label="Histogram title"
                      v-model="editedItem.histogramTitle"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col class="mt-1" cols="2">
                    <v-text-field
                      type="number"
                      dense
                      label="Histogram bins"
                      v-model="editedItem.bins"
                    >
                    </v-text-field>
                  </v-col>
                </v-row>
                <v-row justify="start">
                  <v-col cols="10">
                    <v-simple-table class="simple-table"
                      dense fixed-header height="270">
                      <thead>
                        <tr>
                          <th class="text-left" width="26%" style="border: 1px solid lightgray;">Parameter</th>
                          <th class="text-left" width="29%" style="border: 1px solid lightgray;">Column</th>
                          <th class="text-left" width="8%" style="border: 1px solid lightgray;">Peaks</th>
                          <th class="text-left" width="20%" style="border: 1px solid lightgray;">Color</th>
                          <th class="text-left" style="border: 1px solid lightgray;"></th>
                          <th class="text-left" style="border: 1px solid lightgray;"></th>
                        </tr>
                      </thead>
                      <tbody dense>
                        <tr
                          dense
                          v-for="(item, index) in editedItem.mappings"
                          :key="item.series"
                        >
                          <td style="border: 1px solid lightgray;">
                            <v-text-field
                              dense flat solo hide-details
                              v-model="item.param"
                            >
                          </v-text-field>
                          </td>
                          <td style="border: 1px solid lightgray;">
                            <v-select
                              dense flat solo hide-details
                              :items="csvColumns"
                              v-model="item.col"
                            >
                            </v-select>
                          </td>
                          <td style="border: 1px solid lightgray;">
                            <v-checkbox
                              dense flat solo hide-details
                              v-model="item.showPeaks"
                            ></v-checkbox>
                          </td>
                          <td style="border: 1px solid lightgray;">
                            <v-select
                              :items="colors"
                              dense flat solo hide-details
                              v-model="item.color"
                              item-text="name"
                              item-value="hex"
                            >
                            </v-select>
                          </td>
                          <td style="border: 1px solid lightgray;">
                            <v-btn small :color="item.color"></v-btn>
                          </td>
                          <td style="border: 1px solid lightgray;">
                            <v-icon
                              class="mr-2"
                              @click="deleteChartColumn(index)"
                            >
                              mdi-delete
                            </v-icon>
                          </td>
                        </tr>
                      </tbody>
                    </v-simple-table>
                  </v-col>
                  <v-col cols="2">
                    <v-btn small @click="addChartColumn"> Add Column </v-btn>
                  </v-col>
                </v-row>
              </v-container>
            </v-tab-item>
          </v-tabs>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="close"> Cancel </v-btn>
          <v-btn @click="save"> Save </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showPage2" max-width="800px">
      <v-card>
        <v-card-title>
          <span class="headline">Report Summary</span>
        </v-card-title>
        <v-card-text>
          <v-form>
            <v-text-field
              class="my-0 py-0"
              v-model="editedPage2.title"
              label="Title"
            ></v-text-field>
            <v-text-field
              class="my-0 py-0"
              v-model="editedPage2.subtitle"
              label="Subtitle"
            ></v-text-field>
            <v-simple-table class="my-0 py-0 simple-table"
              style="max-width: 100%; overflow-x: hidden;"
              dense fixed-header>
              <thead class="my-0 py-0">
                <tr class="my-0 py-0">
                  <th class="text-left" width="10%" style="border: 1px solid lightgray;">Num.</th>
                  <th class="text-left" width="45%" style="border: 1px solid lightgray;">Title</th>
                  <th class="text-left" width="45%" style="border: 1px solid lightgray;">Remarks</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="item in editedPage2.requirements"
                  :key="item.num"
                >
                  <td style="border: 1px solid lightgray;">
                    {{ item.num }}
                  </td>
                  <td style="border: 1px solid lightgray;">
                    {{ requirementTableTitle(item.num) }}
                  </td>
                  <td style="border: 1px solid lightgray;">
                    <input class="text-caption table-input" type="text" size="40"
                      v-model="item.remarks"
                    >
                  </td>
                </tr>
                <tr
                  v-for="item in editedPage2.additionalInfo"
                  :key="item.num"
                >
                  <td style="height=20px; border: 1px solid lightgray;">
                    {{ item.num }}
                  </td>
                  <td style="height=20px; border: 1px solid lightgray;">
                    {{ requirementTableTitle(item.num) }}
                  </td>
                  <td style="border: 1px solid lightgray;">
                    <input class="text-caption table-input" type="text" size="40"
                      v-model="item.remarks"
                    >
                  </td>
                </tr>
              </tbody>
            </v-simple-table>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn @click="clearPage2"> Clear </v-btn>
          <v-spacer></v-spacer>
          <v-btn @click="closePage2"> Close </v-btn>
          <v-btn @click="savePage2"> Save </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showCalcColumns" max-width="800px" height="400px">
      <v-card class="mx-auto">
        <v-card-title>
          <span class="headline">Calculated columns</span>
        </v-card-title>
        <v-card-text>
          <v-list>
            <v-list-group
              v-for="(calcColumn, i) in template.calcColumns"
              :key="i"
            >
              <template v-slot:activator>
                <v-list-item-content>
                  <v-list-item-title
                    v-text="calcColumn.name"
                  ></v-list-item-title>
                </v-list-item-content>
              </template>
              <template>
                <v-container>
                  <v-row>
                    <v-col cols="1"> </v-col>
                    <v-col cols="8">
                      <v-simple-table class="simple-table" dense fixed-header>
                        <thead>
                          <tr>
                            <th class="text-left">Param name</th>
                            <th class="text-left">CSV column</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr
                            v-for="(param, j) in calcColumn.params"
                            :key="j"
                          >
                            <td width="120" style="border: 1px solid lightgray;">
                              {{ param.name }}
                            </td>
                            <td width="120" style="border: 1px solid lightgray;">
                              <v-select
                                :items="csvColumns"
                                dense flat solo hide-details
                                v-model="param.col"
                              >
                              </v-select>
                            </td>
                          </tr>
                        </tbody>
                      </v-simple-table>
                    </v-col>
                  </v-row>
                </v-container>
              </template>
            </v-list-group>
          </v-list>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="closeCalcColumns">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showNewReport" max-width="800px" height="400px">
      <v-card class="mx-auto">
        <v-card-title>
          <span class="headline">New Template</span>
        </v-card-title>

        <v-card-text>
          <v-text-field
            v-model="newReportDescription"
            label="Description"
          ></v-text-field>
          <v-checkbox
            class="py-0 mt-4"
            label="Blank template"
            v-model="newBlankReport"
          ></v-checkbox>
          <v-select
            class="my-4"
            label="Copy selected template (optional)"
            dense
            flat
            :items="templatesAsList"
            v-model="selectedTemplate"
            :disabled="newBlankReport"
          >
          </v-select>
          <v-file-input
            ref="fileInput"
            v-model="inputfile"
            accept="text/csv"
            label="CSV File"
            @change="createReport"
          >
          </v-file-input>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="closeNewTemplate">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showLoadReport" max-width="800px" height="400px">
      <v-card class="mx-auto">
        <v-card-title>
          <span class="headline">Load Report Template</span>
        </v-card-title>

        <v-card-text>
          <v-select
            class="my-4"
            label="Select report template"
            dense
            flat
            :items="templatesAsList"
            v-model="selectedTemplate"
          >
          </v-select>
          <v-file-input
            ref="fileInput"
            v-model="inputfile"
            accept="text/csv"
            label="CSV File"
            @change="readFile"
          >
          </v-file-input>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="closeLoadReport">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="templateNameDialog" max-width="600px">
      <v-card>
        <v-card-title>
          Template name
        </v-card-title>
        <v-card-text>
          <v-text-field
            label="Template Name"
            v-model="description"
          ></v-text-field>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="saveTemplateName">
            Ok
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showNewReportDialog" max-width="600px">
      <v-card>
        <v-card-title>
          New Report
        </v-card-title>
        <v-card-text>
          <v-text-field
            label="Report name"
            v-model="newReportName"
          ></v-text-field>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showNewReportDialog=false">
            Cancel
          </v-btn>
          <v-btn @click="newReport">
            OK
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showDeleteReportDialog" max-width="500px">
      <v-card class="mx-auto">
        <v-card-title>
          <span class="text-body-1"></span>
          <v-spacer></v-spacer>
        </v-card-title>
        <v-card-text class="header3">Delete {{report.name}}?</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="deleteReport">Yes</v-btn>
          <v-btn @click="showDeleteReportDialog=false">No</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog class="report-dialog" v-model="showReportDialog" max-width="900px">
      <v-card>
        <v-card-title>
          <span class="headline">Reports</span>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="3">
              <v-list dense>
                <v-list-item-group
                  color="primary"
                  v-model="selectedReport"
                >
                  <v-list-item
                    v-for="(report, index) in templates.reports"
                    :key="index"
                    @click="selectReport(index)"
                  >
                    <v-list-item-content>
                      <v-list-item-title v-text="report.name"></v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                </v-list-item-group>
              </v-list>
            </v-col>
            <v-col cols="9">
              <v-form :disabled="reportTabsDisabled">
              <v-tabs class="mx-1 pt-0"
                  v-model="reportTabs">
                <v-tab> Report info </v-tab>
                <v-tab> Cover </v-tab>

                <v-tab-item>
                  <v-row>
                    <v-col class="mt-4 pt-4 mb-2 pb-2">
                      <v-text-field
                        v-model="report.name"
                        label="Description"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row >
                    <v-col class="my-0 py-0">
                      <v-select
                        label="Select report template"
                        dense
                        flat
                        :items="templatesAsList"
                        v-model="selectedTemplate"
                        @change="updateReportTemplate"
                      >
                      </v-select>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="mt-2 pt-2 mb-0 pb-0">
                      <h4>File name: &nbsp; {{report.fileName}}</h4>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="my-0 py-0">
                      <v-file-input
                        ref="fileInput"
                        v-model="inputfile"
                        accept="text/csv"
                        label="CSV File"
                        @change="readCSVFile"
                      >
                      </v-file-input>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="my-0 py-0">
                      <v-text-field
                        v-model="report.circuit"
                        label="Circuit"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="mt-3 pt-3 mb-3 pb-3">
                      <v-select
                        label="Chart Legends"
                        dense
                        flat
                        :items="chartLegendOptions"
                        v-model="report.chartLegendOption"
                      >
                      </v-select>
                    </v-col>
                  </v-row>
                  <v-row v-for="n in 3" :key="n">
                    <v-col class="my-1 py-1">
                      <p style="color:white;">.</p>
                    </v-col>
                  </v-row>
                </v-tab-item>

                <v-tab-item>
                  <v-container class="report-form">
                    <v-row>
                      <v-col class="mt-2 pt-2 mb-0 pb-0">
                        <v-text-field
                          v-model="report.cover.title"
                          label="Title"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="my-0 py-0">
                        <v-text-field
                          v-model="report.cover.client"
                          label="Client"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="my-0 py-0">
                        <v-text-field
                          v-model="report.cover.siteLocation"
                          label="Site location"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="my-0 py-0">
                        <v-text-field
                          v-model="report.cover.separation"
                          label="Separation"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="my-0 py-0">
                        <v-text-field
                          v-model="report.cover.instrumentUsed"
                          label="Instrument used"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="my-0 py-0">
                        <v-text-field
                          v-model="report.cover.serialNumber"
                          label="serialNumber"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="my-0 py-0">
                        <v-text-field
                          v-model="report.cover.reportDate"
                          label="Report date"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="mt-0 pt-0 mb-0 pb-0">
                        <v-text-field
                          v-model="report.cover.reportAuthor"
                          label="Report author"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-tab-item>
              </v-tabs>
              </v-form>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-btn @click="deleteReportDialog"> Delete report </v-btn>
          <v-spacer></v-spacer>
          <v-btn @click="newReportDialog"> New report </v-btn>
          <v-btn @click="saveReport"> Save </v-btn>
          <v-btn @click="cancelReportDialog"> Close </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="genReportDialog" max-width="320px" height="240px">
      <v-card class="mx-auto">
        <v-card-title>
          <span class="headline">Generating PDF</span>
          <v-spacer></v-spacer>
          <div id="myProgress" ref="myProgress">
            <div id="myBar" ref="myBar"></div>
          </div>
        </v-card-title>
      </v-card>
    </v-dialog>

    <v-dialog v-model="currentTableDialog.show" max-width="720px">
      <v-card>
        <v-card-title>
          <span class="mx-2 headline">Generate Current Table</span>
        </v-card-title>
        <v-card-text>
          <v-row class="my-0 py-n0">
            <v-col>
              <div id="flatpickr" class="mt-2 pt-1 mb-2 pb-2 rounded text-body-2"
                style="padding: 3px; box-shadow: 1px 3px lightgrey;">
                <flat-pickr
                    v-model="currentTableDialog.startDate"
                    :config="dateConfig"
                    class="calendar"
                    placeholder="Start date"
                    name="startDate"/>
              </div>
            </v-col>
          </v-row>
          <v-row class="my-0 py-n0">
            <v-col>
              <div id="flatpickr" class="mt-2 pt-1 mb-0 pb-0 rounded text-body-2"
                style="padding: 3px; box-shadow: 1px 3px lightgrey;">
                <flat-pickr
                    v-model="currentTableDialog.endDate"
                    :config="dateConfig"
                    class="calendar"
                    placeholder="End date"
                    name="endtDate"/>
              </div>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeCurrentTableDialog">
            Close
          </v-btn>
          <v-btn color="blue darken-1" text @click="genCurrentTable">
            Generate
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

    <v-dialog v-model="waitDialog" max-width="500px">
      <v-card>
        <v-card-title class="text-body-1"></v-card-title>
        <v-card-text class="header3">{{ waitMessage }}</v-card-text>
      </v-card>
    </v-dialog>

    <v-row justify="center">
      <div ref="line1" style="width: 800px; height: 300px; display: none;" />
      <div ref="line2" style="width: 800px; height: 300px; display: none;" />
      <div ref="line3" style="width: 800px; height: 300px; display: none;" />
      <div ref="line4" style="width: 800px; height: 300px; display: none;" />
      <div ref="line5" style="width: 800px; height: 300px; display: none;" />
      <div ref="line6" style="width: 800px; height: 300px; display: none;" />
      <div ref="line7" style="width: 800px; height: 300px; display: none;" />
      <div ref="line8" style="width: 800px; height: 300px; display: none;" />
      <div ref="line9" style="width: 800px; height: 300px; display: none;" />
      <div ref="line10" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo1" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo2" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo3" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo4" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo5" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo6" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo7" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo8" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo9" style="width: 800px; height: 300px; display: none;" />
      <div ref="histo10" style="width: 800px; height: 300px; display: none;" />
    </v-row>
  </v-container>
</template>

<style scoped>
.simple-table {
  border-collapse: collapse;
  border-style: hidden;
}

.simple-table th, td {
  border: 1px solid lightgray;
  padding: 4px 4px 4px 4px;
}

.v-input {
  font-size: 1.0em;
}

.report-dialog .v-tabs__content
{
  height: 80vh;
}

#myProgress {
  width: 100%;
  background-color: lightgrey;
}

#myBar {
  width: 1%;
  height: 30px;
  background-color: royalblue;
}
</style>

// TODO: Migrar A typesscript, pasar la configuracion de echart a un plugin
<script>
import Papa from "papaparse";

import pdfMake from "pdfmake/build/pdfmake";
import pdfFonts from "pdfmake/build/vfs_fonts";

import flatPickr from 'vue-flatpickr-component';
import 'flatpickr/dist/flatpickr.css';
import 'flatpickr/dist/themes/material_blue.css';

import * as echarts from 'echarts/lib/echarts'
import 'echarts/lib/chart/bar'
import 'echarts/lib/chart/line'
import 'echarts/lib/component/tooltip'
import 'echarts/lib/component/title'
import 'echarts/lib/component/grid'
import 'echarts/lib/component/legend'
import { MarkLineComponent } from 'echarts/components'
import * as d3 from 'd3'
import { lineChartOptions, sepLineChartOptions, histogramOptions, barChartOptions, minMax2 }
  from '../composables/charts.js'
import { adjustData, adjustDates } from '../composables/util.js'

import colors from '../composables/entity.js'
import document from '../composables/document.js'
import currentTable from '../composables/currentTable.js'
import { getColumns, getCalcColumns, checkTemplate }
  from '../composables/categories.js'

pdfMake.vfs = pdfFonts.pdfMake.vfs
echarts.use([MarkLineComponent])

//  replaced by spread operator {...object}
function copyOf(obj) {
  return JSON.parse(JSON.stringify(obj));
}

function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

const suffix = ' '

function newReport() {
  return {
    id: 0,
    name: "",
    template: null,
    chartLegendOption: "use-param-name",
    cover: {
      title: "",
      client: "",
      siteLocation: "",
      instrumentUsed: "",
      serialNumber: "",
      reportDate: "",
      reportAuthor: "",
    },
  }
}

export default {
  name: "EnergyReport",
  components: {
      flatPickr
  },
  data: function () {
    return {
      tabs: null,
      reportTabs: null,
      colors: colors(),
      version: "1.0",
      alert: false,
      choiceDialog: false,
      waitDialog: false,
      userMessage: "",
      waitMessage: "",
      dialog: false,
      showReportDialog: false,
      reportTabsDisabled: true,
      showNewReportDialog: false,
      showDeleteReportDialog: false,
      genReportDialog: false,
      reportProgress: 0,
      chartGenerator: null,
      inputfile: null,
      newReportDescription: "",
      newBlankReport: false,
      selectedTemplate: null,
      description: "",
      templateNameDialog: false,
      dateConfig: {
          altFormat: 'D, M j, Y',
          altInput: true,
          dateFormat: 'Y-m-d'
      },
      currentTableDialog: {
        show: false,
        startDate: "",
        endDate: ""
      },
      columnDefs: [
        { text: "Num", value: "rank", width: "15%" },
        { text: "Title", value: "title", width: "62%" },
        { text: "Actions", value: "actions", width: "10%", sortable: false },
      ],
      dateColumn: "Date",
      timeColumn: "Time",
      rowData: [],
      csvData: [],
      csvColumns: [],
      minMax: {},
      templates: {},
      energyData: {
        prevTime: 0
      },
      acumEnergyData: {
        prevTime: 0,
        acum: 0
      },
      chartLegendOptions: [
        "use-param-name",
        "use-column-name"
      ],
      newReportName: "",
      selectedReport: -1,
      report: newReport(),
      emptyTemplate: {
        description: "",
        bins: "",
        categories: [],
        calcColumns: [],
        cover: {
          title: "",
          client: "",
          siteLocation: "",
          instrumentUsed: "",
          serialNumber: "",
          reportDate: "",
          reportAuthor: "",
        },
        page2: {
          include: false,
          title: "",
          subtitle: "",
          requirements: [
            { num: 0, section: "", remarks: "" },
            { num: 1, section: "", remarks: "" }
          ],
          additionalInfo: [
            { num: 2, section: "", remarks: "" },
            { num: 3, section: "", remarks: "" },
            { num: 4, section: "", remarks: "" },
            { num: 5, section: "", remarks: "" },
            { num: 6, section: "", remarks: "" },
            { num: 7, section: "", remarks: "" },
            { num: 8, section: "", remarks: "" },
            { num: 9, section: "", remarks: "" }
          ],
        },
      },
      template: {},
      editedIndex: -1,
      defaultRequirement: {
        num: 0,
        requirement: "",
        value1: "",
        value2: "",
        value3: "",
        result: "",
        include: false,
      },
      editedItem: {
        separateCharts: false,
        disabled: false,
        chartTitle: "",
        title: "",
        histogram: "",
        histoColor: "",
        chartLimits: { enable: false, ymin: 0, ymax: 0 },
        factor: undefined,
        bins: undefined,
        mappings: [],
        text: {
          paramName: "",
          paramValue: "",
          paramDef: "",
          limitations: "",
          incParam: false,
          incParamDef: false,
          incLim: false,
          includeRequirements: false,
          reqHeader1: "",
          reqHeader2: "",
          reqHeader3: "",
          requirements: [
            {
              num: 0,
              requirement: "",
              timePercent: "",
              ymin: "",
              ymax: "",
              value1: "",
              value2: "",
              value3: "",
              include: false,
            },
            {
              num: 1,
              requirement: "",
              timePercent: "",
              ymin: "",
              ymax: "",
              value1: "",
              value2: "",
              value3: "",
              include: false,
            },
          ],
        },
      },
      defaultItem: {
        separateCharts: false,
        disabled: false,
        chartTitle: "",
        title: "",
        histogram: "",
        histoColor: "",
        chartLimits: { enable: false, ymin: 0, ymax: 0 },
        factor: undefined,
        bins: undefined,
        mappings: [],
        text: {
          paramName: "",
          paramValue: "",
          paramDef: "",
          limitations: "",
          incParam: false,
          incParamDef: false,
          incLim: false,
          includeRequirements: false,
          reqHeader1: "",
          reqHeader2: "",
          reqHeader3: "",
          requirements: [
            {
              num: 0,
              requirement: "",
              value1: "",
              value2: "",
              value3: "",
              result: "",
              include: false,
            },
            {
              num: 1,
              requirement: "",
              value1: "",
              value2: "",
              value3: "",
              result: "",
              include: false,
            },
          ],
        },
      },
      showCover: false,
      editedCover: {},
      showPage2: false,
      editedPage2: {},
      showCalcColumns: false,
      showNewReport: false,
      showLoadReport: false,
      lineCharts: new Array(10),
      histograms: new Array(10),
      background: "",
      reportFinished: false,
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "New Section" : "Edit Section";
    },
    histogramColor () {
      // TODO: do not log in production mode
      return this.editedItem ? this.editedItem.histoColor : ''
    },
    templatesAsList() {
      return Object.keys(this.templates)
        .filter((key) => key !== "model" && key !== "reports")
        .map((key) => ({ text: this.templates[key].description, value: key }));
    }
  },
  watch: {
    dialog(val) {
      val || this.close();
    },
    dialogDelete(val) {
      val || this.closeDelete();
    }
  },
  beforeCreate() {
    this.template = this.emptyTemplate;
  },
  created() {
    this.initialize();
  },
  methods: {
    getLastId() {
      var max = 0;

      const keys = Object.keys(this.templates);
      keys.forEach(function (id) {
        if (!isNaN(id) && id > max) {
          max = id;
        }
      });

      return (parseInt(max) + 1).toString();
    },
    newTemplate() {
      this.inputfile = null;
      this.newReportDescription = "";
      this.selectedTemplate = "";
      this.showNewReport = true;
    },
    closeNewTemplate() {
      this.showNewReport = false;
    },
    loadTemplate() {
      this.readTemplates().then((result) => {
        this.templates = result;
        this.inputfile = null;
        this.selectedTemplate = "";
        this.showLoadReport = true;
        this.$forceUpdate();
      });
    },
    closeLoadReport() {
      this.showLoadReport = false;
    },
    createReport (file) {
      // TODO: do not log in producion mode
      console.log('createReport')
      if (!file) {
        console.log("file is null or undefined");
        return;
      }
      if (!this.newReportDescription || this.newReportDescription === "") {
        this.showMessage("You must enter a report description");
        return;
      }

      // Copy existing template
      if (this.selectedTemplate) {
        var id = this.getLastId();
        console.log(
          "Making a copy of existing template: ",
          this.selectedTemplate
        );
        this.templates[id] = copyOf(this.templates[this.selectedTemplate]);
        this.templates[id].description = this.newReportDescription;
        this.templates[id].cover = copyOf(this.emptyTemplate.cover);
        this.templates[id].page2 = copyOf(this.emptyTemplate.page2);
        this.selectedTemplate = id;
        this.readFile(file);
        this.showNewReport = false;
        return;
      }

      // Create blank tempalte
      const reader = new FileReader();
      var self = this;
      reader.onload = function (e) {
        console.log("Loading csv file");

        var text = e.target.result;

        const rawData = Papa.parse(text);
        const allcols = self.headerCols(rawData);
        if (allcols.length == 0) {
          this.showMessage("File has no column definitions");
          return;
        }

        self.clearAll();
        var id = self.getLastId();

        self.template = copyOf(self.templates["model"]);
        self.template.description = self.newReportDescription;

        if (self.newBlankReport) {
          self.template.cover = copyOf(self.emptyTemplate.cover);
          self.template.page2 = copyOf(self.emptyTemplate.page2);
          self.template.categories = [];
        }

        self.templates[id] = self.template;
        const csvColumns = allcols.filter(
          (entry) => entry.trim() != "" && entry != "Date" && entry != "Time"
        );
        const calcColumns = getCalcColumns(self.template);
        self.csvColumns = csvColumns.concat(calcColumns);
        self.csvData = [];
      };

      reader.readAsText(file);
      this.showNewReport = false;
    },
    updateHistoColor() {
      this.$forceUpdate();
    },
    dataInterval() {
      const d1 = this.csvData[0].DateTime;
      const d2 = this.csvData[1].DateTime;
      const time = d2.getTime() - d1.getTime();
      return time / 60000;
    },
    showMessage(msg) {
      this.userMessage = msg;
      this.alert = true;
    },
    closeMessage() {
      this.userMessage = "";
      this.alert = false;
    },
    showWaitMessage(msg) {
      this.waitMessage = msg;
      this.waitDialog = true;
    },
    closeWaitMessage() {
      this.waitMessage = "";
      this.waitDialog = false;
    },
    endpoint (op) {
      //return `http://localhost:5000/api/energyreport/${op}`
      return `/energyreport/${op}`;
    },
    readTemplates() {
      const self = this;
      return new Promise(function (resolve) {
        self.$axios.$get(self.endpoint('category'))
          .then((response) => {
            resolve(response)
          })
      })
    },
    initialize () {
      this.getTemplates();
    },
    getTemplates() {
      const self = this;
      self.$axios.$get(this.endpoint('category'))
        .then((response) => {
          self.templates = response
          if (!self.templates.reports) {
            self.templates.reports = []
          }

          for (let i = 0; i < 10; i++) {
            let chartRef = this.$refs['line' + (i + 1)]
            this.lineCharts[i] = echarts.init(chartRef, undefined, { width: 800, height: 380 })
            chartRef = this.$refs['histo' + (i + 1)]
            this.histograms[i] = echarts.init(chartRef, undefined, { width: 800, height: 380 })
          }
        })
        .then(
          self.$axios.$get(this.endpoint('background'))
            .then((response) => {
              self.background = response
            })
        )
    },
    migrateTemplates() {
      const configHeaders = {
        'content-type': 'text/plain',
        Accept: 'text/plain'
      }
      this.$axios({
        url: this.endpoint('category/migrate'),
        method: 'post',
        data: "",
        headers: configHeaders
      })
        .then(() => {
          console.log("Templates migrated from file to database");
          this.getTemplates();
        })
        .catch((error) => {
          this.errorMessage = error.message
          // TODO: Do not log in production mode
          console.error('There was an error when migrating templates from file to database! ', error)
        })
    },
    saveTemplates (showMessage) {
      const configHeaders = {
        'content-type': 'text/plain',
        Accept: 'text/plain'
      }
      const str = JSON.stringify(this.templates)
      this.$axios({
        url: this.endpoint('category'),
        method: 'post',
        data: str,
        headers: configHeaders
      })
        .then(() => {
          if (showMessage) {
            this.showMessage('Templates saved')
            this.inputfile = null
          }
        })
        .catch((error) => {
          this.errorMessage = error.message
          // TODO: Do not log in production mode
          console.error('There was an error! ', error)
        })
    },
    column (data, colName) {
      const result = []
      for (let i = 0; i < data.length; i++) {
        result.push(data[i][colName])
      }
      return result
    },
    reportPeriod () {
      const startDate = this.csvData[0].DateTime
      const endDate = this.csvData[this.csvData.length - 1].DateTime
      const sd = {
        day: startDate.getDate(),
        month: startDate.getMonth() + 1,
        year: startDate.getFullYear(),
        hour: startDate.getHours(),
        min: startDate.getMinutes()
      }
      const ed = {
        day: endDate.getDate(),
        month: endDate.getMonth() + 1,
        year: endDate.getFullYear(),
        hour: endDate.getHours(),
        min: endDate.getMinutes()
      }
      return `${sd.day}/${sd.month}/${sd.year} ${sd.hour}:${sd.min}hrs - ` +
             `${ed.day}/${ed.month}/${ed.year} ${ed.hour}:${ed.min}hrs`
    },
    createLineChart (index) {
      const category = this.template.categories[index]
      const legendOpt = this.report.chartLegendOption
      const chart = this.lineCharts[index]
      const csvData = this.csvData
      const minMax = this.minMax

      if (category.separateCharts) {
        chart.setOption(sepLineChartOptions(csvData, minMax, category, suffix, legendOpt))
      } else {
        chart.setOption(lineChartOptions(csvData, minMax, category, suffix, legendOpt))
      }
      chart.resize()

      const promise = new Promise(function (resolve) {
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          })
          // console.log("resolving line chart", index)
          resolve(image.src)
        })
      })

      return promise
    },
    histoParamName(category) {
      const mappings = category.mappings;
      const idx = mappings.findIndex( column => column.col === category.histogram );
      return idx > -1 ? mappings[idx].param : mappings[idx].col;
    },
    createHistogram (index) {
      const category = this.template.categories[index]
      const numBins = category.bins ? category.bins : 80
      const chart = this.histograms[index]
      // create histogram
      const colName = category.factor
        ? category.histogram + suffix
        : category.histogram

      const data = this.column(this.csvData, colName)

      const colMinMax = this.minMax[colName]
      if (!colMinMax) {
        console.log("No minMax for", colName)
        return;
      }
      const limits = { min: colMinMax.min, max: colMinMax.max }

      const histoGen = d3.bin()
        .domain([limits.min, limits.max])
        .thresholds(numBins - 1)
      const bins = histoGen(data)

      var binCount = [];
      var binValues = [];
      // console.log("Section:", index)
      for (var i = 0; i < bins.length; i++) {
        binCount.push(bins[i].length);
        const value =
          limits.min + ((limits.max - limits.min) * i) / bins.length;
        binValues.push(value.toFixed(2));
      }

      let label = this.report.chartLegendOption === "use-param-name"
          ? this.histoParamName(category)
          : category.histogram;
      label = category.yAxisName ? `${label} [${category.yAxisName}]` : `${label}`
      const options = histogramOptions(category, binValues, binCount, label);
      chart.setOption(options);
      chart.resize();

      const promise = new Promise(function (resolve) {
        chart.on("finished", function () {
          var image = new Image();
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: "white",
          });
          // console.log("resolving histogram", index)
          resolve(image.src);
        });
      });
      return promise;
    },
    createBarChart(index) {
      const category = this.template.categories[index];
      const chart = this.histograms[index];
      var data = this.csvData;

      const col = category.histogram;

      const grp = {};
      const dtf = Intl.DateTimeFormat('en-SG', { month: 'numeric', day: 'numeric', year: 'numeric' });
      data.forEach((d) => {
        const dt = d["DateTime"];
        if (dt) {
          const date = dtf.format(dt);
          //console.log(date, d[col])
          grp[date] = grp[date] ? grp[date] + d[col] : d[col];
        }
      });

      const groups = [];
      Object.keys(grp).forEach((key) =>
        groups.push({ day: key, value: grp[key] })
      );

      const options = barChartOptions(category, groups)
      chart.setOption(options)
      chart.resize()

      const promise = new Promise(function (resolve) {
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          })
          // console.log("resolving bar chart", index)
          resolve(image.src)
        })
      })
      return promise
    },
    createCurrentTable(circuit) {
      const catIndex = this.template.categories.findIndex(cat => cat.chartTitle === "Current");
      if (catIndex === -1) {
        return;
      }
      const category = this.template.categories[catIndex];
      // ISO 8601: yyy-mm-dd
      const dtf = new Intl.DateTimeFormat('sv-SE', {year: 'numeric', month: '2-digit', day: '2-digit'});
      const dtf2 = Intl.DateTimeFormat('en-SG', { month: '2-digit', day: '2-digit', year: 'numeric' });
      var data = this.csvData;
      const params = {};
      for (const mapping of category.mappings) {
        params[mapping.param] = mapping.col;
      }
      const paramNames = Object.keys(params);

      const current = {};
      const currentTable = {id: 0, circuit: circuit, startDate: null, endDate: null, currentData: []};
      for (const row of data) {
        const dt = row["DateTime"];
        if (dt) {
          const date = dtf.format(dt);

          if (!current[date]) {
            current[date] = { date: dtf2.format(dt) };
          }

          for (const param of paramNames) {
            current[date][param] = current[date][param] ?
              Math.max(current[date][param], row[params[param]]) :
              row[params[param]];
          }
        }
      }

      const currentData = [];
      for (const date of Object.keys(current).sort()) {
        if (!currentTable.startDate || date < currentTable.startDate) {
          currentTable.startDate = date;
        }
        if (!currentTable.endDate || date > currentTable.endDate) {
          currentTable.endDate = date;
        }
        currentData.push(current[date]);
      }
      currentTable.currentData = JSON.stringify(currentData);

      return currentTable;
    },
    saveCurrentTable(currentTable) {
      //console.log("currentTable", JSON.stringify(currentTable));
      this.$axios({
        url: this.endpoint('current-table'),
        method: 'post',
        data: currentTable,
        // headers: configHeaders
      })
        .then(() => {
          console.log("current table saved")
        })
        .catch((error) => {
          this.errorMessage = error.message
          // TODO: Do not log in production mode
          console.error('There was an error! ', error)
        })
    },
    doReport () {
      if (!this.template) {
        this.showMessage("No template loaded");
        return;
      }
      if (Object.keys(this.template).length === 0) {
        this.showMessage("No template loaded");
        return;
      }
      if (this.csvColumns.length === 0 || this.csvData.length === 0) {
        this.showMessage('No data available. Please load an approriate CSV file')
        return
      }
      this.genReport();
      const currentTable = this.createCurrentTable(this.report.circuit);
      this.saveCurrentTable(currentTable);
    },
    updateProgress(value) {
      const progress = this.reportProgress + value;
      this. reportProgress = progress < 100 ? progress : 100;
      this.$refs.myBar.style.width = this.reportProgress + "%";
    },
    async genReport() {
      console.log("Generating PDF");

      this.genReportDialog = true;
      await new Promise(resolve => setTimeout(resolve, 100));
      this.reportProgress = 0;
      this.updateProgress(0);

      // Parse the data again to include new sections
      this.parseData(getColumns(this.template));
      this.reportProgress = 10;
      const promises = [];
      for (var i = 0; i < this.template.categories.length; i++) {
        const lineChart = this.createLineChart(i)
        promises.push(lineChart);
        this.updateProgress(5);
        await new Promise(resolve => setTimeout(resolve, 100));
        if (this.template.categories[i].dailyTotals) {
          const chart = this.createBarChart(i)
          promises.push(chart);
        } else {
          const chart = this.createHistogram(i)
          promises.push(chart);
        }
        this.updateProgress(5);
        await new Promise(resolve => setTimeout(resolve, 100));
      }

      const charts = await Promise.all(promises);

      this.genReportDialog = false;
      await new Promise(resolve => setTimeout(resolve, 100));
      this.reportProgress = 0;
      this.updateProgress(0);

      var doc = document(
        this.report,
        this.template.page2,
        this.background,
        this.reportPeriod(),
        this.template.categories,
        charts,
        this.dataInterval(),
        this.csvData,
        this.minMax,
        suffix
      );
      const pdf = pdfMake.createPdf(doc);
      pdf.download(this.report.cover.title+".pdf");
      //pdf.open();
      //console.log("PDF created");
    },
    genCurrentTable() {
      const self = this
      const path = `current-table/${this.currentTableDialog.startDate}/${this.currentTableDialog.endDate}`
      self.$axios.$get(this.endpoint(path))
        .then((response) => {
          //console.log("response", JSON.stringify(response));
          const tbl = response;

          var doc = currentTable(tbl);
          const pdf = pdfMake.createPdf(doc);
          pdf.download(`Current table, ${this.currentTableDialog.startDate} to ${this.currentTableDialog.endDate}.pdf`);

          self.closeCurrentTableDialog();
        })
    },
    dateTime(dateStr, timeStr) {
      // console.log(dateStr, timeStr)
      if (!dateStr || !timeStr) {
        return null;
      }
      const da = dateStr.split(/[/.-]/);
      if (parseInt(da[2]) > 31) {
        const temp = da[0];
        da[0] = da[2];
        da[2] = temp;
      }

      let ta = "";
      if (timeStr.includes("AM") || timeStr.includes("am") || timeStr.includes("PM") || timeStr.includes("pm")) {
        const timeArray = timeStr.split(" ");
        let ma = "";
        let ts = timeArray[0];
        if (timeArray.length == 1) {
          if (ts.includes("AM")) {
            ma = "AM";
            ts = timeArray[0].replace("AM", "");
          } else if (ts.includes("am")) {
            ma = "AM";
            ts = timeArray[0].replace("am", "");
          } else if (ts.includes("PM")) {
            ma = "PM";
            ts = timeArray[0].replace("PM", "");
          } else if (ts.includes("pm")) {
            ma = "PM";
            ts = timeArray[0].replace("pm", "");
          }
        } else {
          ts = timeArray[0];
          ma = timeArray[1].toUpperCase();
        }
        ta = ts.split(":");
        const hour = parseInt(ta[0]);
        if (ma === "AM" && hour === 12) {
          ta[0] = 0;
        }
        if (ma === "PM" && hour < 12) {
          ta[0] = parseInt(ta[0]) + 12;
        }
      } else {
        ta = timeStr.split(":");
      }

      const dateTime = new Date(da[0], da[1] - 1, da[2], ta[0], ta[1], ta[2]);
      //return dateTime.getTime();
      return dateTime;
    },
    headerCols(rawData) {
      const rows = rawData.data;
      var firstRow = 0;
      for (var i = 0; i < rows.length; i++) {
        const row = rows[i];
        const dateIndex = row.findIndex( v => v.toLowerCase() === "date" );
        const timeIndex = row.findIndex( v => v.toLowerCase() === "time" );
        const numCols = row.reduce( (accum, value) => accum += value.trim().length > 0 ? 1 : 0, 0 );
        if (dateIndex > -1 && timeIndex > -1 && numCols > 5) {
          if (firstRow > 0) {
            rawData.data.splice(0, firstRow);
          }
          return rows[0];
        }
        firstRow++;
      }
      return [];
    },
    clearAll() {
      this.template = self.emptyTemplate;
      this.csvData = [];
      this.csvColumns = [];
    },
    updateMinMax(value, colName, index, minMax) {
      let colMinMax = minMax[colName];
      if (!colMinMax) {
        colMinMax = { colName: colName, maxIndex: -1, min: Number.MAX_VALUE, max: -Number.MAX_VALUE };
        minMax[colName] = colMinMax;
      }
      if (!isNaN(value)) {
        if (value < colMinMax.min) {
          colMinMax.min = value;
        }
        if (value > colMinMax.max) {
          colMinMax.maxIndex = index;
          colMinMax.max = value;
        }
      }
    },
    parseData: function(cols) {
        const self = this;

        self.minMax = {};
        const calcColumns = self.template.calcColumns;

        const text = Papa.unparse(this.rawData);
        const data = [];
        let index = 0;
        const csvData = Papa.parse(text, {
          header: true,
          dynamicTyping: true,
          step: function (parsedRow) {
            const dt = self.dateTime(
              parsedRow.data[self.dateColumn],
              parsedRow.data[self.timeColumn]
            );
            if (!dt) {
              return;
            }
            var row = { index: index, DateTime: dt };
            for (let i = 0; i < cols.length; i++) {
              const col = cols[i];
              var colName = col.name;
              if (colName == "Date" || colName == "Time") {
                continue;
              }
              let value = parseFloat(parsedRow.data[col.name]);
              if (col.factor) {
                colName += suffix;
                value *= col.factor;
              }
              if (!row[colName]) {
                row[colName] = parseFloat(value);
                // update the min and max values for this column
                self.updateMinMax(value, colName, index, self.minMax)
              }
            }

            for (let i = 0; i < calcColumns.length; i++) {
              const column = calcColumns[i].name;
              const params = calcColumns[i].params;
              const value = self[column](column, row, index, params);
              if (!row[column]) {
                row[column] = parseFloat(value);
                // update the min and max values for this column
                self.updateMinMax(value, column, index, self.minMax)
              }
            }

            data.push(row);
            index++;
          },
        });

        if (data.length == 0) {
          this.showMessage("No data rows found");
          return;
        }

        const csvColumns = csvData.meta.fields;
        self.csvColumns = csvColumns.filter(
          (entry) => entry.trim() != "" && entry != "Date" && entry != "Time"
        );
        self.csvData = data;

        for (var i = 0; i < calcColumns.length; i++) {
          const column = calcColumns[i].name;
          self.csvColumns.push(column);
        }

        self.showLoadReport = false;
    },
    readFile: function (file) {
      if (!file) {
        return;
      }
      this.showWaitMessage("Loading CSV file. Please Wait");
      const reader = new FileReader();
      var self = this;
      reader.onload = function (e) {
        console.log("Loading csv file");

        var text = e.target.result;

        const rawData = Papa.parse(text);
        const allcols = self.headerCols(rawData);
        if (allcols.length == 0) {
          this.showMessage("File has no column definitions");
          return;
        }

        self.rawData = rawData;

        self.clearAll();
        self.template = self.templates[self.selectedTemplate];

        const cols = getColumns(self.template);

        const missingColumns = checkTemplate(self.template, allcols, cols);
        if (missingColumns.length) {
          let msg = "This CSV file is missing the following columns present in the template\n\n" +
              missingColumns.join(", ") +
              "\n\nYou should edit the template and select alternate columns or select a different file";
          self.showMessage(msg);
        }

        self.parseData(cols);
        self.closeWaitMessage();
      };
      reader.readAsText(file);
    },
    newItem() {
      if (
        this.csvColumns.length == 0 ||
        Object.keys(this.template).length == 0
      ) {
        this.showMessage(
          "No data available. Please load a template and CSV file"
        );
        return;
      }

      this.tabs = "Text";
      this.editedIndex = -1;
      this.editedItem = JSON.parse(JSON.stringify(this.defaultItem));
      this.dialog = true;
    },
    editItem(item) {
      this.tabs = "Text";
      this.editedIndex = this.template.categories.indexOf(item);
      this.editedItem = JSON.parse(JSON.stringify(this.defaultItem));
      const itemCopy = JSON.parse(JSON.stringify(item));
      this.editedItem = Object.assign(this.editedItem, itemCopy);

      if (!this.editedItem.text.requirements) {
        this.editedItem.text.requirements = [];
      }
      for (var i = this.editedItem.text.requirements.length; i < 2; i++) {
        const req = Object.assign({}, this.defaultRequirement);
        req.num = i;
        this.editedItem.text.requirements.push(req);
      }

      this.dialog = true;
    },
    addChartColumn() {
      const mappings = this.editedItem.mappings;

      var num = 1;
      if (mappings.length > 0) {
        const lastSeries = mappings[mappings.length - 1].series;
        num = parseInt(lastSeries) + 1;
      }
      this.editedItem.mappings.push({
        series: num.toString(),
        col: "",
        color: "",
      });
    },
    deleteChartColumn(index) {
      this.editedItem.mappings.splice(index, 1);
    },
    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
      });
    },
    save() {
      if (this.editedIndex > -1) {
        Object.assign(
          this.template.categories[this.editedIndex],
          this.editedItem
        );
      } else {
        this.template.categories.push(this.editedItem);
      }
      this.saveTemplates(false);
      this.close();
    },
    dragstartHandler(event, index) {
      const data = { section: index };
      event.dataTransfer.setData("application/json", JSON.stringify(data));
    },
    dragoverHandler(event) {
      event.preventDefault();
      event.dataTransfer.dropEffect = "move"
    },
    dropHandler(event, index) {
      event.preventDefault();
      const data = JSON.parse(event.dataTransfer.getData("application/json"));
      const target = this.template.categories[data.section];
      this.template.categories.splice(data.section, 1);
      this.template.categories.splice(index, 0, target);
      this.saveTemplates(false);
    },
    openCurrentTableDialog() {
      this.currentTableDialog.startDate = "";
      this.currentTableDialog.endDate = "";
      this.currentTableDialog.show = true;
    },
    closeCurrentTableDialog() {
      this.currentTableDialog.show = false;
    },
    openReportDialog() {
      this.reportTabs = "Report info";
      this.showReportDialog = true;
    },
    newReportDialog() {
      this.newReportName = "";
      this.showNewReportDialog = true;
    },
    deleteReportDialog() {
      if (this.selectedReport > -1) {
        this.showDeleteReportDialog = true;
      }
    },
    deleteReport() {
      if (this.selectedReport === -1) {
        return;
      }
      this.templates.reports.splice(this.selectedReport, 1);
      if (this.templates.reports.length > 0) {
        this.selectReport(0);
      }  else {
        this.selectedReport = -1;
        this.report = newReport();
        this.selectedTemplate = null;
        this.template = {};
        this.inputfile = null;
        this.reportTabsDisabled = true;
      }
      this.showDeleteReportDialog = false;
      this.reportTabs = "Report info";
    },
    updateReportTemplate() {
      this.report.template = this.selectedTemplate;
    },
    readCSVFile(file) {
      this.readFile(file);
      this.report.fileName = file.name;
    },
    nextReportId() {
      const reports = this.templates.reports
      if (reports.length === 0) {
        return 0
      }
      return reports[reports.length-1].id + 1
    },
    newReport() {
      const report = newReport();
      report.id = this.nextReportId();
      report.name = this.newReportName;

      this.templates.reports.push(report);
      this.report = report;
      this.selectedReport = this.templates.reports.length-1;
      this.selectedTemplate = null;
      this.template = {};
      this.inputfile = null;
      this.showNewReportDialog = false;
      this.reportTabs = "Report info";
      this.reportTabsDisabled = false;
    },
    selectReport(index) {
      if (this.selectedReport === index) {
        return;
      }
      this.selectedReport = index;
      this.report = this.templates.reports[index];
      if (this.report.template) {
        this.selectedTemplate = this.report.template;
        this.template = this.templates[this.report.template];
      } else {
        this.selectedTemplate = null;
        this.template = {};
      }
      this.inputfile = null;
      this.reportTabs = "Report info";
      this.reportTabsDisabled = false;
    },
    cancelReportDialog() {
      if (this.templates.reports.length > 0 && this.selectedReport > -1) {
        if (!this.selectedTemplate || !this.inputfile) {
          this.showMessage("To generate PDFs and edit templates, you must select a template and a CSV file");
        }
      }
      this.showReportDialog = false;
      this.$forceUpdate();
    },
    saveReport() {
      if (this.templates.reports.length > 0 && this.selectedReport > -1) {
        if (!this.selectedTemplate || !this.inputfile) {
          this.showMessage("To generate PDFs and edit templates, you must select a template and a CSV file");
        }
      }
      this.saveTemplates();
      this.$forceUpdate();
    },
    loadReport() {
      const reader = new FileReader();
      var self = this;
      reader.onload = function (e) {
        console.log("Loading report");
        let str = e.target.result;
        const report = JSON.stringify(str);
        this.selectedTemplate = report.template;
        this.report.cover = report.cover;
      }
      reader.readAsText(file);
    },
    editTemplateName() {
      this.description = this.template.description;
      this.templateNameDialog=true;
    },
    saveTemplateName() {
      if (this.description !== this.template.description) {
        console.log("saving template name");
        this.template.description = this.description;
        this.saveTemplates();
      }
      this.templateNameDialog=false;
    },
    showCoverForm() {
      this.coverVisible = true;
      this.reportVisible = false;
    },
    showReportForm() {
      this.coverVisible = false;
      this.reportVisible = true;
    },
    requirementTableTitle(num) {
      const cat = this.template.categories[num];
      return cat ? cat.title : ""
    },
    openPage2() {
      if (
        this.csvColumns.length == 0 ||
        Object.keys(this.template).length == 0
      ) {
        this.showMessage(
          "No data available. Please load a template and CSV file"
        );
        return;
      }
      if (this.template.page2) {
        this.editedPage2 = Object.assign({}, this.template.page2);
      } else {
        this.editedPage2 = Object.assign({}, this.emptyTemplate.page2);
      }
      this.showPage2 = true;
    },
    savePage2() {
      this.template.page2 = Object.assign({}, this.editedPage2);
      this.saveTemplates(false);
      this.showPage2 = false;
    },
    clearPage2() {
      this.editedPage2 = {
        include: false,
        title: "",
        subtitle: "",
      };
    },
    closePage2() {
      this.showPage2 = false;
    },
    openCalcColumns() {
      if (
        this.csvColumns.length == 0 ||
        Object.keys(this.template).length == 0
      ) {
        this.showMessage(
          "No data available. Please load a template and CSV file"
        );
        return;
      }
      this.showCalcColumns = true;
    },
    closeCalcColumns() {
      this.showCalcColumns = false;
      this.saveTemplates(false);
    },
    VoltageUnbalance(column, row, i, params) {
      const voltage = Array.from(
        params,
        (param) => row[param.col]
      );
      const av = voltage.reduce((a, b) => a + b) / voltage.length;
      const mx = Math.max(...voltage);
      const value = ((mx - av) / av) * 100;
      return value;
    },
    AcumEnergy(column, row, i, params) {
      if (i==0) {
        this.acumEnergyData.prevTime = row.DateTime.getTime();
        this.acumEnergyData.acum = 0;
        return 0;
      }
      if (!row.DateTime) {
        return;
      }

      const time = row.DateTime.getTime();
      const elapsedTime = time - this.acumEnergyData.prevTime;
      const hours = elapsedTime / 3600000;

      const energy = (row[params[0].col] * hours) / 1000;
      this.acumEnergyData.acum += energy;
      this.acumEnergyData.prevTime = time;

      return this.acumEnergyData.acum;
    },
    Energy(column, row, i, params) {
      if (i==0) {
        this.energyData.prevTime = row.DateTime.getTime();
        return 0;
      }
      if (!row.DateTime) {
        return;
      }
      const time = row.DateTime.getTime();
      const elapsedTime = (time - this.energyData.prevTime) / 3600000;
      const energy = (row[params[0].col] * elapsedTime) / 1000;
      this.energyData.prevTime = time;
      return energy;
    },
    MinPowerFactor(column, row, i, params) {
      const result =
        row[params[0].col] / row[params[1].col];
      return result;
    },
    AvePowerFactor(column, row, i, params) {
      const result =
        row[params[0].col] / row[params[1].col];
      return result;
    },
    MaxPowerFactor(column, row, i, params) {
      const result =
        row[params[0].col] / row[params[1].col];
      return result;
    },
    MinApparentPower(column, row, i, params) {
        const result =
          (row[params[0].col] / row[params[1].col]) / 1000;
        return result;
    },
    AveApparentPower(column, row, i, params) {
        const result =
          (row[params[0].col] / row[params[1].col]) / 1000;
        return result;
    },
    MaxApparentPower(column, row, i, params) {
        const result =
          (row[params[0].col] / row[params[1].col]) / 1000;
        return result;
    },
    adjustCsvDates() {
      const startDate = new Date(2021, 7, 30, 13, 0, 0)
      const eData = adjustDates(this.csvData, startDate, this.template.calcColumns)
      const txt = Papa.unparse(eData)
      var blob = new Blob([txt],
          { type: "text/plain;charset=utf-8" });
      saveAs(blob, "date-adjusted-energy-data.csv");
    },
    expandCsvData() {
      const eData = adjustData(this.csvData, 376, this.template.calcColumns)
      const txt = Papa.unparse(eData)
      var blob = new Blob([txt],
          { type: "text/plain;charset=utf-8" });
      saveAs(blob, "date-adjusted-energy-data.csv");
    }
  },
};
</script>
