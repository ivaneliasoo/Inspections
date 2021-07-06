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

            <v-col cols="8">
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
                  <template #top>
                    <v-toolbar flat>
                      <v-toolbar-title class="title">
                        Sections
                      </v-toolbar-title>
                      <v-divider class="mx-4" inset vertical />
                      <v-spacer />
                      <v-btn small @click="newItem">
                        New Section
                      </v-btn>
                    </v-toolbar>
                  </template>
                  <template #item.actions="{ item }">
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
            <v-col cols="10" />
            <v-col cols="2">
              <p justify="end" style="font-size: small;">
                Version {{ version }}
              </p>
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
              <v-text-field
                v-model="editedItem.title"
                class="ml-2 mr-4 mt-4 pt-0"
                label="Title"
              />
            </v-col>
            <v-col cols="2">
              <v-checkbox
                v-model="editedItem.disabled"
                class="py-0 mt-4"
                label="Disabled"
              />
            </v-col>
          </v-row>
          <v-tabs v-model="tabs" class="mx-1 pt-0">
            <v-tab>
              Text
            </v-tab>
            <v-tab>
              Charts
            </v-tab>
            <v-tab-item>
              <v-container>
                <v-row class="pt-4">
                  <v-col cols="1">
                    <v-checkbox v-model="editedItem.text.incParam" class="ml-3" />
                  </v-col>
                  <v-col cols="5">
                    <v-text-field
                      v-model="editedItem.text.paramName"
                      flat
                      label="Parameter name"
                      value=""
                    />
                  </v-col>
                  <v-col cols="5">
                    <v-text-field
                      v-model="editedItem.text.paramValue"
                      flat
                      label="Value"
                      value=""
                    />
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col cols="1">
                    <v-checkbox v-model="editedItem.text.incParamDef" class="ml-3" />
                  </v-col>
                  <v-col cols="11">
                    <v-textarea
                      v-model="editedItem.text.paramDef"
                      dense
                      flat
                      label="Parameter Definition"
                      rows="2"
                      clearable
                      clear-icon="mdi-close-circle"
                    />
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col cols="1">
                    <v-checkbox v-model="editedItem.text.incLim" class="ml-3" />
                  </v-col>
                  <v-col cols="11">
                    <v-textarea
                      v-model="editedItem.text.limitations"
                      dense
                      flat
                      label="Limitations"
                      rows="2"
                      clearable
                      clear-icon="mdi-close-circle"
                    />
                  </v-col>
                </v-row>

                <v-row class="my-0 py-0">
                  <v-col class="my-0 py-0" cols="1">
                    <v-checkbox
                      v-model="editedItem.text.includeRequirements"
                      class="ml-3 my-0 py-0"
                    />
                  </v-col>
                  <v-col class="my-0 py-0" cols="4">
                    <v-label>Include requierements table</v-label>
                  </v-col>
                </v-row>

                <v-simple-table class="my-0 py-0" dense fixed-header>
                  <template #default class="my-0 py-0">
                    <thead class="my-0 py-0">
                      <tr class="my-0 py-0">
                        <th class="text-left" />
                        <th class="text-left" width="260">
                          Requirement
                        </th>
                        <th class="text-left" width="260">
                          <v-text-field v-model="editedItem.text.reqHeader1" class="my-0 py-0" />
                        </th>
                        <th class="text-left" width="260">
                          <v-text-field v-model="editedItem.text.reqHeader2" class="my-0 py-0" />
                        </th>
                        <th class="text-left" width="260">
                          <v-text-field v-model="editedItem.text.reqHeader3" class="my-0 py-0" />
                        </th>
                        <th class="my-0 py-0 text-left">
                          Result
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr
                        v-for="item in editedItem.text.requirements"
                        :key="item.num"
                        dense
                      >
                        <td width="60">
                          <v-checkbox v-model="item.include" />
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.requirement" />
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.value1" />
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.value2" />
                        </td>
                        <td width="260">
                          <v-text-field v-model="item.value3" />
                        </td>
                        <td width="80">
                          <v-text-field v-model="item.result" />
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
                  <v-col cols="2">
                    <v-checkbox
                      v-model="editedItem.dailyTotals"
                      label="Daily totals"
                      hide-details
                    />
                  </v-col>
                  <v-col cols="4">
                    <v-select
                      v-model="editedItem.histogram"
                      :items="csvColumns"
                      dense
                      class="mt-6"
                      label="Histogram column"
                    />
                  </v-col>
                  <v-col cols="4">
                    <v-select
                      v-model="editedItem.histoColor"
                      :items="colors"
                      dense
                      class="mt-6"
                      label="Histogram color"
                      item-text="name"
                      item-value="hex"
                      @change="updateHistoColor"
                    />
                  </v-col>
                  <v-col cols="2">
                    <v-btn class="mt-3" :color="editedItem.histoColor" />
                  </v-col>
                </v-row>
                <v-row class="py-0 my-0">
                  <v-col cols="2">
                    <v-checkbox
                      v-model="editedItem.chartLimits.enable"
                      label="Set limits"
                      hide-details
                    />
                  </v-col>
                  <v-col cols="2">
                    <v-text-field
                      v-model="editedItem.chartLimits.ymin"
                      type="number"
                      :disabled="!editedItem.chartLimits.enable"
                      label="Min value"
                    >
                      >
                    </v-text-field>
                  </v-col>
                  <v-col cols="2">
                    <v-text-field
                      v-model="editedItem.chartLimits.ymax"
                      type="number"
                      :disabled="!editedItem.chartLimits.enable"
                      label="Max value"
                    >
                      >
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field
                      v-model="editedItem.yAxisName"
                      label="Units"
                    />
                  </v-col>
                  <v-col cols="2">
                    <v-text-field
                      v-model="editedItem.factor"
                      type="number"
                      label="Scale factor"
                    />
                  </v-col>
                </v-row>
                <v-row class="py-0 my-0">
                  <v-col cols="2">
                    <v-checkbox
                      v-model="editedItem.separateCharts"
                      class="py-0 my-0"
                      label="Separate charts"
                      hide-details
                    />
                  </v-col>
                  <v-col class="mt-1" cols="4">
                    <v-text-field
                      v-model="editedItem.chartTitle"
                      dense
                      label="Chart title"
                    />
                  </v-col>
                  <v-col class="mt-1" cols="4">
                    <v-text-field
                      v-model="editedItem.histogramTitle"
                      dense
                      label="Histogram title"
                    />
                  </v-col>
                  <v-col class="mt-1" cols="2">
                    <v-text-field
                      v-model="editedItem.bins"
                      type="number"
                      dense
                      label="Histogram bins"
                    />
                  </v-col>
                </v-row>
                <v-row justify="start">
                  <v-col cols="10">
                    <v-simple-table dense fixed-header height="270">
                      <template #default dense>
                        <thead>
                          <tr>
                            <th class="text-left" width="43%">
                              Columns
                            </th>
                            <th class="text-left" width="35%">
                              Color
                            </th>
                            <th class="text-left" />
                            <th class="text-left" />
                          </tr>
                        </thead>
                        <tbody dense>
                          <tr v-for="(item, index) in editedItem.mappings" :key="item.series" dense>
                            <td>
                              <v-select v-model="item.col" dense flat :items="csvColumns" />
                            </td>
                            <td>
                              <v-select
                                v-model="item.color"
                                :items="colors"
                                dense
                                item-text="name"
                                item-value="hex"
                              />
                            </td>
                            <td>
                              <v-btn :color="item.color" />
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
          <v-spacer />
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
              v-model="editedCover.title"
              label="Title"
            />
            <v-text-field
              v-model="editedCover.client"
              label="Client"
            />
            <v-text-field
              v-model="editedCover.siteLocation"
              label="Site location"
            />
            <v-text-field
              v-model="editedCover.instrumentUsed"
              label="Instrument used"
            />
            <v-text-field
              v-model="editedCover.serialNumber"
              label="serialNumber"
            />
            <v-text-field
              v-model="editedCover.reportDate"
              label="Report date"
            />
            <v-text-field
              v-model="editedCover.reportAuthor"
              label="Report author"
            />
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn color="blue darken-1" text @click="clearCover">
            Clear
          </v-btn>
          <v-spacer />
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
              v-model="editedPage2.include"
              label="Include second page"
              hide-details
            />
            <v-text-field
              v-model="editedPage2.title"
              label="Title"
            />
            <v-text-field
              v-model="editedPage2.subtitle"
              label="Subtitle"
            />
            <v-textarea
              v-model="editedPage2.p1"
              label="Paragraph 1"
              rows="2"
              clearable
              clear-icon="mdi-close-circle"
            />
            <v-textarea
              v-model="editedPage2.p2"
              label="Paragraph 2"
              rows="2"
              clearable
              clear-icon="mdi-close-circle"
            />
            <v-textarea
              v-model="editedPage2.p3"
              label="Paragraph 3"
              rows="2"
              clearable
              clear-icon="mdi-close-circle"
            />
            <v-textarea
              v-model="editedPage2.p4"
              label="Paragraph 4"
              rows="2"
              clearable
              clear-icon="mdi-close-circle"
            />
            <v-textarea
              v-model="editedPage2.p5"
              label="Paragraph 5"
              rows="2"
              clearable
              clear-icon="mdi-close-circle"
            />
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn color="blue darken-1" text @click="clearPage2">
            Clear
          </v-btn>
          <v-spacer />
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
              <template #activator>
                <v-list-item-content>
                  <v-list-item-title v-text="calcColumn.name" />
                </v-list-item-content>
              </template>
              <template>
                <v-container>
                  <v-row>
                    <v-col cols="1" />
                    <v-col cols="8">
                      <v-simple-table dense fixed-header>
                        <template #default>
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
                                <v-select v-model="param.col" :items="csvColumns" dense />
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
          <v-spacer />
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
            v-model="newReportDescription"
            label="Description"
          />
          <v-checkbox
            v-model="newBlankReport"
            class="py-0 mt-4"
            label="Blank template"
          />
          <v-select
            v-model="selectedTemplate"
            class="my-4"
            label="Copy selected template (optional)"
            dense
            flat
            :items="templatesAsList"
            :disabled="newBlankReport"
          />
          <v-file-input
            ref="fileInput"
            v-model="inputfile"
            accept="text/csv"
            label="CSV File"
            @change="createReport"
          />
        </v-card-text>

        <v-card-actions>
          <v-spacer />
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
            v-model="selectedTemplate"
            class="my-4"
            label="Select report template"
            dense
            flat
            :items="templatesAsList"
          />
          <v-file-input
            ref="fileInput"
            v-model="inputfile"
            accept="text/csv"
            label="CSV File"
            @change="readFile"
          />
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn color="blue darken-1" text @click="closeLoadReport">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="alert" max-width="500px">
      <v-card>
        <v-card-title class="text-body-1" />
        <v-card-text class="header3">
          {{ userMessage }}
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="blue darken-1" text @click="closeMessage">
            OK
          </v-btn>
          <v-spacer />
        </v-card-actions>
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

