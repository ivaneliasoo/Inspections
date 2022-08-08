<template>
  <v-container>
    <div v-if="reportTableVisible">
      <v-row>
        <v-col cols="1" class="my-0 py-0 text-left" />
        <v-col cols="5" class="my-0 py-0 text-left text-h5">
          Power Analyzer Report
        </v-col>
        <!-- <v-col cols="3" class="my-0 py-0 text-center">
        </v-col> -->
        <v-col cols="4" class="my-0 py-0 text-center">
          <v-btn small @click="editTemplates"> Edit Templates </v-btn>
          <v-btn small @click="newReport"> New From Template </v-btn>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12">
          <div
            style="width: 1024px; overflow-y: scroll"
            class="invisible-scrollbar"
          >
            <table class="search-box" width="100%">
              <tbody>
                <tr>
                  <td class="search-box" style="width: 100%">
                    <EnergyReportSearch
                      :data="reports"
                      :templates="templates"
                      :field-names="[
                        'name',
                        'location',
                        'template',
                        'circuit',
                        'dateCreated',
                      ]"
                      place-holder="Search"
                      :size="52"
                      :compare-func="compareFunc"
                    />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div
            style="width: 1024px; overflow-y: scroll"
            class="invisible-scrollbar"
          >
            <table width="100%">
              <tbody>
                <tr class="table-header">
                  <td
                    class="font-weight-bold text-center header-row"
                    style="width: 18%"
                    title="Click to sort by project"
                    @click="sortByField('name')"
                  >
                    Project
                    <v-icon>
                      {{ sortIcon('project') }}
                    </v-icon>
                  </td>
                  <td
                    class="font-weight-bold text-center header-row"
                    style="width: 18%"
                    title="Click to sort by location"
                    @click="sortByField('location')"
                  >
                    Location
                    <v-icon>
                      {{ sortIcon('location') }}
                    </v-icon>
                  </td>
                  <td
                    class="font-weight-bold text-center header-row"
                    style="width: 16%"
                    title="Click to sort by location"
                    @click="sortByField('template')"
                  >
                    Template
                    <v-icon>
                      {{ sortIcon('template') }}
                    </v-icon>
                  </td>
                  <td
                    class="font-weight-bold text-center header-row"
                    style="width: 16%"
                    title="Click to sort by location"
                    @click="sortByField('circuit')"
                  >
                    Circuit Board
                    <v-icon>
                      {{ sortIcon('circuit') }}
                    </v-icon>
                  </td>
                  <td
                    class="font-weight-bold text-center header-row"
                    style="width: 14%"
                    title="Click to sort by date"
                    @click="sortByField('date')"
                  >
                    Date created
                    <v-icon>
                      {{ sortIcon('date') }}
                    </v-icon>
                  </td>
                  <td
                    class="font-weight-bold text-center header-row"
                    style="width: 20%"
                    title="Click for original sort order"
                    @click="sortById"
                  />
                </tr>
              </tbody>
            </table>
          </div>
          <div style="height: 75vh; width: 1024px; overflow-y: scroll">
            <table id="reports" class="report-table">
              <tbody>
                <tr
                  v-for="(report, index) in reports"
                  :key="index"
                  style="height: 0"
                >
                  <td
                    class="text-caption text-left table-row"
                    style="width: 18%"
                  >
                    {{ report.name }}
                  </td>
                  <td
                    class="text-caption text-left table-row"
                    style="width: 18%"
                  >
                    {{ report.location }}
                  </td>
                  <td
                    class="text-caption text-left table-row"
                    style="width: 16%"
                  >
                    {{
                      templates &&
                      Object.keys(templates).length > 0 > 0 &&
                      report.template > 0
                        ? templates[report.template].description
                        : ''
                    }}
                  </td>
                  <td
                    class="text-caption text-left table-row"
                    style="width: 16%"
                  >
                    {{ report.circuit }}
                  </td>
                  <td
                    class="text-caption text-center table-row"
                    style="width: 14%"
                  >
                    {{ report.dateCreated }}
                  </td>
                  <td
                    class="text-caption text-center table-row"
                    style="width: 20%"
                  >
                    <v-icon dense @click="editReport(index)">
                      mdi-pencil
                    </v-icon>
                    &nbsp;
                    <v-icon dense @click="editCover(index)">
                      mdi-book-edit
                    </v-icon>
                    &nbsp;
                    <v-icon dense @click="getReport(report.id, doReport)">
                      mdi-file-pdf-box
                    </v-icon>
                    <v-icon
                      large
                      @click="getReport(report.id, genCurrentTable)"
                    >
                      mdi-alpha-i
                    </v-icon>
                    <v-icon dense @click="deleteReport(index)">
                      mdi-delete
                    </v-icon>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </v-col>
      </v-row>
    </div>

    <div v-if="templateTableVisible">
      <v-row>
        <v-col cols="1" class="my-0 py-0 text-center">
          <v-btn icon small @click="goBack">
            <v-icon> mdi-arrow-left </v-icon>
          </v-btn>
        </v-col>
        <v-col cols="11" class="my-0 py-0 text-left text-h5">
          Power Analyzer Report - Edit Templates
        </v-col>
      </v-row>
      <v-row>
        <v-col class="mb-4">
          <v-card height="550" width="920" class="pa-md-4">
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
                    <!-- <v-list-item @click="newTemplate">
                      <v-list-item-icon>
                        <v-icon>mdi-new-box</v-icon>
                      </v-list-item-icon>
                      <v-list-item-content>
                        <v-list-item-title>New template</v-list-item-title>
                      </v-list-item-content>
                    </v-list-item> -->
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
                        <v-list-item-title
                          >Calculated Columns</v-list-item-title
                        >
                      </v-list-item-content>
                    </v-list-item>
                    <v-list-item @click="doTestReport">
                      <v-list-item-icon>
                        <v-icon>mdi-view-dashboard</v-icon>
                      </v-list-item-icon>
                      <v-list-item-content>
                        <v-list-item-title
                          >Generate Test Report</v-list-item-title
                        >
                      </v-list-item-content>
                    </v-list-item>
                    <v-list-item @click="genTestCurrentTable">
                      <v-list-item-icon>
                        <v-icon>mdi-view-dashboard</v-icon>
                      </v-list-item-icon>
                      <v-list-item-content>
                        <v-list-item-title
                          >Generate Test Current Table</v-list-item-title
                        >
                      </v-list-item-content>
                    </v-list-item>
                    <v-list-item @click="checkSaveTemplates">
                      <v-list-item-icon>
                        <v-icon>mdi-view-dashboard</v-icon>
                      </v-list-item-icon>
                      <v-list-item-content>
                        <v-list-item-title>Save Templates</v-list-item-title>
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
                        <v-icon
                          v-show="template.description"
                          small
                          @click="editTemplateName"
                        >
                          mdi-pencil
                        </v-icon>
                        {{ template.description }}
                      </div>
                      <v-spacer />
                      <v-btn small @click="newItem"> New Section </v-btn>
                    </v-toolbar>
                  </v-col>
                </v-row>
                <v-row justify="center">
                  <v-col class="my-n2 py-n2">
                    <v-simple-table
                      class="simple-table"
                      height="400px"
                      style="width: 100%; overflow-x: hidden; overflow-y: auto"
                      dense
                      fixed-header
                    >
                      <thead class="text-caption">
                        <tr>
                          <th style="width: 10%">Num</th>
                          <th style="width: 80%">Title</th>
                          <th style="width: 10%">Actions</th>
                        </tr>
                      </thead>
                      <tbody class="text-caption text-left">
                        <tr
                          v-for="(category, index) in template.categories"
                          :key="index"
                          draggable="true"
                          @dragover="dragoverHandler($event)"
                          @dragstart="dragstartHandler($event, index)"
                          @drop="dropHandler($event, index)"
                        >
                          <td style="border: 1px solid lightgray; width: 10%">
                            {{ index + 1 }}
                          </td>
                          <td style="border: 1px solid lightgray; width: 80%">
                            {{ category.title }}
                          </td>
                          <td
                            class="mx-4 px-4"
                            style="border: 1px solid lightgray; width: 10"
                          >
                            <v-icon
                              small
                              class="mx-1"
                              @click="editItem(category)"
                            >
                              mdi-pencil
                            </v-icon>
                            <v-label>
                              {{ category.disabled ? 'x' : '' }}
                            </v-label>
                          </td>
                        </tr>
                      </tbody>
                    </v-simple-table>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
            <!-- <v-row>
              <v-col cols="10"> </v-col>
              <v-col cols="2">
                <p justify="end" style="font-size: small;">
                  Version {{ version }}
                </p>
              </v-col>
            </v-row> -->
          </v-card>
        </v-col>
      </v-row>
    </div>

    <v-dialog v-model="dialog" max-width="1000px">
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
            <v-tab> Text </v-tab>
            <v-tab> Charts </v-tab>
            <v-tab-item>
              <v-container>
                <v-row class="pt-4">
                  <v-col cols="1">
                    <v-checkbox
                      v-model="editedItem.text.incParam"
                      class="ml-3"
                    />
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
                    <v-checkbox
                      v-model="editedItem.text.incParamDef"
                      class="ml-3"
                    />
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

                <v-simple-table
                  class="my-0 py-0 simple-table"
                  style="max-width: 100%; overflow-x: hidden"
                  dense
                  fixed-header
                >
                  <thead>
                    <tr class="my-0 py-0">
                      <th
                        class="text-left"
                        style="width: 10%; border: 2px solid lightgray"
                      />
                      <th
                        class="text-left"
                        style="width: 10%; border: 2px solid lightgray"
                      >
                        % Time
                      </th>
                      <th
                        class="text-left"
                        style="width: 10%; border: 2px solid lightgray"
                      >
                        Min value
                      </th>
                      <th
                        class="text-left"
                        style="width: 10%; border: 2px solid lightgray"
                      >
                        Max value
                      </th>
                      <th
                        class="text-left"
                        style="width: 20%; border: 2px solid lightgray"
                      >
                        <v-text-field
                          v-model="editedItem.text.reqHeader1"
                          flat
                          solo
                          hide-details
                        />
                      </th>
                      <th
                        class="text-left"
                        style="width: 20%; border: 2px solid lightgray"
                      >
                        <v-text-field
                          v-model="editedItem.text.reqHeader2"
                          flat
                          solo
                          hide-details
                        />
                      </th>
                      <th
                        class="text-left"
                        style="width: 20%; border: 2px solid lightgray"
                      >
                        <v-text-field
                          v-model="editedItem.text.reqHeader3"
                          flat
                          solo
                          hide-details
                        />
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr
                      v-for="item in editedItem.text.requirements"
                      :key="item.num"
                    >
                      <td style="width: 10%; border: 2px solid lightgray">
                        <v-checkbox
                          v-model="item.include"
                          flat
                          solo
                          hide-details
                          class="ma-0 pa-0"
                        />
                      </td>
                      <td style="width: 10%; border: 2px solid lightgray">
                        <v-text-field
                          v-model="item.timePercent"
                          flat
                          solo
                          hide-details
                        />
                      </td>
                      <td style="width: 10%; border: 2px solid lightgray">
                        <v-text-field
                          v-model="item.ymin"
                          flat
                          solo
                          hide-details
                        />
                      </td>
                      <td style="width: 10%; border: 2px solid lightgray">
                        <v-text-field
                          v-model="item.ymax"
                          flat
                          solo
                          hide-details
                        />
                      </td>
                      <td style="width: 20%; border: 2px solid lightgray">
                        <v-select
                          v-model="item.value1"
                          dense
                          flat
                          solo
                          hide-details
                          class="ma-0 pa-0"
                          :items="csvColumns"
                        />
                      </td>
                      <td style="width: 20%; border: 2px solid lightgray">
                        <v-select
                          v-model="item.value2"
                          dense
                          flat
                          solo
                          hide-details
                          class="ma-0 pa-0"
                          :items="csvColumns"
                        />
                      </td>
                      <td style="width: 20%; border: 2px solid lightgray">
                        <v-select
                          v-model="item.value3"
                          dense
                          flat
                          solo
                          hide-details
                          class="ma-0 pa-0"
                          :items="csvColumns"
                        />
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
                    <v-simple-table
                      class="simple-table"
                      dense
                      fixed-header
                      height="270"
                    >
                      <thead>
                        <tr>
                          <th
                            class="text-left"
                            style="width: 26%; border: 1px solid lightgray"
                          >
                            Parameter
                          </th>
                          <th
                            class="text-left"
                            style="width: 29%; border: 1px solid lightgray"
                          >
                            Column
                          </th>
                          <th
                            class="text-left"
                            style="width: 8%; border: 1px solid lightgray"
                          >
                            Peaks
                          </th>
                          <th
                            class="text-left"
                            style="width: 20%; border: 1px solid lightgray"
                          >
                            Color
                          </th>
                          <th
                            class="text-left"
                            style="border: 1px solid lightgray"
                          />
                          <th
                            class="text-left"
                            style="border: 1px solid lightgray"
                          />
                        </tr>
                      </thead>
                      <tbody dense>
                        <tr
                          v-for="(item, index) in editedItem.mappings"
                          :key="item.series"
                          dense
                        >
                          <td style="width: 26%; border: 1px solid lightgray">
                            <v-text-field
                              v-model="item.param"
                              dense
                              flat
                              solo
                              hide-details
                            />
                          </td>
                          <td style="width: 29%; border: 1px solid lightgray">
                            <v-select
                              v-model="item.col"
                              dense
                              flat
                              solo
                              hide-details
                              :items="csvColumns"
                            />
                          </td>
                          <td style="width: 8%; border: 1px solid lightgray">
                            <v-checkbox
                              v-model="item.showPeaks"
                              dense
                              flat
                              solo
                              hide-details
                            />
                          </td>
                          <td style="width: 20%; border: 1px solid lightgray">
                            <v-select
                              v-model="item.color"
                              :items="colors"
                              dense
                              flat
                              solo
                              hide-details
                              item-text="name"
                              item-value="hex"
                            />
                          </td>
                          <td style="border: 1px solid lightgray">
                            <v-btn small :color="item.color" />
                          </td>
                          <td style="border: 1px solid lightgray">
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
          <v-spacer />
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
              v-model="editedPage2.title"
              class="my-0 py-0"
              label="Title"
            />
            <v-text-field
              v-model="editedPage2.subtitle"
              class="my-0 py-0"
              label="Subtitle"
            />
            <v-simple-table
              class="my-0 py-0 simple-table"
              style="max-width: 100%; overflow-x: hidden"
              dense
              fixed-header
            >
              <thead class="my-0 py-0">
                <tr class="my-0 py-0">
                  <th
                    class="text-left"
                    style="width: 10%; border: 1px solid lightgray"
                  >
                    Num.
                  </th>
                  <th
                    class="text-left"
                    style="width: 45%; border: 1px solid lightgray"
                  >
                    Title
                  </th>
                  <th
                    class="text-left"
                    style="width: 45%; border: 1px solid lightgray"
                  >
                    Remarks
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in editedPage2.requirements" :key="item.num">
                  <td style="width: 10%; border: 1px solid lightgray">
                    {{ item.num }}
                  </td>
                  <td style="width: 45%; border: 1px solid lightgray">
                    {{ requirementTableTitle(item.num) }}
                  </td>
                  <td style="width: 45%; border: 1px solid lightgray">
                    <input
                      v-model="item.remarks"
                      class="text-caption table-input"
                      type="text"
                      size="40"
                    />
                  </td>
                </tr>
                <tr v-for="item in editedPage2.additionalInfo" :key="item.num">
                  <td
                    style="
                      width: 10%;
                      height: 20px;
                      border: 1px solid lightgray;
                    "
                  >
                    {{ item.num }}
                  </td>
                  <td
                    style="width: 45%; height: 20px; border: 1px solid lightgray;"
                  >
                    {{ requirementTableTitle(item.num) }}
                  </td>
                  <td style="width: 45%; border: 1px solid lightgray">
                    <input
                      v-model="item.remarks"
                      class="text-caption table-input"
                      type="text"
                      size="40"
                    />
                  </td>
                </tr>
              </tbody>
            </v-simple-table>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn @click="clearPage2"> Clear </v-btn>
          <v-spacer />
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
                      <v-simple-table class="simple-table" dense fixed-header>
                        <thead>
                          <tr>
                            <th class="text-left" style="width: 50%">
                              Param name
                            </th>
                            <th class="text-left" style="width: 50%">
                              CSV column
                            </th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-for="(param, j) in calcColumn.params" :key="j">
                            <td style="width: 50%; border: 1px solid lightgray">
                              {{ param.name }}
                            </td>
                            <td style="width: 50%; border: 1px solid lightgray">
                              <v-select
                                v-model="param.col"
                                :items="csvColumns"
                                dense
                                flat
                                solo
                                hide-details
                              />
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
          <v-spacer />
          <v-btn @click="closeCalcColumns"> Close </v-btn>
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
          <v-btn @click="closeLoadReport"> Close </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="templateNameDialog" max-width="600px">
      <v-card>
        <v-card-title> Template name </v-card-title>
        <v-card-text>
          <v-text-field v-model="description" label="Template Name" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn @click="saveTemplateName"> Ok </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog
      v-model="reportDialog.showReportInfo"
      class="report-dialog"
      max-width="900px"
    >
      <v-card>
        <v-card-title>
          <span class="headline">Reports</span>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col>
              <v-form>
                <v-row>
                  <v-col class="mt-4 pt-4 mb-0 pb-0">
                    <v-text-field v-model="report.name" label="Project" />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="mt-0 pt-0 mb-2 pb-2">
                    <v-text-field
                      v-model="report.location"
                      label="Site location"
                    />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="my-0 py-0">
                    <v-select
                      v-model="report.template"
                      label="Select report template"
                      dense
                      flat
                      :items="templatesAsList"
                    />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="mt-2 pt-2 mb-0 pb-0">
                    <h4>File name: &nbsp; {{ report.fileName }}</h4>
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
                    />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="my-0 py-0">
                    <v-text-field v-model="report.circuit" label="Circuit" />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="mt-3 pt-3 mb-3 pb-3">
                    <v-select
                      v-model="report.chartLegendOption"
                      label="Chart Legends"
                      dense
                      flat
                      :items="chartLegendOptions"
                    />
                  </v-col>
                </v-row>
              </v-form>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn @click="saveReport(report)"> Save &amp; close </v-btn>
          <v-btn @click="cancelReportDialog"> Cancel </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog
      v-model="reportDialog.showReportCover"
      class="report-dialog"
      max-width="900px"
    >
      <v-card>
        <v-card-title>
          <span class="headline">Report Cover</span>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col>
              <v-form>
                <v-container class="report-form">
                  <v-row>
                    <v-col class="mt-2 pt-2 mb-0 pb-0">
                      <v-text-field
                        v-model="report.cover.title"
                        label="Title"
                      />
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="my-0 py-0">
                      <v-text-field
                        v-model="report.cover.client"
                        label="Client"
                      />
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="my-0 py-0">
                      <v-text-field
                        v-model="report.cover.separation"
                        label="Separation"
                      />
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="my-0 py-0">
                      <v-text-field
                        v-model="report.cover.instrumentUsed"
                        label="Instrument used"
                      />
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="my-0 py-0">
                      <v-text-field
                        v-model="report.cover.serialNumber"
                        label="serialNumber"
                      />
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="mt-0 pt-0 mb-0 pb-0">
                      <v-text-field
                        v-model="report.cover.reportAuthor"
                        label="Report author"
                      />
                    </v-col>
                  </v-row>
                </v-container>
              </v-form>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn @click="saveReport(report)"> Save &amp; close </v-btn>
          <v-btn @click="cancelCoverDialog"> Cancel </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="genReportDialog" max-width="320px" height="240px">
      <v-card class="mx-auto">
        <v-card-title>
          <span class="headline">Generating PDF</span>
          <v-spacer />
          <div id="my-progress" ref="myProgress">
            <div id="my-bar" ref="myBar" />
          </div>
        </v-card-title>
      </v-card>
    </v-dialog>

    <v-dialog v-model="alert" max-width="600px">
      <v-card>
        <v-card-title class="text-body-1" />
        <v-card-text>
          <p class="header3" style="white-space: pre-wrap">
            {{ userMessage }}
          </p>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn @click="closeMessage"> OK </v-btn>
          <v-spacer />
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="confirm" max-width="600px">
      <v-card>
        <v-card-title class="text-body-1" />
        <v-card-text>
          <p class="header3" style="white-space: pre-wrap">
            {{ userMessage }}
          </p>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn @click="confirmOk"> OK </v-btn>
          <v-btn @click="confirmCancel"> Cancel </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="waitDialog" max-width="500px">
      <v-card>
        <v-card-title class="text-body-1" />
        <v-card-text class="header3">
          {{ waitMessage }}
        </v-card-text>
      </v-card>
    </v-dialog>

    <v-row justify="center">
      <div ref="line1" style="width: 800px; height: 300px; display: none" />
      <div ref="line2" style="width: 800px; height: 300px; display: none" />
      <div ref="line3" style="width: 800px; height: 300px; display: none" />
      <div ref="line4" style="width: 800px; height: 300px; display: none" />
      <div ref="line5" style="width: 800px; height: 300px; display: none" />
      <div ref="line6" style="width: 800px; height: 300px; display: none" />
      <div ref="line7" style="width: 800px; height: 300px; display: none" />
      <div ref="line8" style="width: 800px; height: 300px; display: none" />
      <div ref="line9" style="width: 800px; height: 300px; display: none" />
      <div ref="line10" style="width: 800px; height: 300px; display: none" />
      <div ref="histo1" style="width: 800px; height: 300px; display: none" />
      <div ref="histo2" style="width: 800px; height: 300px; display: none" />
      <div ref="histo3" style="width: 800px; height: 300px; display: none" />
      <div ref="histo4" style="width: 800px; height: 300px; display: none" />
      <div ref="histo5" style="width: 800px; height: 300px; display: none" />
      <div ref="histo6" style="width: 800px; height: 300px; display: none" />
      <div ref="histo7" style="width: 800px; height: 300px; display: none" />
      <div ref="histo8" style="width: 800px; height: 300px; display: none" />
      <div ref="histo9" style="width: 800px; height: 300px; display: none" />
      <div ref="histo10" style="width: 800px; height: 300px; display: none" />
    </v-row>
  </v-container>
