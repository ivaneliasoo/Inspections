<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <h1 class="display-2 font-weight-bold mb-3">Energy Report</h1>

        <v-card height="550" width="890" class="pa-md-4 mx-auto">
          <v-row>
            <v-col cols=4>
              <v-navigation-drawer permanent>
                <v-list-item class="text-left">
                  <v-list-item-content>
                    <v-list-item-title class="title">
                      Actions
                    </v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
                <v-list dense flat class="text-left">
                  <v-list-item @click="newReport">
                    <v-list-item-icon>
                      <v-icon>mdi-new-box</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>New report</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="loadReport">
                    <v-list-item-icon>
                      <v-icon>mdi-open-in-new</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Load report</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="openCover">
                    <v-list-item-icon>
                      <v-icon>mdi-pencil</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Edit Cover</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                  <v-list-item @click="openPage2">
                    <v-list-item-icon>
                      <v-icon>mdi-pencil</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Edit Page 2</v-list-item-title>
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
                  <v-list-item @click="saveTemplates">
                    <v-list-item-icon>
                      <v-icon>mdi-content-save</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title>Save Templates</v-list-item-title>
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
                </v-list>
              </v-navigation-drawer>
            </v-col>

            <v-col cols=8>
              <v-row justify="center">
                <v-data-table
                  dense
                  :headers="columnDefs"
                  :items="template.categories"
                  item-key="category"
                  class="elevation-1"
                  height="360"
                >
                  <template slot="item.rank" slot-scope="props">
                    {{ props.index + 1 }}
                  </template>
                  <template v-slot:top>
                    <v-toolbar flat>
                      <v-toolbar-title class="title">Sections</v-toolbar-title>
                      <v-divider class="mx-4" inset vertical></v-divider>
                      <v-spacer></v-spacer>
                      <v-btn small @click="newItem">New Section</v-btn>
                    </v-toolbar>
                  </template>
                  <template v-slot:item.actions="{ item }">
                    <v-icon small class="mx-1" @click="editItem(item)">
                      mdi-pencil
                    </v-icon>
                    <v-label>
                      {{ item.disabled ? "x" : "" }}
                    </v-label>
                  </template>
                </v-data-table>
              </v-row>
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="10">
            </v-col>
            <v-col cols="2">
              <p justify="end" style="font-size: small;">Version {{ version }}</p>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog v-model="dialog" max-width="980px">
      <v-card>
        <v-card-title>
          <span class="mx-2 headline">{{ formTitle }}</span>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="10">
              <v-text-field class="ml-2 mr-4 mt-4 pt-0"
                v-model="editedItem.title" label="Title"
              ></v-text-field>
            </v-col>
            <v-col cols="2">
              <v-checkbox class="py-0 mt-4"
                label="Disabled"
                v-model="editedItem.disabled"
              ></v-checkbox>
            </v-col>
          </v-row>
          <v-tabs class="mx-1 pt-0" v-model="tabs">
            <v-tab>
              Text
            </v-tab>
            <v-tab>
              Charts
            </v-tab>
            <v-tab-item>
              <v-container>
                <v-row class="pt-4">
                  <v-col cols=1>
                    <v-checkbox class="ml-3" v-model="editedItem.text.incParam"></v-checkbox>
                  </v-col>
                  <v-col cols=5>
                    <v-text-field flat
                      label="Parameter name" value=""
                      v-model="editedItem.text.paramName"
                    ></v-text-field>
                  </v-col>
                  <v-col cols=5>
                    <v-text-field flat
                      label="Value" value=""
                      v-model="editedItem.text.paramValue"
                    ></v-text-field>
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col cols=1>
                    <v-checkbox class="ml-3" v-model="editedItem.text.incParamDef"></v-checkbox>
                  </v-col>
                  <v-col cols=11>
                    <v-textarea dense flat label="Parameter Definition" rows=2
                      clearable clear-icon="mdi-close-circle"
                      v-model="editedItem.text.paramDef">
                    </v-textarea>
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col cols=1>
                    <v-checkbox class="ml-3" v-model="editedItem.text.incLim"></v-checkbox>
                  </v-col>
                  <v-col cols=11>
                    <v-textarea dense flat label="Limitations" rows=2
                      clearable clear-icon="mdi-close-circle"
                      v-model="editedItem.text.limitations">
                    </v-textarea>
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col class="my-0 py-0" cols=1>
                    <v-checkbox class="ml-3 my-0 py-0"
                      v-model="editedItem.text.includeRequirements"
                    ></v-checkbox>
                  </v-col>
                  <v-col class="my-0 py-0" cols=4>
                      <v-label>Include requierements table</v-label>
                  </v-col>
                </v-row>

                <v-simple-table class="my-0 py-0" dense fixed-header>
                  <template v-slot:default class="my-0 py-0">
                    <thead class="my-0 py-0">
                      <tr class="my-0 py-0">
                        <th class="text-left">
                        </th>
                        <th class="text-left" width="260">
                          Requirement
                        </th>
                        <th class="text-left" width="260">
                          <v-text-field class="my-0 py-0" v-model="editedItem.text.reqHeader1"></v-text-field>
                        </th>
                        <th class="text-left" width="260">
                          <v-text-field class="my-0 py-0" v-model="editedItem.text.reqHeader2"></v-text-field>
                        </th>
                        <th class="text-left" width="260">
                          <v-text-field class="my-0 py-0" v-model="editedItem.text.reqHeader3"></v-text-field>
                        </th>
                        <th class="my-0 py-0 text-left">
                          Result
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr dense v-for="item in editedItem.text.requirements"
                          :key="item.num">
                        <td width="60">
                          <v-checkbox v-model="item.include"></v-checkbox>
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.requirement"></v-text-field>
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.value1"></v-text-field>
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.value2"></v-text-field>
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.value3"></v-text-field>
                        </td>
                        <td width="80">
                          <v-text-field v-model="item.result"></v-text-field>
                        </td>
                      </tr>
                    </tbody>
                  </template>
                </v-simple-table>
              </v-container>
            </v-tab-item>

            <v-tab-item>
              <v-container>
                <v-row class="py-0 my-0">
                  <v-col cols=2>
                    <v-checkbox
                      label="Daily totals"
                      v-model="editedItem.dailyTotals"
                      hide-details
                    ></v-checkbox>
                  </v-col>
                  <v-col cols=4>
                    <v-select :items="csvColumns" dense v-model="editedItem.histogram"
                        class="mt-6"
                        label="Histogram column">
                    </v-select>
                  </v-col>
                  <v-col cols=4>
                    <v-select :items="colors" dense v-model="editedItem.histoColor"
                        class="mt-6"
                        label="Histogram color" item-text="name" item-value="hex"
                        v-on:change="updateHistoColor">
                    </v-select>
                  </v-col>
                  <v-col cols=2>
                    <v-btn class="mt-3" :color="editedItem.histoColor"></v-btn>
                  </v-col>
                </v-row>
                <v-row class="py-0 my-0">
                  <v-col cols=2>
                    <v-checkbox
                      label="Set limits"
                      v-model="editedItem.chartLimits.enable"
                      hide-details
                    ></v-checkbox>
                  </v-col>
                  <v-col cols=2>
                    <v-text-field type="number" v-model="editedItem.chartLimits.ymin"
                        :disabled="!editedItem.chartLimits.enable" label="Min value">>
                    </v-text-field>
                  </v-col>
                  <v-col cols=2>
                    <v-text-field type="number" v-model="editedItem.chartLimits.ymax"
                        :disabled="!editedItem.chartLimits.enable" label="Max value">>
                    </v-text-field>
                  </v-col>
                  <v-col cols=4>
                    <v-text-field label="Units"
                      v-model="editedItem.yAxisName">
                    </v-text-field>
                  </v-col>
                  <v-col cols=2>
                    <v-text-field type="number" label="Scale factor"
                      v-model="editedItem.factor">
                    </v-text-field>
                  </v-col>
                </v-row>
                <v-row class="py-0 my-0">
                  <v-col cols="2">
                    <v-checkbox class="py-0 my-0"
                      label="Separate charts"
                      v-model="editedItem.separateCharts"
                      hide-details
                    ></v-checkbox>
                  </v-col>
                  <v-col class="mt-1" cols="4">
                    <v-text-field dense label="Chart title"
                      v-model="editedItem.chartTitle">
                    </v-text-field>
                  </v-col>
                  <v-col class="mt-1" cols="4">
                    <v-text-field dense label="Histogram title"
                      v-model="editedItem.histogramTitle">
                    </v-text-field>
                  </v-col>
                  <v-col class="mt-1" cols="2">
                    <v-text-field type="number" dense label="Histogram bins"
                      v-model="editedItem.bins">
                    </v-text-field>
                  </v-col>
                </v-row>
                <v-row justify="start">
                  <v-col cols="10">
                    <v-simple-table dense fixed-header height="270">
                      <template dense v-slot:default>
                        <thead>
                          <tr>
                            <th class="text-left" width="43%">
                              Columns
                            </th>
                            <th class="text-left" width="35%">
                              Color
                            </th>
                            <th class="text-left">
                            </th>
                            <th class="text-left">
                            </th>
                          </tr>
                        </thead>
                        <tbody dense>
                          <tr dense v-for="(item, index) in editedItem.mappings" :key="item.series">
                            <td>
                              <v-select dense flat :items="csvColumns" v-model="item.col">
                              </v-select>
                            </td>
                            <td>
                              <v-select :items="colors" dense v-model="item.color"
                                  item-text="name" item-value="hex">
                              </v-select>
                            </td>
                            <td>
                              <v-btn :color="item.color"></v-btn>
                            </td>
                            <td>
                              <v-icon class="mr-2" @click="deleteChartColumn(index)">
                                mdi-delete
                              </v-icon>
                            </td>
                          </tr>
                        </tbody>
                      </template>
                    </v-simple-table>
                  </v-col>
                  <v-col cols="2">
                    <v-btn @click="addChartColumn">
                      Add Column
                    </v-btn>
                  </v-col>
                </v-row>
              </v-container>
            </v-tab-item>
          </v-tabs>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="close">
            Cancel
          </v-btn>
          <v-btn color="blue darken-1" text @click="save">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showCover" max-width="800px">
      <v-card>
        <v-card-title>
          <span class="headline">Cover</span>
        </v-card-title>
        <v-card-text>
          <v-form>
            <v-text-field
              v-model="editedCover.title" label="Title"
            ></v-text-field>
            <v-text-field
              v-model="editedCover.client" label="Client"
            ></v-text-field>
            <v-text-field
              v-model="editedCover.siteLocation" label="Site location"
            ></v-text-field>
            <v-text-field
              v-model="editedCover.instrumentUsed" label="Instrument used"
            ></v-text-field>
            <v-text-field
              v-model="editedCover.serialNumber" label="serialNumber"
            ></v-text-field>
            <v-text-field
              v-model="editedCover.reportDate" label="Report date"
            ></v-text-field>
            <v-text-field
              v-model="editedCover.reportAuthor" label="Report author"
            ></v-text-field>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn color="blue darken-1" text @click="clearCover">
            Clear
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeCover">
            Close
          </v-btn>
          <v-btn color="blue darken-1" text @click="saveCover">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showPage2" max-width="800px">
      <v-card>
        <v-card-title>
          <span class="headline">Page 2</span>
        </v-card-title>
        <v-card-text>
          <v-form>
            <v-checkbox
              label="Include second page"
              v-model="editedPage2.include"
              hide-details
            ></v-checkbox>
            <v-text-field
              v-model="editedPage2.title" label="Title"
            ></v-text-field>
            <v-text-field
              v-model="editedPage2.subtitle" label="Subtitle"
            ></v-text-field>
            <v-textarea
              label="Paragraph 1"
              rows=2
              clearable
              clear-icon="mdi-close-circle"
              v-model="editedPage2.p1"
            ></v-textarea>
            <v-textarea
              label="Paragraph 2"
              rows=2
              clearable
              clear-icon="mdi-close-circle"
              v-model="editedPage2.p2"
            ></v-textarea>
            <v-textarea
              label="Paragraph 3"
              rows=2
              clearable
              clear-icon="mdi-close-circle"
              v-model="editedPage2.p3"
            ></v-textarea>
            <v-textarea
              label="Paragraph 4"
              rows=2
              clearable
              clear-icon="mdi-close-circle"
              v-model="editedPage2.p4"
            ></v-textarea>
            <v-textarea
              label="Paragraph 5"
              rows=2
              clearable
              clear-icon="mdi-close-circle"
              v-model="editedPage2.p5"
            ></v-textarea>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn color="blue darken-1" text @click="clearPage2">
            Clear
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closePage2">
            Close
          </v-btn>
          <v-btn color="blue darken-1" text @click="savePage2">
            Save
          </v-btn>
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
            <v-list-group v-for="(calcColumn, i) in template.calcColumns" :key="i">
              <template v-slot:activator>
                <v-list-item-content>
                  <v-list-item-title v-text="calcColumn.name"></v-list-item-title>
                </v-list-item-content>
              </template>
              <template>
                <v-container>
                  <v-row>
                    <v-col cols=1>
                    </v-col>
                    <v-col cols=8>
                      <v-simple-table dense fixed-header>
                        <template v-slot:default>
                          <thead>
                            <tr>
                              <th class="text-left">
                                Param name
                              </th>
                              <th class="text-left">
                                CSV column
                              </th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr v-for="(param, j) in calcColumn.params" :key="j">
                              <td width="120">
                                {{ param.name }}
                              </td>
                              <td width="120">
                                <v-select :items="csvColumns" dense v-model="param.col">
                                </v-select>
                              </td>
                            </tr>
                          </tbody>
                        </template>
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
          <v-btn color="blue darken-1" text @click="closeCalcColumns">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showNewReport" max-width="800px" height="400px">
      <v-card class="mx-auto">

        <v-card-title>
          <span class="headline">New Report Template</span>
        </v-card-title>

        <v-card-text>
          <v-text-field
            v-model="newReportDescription" label="Description"
          ></v-text-field>
          <v-checkbox class="py-0 mt-4"
            label="Blank template"
            v-model="newBlankReport"
          ></v-checkbox>
          <v-select
            class="my-4"
            label="Copy selected template (optional)"
            dense flat
            :items="templatesAsList"
            v-model="selectedTemplate"
            :disabled="newBlankReport">
          </v-select>
          <v-file-input
            ref="fileInput"
            v-model="inputfile"
            accept="text/csv"
            label="CSV File"
            @change="createReport">
          </v-file-input>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeNewReport">
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
            dense flat
            :items="templatesAsList"
            v-model="selectedTemplate">
          </v-select>
          <v-file-input
            ref="fileInput"
            v-model="inputfile"
            accept="text/csv"
            label="CSV File"
            @change="readFile">
          </v-file-input>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeLoadReport">
            Close
          </v-btn>
        </v-card-actions>

      </v-card>
    </v-dialog>

    <v-dialog v-model="alert" max-width="500px">
      <v-card>
        <v-card-title class="text-body-1"></v-card-title>
        <v-card-text class="header3">{{ userMessage }}</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeMessage">OK</v-btn>
          <v-spacer></v-spacer>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-row justify="center">
      <div ref="line1"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line2"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line3"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line4"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line5"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line6"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line7"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line8"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line9"   style="width: 800px; height: 300px; display: none;"></div>
      <div ref="line10"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo1"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo2"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo3"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo4"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo5"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo6"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo7"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo8"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo9"  style="width: 800px; height: 300px; display: none;"></div>
      <div ref="histo10" style="width: 800px; height: 300px; display: none;"></div>
    </v-row>

  </v-container>