// TODO: Migrar A typesscript, pasar la configuracion de echart a un plugin
<script>

import Papa from 'papaparse'

import pdfMake from 'pdfmake/build/pdfmake'
import pdfFonts from 'pdfmake/build/vfs_fonts'

import * as echarts from 'echarts/lib/echarts'
import 'echarts/lib/chart/bar'
import 'echarts/lib/chart/line'
import 'echarts/lib/component/tooltip'
import 'echarts/lib/component/title'
import 'echarts/lib/component/grid'
import 'echarts/lib/component/legend'
import { MarkLineComponent } from 'echarts/components'
import * as d3 from 'd3'
import { lineChartOptions, sepLineChartOptions, histogramOptions, barChartOptions, minMax }
  from '../composables/charts.js'

// import 'blueimp-md5';

import colors from '../composables/entity.js'
import document from '../composables/document.js'
import { getColumns, getCalcColumns, checkTemplate }
  from '../composables/categories.js'

pdfMake.vfs = pdfFonts.pdfMake.vfs
echarts.use([MarkLineComponent])

//  replaced by spread operator {...object}
// function copyOf(obj) {
//   return JSON.parse(JSON.stringify(obj));
// }

const suffix = ' '

export default {
  name: 'EnergyReport',
  data () {
    return {
      tabs: null,
      colors: colors(),
      version: '1.0',
      alert: false,
      userMessage: '',
      dialog: false,
      inputfile: null,
      newReportDescription: '',
      newBlankReport: false,
      selectedTemplate: null,
      description: '',
      columnDefs: [
        { text: 'Num', value: 'rank', width: '15%' },
        { text: 'Title', value: 'title', width: '62%' },
        { text: 'Actions', value: 'actions', width: '10%', sortable: false }
      ],
      dateColumn: 'Date',
      timeColumn: 'Time',
      csvData: [],
      csvColumns: [],
      templates: [],
      emptyTemplate: {
        description: '',
        bins: '',
        categories: [],
        calcColumns: [],
        cover: {
          title: '',
          client: '',
          siteLocation: '',
          instrumentUsed: '',
          serialNumber: '',
          reportDate: '',
          reportAuthor: ''
        },
        page2: {
          include: false,
          title: '',
          subtitle: '',
          p1: '',
          p2: '',
          p3: '',
          p4: '',
          p5: ''
        }
      },
      template: {},
      editedIndex: -1,
      defaultRequirement: { num: 0, requirement: '', value1: '', value2: '', value3: '', result: '', include: false },
      editedItem: {
        separateCharts: false,
        disabled: false,
        chartTitle: '',
        title: '',
        histogram: '',
        histoColor: '',
        chartLimits: { enable: false, ymin: 0, ymax: 0 },
        factor: undefined,
        bins: undefined,
        mappings: [],
        text: {
          paramName: '',
          paramValue: '',
          paramDef: '',
          limitations: '',
          incParam: false,
          incParamDef: false,
          incLim: false,
          includeRequirements: false,
          reqHeader1: '',
          reqHeader2: '',
          reqHeader3: '',
          requirements: [
            { num: 0, requirement: '', value1: '', value2: '', value3: '', result: '', include: false },
            { num: 1, requirement: '', value1: '', value2: '', value3: '', result: '', include: false }
          ]
        }
      },
      defaultItem: {
        separateCharts: false,
        disabled: false,
        chartTitle: '',
        title: '',
        histogram: '',
        histoColor: '',
        chartLimits: { enable: false, ymin: 0, ymax: 0 },
        factor: undefined,
        bins: undefined,
        mappings: [],
        text: {
          paramName: '',
          paramValue: '',
          paramDef: '',
          limitations: '',
          incParam: false,
          incParamDef: false,
          incLim: false,
          includeRequirements: false,
          reqHeader1: '',
          reqHeader2: '',
          reqHeader3: '',
          requirements: [
            { num: 0, requirement: '', value1: '', value2: '', value3: '', result: '', include: false },
            { num: 1, requirement: '', value1: '', value2: '', value3: '', result: '', include: false }
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
      background: '',
      reportFinished: false
    }
  },
  computed: {
    formTitle () {
      return this.editedIndex === -1 ? 'New Section' : 'Edit Section'
    },
    histogramColor () {
      // TODO: do not log in production mode
      console.log(this.editedItem.histoColor)
      return this.editedItem ? this.editedItem.histoColor : ''
    },
    templatesAsList () {
      return Object.keys(this.templates)
        .filter(key => key !== 'model')
        .map(key => ({ text: this.templates[key].description, value: key }))
    }
  },
  watch: {
    dialog (val) {
      val || this.close()
    },
    dialogDelete (val) {
      val || this.closeDelete()
    }
  },
  beforeCreate () {
    this.template = this.emptyTemplate
  },
  created () {
    this.initialize()
  },
  methods: {
    getLastId () {
      let max = 0

      const keys = Object.keys(this.templates)
      keys.forEach(function (id) {
        if (!isNaN(id) && id > max) {
          max = id
        }
      })

      return (parseInt(max) + 1).toString()
    },
    newReport () {
      this.inputfile = null
      this.newReportDescription = ''
      this.selectedTemplate = ''
      this.showNewReport = true
    },
    closeNewReport () {
      this.showNewReport = false
    },
    loadReport () {
      this.readTemplates().then((result) => {
        this.templates = result
        this.inputfile = null
        this.selectedTemplate = ''
        this.showLoadReport = true
        this.$forceUpdate()
      })
    },
    closeLoadReport () {
      this.showLoadReport = false
    },
    createReport (file) {
      // TODO: do not log in producion mode
      console.log('createReport')
      if (!file) {
        // TODO: Do not log in production mode
        console.log('file is null or undefined')
        return
      }
      if (!this.newReportDescription || this.newReportDescription === '') {
        this.showMessage('You must enter a report description')
        return
      }

      // Copy existing template
      if (this.selectedTemplate) {
        const id = this.getLastId()
        console.log('Making a copy of existing template: ', this.selectedTemplate)
        this.templates[id] = { ...his.templates[this.selectedTemplate] }
        this.templates[id].description = this.newReportDescription
        this.templates[id].cover = { ...this.emptyTemplate.cover }
        this.templates[id].page2 = { ...this.emptyTemplate.page2 }
        this.selectedTemplate = id
        this.readFile(file)
        this.showNewReport = false
        return
      }

      // Create blank tempalte
      const reader = new FileReader()
      const self = this
      reader.onload = function (e) {
        // TODO: Do not log in produciont mode
        console.log('Loading csv file')

        const text = e.target.result

        const rawData = Papa.parse(text)
        const allcols = self.headerCols(rawData)
        // TODO: Use ===  insted of ==
        if (allcols.length == 0) {
          this.showMessage('File has no column definitions')
          return
        }

        self.clearAll()
        const id = self.getLastId()

        self.template = { ...self.templates.model }
        self.template.description = self.newReportDescription

        if (self.newBlankReport) {
          self.template.cover = { ...self.emptyTemplate.cover }
          self.template.page2 = { ...self.emptyTemplate.page2 }
          self.template.categories = []
        }

        self.templates[id] = self.template
        // TODO: Use !==  insted of !=
        const csvColumns = allcols.filter(entry => entry.trim() != '' && entry != 'Date' && entry != 'Time')
        const calcColumns = getCalcColumns(self.template)
        self.csvColumns = csvColumns.concat(calcColumns)
        self.csvData = []
      }

      reader.readAsText(file)
      this.showNewReport = false
    },
    updateHistoColor () {
      this.$forceUpdate()
    },
    dataInterval () {
      const d1 = this.csvData[0].DateTime
      const d2 = this.csvData[1].DateTime
      const time = d2.getTime() - d1.getTime()
      return time / 60000
    },
    showMessage (msg) {
      this.userMessage = msg
      this.alert = true
    },
    closeMessage () {
      this.userMessage = ''
      this.alert = false
    },
    endpoint (op) {
      return `/api/energyreport/${op}`
    },
    readTemplates () {
      const self = this
      return new Promise(function (resolve) {
        self.$axios.$get(self.endpoint('category'))
          .then((response) => {
            resolve(response)
          })
      })
    },
    initialize () {
      const self = this
      self.$axios.$get(this.endpoint('category'))
        .then((response) => {
          self.templates = response
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
    saveTemplates () {
      // TODO: Ise ===  insted of ==
      if (Object.keys(this.template).length == 0) {
        this.showMessage('No data available. Please load a template and CSV file')
        return
      }
      // TODO: Ise ===  insted of ==
      if (this.csvColumns.length == 0) {
        this.showMessage('No data available. Please create/load a template and CSV file')
        return
      }
      const configHeaders = {
        'content-type': 'text/plain',
        Accept: 'text/plain'
      }
      const str = JSON.stringify(this.templates)
      this.$axios({
        endpoint: this.endpoint('category'),
        method: 'post',
        data: str,
        headers: configHeaders
      })
        .then(() => {
          this.showMessage('Templates saved')
          this.inputfile = null
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
      const chart = this.lineCharts[index]
      const csvData = this.csvData
      const promise = new Promise(function (resolve) {
        if (category.separateCharts) {
          chart.setOption(sepLineChartOptions(csvData, category, suffix))
        } else {
          chart.setOption(lineChartOptions(csvData, category, suffix))
        }
        chart.resize()
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          })
          resolve(image.src)
        })
      })

      return promise
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

      const limits = { min: 1000000000, max: -1000000000 }
      minMax(this.csvData, colName, limits)

      const histoGen = d3.bin()
        .domain([limits.min, limits.max])
        .thresholds(numBins - 1)
      const bins = histoGen(data)

      const binCount = []
      const binValues = []

      for (let i = 0; i < bins.length; i++) {
        binCount.push(bins[i].length)
        // TODO: Use ===  insted of ==
        const value = bins[i].length == 0 ? 0 : bins[i].reduce((a, b) => a + b, 0) / bins[i].length
        binValues.push('' + value.toFixed(2))
      }

      const promise = new Promise(function (resolve) {
        const options = histogramOptions(category, binValues, binCount)
        chart.setOption(options)
        chart.resize()
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          })
          resolve(image.src)
        })
      })
      return promise
    },
    createBarChart (index) {
      const category = this.template.categories[index]
      const chart = this.histograms[index]
      const data = this.csvData

      const col = category.histogram

      const grp = {}
      data.forEach((d) => {
        const dt = d.DateTime
        if (dt) {
          const date = dt.toLocaleDateString('en-US', { month: '2-digit', day: '2-digit' })
          grp[date] = grp[date] ? grp[date] + d[col] : d[col]
        }
      })

      const groups = []
      Object.keys(grp).forEach(key => groups.push({ day: key, value: grp[key] }))

      const promise = new Promise(function (resolve) {
        const options = barChartOptions(category, groups)
        chart.setOption(options)
        chart.resize()
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white'
          })
          resolve(image.src)
          resolve(0)
        })
      })
      return promise
    },
    doReport () {
      // TODO: Use ===  insted of ==
      if (Object.keys(this.template).length == 0) {
        this.showMessage('No template loaded')
        return
      }
      // TODO: Use ===  insted of ==
      if (this.csvColumns.length == 0 || this.csvData.length == 0) {
        this.showMessage('No data available. Please load an approriate CSV file')
        return
      }
      const self = this
      this.reportFinished = false
      this.genReport()
      setTimeout(function () {
        if (!self.reportFinished) {
          // console.log("Timeout, trying again");
          self.genReport()
        }
      }, 1200)
    },
    async genReport () {
      // TODO: do not log in production mode
      console.log('Generating PDF')

      const promises = []
      for (let i = 0; i < this.template.categories.length; i++) {
        promises.push(this.createLineChart(i))
        if (this.template.categories[i].dailyTotals) {
          promises.push(this.createBarChart(i))
        } else {
          promises.push(this.createHistogram(i))
        }
      }
      const charts = await Promise.all(promises)

      this.reportFinished = true
      // console.log("All charts created");
      const doc = document(this.template.cover, this.template.page2, this.background, this.reportPeriod(),
        this.template.categories, charts, this.dataInterval())
      const pdf = pdfMake.createPdf(doc)
      pdf.download('energy-report.pdf')
      // pdf.open();
      // TODO: Do not log in production mode
      console.log('PDF created')
    },
    dateTime (dateStr, timeStr) {
      if (!dateStr || !timeStr) {
        return null
      }
      const da = dateStr.split(/[/.-]/)
      const ta = timeStr.split(':')
      if (parseInt(da[2]) > 31) {
        const temp = da[0]
        da[0] = da[2]
        da[2] = temp
      }
      const dateTime = new Date(da[0], da[1] - 1, da[2], ta[0], ta[1], ta[2])
      // return dateTime.getTime();
      return dateTime
    },
    headerCols (rawData) {
      const rows = rawData.data
      let firstRow = 0
      for (let i = 0; i < rows.length; i++) {
        const row = rows[i]
        if (row.length > 5) {
          if (firstRow > 0) {
            rawData.data.splice(0, firstRow)
          }
          return rows[0]
        }
        firstRow++
      }
      return []
    },
    clearAll () {
      this.template = self.emptyTemplate
      this.csvData = []
      this.csvColumns = []
    },
    readFile (file) {
      if (!file) {
        return
      }
      const reader = new FileReader()
      const self = this
      reader.onload = function (e) {
        // TODO: Do not log in production mode
        console.log('Loading csv file')

        let text = e.target.result

        const rawData = Papa.parse(text)
        const allcols = self.headerCols(rawData)
        // TODO: Use ===  insted of ==
        if (allcols.length == 0) {
          this.showMessage('File has no column definitions')
          return
        }

        self.clearAll()
        self.template = self.templates[self.selectedTemplate]

        const cols = getColumns(self.template)

        if (!checkTemplate(self.template, allcols, cols)) {
          self.showMessage('The template is not compatible with the CSV file')
          return
        }

        text = Papa.unparse(rawData)

        const data = []
        let index = 0
        const csvData = Papa.parse(text, {
          header: true,
          dynamicTyping: true,
          step (parsedRow) {
            const dt = self.dateTime(parsedRow.data[self.dateColumn], parsedRow.data[self.timeColumn])
            if (!dt) {
              return
            }
            const row = { index: index++, DateTime: dt }
            for (let i = 0; i < cols.length; i++) {
              const col = cols[i]
              let colName = col.name
              // TODO: Use ===  insted of ==
              if (colName == 'Date' || colName == 'Time') {
                continue
              }
              let value = parseFloat(parsedRow.data[col.name])
              if (col.factor) {
                colName += suffix
                value *= col.factor
              }
              // if (col.suffix) {
              //   colName += col.suffix;
              // }
              if (!row[colName]) {
                row[colName] = value
              }
            }
            data.push(row)
          }
        })

        // TODO: Use ===  insted of ==
        if (data.length == 0) {
          this.showMessage('No data rows found')
          return
        }

        const csvColumns = csvData.meta.fields
        // TODO: Use !==  insted of !=
        self.csvColumns = csvColumns.filter(entry => entry.trim() != '' && entry != 'Date' && entry != 'Time')
        self.csvData = data

        const calcColumns = self.template.calcColumns
        for (let i = 0; i < calcColumns.length; i++) {
          const column = calcColumns[i].name
          const params = calcColumns[i].params
          self[column](column, params)
          self.csvColumns.push(column)
        }
        self.showLoadReport = false
      }
      reader.readAsText(file)
    },
    newItem () {
      // TODO: Use ===  insted of ==
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage('No data available. Please load a template and CSV file')
        return
      }

      this.tabs = 'Text'
      this.editedIndex = -1
      this.editedItem = JSON.parse(JSON.stringify(this.defaultItem))
      this.dialog = true
    },
    editItem (item) {
      this.tabs = 'Text'
      this.editedIndex = this.template.categories.indexOf(item)
      this.editedItem = JSON.parse(JSON.stringify(this.defaultItem))
      const itemCopy = JSON.parse(JSON.stringify(item))
      this.editedItem = Object.assign(this.editedItem, itemCopy)

      if (!this.editedItem.text.requirements) {
        this.editedItem.text.requirements = []
      }
      for (let i = this.editedItem.text.requirements.length; i < 2; i++) {
        const req = Object.assign({}, this.defaultRequirement)
        req.num = i
        this.editedItem.text.requirements.push(req)
      }
      // TODO: Do not log in production mode
      console.log('factor:', this.editedItem.factor)

      this.dialog = true
    },
    addChartColumn () {
      const mappings = this.editedItem.mappings

      let num = 1
      if (mappings.length > 0) {
        const lastSeries = mappings[mappings.length - 1].series
        // TODO: Do not log in production mode
        console.log('lastSeries: ', lastSeries)
        num = parseInt(lastSeries) + 1
      }
      // TODO: do not log in porduction mode
      console.log('series name: ', num)
      this.editedItem.mappings.push({
        series: num.toString(),
        col: '',
        color: ''
      })
    },
    deleteChartColumn (index) {
      this.editedItem.mappings.splice(index, 1)
    },
    close () {
      this.dialog = false
      this.$nextTick(() => {
        this.editedIndex = -1
      })
    },
    save () {
      if (this.editedIndex > -1) {
        // TODO: Do not log in production mode
        console.log('factor:', this.editedItem.factor)
        console.log('factor?:', (!!this.editedItem.factor))
        Object.assign(this.template.categories[this.editedIndex], this.editedItem)
      } else {
        this.template.categories.push(this.editedItem)
      }
      this.close()
    },
    openCover () {
      // TODO: Use ===  insted of ==
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage('No data available. Please load a template and CSV file')
        return
      }
      this.editedCover = Object.assign({}, this.template.cover)
      this.showCover = true
    },
    saveCover () {
      this.template.cover = Object.assign({}, this.editedCover)
      this.showCover = false
    },
    clearCover () {
      this.editedCover = {
        title: '',
        client: '',
        siteLocation: '',
        instrumentUsed: '',
        serialNumber: '',
        reportDate: '',
        reportAuthor: ''
      }
    },
    closeCover () {
      this.showCover = false
    },
    openPage2 () {
      // TODO: Use ===  insted of ==
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage('No data available. Please load a template and CSV file')
        return
      }
      this.editedPage2 = Object.assign({}, this.template.page2)
      this.showPage2 = true
    },
    savePage2 () {
      this.template.page2 = Object.assign({}, this.editedPage2)
      this.showPage2 = false
    },
    clearPage2 () {
      this.editedPage2 = {
        include: false,
        title: '',
        subtitle: '',
        p1: '',
        p2: '',
        p3: '',
        p4: '',
        p5: ''
      }
    },
    closePage2 () {
      this.showPage2 = false
    },
    openCalcColumns () {
      // TODO: Use ===  insted of ==
      if (this.csvColumns.length == 0 || Object.keys(this.template).length == 0) {
        this.showMessage('No data available. Please load a template and CSV file')
        return
      }
      this.showCalcColumns = true
    },
    closeCalcColumns () {
      this.showCalcColumns = false
    },
    VoltageUnbalance (column, params) {
      for (let i = 0; i < this.csvData.length; i++) {
        const voltage = Array.from(params, param => this.csvData[i][param.col])
        const av = voltage.reduce((a, b) => a + b) / voltage.length
        const mx = Math.max(...voltage)
        this.csvData[i][column] = ((mx - av) / av) * 100
      }
    },
    AcumEnergy (column, params) {
      let prevTime = this.csvData[0].DateTime.getTime()
      let acum = this.csvData[0][params[0].col]
      this.csvData[0][column] = 0
      for (let i = 1; i < this.csvData.length; i++) {
        if (!this.csvData[i].DateTime) {
          continue
        }
        const time = this.csvData[i].DateTime.getTime()
        const elapsedTime = time - prevTime
        const hours = elapsedTime / 3600000
        acum += this.csvData[i][params[0].col]
        const energy = (acum * hours) / 1000
        this.csvData[i][column] = energy
        prevTime = time
      }
    },
    Energy (column, params) {
      for (let i = 0; i < this.csvData.length; i++) {
        if (!this.csvData[i].DateTime) {
          continue
        }
        // TODO: Use ===  insted of ==
        const elapsedTime = i == 0
          ? (this.csvData[i + 1].DateTime.getTime() - this.csvData[i].DateTime.getTime()) / 3600000
          : (this.csvData[i].DateTime.getTime() - this.csvData[i - 1].DateTime.getTime()) / 3600000
        this.csvData[i][column] = (this.csvData[i][params[0].col] * elapsedTime) / 1000
      }
    },
    MinPowerFactor (column, params) {
      for (let i = 0; i < this.csvData.length; i++) {
        const result = this.csvData[i][params[0].col] / this.csvData[i][params[1].col]
        this.csvData[i][column] = result
      }
    },
    AvePowerFactor (column, params) {
      for (let i = 0; i < this.csvData.length; i++) {
        const result = this.csvData[i][params[0].col] / this.csvData[i][params[1].col]
        this.csvData[i][column] = result
      }
    },
    MaxPowerFactor (column, params) {
      for (let i = 0; i < this.csvData.length; i++) {
        const result = this.csvData[i][params[0].col] / this.csvData[i][params[1].col]
        this.csvData[i][column] = result
      }
    }
  }
}

</script>