</template>

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
import {
  lineChartOptions,
  sepLineChartOptions,
  histogramOptions,
  barChartOptions,
} from '../../utils/charts.js'
import { adjustData, adjustDates, getColName } from '../../utils/util.js'

import colors from '../../utils/colors.js'
import { EnergyReport } from '../../utils/entity.js'
import document from '../../utils/document.js'
import currentTable from '../../utils/currentTable.js'
import { getColumns, checkTemplate } from '../../utils/categories.js'

pdfMake.vfs = pdfFonts.pdfMake.vfs
echarts.use([MarkLineComponent])

const suffix = ' '

export default {
  name: 'EnergyReport',
  data: function () {
    return {
      tabs: null,
      reportTabs: null,
      reports: [],
      colors: colors(),
      version: '1.0',
      alert: false,
      confirm: false,
      choiceDialog: false,
      waitDialog: false,
      userMessage: '',
      waitMessage: '',
      dialog: false,
      reportDialog: {
        showReportInfo: false,
        showReportCover: false,
        newReport: false,
      },
      templateTableVisible: false,
      reportTableVisible: true,
      showNewReportDialog: false,
      showDeleteReportDialog: false,
      genReportDialog: false,
      reportProgress: 0,
      chartGenerator: null,
      inputfile: null,
      newReportDescription: '',
      newBlankReport: false,
      selectedTemplate: null,
      description: '',
      templateNameDialog: false,
      dateConfig: {
        altFormat: 'D, M j, Y',
        altInput: true,
        dateFormat: 'Y-m-d',
      },
      columnDefs: [
        { text: 'Num', value: 'rank', width: '15%' },
        { text: 'Title', value: 'title', width: '62%' },
        { text: 'Actions', value: 'actions', width: '10%', sortable: false },
      ],
      dateColumn: 'Date',
      timeColumn: 'Time',
      rowData: [],
      csvData: [],
      csvColumns: [],
      templateCols: [],
      minMax: {},
      templates: {},
      energyData: {
        prevTime: 0,
      },
      acumEnergyData: {
        prevTime: 0,
        acum: 0,
      },
      chartLegendOptions: ['use-param-name', 'use-column-name'],
      newReportName: '',
      selectedReport: -1,
      report: new EnergyReport(),
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
          reportAuthor: '',
        },
        page2: {
          include: false,
          title: '',
          subtitle: '',
          requirements: [
            { num: 0, section: '', remarks: '' },
            { num: 1, section: '', remarks: '' },
          ],
          additionalInfo: [
            { num: 2, section: '', remarks: '' },
            { num: 3, section: '', remarks: '' },
            { num: 4, section: '', remarks: '' },
            { num: 5, section: '', remarks: '' },
            { num: 6, section: '', remarks: '' },
            { num: 7, section: '', remarks: '' },
            { num: 8, section: '', remarks: '' },
            { num: 9, section: '', remarks: '' },
          ],
        },
      },
      template: {},
      editedIndex: -1,
      defaultRequirement: {
        num: 0,
        requirement: '',
        value1: '',
        value2: '',
        value3: '',
        result: '',
        include: false,
      },
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
            {
              num: 0,
              requirement: '',
              timePercent: '',
              ymin: '',
              ymax: '',
              value1: '',
              value2: '',
              value3: '',
              include: false,
            },
            {
              num: 1,
              requirement: '',
              timePercent: '',
              ymin: '',
              ymax: '',
              value1: '',
              value2: '',
              value3: '',
              include: false,
            },
          ],
        },
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
            {
              num: 0,
              requirement: '',
              value1: '',
              value2: '',
              value3: '',
              result: '',
              include: false,
            },
            {
              num: 1,
              requirement: '',
              value1: '',
              value2: '',
              value3: '',
              result: '',
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
      background: '',
      reportFinished: false,
      sortField: '',
      compareFunc: 'includes',
      sorted: {
        project: false,
        location: false,
        date: false,
      },
      descending: {
        project: true,
        location: true,
        date: true,
      },
    }
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? 'New Section' : 'Edit Section'
    },
    histogramColor() {
      // TODO: do not log in production mode
      return this.editedItem ? this.editedItem.histoColor : ''
    },
    templatesAsList() {
      return Object.keys(this.templates)
        .filter((key) => key !== 'model' && key !== 'reports')
        .map((key) => ({
          text: this.templates[key].description,
          value: parseInt(key),
        }))
    },
    reportName() {
      if (!this.reports || this.reports.length === 0) {
        return ''
      }
      return this.selectedReport === -1
        ? ''
        : this.reports[this.selectedReport].name
    },
  },
  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDelete()
    },
  },
  beforeCreate() {
    this.template = this.emptyTemplate
  },
  mounted() {
    this.initialize()
  },
  methods: {
    editTemplates() {
      this.templateTableVisible = true
      this.reportTableVisible = false
    },
    goBack() {
      this.templateTableVisible = false
      this.reportTableVisible = true
    },
    resetSortState() {
      this.sorted = {
        project: false,
        location: false,
        date: false,
      }
      this.descending = {
        project: true,
        location: true,
        date: true,
      }
    },
    sortIcon(field) {
      return this.sorted[field] && this.descending[field]
        ? 'mdi-arrow-down-thin'
        : 'mdi-arrow-up-thin'
    },
    sortById() {
      this.sortField = 'id'
      this.resetSortState()
      this.reports.sort((t1, t2) => t1.id - t2.id)
      this.$forceUpdate()
    },
    sortByField(field) {
      this.sortField = field
      const sorted = this.sorted[field]
      const descending = this.descending[field]
      this.resetSortState()
      if (sorted && descending) {
        this.descending[field] = false
      } else {
        this.descending[field] = true
      }
      this.sorted[field] = true
      let getValue
      if (field === 'date') {
        getValue = (row) => this.toIsoDate(row.dateCreated)
      } else if (field === 'template') {
        getValue = (row) =>
          this.templates &&
          Object.keys(this.templates).length > 0 &&
          row.template > 0
            ? this.templates[row.template].description
            : ''
      } else {
        getValue = (row) => row[field]
      }

      this.sortTable(field, getValue)
    },
    toIsoDate(date) {
      const a = date.split('/')
      return a[2] + '-' + a[1] + '-' + a[0]
    },
    sortTable(field, getValue) {
      this.reports.sort((r1, r2) => {
        let v1, v2
        if (this.descending[field]) {
          v1 = getValue(r1)
          v2 = getValue(r2)
        } else {
          v1 = getValue(r2)
          v2 = getValue(r1)
        }
        if (v1 < v2) {
          return -1
        } else if (v1 > v2) {
          return 1
        } else {
          return 0
        }
      })
    },
    getLastId() {
      let max = 0

      const keys = Object.keys(this.templates)
      keys.forEach(function (id) {
        if (!isNaN(id) && id > max) {
          max = id
        }
      })

      return (parseInt(max) + 1).toString()
    },
    // newTemplate() {
    //   this.inputfile = null;
    //   this.selectedTemplate = "";
    //   this.template = this.emptyTemplate;
    //   this.$forceUpdate();
    // },
    loadTemplate() {
      this.readTemplates().then((result) => {
        this.templates = result
        this.inputfile = null
        this.selectedTemplate = ''
        this.showLoadReport = true
        this.$forceUpdate()
      })
    },
    closeLoadReport() {
      this.showLoadReport = false
    },
    updateHistoColor() {
      this.$forceUpdate()
    },
    dataInterval() {
      const d1 = this.csvData[0].DateTime
      const d2 = this.csvData[1].DateTime
      const time = d2.getTime() - d1.getTime()
      return time / 60000
    },
    showMessage(msg) {
      this.userMessage = msg
      this.alert = true
    },
    closeMessage() {
      this.userMessage = ''
      this.alert = false
    },
    showWaitMessage(msg) {
      this.waitMessage = msg
      this.waitDialog = true
    },
    closeWaitMessage() {
      this.waitMessage = ''
      this.waitDialog = false
    },
    showConfirmMessage(msg, action) {
      this.userMessage = msg
      this.confirm = true
      this.confirmAction = action
    },
    confirmOk() {
      this.userMessage = ''
      this.confirm = false
      if (this.confirmAction) {
        this.confirmAction('ok')
      }
    },
    confirmCancel() {
      this.userMessage = ''
      this.confirm = false
      if (this.confirmAction) {
        this.confirmAction('cancel')
      }
    },
    endpoint(op) {
      // return `http://localhost:5000/api/energyreport/${op}`
      return `/energyreport/${op}`
    },
    readTemplates() {
      const self = this
      return new Promise(function (resolve) {
        self
          .$axios({
            url: self.endpoint('category'),
            method: 'GET',
            crossDomain: true,
            responseType: 'json',
          })
          .then((response) => {
            resolve(response.data)
          })
      })
    },
    initialize() {
      console.log('getTemplates')
      const self = this
      self
        .readTemplates()
        .then((response) => {
          self.templates = response
          for (let i = 0; i < 10; i++) {
            let chartRef = this.$refs['line' + (i + 1)]
            this.lineCharts[i] = echarts.init(chartRef, undefined, {
              width: 800,
              height: 380,
            })
            chartRef = this.$refs['histo' + (i + 1)]
            this.histograms[i] = echarts.init(chartRef, undefined, {
              width: 800,
              height: 380,
            })
          }
        })
        .then(self.getReports)
        .then(
          self.$axios.$get(this.endpoint('background')).then((response) => {
            self.background = response
          })
        )
    },
    getReports() {
      this.$axios({
        url: this.endpoint('report'),
        method: 'GET',
        crossDomain: true,
        responseType: 'json',
        transformResponse: (response) => {
          return JSON.parse(response, (key, value) => {
            if (key == 'dateCreated') {
              return new Intl.DateTimeFormat('en-SG').format(new Date(value))
            }
            return value
          })
        },
      })
        .then((response) => {
          this.reports = response.data
          if (this.reports.length === 0 && self.templates.reports) {
            self.migrateReports()
          }
        })
        .catch(function (error) {
          console.log(error)
        })
    },
    migrateReports() {
      for (let i = 0; i < this.templates.reports.length; i++) {
        const oldReport = this.templates.reports[i]
        const report = new EnergyReport({
          name: oldReport.name,
          location: oldReport.cover.siteLocation,
          template: oldReport.template,
          // dateCreated: oldReport.cover.reportDate,
          fileName: oldReport.fileName,
          circuit: oldReport.circuit,
          chartLegendOption: oldReport.chartLegendOption,
          // csvData: { columns: [], rows: [] },
          rawCsvData: [],
          cover: {
            title: oldReport.cover.title,
            client: oldReport.cover.client,
            separation: oldReport.cover.separation,
            reportAuthor: oldReport.cover.reportAuthor,
            instrumentUsed: oldReport.cover.instrumentUsed,
            serialNumber: oldReport.cover.serialNumber,
          },
        })
        this.reportDialog.newReport = true
        this.saveReport(report)
      }
    },
    checkSaveTemplates() {
      const templates = this.templates
      for (const key of Object.keys(templates)) {
        if (key === 'reports') {
          continue
        }
        const template = templates[key]

        const calcColumns = template.calcColumns
        for (const calcColumn of calcColumns) {
          for (const param of calcColumn.params) {
            param.col = param.col.replace(' ', '-').toUpperCase()
          }
        }

        const categories = template.categories
        for (const category of categories) {
          if (
            calcColumns.findIndex((c) => c.name === category.histogram) === -1
          ) {
            category.histogram = category.histogram
              .replace(' ', '-')
              .toUpperCase()
          }
          const mappings = category.mappings
          for (const mapping of mappings) {
            if (calcColumns.findIndex((c) => c.name === mapping.col) === -1) {
              mapping.col = mapping.col.replace(' ', '-').toUpperCase()
            }
          }
          if (category.text) {
            const requirements = category.text.requirements
            if (requirements) {
              for (const req of requirements) {
                if (req.value1) {
                  req.value1 = req.value1.replace(' ', '-').toUpperCase()
                }
                if (req.value2) {
                  req.value2 = req.value2.replace(' ', '-').toUpperCase()
                }
                if (req.value3) {
                  req.value3 = req.value3.replace(' ', '-').toUpperCase()
                }
              }
            }
          }
        }
      }
      this.saveTemplates(true)
    },
    saveTemplates(showMessage) {
      const configHeaders = {
        'content-type': 'text/plain',
        Accept: 'text/plain',
      }
      const str = JSON.stringify(this.templates)
      this.$axios({
        url: this.endpoint('category'),
        method: 'post',
        data: str,
        headers: configHeaders,
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
    column(data, colName) {
      const result = []
      for (let i = 0; i < data.length; i++) {
        result.push(data[i][colName])
      }
      return result
    },
    reportPeriod() {
      const startDate = this.csvData[0].DateTime
      const endDate = this.csvData[this.csvData.length - 1].DateTime
      const sd = {
        day: startDate.getDate(),
        month: startDate.getMonth() + 1,
        year: startDate.getFullYear(),
        hour: startDate.getHours(),
        min: startDate.getMinutes(),
      }
      const ed = {
        day: endDate.getDate(),
        month: endDate.getMonth() + 1,
        year: endDate.getFullYear(),
        hour: endDate.getHours(),
        min: endDate.getMinutes(),
      }
      return (
        `${sd.day}/${sd.month}/${sd.year} ${sd.hour}:${sd.min}hrs - ` +
        `${ed.day}/${ed.month}/${ed.year} ${ed.hour}:${ed.min}hrs`
      )
    },
    createLineChart(index) {
      const category = this.template.categories[index]
      const legendOpt = this.report.chartLegendOption
      const chart = this.lineCharts[index]
      const csvData = this.csvData
      const minMax = this.minMax

      if (category.separateCharts) {
        chart.setOption(
          sepLineChartOptions(
            csvData,
            minMax,
            category,
            suffix,
            legendOpt,
            this.template.useHyphen
          )
        )
      } else {
        chart.setOption(
          lineChartOptions(
            csvData,
            minMax,
            category,
            suffix,
            legendOpt,
            this.template.useHyphen
          )
        )
      }
      chart.resize()

      const promise = new Promise(function (resolve) {
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white',
          })
          // console.log("resolving line chart", index)
          resolve(image.src)
        })
      })

      return promise
    },
    histoParamName(category) {
      const mappings = category.mappings
      const idx = mappings.findIndex(
        (column) => column.col === category.histogram.toString()
      )
      return idx > -1 ? mappings[idx].param : mappings[idx].col
    },
    createHistogram(index) {
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
        console.log('No minMax for', colName)
        return
      }
      const limits = { min: colMinMax.min, max: colMinMax.max }

      const histoGen = d3
        .bin()
        .domain([limits.min, limits.max])
        .thresholds(numBins - 1)
      const bins = histoGen(data)

      const binCount = []
      const binValues = []
      // console.log("Section:", index)
      for (let i = 0; i < bins.length; i++) {
        binCount.push(bins[i].length)
        const value = limits.min + ((limits.max - limits.min) * i) / bins.length
        binValues.push(value.toFixed(2))
      }

      let label =
        this.report.chartLegendOption === 'use-param-name'
          ? this.histoParamName(category)
          : category.histogram
      label = category.yAxisName
        ? `${label} [${category.yAxisName}]`
        : `${label}`
      const options = histogramOptions(category, binValues, binCount, label)
      chart.setOption(options)
      chart.resize()

      const promise = new Promise(function (resolve) {
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white',
          })
          // console.log("resolving histogram", index)
          resolve(image.src)
        })
      })
      return promise
    },
    createBarChart(index) {
      const category = this.template.categories[index]
      const chart = this.histograms[index]
      const data = this.csvData

      const col = category.histogram

      const grp = {}
      const dtf = Intl.DateTimeFormat('en-SG', {
        month: 'numeric',
        day: 'numeric',
        year: 'numeric',
      })
      data.forEach((d) => {
        const dt = d.DateTime
        if (dt) {
          const date = dtf.format(dt)
          // console.log(date, d[col])
          grp[date] = grp[date] ? grp[date] + d[col] : d[col]
        }
      })

      const groups = []
      Object.keys(grp).forEach((key) =>
        groups.push({ day: key, value: grp[key] })
      )

      const options = barChartOptions(category, groups)
      chart.setOption(options)
      chart.resize()

      const promise = new Promise(function (resolve) {
        chart.on('finished', function () {
          const image = new Image()
          image.src = chart.getDataURL({
            pixelRation: 2,
            backgroundColor: 'white',
          })
          // console.log("resolving bar chart", index)
          resolve(image.src)
        })
      })
      return promise
    },
    doTestReport() {
      if (!this.template) {
        this.showMessage('No template loaded')
        return
      }
      if (Object.keys(this.template).length === 0) {
        this.showMessage('No template loaded')
        return
      }
      if (this.csvColumns.length === 0 || this.csvData.length === 0) {
        this.showMessage(
          'No data available. Please load an approriate CSV file'
        )
        return
      }
      this.genReport()
    },
    doReport(index) {
      if (this.report.template) {
        this.selectedTemplate = this.report.template
        this.template = this.templates[this.report.template]
      } else {
        this.showMessage(
          'No report template selected. You must select a report template'
        )
        return
      }
      // console.log("report.rawCsvData", this.report.rawCsvData);
      this.rawData = this.report.rawCsvData
      if (!this.rawData || this.rawData.length === 0) {
        this.showMessage(
          'No data available. Please load an approriate CSV file'
        )
        return
      }

      this.genReport()
    },
    updateProgress(value) {
      const progress = this.reportProgress + value
      this.reportProgress = progress < 100 ? progress : 100
      this.$refs.myBar.style.width = this.reportProgress + '%'
    },
    async genReport() {
      this.genReportDialog = true
      await new Promise((resolve) => setTimeout(resolve, 100))
      this.reportProgress = 0
      this.updateProgress(0)

      // Parse the data again to include new sections
      this.parseData(getColumns(this.template))

      if (this.csvColumns.length === 0 || this.csvData.length === 0) {
        this.showMessage(
          'No data available. Please load an approriate CSV file'
        )
        return
      }

      this.reportProgress = 10
      const promises = []
      for (let i = 0; i < this.template.categories.length; i++) {
        const lineChart = this.createLineChart(i)
        promises.push(lineChart)
        this.updateProgress(5)
        await new Promise((resolve) => setTimeout(resolve, 100))
        if (this.template.categories[i].dailyTotals) {
          const chart = this.createBarChart(i)
          promises.push(chart)
        } else {
          const chart = this.createHistogram(i)
          promises.push(chart)
        }
        this.updateProgress(5)
        await new Promise((resolve) => setTimeout(resolve, 100))
      }

      const charts = await Promise.all(promises)

      this.genReportDialog = false
      await new Promise((resolve) => setTimeout(resolve, 100))
      this.reportProgress = 0
      this.updateProgress(0)

      // console.log(JSON.stringify(this.template.categories));
      const doc = document(
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
      )
      const pdf = pdfMake.createPdf(doc)
      pdf.download(this.report.cover.title + '.pdf')
      // pdf.open();
      // console.log("PDF created");
    },
    genCurrentTable() {
      if (this.report.template) {
        this.selectedTemplate = this.report.template
        this.template = this.templates[this.report.template]
      } else {
        this.showMessage(
          'No report template selected. You must select a report template'
        )
        return
      }
      this.rawData = this.report.rawCsvData
      if (!this.rawData || this.rawData.length === 0) {
        this.showMessage(
          'No data available. Please load an approriate CSV file'
        )
        return
      }
      this.generateCurrentTable(this.report.circuit)
    },
    genTestCurrentTable() {
      if (!this.template) {
        this.showMessage('No template loaded')
        return
      }
      if (Object.keys(this.template).length === 0) {
        this.showMessage('No template loaded')
        return
      }
      if (this.csvColumns.length === 0 || this.csvData.length === 0) {
        this.showMessage(
          'No data available. Please load an approriate CSV file'
        )
        return
      }
      this.generateCurrentTable('TestCurrentTable')
    },
    generateCurrentTable(circuit) {
      const ct = this.createCurrentTable(circuit)
      const doc = currentTable([ct])
      const pdf = pdfMake.createPdf(doc)
      pdf.download(`Current table, ${ct.circuit}.pdf`)
    },
    createCurrentTable(circuit) {
      this.parseData(getColumns(this.template))
      if (this.csvColumns.length === 0 || this.csvData.length === 0) {
        this.showMessage(
          'No data available. Please load an approriate CSV file'
        )
        return
      }

      const catIndex = this.template.categories.findIndex(
        (cat) => cat.chartTitle === 'Current'
      )
      if (catIndex === -1) {
        return
      }
      const category = this.template.categories[catIndex]
      // ISO 8601: yyy-mm-dd
      const dtf = new Intl.DateTimeFormat('sv-SE', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
      })
      const dtf2 = Intl.DateTimeFormat('en-SG', {
        month: '2-digit',
        day: '2-digit',
        year: 'numeric',
      })

      const data = this.csvData

      const params = {}
      for (const mapping of category.mappings) {
        params[mapping.param] = mapping.col
      }
      const paramNames = Object.keys(params)

      const current = {}
      const currentTable = {
        id: 0,
        circuit,
        startDate: null,
        endDate: null,
        currentData: [],
      }
      for (const row of data) {
        const dt = row.DateTime
        if (dt) {
          const date = dtf.format(dt)

          if (!current[date]) {
            current[date] = { date: dtf2.format(dt) }
          }

          for (const param of paramNames) {
            current[date][param] = current[date][param]
              ? Math.max(current[date][param], row[params[param]])
              : row[params[param]]
          }
        }
      }

      const currentData = []
      for (const date of Object.keys(current).sort()) {
        if (!currentTable.startDate || date < currentTable.startDate) {
          currentTable.startDate = date
        }
        if (!currentTable.endDate || date > currentTable.endDate) {
          currentTable.endDate = date
        }
        currentData.push(current[date])
      }
      currentTable.currentData = JSON.stringify(currentData)

      return currentTable
    },
    dateTime(dateStr, timeStr) {
      // console.log(dateStr, timeStr)
      if (!dateStr || !timeStr) {
        return null
      }
      const da = dateStr.split(/[/.-]/)
      if (parseInt(da[2]) > 31) {
        const temp = da[0]
        da[0] = da[2]
        da[2] = temp
      }

      let ta = ''
      if (
        timeStr.includes('AM') ||
        timeStr.includes('am') ||
        timeStr.includes('PM') ||
        timeStr.includes('pm')
      ) {
        const timeArray = timeStr.split(' ')
        let ma = ''
        let ts = timeArray[0]
        if (timeArray.length == 1) {
          if (ts.includes('AM')) {
            ma = 'AM'
            ts = timeArray[0].replace('AM', '')
          } else if (ts.includes('am')) {
            ma = 'AM'
            ts = timeArray[0].replace('am', '')
          } else if (ts.includes('PM')) {
            ma = 'PM'
            ts = timeArray[0].replace('PM', '')
          } else if (ts.includes('pm')) {
            ma = 'PM'
            ts = timeArray[0].replace('pm', '')
          }
        } else {
          ts = timeArray[0]
          ma = timeArray[1].toUpperCase()
        }
        ta = ts.split(':')
        const hour = parseInt(ta[0])
        if (ma === 'AM' && hour === 12) {
          ta[0] = 0
        }
        if (ma === 'PM' && hour < 12) {
          ta[0] = parseInt(ta[0]) + 12
        }
      } else {
        ta = timeStr.split(':')
      }

      const dateTime = new Date(da[0], da[1] - 1, da[2], ta[0], ta[1], ta[2])
      // return dateTime.getTime();
      return dateTime
    },
    headerCols(rawData) {
      const rows = rawData.data
      let firstRow = 0
      for (let i = 0; i < rows.length; i++) {
        const row = rows[i]
        const dateIndex = row.findIndex((v) => v.toLowerCase() === 'date')
        const timeIndex = row.findIndex((v) => v.toLowerCase() === 'time')
        const numCols = row.reduce(
          (accum, value) => (accum += value.trim().length > 0 ? 1 : 0),
          0
        )
        if (dateIndex > -1 && timeIndex > -1 && numCols > 5) {
          if (firstRow > 0) {
            rawData.data.splice(0, firstRow)
          }
          const headerRow = rows[0].map((col) => getColName(col))
          rows[0] = headerRow
          return rows[0]
        }
        firstRow++
      }
      return []
    },
    clearAll() {
      this.template = self.emptyTemplate
      this.csvData = []
      this.csvColumns = []
    },
    updateMinMax(value, colName, index, minMax) {
      let colMinMax = minMax[colName]
      if (!colMinMax) {
        colMinMax = {
          colName,
          maxIndex: -1,
          min: Number.MAX_VALUE,
          max: -Number.MAX_VALUE,
        }
        minMax[colName] = colMinMax
      }
      if (!isNaN(value)) {
        if (value < colMinMax.min) {
          colMinMax.min = value
        }
        if (value > colMinMax.max) {
          colMinMax.maxIndex = index
          colMinMax.max = value
        }
      }
    },
    parseData: function (cols) {
      const self = this
      self.minMax = {}
      const calcColumns = self.template.calcColumns
      const text = Papa.unparse(this.rawData)
      const data = []
      let index = 0
      const csvData = Papa.parse(text, {
        header: true,
        dynamicTyping: true,
        delimiter: '',
        step: function (parsedRow) {
          const dt = self.dateTime(
            parsedRow.data[self.dateColumn],
            parsedRow.data[self.timeColumn]
          )
          if (!dt) {
            return
          }
          const row = { index, DateTime: dt }
          for (let i = 0; i < cols.length; i++) {
            const col = cols[i]
            let colName = col.name
            if (colName == 'Date' || colName == 'Time') {
              continue
            }
            let value = parseFloat(parsedRow.data[col.name])
            if (col.factor) {
              colName += suffix
              value *= col.factor
            }
            if (!row[colName]) {
              row[colName] = parseFloat(value)
              // update the min and max values for this column
              self.updateMinMax(value, colName, index, self.minMax)
            }
          }

          for (let i = 0; i < calcColumns.length; i++) {
            const column = calcColumns[i].name
            const params = calcColumns[i].params
            const value = self[column](column, row, index, params)
            if (!row[column]) {
              row[column] = parseFloat(value)
              // update the min and max values for this column
              self.updateMinMax(value, column, index, self.minMax)
            }
          }

          data.push(row)
          index++
        },
      })

      if (data.length == 0) {
        this.showMessage('No data rows found')
        return
      }

      const csvColumns = csvData.meta.fields
      self.csvColumns = csvColumns.filter(
        (entry) => entry.trim() != '' && entry != 'Date' && entry != 'Time'
      )
      self.csvData = data

      for (let i = 0; i < calcColumns.length; i++) {
        const column = calcColumns[i].name
        self.csvColumns.push(column)
      }

      self.showLoadReport = false
    },
    readFile: function (file, action) {
      if (!file) {
        return
      }
      this.showWaitMessage('Loading CSV file. Please Wait')
      const reader = new FileReader()
      const self = this
      reader.onload = function (e) {
        const text = e.target.result
        const rawData = Papa.parse(text)
        const csvCols = self.headerCols(rawData)
        if (csvCols.length == 0) {
          self.showMessage('File has no column definitions')
          return
        }

        self.rawData = rawData

        self.clearAll()
        self.template = self.templates[self.selectedTemplate]

        this.templateCols = getColumns(self.template)

        const missingColumns = checkTemplate(
          self.template,
          csvCols,
          this.templateCols
        )
        if (missingColumns.length) {
          const msg =
            'This CSV file is missing the following columns present in the template\n\n' +
            missingColumns.join(', ') +
            '\n\nYou should edit the template and select alternate columns or select a different file'
          self.showMessage(msg)
        }

        self.parseData(this.templateCols)
        self.closeWaitMessage()

        if (action) {
          action()
        }
      }
      reader.readAsText(file)
    },
    newItem() {
      if (
        this.csvColumns.length == 0 ||
        Object.keys(this.template).length == 0
      ) {
        this.showMessage(
          'No data available. Please load a template and CSV file'
        )
        return
      }

      this.tabs = 'Text'
      this.editedIndex = -1
      this.editedItem = JSON.parse(JSON.stringify(this.defaultItem))
      this.dialog = true
    },
    editItem(item) {
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

      this.dialog = true
    },
    addChartColumn() {
      const mappings = this.editedItem.mappings

      let num = 1
      if (mappings.length > 0) {
        const lastSeries = mappings[mappings.length - 1].series
        num = parseInt(lastSeries) + 1
      }
      this.editedItem.mappings.push({
        series: num.toString(),
        col: '',
        color: '',
      })
    },
    deleteChartColumn(index) {
      this.editedItem.mappings.splice(index, 1)
    },
    close() {
      this.dialog = false
      this.$nextTick(() => {
        this.editedIndex = -1
      })
    },
    save() {
      if (this.editedIndex > -1) {
        Object.assign(
          this.template.categories[this.editedIndex],
          this.editedItem
        )
      } else {
        this.template.categories.push(this.editedItem)
      }
      this.saveTemplates(false)
      this.close()
    },
    dragstartHandler(event, index) {
      const data = { section: index }
      event.dataTransfer.setData('application/json', JSON.stringify(data))
    },
    dragoverHandler(event) {
      event.preventDefault()
      event.dataTransfer.dropEffect = 'move'
    },
    dropHandler(event, index) {
      event.preventDefault()
      const data = JSON.parse(event.dataTransfer.getData('application/json'))
      const target = this.template.categories[data.section]
      this.template.categories.splice(data.section, 1)
      this.template.categories.splice(index, 0, target)
      this.saveTemplates(false)
    },
    openCurrentTableDialog() {
      this.currentTableDialog.startDate = ''
      this.currentTableDialog.endDate = ''
      this.currentTableDialog.show = true
    },
    closeCurrentTableDialog() {
      this.currentTableDialog.show = false
    },
    newReportDialog() {
      this.newReportName = ''
      this.showNewReportDialog = true
    },
    deleteReport(index) {
      this.showConfirmMessage(
        `Are you sure you want to delete report: ${this.reports[index].name}`,
        (result) => {
          if (result === 'ok') {
            ;(async () => {
              const self = this
              const id = this.reports[index].id
              this.$axios({
                url: self.endpoint('report/' + id),
                method: 'DELETE',
                crossDomain: true,
              })
                .then(function (response) {
                  self.getReports()
                })
                .catch(function (error) {
                  console.log(error)
                  self.showMessage(
                    'There was a server error when loading the report'
                  )
                })
            })()
          }
        }
      )
    },
    updateReportTemplate() {
      this.report.template = this.selectedTemplate
    },
    readCSVFile(file) {
      this.report.fileName = file.name
      this.selectedTemplate = this.report.template
      this.readFile(file, () => {
        this.report.rawCsvData = this.rawData
      })
    },
    newReport() {
      this.report = new EnergyReport()
      this.report.chartLegendOption = 'use-param-name'
      this.openReportDialog(true)
    },
    editReport(index) {
      this.selectedReport = index
      this.getReport(this.reports[index].id, this.openReportDialog)
    },
    editCover(index) {
      this.selectedReport = index
      this.getReport(this.reports[index].id, this.openCoverDialog)
    },
    cancelReportDialog() {
      this.reportDialog.showReportInfo = false
      this.$forceUpdate()
    },
    cancelCoverDialog() {
      this.reportDialog.showReportCover = false
      this.$forceUpdate()
    },
    openReportDialog(isNewReport) {
      this.reportDialog.newReport = !!isNewReport
      this.reportDialog.showReportInfo = true
      this.reportDialog.showReportCover = false
    },
    openCoverDialog() {
      this.reportDialog.newReport = false
      this.reportDialog.showReportInfo = false
      this.reportDialog.showReportCover = true
    },
    getReport(id, action, param) {
      ;(async () => {
        const self = this
        this.$axios({
          url: self.endpoint('report/' + id),
          method: 'GET',
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse,
        })
          .then(function (response) {
            self.report = new EnergyReport(response.data)
            if (action) {
              action(param)
            }
          })
          .catch(function (error) {
            console.log(error)
            self.showMessage('There was a server error when loading the report')
          })
      })()
    },
    parseResponse(response) {
      const report = JSON.parse(response, (key, value) => {
        if (key == 'dateCreated') {
          return new Date(value)
        } else if (key == 'rawCsvData') {
          return JSON.parse(value)
        }
        return value
      })
      return report
    },
    saveReport(report, showMessage) {
      ;(async () => {
        const self = this
        this.$axios({
          url: self.endpoint('report'),
          method: this.reportDialog.newReport ? 'POST' : 'PUT',
          data: report,
          crossDomain: true,
          responseType: 'json',
          transformResponse: this.parseResponse,
        })
          .then(function (response) {
            self.getReports()
            self.reportDialog.showReportInfo = false
            self.reportDialog.showReportCover = false
          })
          .catch(function (error) {
            console.log(error)
            self.showMessage('There was a server error when saving the report')
          })
      })()
    },
    saveCover() {
      this.saveTemplates()
      this.$forceUpdate()
      this.showReportCover = false
    },
    loadReport() {
      const reader = new FileReader()
      const self = this
      reader.onload = function (e) {
        console.log('Loading report')
        const str = e.target.result
        const report = JSON.stringify(str)
        this.selectedTemplate = report.template
        this.report.cover = report.cover
      }
      reader.readAsText(file)
    },
    editTemplateName() {
      this.description = this.template.description
      this.templateNameDialog = true
    },
    saveTemplateName() {
      if (this.description !== this.template.description) {
        console.log('saving template name')
        this.template.description = this.description
        this.saveTemplates()
      }
      this.templateNameDialog = false
    },
    showCoverForm() {
      this.coverVisible = true
      this.reportVisible = false
    },
    showReportForm() {
      this.coverVisible = false
      this.reportVisible = true
    },
    requirementTableTitle(num) {
      const cat = this.template.categories[num]
      return cat ? cat.title : ''
    },
    openPage2() {
      if (
        this.csvColumns.length == 0 ||
        Object.keys(this.template).length == 0
      ) {
        this.showMessage(
          'No data available. Please load a template and CSV file'
        )
        return
      }
      if (this.template.page2) {
        this.editedPage2 = Object.assign({}, this.template.page2)
      } else {
        this.editedPage2 = Object.assign({}, this.emptyTemplate.page2)
      }
      this.showPage2 = true
    },
    savePage2() {
      this.template.page2 = Object.assign({}, this.editedPage2)
      this.saveTemplates(false)
      this.showPage2 = false
    },
    clearPage2() {
      this.editedPage2 = {
        include: false,
        title: '',
        subtitle: '',
      }
    },
    closePage2() {
      this.showPage2 = false
    },
    openCalcColumns() {
      if (
        this.csvColumns.length == 0 ||
        Object.keys(this.template).length == 0
      ) {
        this.showMessage(
          'No data available. Please load a template and CSV file'
        )
        return
      }
      this.showCalcColumns = true
    },
    closeCalcColumns() {
      this.showCalcColumns = false
      this.saveTemplates(false)
    },
    VoltageUnbalance(column, row, i, params) {
      const voltage = Array.from(params, (param) => row[param.col])
      const av = voltage.reduce((a, b) => a + b) / voltage.length
      const mx = Math.max(...voltage)
      const value = ((mx - av) / av) * 100
      return value
    },
    AcumEnergy(column, row, i, params) {
      if (i == 0) {
        this.acumEnergyData.prevTime = row.DateTime.getTime()
        this.acumEnergyData.acum = 0
        return 0
      }
      if (!row.DateTime) {
        return
      }

      const time = row.DateTime.getTime()
      const elapsedTime = time - this.acumEnergyData.prevTime
      const hours = elapsedTime / 3600000

      const energy = (row[params[0].col] * hours) / 1000
      this.acumEnergyData.acum += energy
      this.acumEnergyData.prevTime = time

      return this.acumEnergyData.acum
    },
    Energy(column, row, i, params) {
      if (i == 0) {
        this.energyData.prevTime = row.DateTime.getTime()
        return 0
      }
      if (!row.DateTime) {
        return
      }
      const time = row.DateTime.getTime()
      const elapsedTime = (time - this.energyData.prevTime) / 3600000
      const energy = (row[params[0].col] * elapsedTime) / 1000
      this.energyData.prevTime = time
      return energy
    },
    MinPowerFactor(column, row, i, params) {
      const result = row[params[0].col] / row[params[1].col]
      return result
    },
    AvePowerFactor(column, row, i, params) {
      const result = row[params[0].col] / row[params[1].col]
      return result
    },
    MaxPowerFactor(column, row, i, params) {
      const result = row[params[0].col] / row[params[1].col]
      return result
    },
    MinApparentPower(column, row, i, params) {
      const result = row[params[0].col] / row[params[1].col] / 1000
      return result
    },
    AveApparentPower(column, row, i, params) {
      const result = row[params[0].col] / row[params[1].col] / 1000
      return result
    },
    MaxApparentPower(column, row, i, params) {
      const result = row[params[0].col] / row[params[1].col] / 1000
      return result
    },
    adjustCsvDates() {
      const startDate = new Date(2021, 7, 30, 13, 0, 0)
      const eData = adjustDates(
        this.csvData,
        startDate,
        this.template.calcColumns
      )
      const txt = Papa.unparse(eData)
      const blob = new Blob([txt], { type: 'text/plain;charset=utf-8' })
      saveAs(blob, 'date-adjusted-energy-data.csv')
    },
    expandCsvData() {
      const eData = adjustData(this.csvData, 376, this.template.calcColumns)
      const txt = Papa.unparse(eData)
      const blob = new Blob([txt], { type: 'text/plain;charset=utf-8' })
      saveAs(blob, 'date-adjusted-energy-data.csv')
    },
  },
}
</script>