</template>

// TODO: Migrar A typesscript, pasar la configuracion de echart a un plugin
<script>

import Papa from "papaparse";

import pdfMake from "pdfmake/build/pdfmake";
import pdfFonts from "pdfmake/build/vfs_fonts";

import * as echarts from 'echarts/lib/echarts';
import 'echarts/lib/chart/bar';
import 'echarts/lib/chart/line';
import 'echarts/lib/component/tooltip';
import 'echarts/lib/component/title';
import 'echarts/lib/component/grid';
import 'echarts/lib/component/legend';
import { MarkLineComponent } from 'echarts/components';
import {lineChartOptions, sepLineChartOptions, histogramOptions, barChartOptions, minMax}
    from '../assets/js/charts.js';

import * as d3 from "d3";
//import 'blueimp-md5';

import colors from '../assets/js/entity.js'
import document from '../assets/js/document.js';
import { getColumns, getCalcColumns, checkTemplate }
    from '../assets/js/categories.js';

pdfMake.vfs = pdfFonts.pdfMake.vfs;
echarts.use([MarkLineComponent]);

//  replaced by spread operator {...object}
// function copyOf(obj) {
//   return JSON.parse(JSON.stringify(obj));
// }

const suffix = " ";

export default {
  name: "EnergyReport",
  data: function () {
    return {
      tabs: null,
      colors: colors(),
      version: "1.0",
      alert: false,
      userMessage: "",
      dialog: false,
      inputfile: null,
      newReportDescription: "",
      newBlankReport: false,
      selectedTemplate: null,
      description: "",
      columnDefs: [
        { text: 'Num', value: 'rank', width: "15%" },
        { text: "Title", value: "title", width: "62%" },
        { text: 'Actions', value: 'actions', width: "10%", sortable: false },
      ],
      dateColumn: 'Date',
      timeColumn: 'Time',
      csvData: [],
      csvColumns: [],
      templates: [],
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
          reportAuthor: ""
        },
        page2: {
          include: false,
          title: "",
          subtitle: "",
          p1: "",
          p2: "",
          p3: "",
          p4: "",
          p5: ""
        }
      },
      template: {},
      editedIndex: -1,
      defaultRequirement: {num: 0, requirement: "", value1: "", value2: "", value3: "", result: "", include: false},
      editedItem: {
        separateCharts: false, disabled: false, chartTitle: "",
        title: "", histogram: "", histoColor: "", chartLimits: {enable: false, ymin: 0, ymax: 0},
        factor: undefined, bins: undefined,
        mappings: [],
        text: {
          paramName: "", paramValue: "", paramDef: "", limitations: "", incParam: false, incParamDef: false, incLim: false,
          includeRequirements: false,
          reqHeader1: "", reqHeader2: "", reqHeader3: "",
          requirements: [
            {num: 0, requirement: "", value1: "", value2: "", value3: "", result: "", include: false},
            {num: 1, requirement: "", value1: "", value2: "", value3: "", result: "", include: false}
          ]
        }
      },
      defaultItem: {
        separateCharts: false, disabled: false, chartTitle: "",
        title: "", histogram: "", histoColor: "", chartLimits: {enable: false, ymin: 0, ymax: 0},
        factor: undefined, bins: undefined,
        mappings: [],
        text: {
          paramName: "", paramValue: "", paramDef: "", limitations: "", incParam: false, incParamDef: false, incLim: false,
          includeRequirements: false,
          reqHeader1: "", reqHeader2: "",  reqHeader3: "",
          requirements: [
            {num: 0, requirement: "", value1: "", value2: "", value3: "", result: "", include: false},
            {num: 1, requirement: "", value1: "", value2: "", value3: "", result: "", include: false}
          ]
        }
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
      reportFinished: false
    }
  },
  computed: {
    formTitle () {
      return this.editedIndex === -1 ? 'New Section' : 'Edit Section'
    },
    histogramColor() {
      console.log(this.editedItem.histoColor);
      return this.editedItem ? this.editedItem.histoColor : "";
    },
    templatesAsList() {
      return Object.keys(this.templates)
        .filter((key) => key !== "model")
        .map((key) => ({text: this.templates[key].description, value: key}))
    }
  },
  watch: {
    dialog (val) {
      val || this.close()
    },
    dialogDelete (val) {
      val || this.closeDelete()
    },
  },
  beforeCreate() {
    this.template = this.emptyTemplate;
  },
  created() {
    this.initialize()
  },
  methods: {
    getLastId() {
      var max = 0;

      const keys = Object.keys(this.templates);
      keys.forEach(function (id) {
        if (!isNaN(id) && id > max) {
          max = id;
        }
      })

      return (parseInt(max)+1).toString();
    },
    newReport() {
      this.inputfile = null;
      this.newReportDescription = "";
      this.selectedTemplate = "";
      this.showNewReport = true;
    },
    closeNewReport() {
      this.showNewReport = false;
    },
    loadReport() {
      this.readTemplates().then( result => {
        this.templates = result;
        this.inputfile = null;
        this.selectedTemplate = "";
        this.showLoadReport = true;
        this.$forceUpdate();
      })
    },
    closeLoadReport() {
      this.showLoadReport = false;
    },
    createReport(file) {
      console.log("createReport");
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
        console.log("Making a copy of existing template: ", this.selectedTemplate);
        this.templates[id] = {...his.templates[this.selectedTemplate]};
        this.templates[id].description = this.newReportDescription;
        this.templates[id].cover = {...this.emptyTemplate.cover};
        this.templates[id].page2 = {...this.emptyTemplate.page2};
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

        self.template = {...self.templates["model"]};
        self.template.description = self.newReportDescription;

        if (self.newBlankReport) {
          self.template.cover = {...self.emptyTemplate.cover};
          self.template.page2 = {...self.emptyTemplate.page2};
          self.template.categories = [];
        }

        self.templates[id] = self.template;
        const csvColumns = allcols.filter(entry => entry.trim() != '' && entry != "Date" && entry != "Time");
        const calcColumns = getCalcColumns(self.template);
        self.csvColumns = csvColumns.concat(calcColumns);
        self.csvData = [];
      }

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
    endpoint(op) {
      return `/energyreport/${op}`;
    },
    readTemplates() {
      const self = this;
      return new Promise(function(resolve) {
        this.$axios.$get(self.endpoint("category"))
          .then(response => {
            resolve(response);
          })
      })
    },
    initialize() {
      const self = this;
      this.$axios.$get(this.endpoint("category"))
        .then(response => {
          self.templates = response;
          for (var i=0; i<10; i++) {
            var chartRef = this.$refs["line"+(i+1)];
            this.lineCharts[i] = echarts.init(chartRef, undefined, {width: 800, height: 380});
            chartRef = this.$refs["histo"+(i+1)];
            this.histograms[i] = echarts.init(chartRef, undefined, {width: 800, height: 380});
          }
        })
      .then(
        this.$axios.$get(this.endpoint("background"))
          .then(response => {
            self.background = response;
          })
      )
    },
    saveTemplates() {
      if (Object.keys(this.template).length == 0) {
        this.showMessage("No data available. Please load a template and CSV file");
        return;
      }
      if (this.csvColumns.length == 0) {
        this.showMessage("No data available. Please create/load a template and CSV file");
        return;
      }
      const configHeaders = {
        "content-type": "text/plain",
        "Accept": "text/plain"
      };
      const str = JSON.stringify(this.templates);
      this.$axios({
        endpoint: this.endpoint("category"),
        method: "post",
        data: str,
        headers: configHeaders
      })
      .then(() => {
        this.showMessage("Templates saved")
        this.inputfile = null
      })
      .catch(error => {
          this.errorMessage = error.message;
          console.error("There was an error! ", error);
      });
    },
    column(data, colName) {
      var result = [];
      for (var i=0; i<data.length; i++) {
        result.push(data[i][colName]);
      }
      return result;
    },
    reportPeriod() {
      const startDate = this.csvData[0]["DateTime"];
      const endDate = this.csvData[this.csvData.length-1]["DateTime"];
      const sd = {
        day: startDate.getDate(),
        month: startDate.getMonth()+1,
        year: startDate.getFullYear(),
        hour: startDate.getHours(),
        min: startDate.getMinutes()
      }
      const ed = {
        day: endDate.getDate(),
        month: endDate.getMonth()+1,
        year: endDate.getFullYear(),
        hour: endDate.getHours(),
        min: endDate.getMinutes()
      }
      return `${sd.day}/${sd.month}/${sd.year} ${sd.hour}:${sd.min}hrs - ` +
             `${ed.day}/${ed.month}/${ed.year} ${ed.hour}:${ed.min}hrs`
    },
    createLineChart(index) {
      const category = this.template.categories[index];
      const chart = this.lineCharts[index];
      const csvData = this.csvData;
      const promise = new Promise(function (resolve) {
        if (category.separateCharts) {
          chart.setOption(sepLineChartOptions(csvData, category, suffix));
        } else {
          chart.setOption(lineChartOptions(csvData, category, suffix));
        }
        chart.resize();
        chart.on('finished', function() {
          var image = new Image();
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          });
          resolve(image.src);
        });
      });

      return promise;
    },
    createHistogram(index) {
      const category = this.template.categories[index];
      const numBins = category.bins ? category.bins : 80;
      const chart = this.histograms[index];
      // create histogram
      const colName = category.factor ?
        category.histogram + suffix :
        category.histogram;

      var data = this.column(this.csvData, colName);

      var limits = { min: 1000000000, max: -1000000000};
      minMax(this.csvData, colName, limits);

      var histoGen = d3.bin()
        .domain([limits.min, limits.max])
        .thresholds(numBins-1);
      var bins = histoGen(data)

      var binCount = [];
      var binValues = [];

      for (var i=0; i<bins.length; i++) {
        binCount.push(bins[i].length);
        const value = bins[i].length == 0 ? 0 : bins[i].reduce((a, b) => a + b, 0)/bins[i].length;
        binValues.push(""+value.toFixed(2));
      }

      const promise = new Promise(function (resolve) {
        const options = histogramOptions(category, binValues, binCount);
        chart.setOption(options);
        chart.resize();
        chart.on('finished', function() {
          var image = new Image();
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          });
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
      data.forEach( d => {
        const dt = d['DateTime'];
        if (dt) {
          const date = dt.toLocaleDateString("en-US", {month: "2-digit", day: "2-digit"});
          grp[date] = grp[date] ? grp[date] + d[col] : d[col];
        }
      });

      const groups = [];
      Object.keys(grp).forEach( key => groups.push({ day: key, value: grp[key] }));

      const promise = new Promise(function (resolve) {
        const options = barChartOptions(category, groups);
        chart.setOption(options);
        chart.resize();
        chart.on('finished', function() {
          var image = new Image();
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          });
         resolve(image.src);
          resolve(0);
        });
      });
      return promise;
    },
    doReport() {
      if (Object.keys(this.template).length == 0) {
        this.showMessage("No template loaded");
        return;
      }
      if (this.csvColumns.length == 0 || this.csvData.length == 0) {
        this.showMessage("No data available. Please load an approriate CSV file");
        return;
      }
      const self = this;
      this.reportFinished = false;
      this.genReport();
      setTimeout(function() {
        if (! self.reportFinished) {
          //console.log("Timeout, trying again");
          self.genReport();
        }
      }, 1200);
    },
    async genReport() {
      console.log("Generating PDF");

      const promises = [];
      for (var i=0; i<this.template.categories.length; i++) {
        promises.push(this.createLineChart(i));
        if (this.template.categories[i].dailyTotals) {
          promises.push(this.createBarChart(i));
        } else {
          promises.push(this.createHistogram(i));
        }
      }
      const charts = await Promise.all(promises);

      this.reportFinished = true;
      //console.log("All charts created");
      var doc = document(this.template.cover, this.template.page2, this.background, this.reportPeriod(),
          this.template.categories, charts, this.dataInterval());
      const pdf = pdfMake.createPdf(doc);
      pdf.download("energy-report.pdf");
      //pdf.open();
      console.log("PDF created");
    },
    dateTime(dateStr, timeStr) {
      if (!dateStr || !timeStr) {
        return null;
      }
      const da = dateStr.split(/[/.-]/);
      const ta = timeStr.split(':');
      if (parseInt(da[2])>31) {
        const temp = da[0];
        da[0] = da[2];
        da[2] = temp;
      }
      const dateTime = new Date(da[0],da[1]-1, da[2], ta[0], ta[1], ta[2]);
      //return dateTime.getTime();
      return dateTime;
    },
    headerCols(rawData) {
      const rows = rawData.data;
      var firstRow = 0;
      for (var i=0; i<rows.length; i++) {
        const row = rows[i];
        if (row.length > 5) {
          if (firstRow > 0) {
            rawData.data.splice(0, firstRow);
          }
          return rows[0];
        }
        firstRow++;
      }
      return []
    },
    clearAll() {
      this.template = self.emptyTemplate;
      this.csvData = [];
      this.csvColumns = [];
    },
    readFile: function(file) {
      if (!file) {
        return;
      }
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
        self.template = self.templates[self.selectedTemplate];

        const cols = getColumns(self.template);

        if (!checkTemplate(self.template, allcols, cols)) {
          self.showMessage("The template is not compatible with the CSV file");
          return;
        }

        text = Papa.unparse(rawData);

        const data = [];
        let index = 0;
        const csvData = Papa.parse(text, {
          header: true,
          dynamicTyping: true,
          step: function(parsedRow) {
            const dt = self.dateTime(parsedRow.data[self.dateColumn], parsedRow.data[self.timeColumn]);
            if (!dt) {
              return;
            }
            var row = { index: index++, DateTime: dt };
            for (var i=0; i<cols.length; i++) {
              const col = cols[i];
              var colName = col.name;
              if (colName == "Date" || colName == "Time") {
                continue;
              }
              var value = parseFloat(parsedRow.data[col.name]);
              if (col.factor) {
                colName += suffix;
                value *= col.factor;
              }
              // if (col.suffix) {
              //   colName += col.suffix;
              // }
              if (!row[colName]) {
                row[colName] = value;
              }
            }
            data.push(row);
          }
        });

        if (data.length == 0) {
          this.showMessage("No data rows found");
          return;
        }

        const csvColumns = csvData.meta.fields;
        self.csvColumns = csvColumns.filter(entry => entry.trim() != '' && entry != "Date" && entry != "Time");
        self.csvData = data;

        const calcColumns = self.template.calcColumns;
        for (var i=0; i<calcColumns.length; i++) {
          const column = calcColumns[i].name;
          const params = calcColumns[i].params;
          self[column](column, params);
          self.csvColumns.push(column);
        }
        self.showLoadReport = false;
      };
      reader.readAsText(file);
    },
    newItem () {
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage("No data available. Please load a template and CSV file");
        return;
      }

      this.tabs = "Text";
      this.editedIndex = -1;
      this.editedItem = JSON.parse(JSON.stringify(this.defaultItem));
      this.dialog = true;
    },
    editItem (item) {
      this.tabs = "Text";
      this.editedIndex = this.template.categories.indexOf(item)
      this.editedItem = JSON.parse(JSON.stringify(this.defaultItem));
      const itemCopy = JSON.parse(JSON.stringify(item));
      this.editedItem = Object.assign(this.editedItem, itemCopy);

      if (!this.editedItem.text.requirements) {
        this.editedItem.text.requirements = [];
      }
      for (var i=this.editedItem.text.requirements.length; i<2; i++) {
        const req = Object.assign({}, this.defaultRequirement);
        req.num = i;
        this.editedItem.text.requirements.push(req);
      }
      console.log("factor:", this.editedItem.factor);

      this.dialog = true;
    },
    addChartColumn () {
      const mappings = this.editedItem.mappings;

      var num = 1;
      if (mappings.length > 0) {
        const lastSeries = mappings[mappings.length-1].series;
        console.log("lastSeries: ", lastSeries);
        num = parseInt(lastSeries) + 1;
      }
      console.log("series name: ", num);
      this.editedItem.mappings.push({
        "series": num.toString(),
        "col": "",
        "color": ""
      });
    },
    deleteChartColumn(index) {
      this.editedItem.mappings.splice(index, 1);
    },
    close () {
      this.dialog = false
      this.$nextTick(() => {
        this.editedIndex = -1
      })
     },
    save () {
      if (this.editedIndex > -1) {
        console.log("factor:", this.editedItem.factor);
        console.log("factor?:", (this.editedItem.factor ? true : false));
        Object.assign(this.template.categories[this.editedIndex], this.editedItem);
      } else {
        this.template.categories.push(this.editedItem);
      }
      this.close();
    },
    openCover() {
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage("No data available. Please load a template and CSV file");
        return;
      }
      this.editedCover = Object.assign({}, this.template.cover)
      this.showCover = true;
    },
    saveCover() {
      this.template.cover = Object.assign({}, this.editedCover)
      this.showCover = false;
    },
    clearCover() {
      this.editedCover = {
        title: "",
        client: "",
        siteLocation: "",
        instrumentUsed: "",
        serialNumber: "",
        reportDate: "",
        reportAuthor: ""
      }
    },
    closeCover() {
      this.showCover = false;
    },
    openPage2() {
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage("No data available. Please load a template and CSV file");
        return;
      }
      this.editedPage2 = Object.assign({}, this.template.page2)
      this.showPage2 = true;
    },
    savePage2() {
      this.template.page2 = Object.assign({}, this.editedPage2)
      this.showPage2 = false;
    },
    clearPage2() {
      this.editedPage2 = {
        include: false,
        title: "",
        subtitle: "",
        p1: "",
        p2: "",
        p3: "",
        p4: "",
        p5: ""
      }
    },
    closePage2() {
      this.showPage2 = false;
    },
    openCalcColumns() {
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage("No data available. Please load a template and CSV file");
        return;
      }
      this.showCalcColumns = true;
    },
    closeCalcColumns() {
      this.showCalcColumns = false;
    },
    VoltageUnbalance(column, params) {
      for (var i=0; i<this.csvData.length; i++) {
        const voltage = Array.from(params, param => this.csvData[i][param.col]);
        const av = voltage.reduce((a, b) => a + b) / voltage.length;
        const mx = Math.max(...voltage);
        this.csvData[i][column] = ((mx - av)/av)*100;
      }
    },
    AcumEnergy(column, params) {
      var prevTime = this.csvData[0].DateTime.getTime();
      var acum = this.csvData[0][params[0].col];
      this.csvData[0][column] = 0;
      for (var i=1; i<this.csvData.length; i++) {
        if (!this.csvData[i].DateTime) {
          continue;
        }
        const time = this.csvData[i].DateTime.getTime();
        const elapsedTime = time - prevTime;
        const hours = elapsedTime/3600000;
        acum += this.csvData[i][params[0].col];
        const energy = (acum * hours)/1000;
        this.csvData[i][column] = energy;
        prevTime = time;
      }
    },
    Energy(column, params) {
      for (var i=0; i<this.csvData.length; i++) {
        if (!this.csvData[i].DateTime) {
          continue;
        }
        const elapsedTime = i == 0 ?
            (this.csvData[i+1].DateTime.getTime() - this.csvData[i].DateTime.getTime())/3600000 :
            (this.csvData[i].DateTime.getTime() - this.csvData[i-1].DateTime.getTime())/3600000;
        this.csvData[i][column] = (this.csvData[i][params[0].col] * elapsedTime)/1000;
      }
    },
    MinPowerFactor(column, params) {
      for (var i=0; i<this.csvData.length; i++) {
        const result = this.csvData[i][params[0].col] / this.csvData[i][params[1].col];
        this.csvData[i][column] = result;
      }
    },
    AvePowerFactor(column, params) {
      for (var i=0; i<this.csvData.length; i++) {
        const result = this.csvData[i][params[0].col] / this.csvData[i][params[1].col];
        this.csvData[i][column] = result;
      }
    },
    MaxPowerFactor(column, params) {
      for (var i=0; i<this.csvData.length; i++) {
        const result = this.csvData[i][params[0].col] / this.csvData[i][params[1].col];
        this.csvData[i][column] = result;
      }
    }
  }
}

</script>