// TODO: Migrar A typesscript, pasar la configuracion de echart a un plugin
<style scoped>
.simple-table {
  border-collapse: collapse;
  border-style: hidden;
}

.simple-table th, /* stylelint-disable-line no-descending-specificity */
.simple-table td {/* stylelint-disable-line no-descending-specificity */
  border: 1px solid lightgray;
  padding: 4px;
}

.v-input {
  font-size: 1em;
}

.report-dialog .v-tabs__content {/* stylelint-disable-line selector-class-pattern */
  height: 80vh;
}

#my-progress {
  width: 100%;
  background-color: lightgrey;
}

#my-bar {
  width: 1%;
  height: 30px;
  background-color: royalblue;
}

html {
  overflow-y: auto;
}

/* stylelint-disable no-descending-specificity */
.report-table,
td,
th {
  width: 100%;

  /* border: 1px solid black; */
}
/* stylelint-enable no-descending-specificity */

.report-table td,
.report-table th {
  padding: 5px;
}

.search-box {
  border: 1px solid lightgray;
}

.table-row {
  border-right: 1px solid black;
  border-left: 1px solid black;
  border-bottom: 1px solid black;
  padding: 5px;
}

.table-header {
  border-top: 1px solid black;
  border-left: 1px solid black;
  border-right: 1px solid black;
  border-bottom: 1px solid black;
}

.header-row {
  border-right: 1px solid black;
}

.invisible-scrollbar::-webkit-scrollbar {
  color: white;
}
</style>

// TODO: Migrar A typesscript, pasar la configuracion de echart a un plugin
<style scoped>
.simple-table {
  border-collapse: collapse;
  border-style: hidden;
}

.simple-table th, /* stylelint-disable-line no-descending-specificity */
td { /* stylelint-disable-line no-descending-specificity */
  border: 1px solid lightgray;
  padding: 4px;
}

.v-input {
  font-size: 1em;
}

.report-dialog .v-tabs__content {/* stylelint-disable-line selector-class-pattern */
  height: 80vh;
}
</style>
