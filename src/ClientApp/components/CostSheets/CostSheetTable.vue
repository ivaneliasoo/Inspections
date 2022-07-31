<template>
  <div>
    <v-row>
      <v-col class="my-0 py-0 text-left" cols="1">
        <cost-sheet-menu
          @show-cost-sheets="showCostSheets"
          @show-templates="showTemplates"
        />
      </v-col>
      <v-col cols="5" class="my-0 py-0 text-left text-h5">
        Costing Sheets
      </v-col>
      <v-col cols="3" class="my-0 py-0 text-center">
        <v-btn small @click="newFromTemplate">
          New from template
        </v-btn>
      </v-col>
      <v-col cols="3" class="my-0 py-0 text-center">
        <v-btn small @click="newCostSheet">
          New cost sheet
        </v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <div style="width: 800px; overflow-y: scroll;" class="invisible-scrollbar">
          <table
            class="search-box"
            width="100%"
          >
            <tbody>
              <tr>
                <td colspan="2" class="search-box">
                  <searchField
                    :data="costSheets"
                    :field-names="['project', 'location', 'dateCreated']"
                    place-holder="Search"
                    :size="52"
                    :compare-func="compareFunc"
                    @scroll_to="scrollTo"
                  />
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div style="width: 800px; overflow-y: scroll;" class="invisible-scrollbar">
          <table width="100%">
            <tbody>
              <tr class="table-header">
                <td
                  class="font-weight-bold text-center header-row"
                  style="width:34%;"
                  title="Click to sort by project"
                  @click="sortByField('project')"
                >
                  Project
                  <v-icon>
                    {{ sortIcon('project') }}
                  </v-icon>
                </td>
                <td
                  class="font-weight-bold text-center header-row"
                  style="width:34%;"
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
                  style="width:20%;"
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
                  style="width:12%;"
                  title="Click for original sort order"
                  @click="sortById"
                />
              </tr>
            </tbody>
          </table>
        </div>
        <div style="height: 75vh; width: 800px; overflow-y: scroll;">
          <table
            id="cost-sheets"
            class="costsheet-table"
          >
            <tbody>
              <tr v-for="(sheet, index) in costSheets" :key="index" style="height: 0px;">
                <td class="text-caption text-left table-row" style="width:34%;">
                  {{ sheet.project }}
                </td>
                <td class="text-caption text-left table-row" style="width:34%;">
                  {{ sheet.location }}
                </td>
                <td class="text-caption text-center table-row" style="width:20%;">
                  {{ sheet.dateCreated }}
                </td>
                <td class="text-caption text-center table-row" style="width:12%;">
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
</template>

<script>
import { Section, Item, CostSheet }
  from '../../utils/costsheets/entity.js'
import { endpoint } from '../../utils/costsheets/util.js'

export default {
  props: {
    costSheets: Array,
    triggerUpdate: Number
  },
  data: () => ({
    sortField: '',
    compareFunc: 'includes',
    sorted: {
      project: false,
      location: false,
      date: false
    },
    descending: {
      project: true,
      location: true,
      date: true
    }
  }),
  methods: {
    resetSortState () {
      this.sorted = {
        project: false,
        location: false,
        date: false
      }
      this.descending = {
        project: true,
        location: true,
        date: true
      }
    },
    showCostSheets () {
      this.$emit('show-cost-sheets')
    },
    showTemplates () {
      this.$emit('show-templates')
    },
    editCostSheet (index) {
      this.$emit('edit-sheet', index)
    },
    newCostSheet () {
      this.$emit('new-sheet')
    },
    newFromTemplate () {
      this.$emit('new-from-template', 'loadTemplateAsSheet')
    },
    deleteCostSheet (index) {
      this.$emit('delete-sheet', index)
    },
    sortIcon (field) {
      return (this.sorted[field] && this.descending[field]) ? 'mdi-arrow-down-thin' : 'mdi-arrow-up-thin'
    },
    sortById () {
      this.sortField = 'id'
      this.resetSortState()
      this.costSheets.sort((t1, t2) => t1.id - t2.id)
      this.$forceUpdate()
    },
    sortByField (field) {
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
      const getValue = (field === 'date')
        ? row => this.toIsoDate(row.dateCreated)
        : row => row[field]
      this.sortTable(field, getValue)
    },
    toIsoDate (date) {
      const a = date.split('/')
      return a[2] + '-' + a[1] + '-' + a[0]
    },
    sortTable (field, getValue) {
      this.costSheets.sort((r1, r2) => {
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
    scrollTo (line) {
      const rows = document.querySelectorAll('#cost-sheets tr')
      const row = rows[line]
      row.scrollIntoView({
        behavior: 'auto',
        block: 'start'
      })
      if (this.curRow) {
        this.curRow.style.background = 'white'
      }
      row.style.background = 'lightgrey'
      this.curRow = row
    }
  }
}
</script>

<style scoped>
html {
  overflow-y: auto;
}

.costsheet-table, td, th {
  width: 100%;
  /* border: 1px solid black; */
}

.costsheet-table td, .costsheet-table th {
  padding: 5px 5px 5px 5px;
}

.search-box {
  border: 1px solid lightgray;
}

.table-row {
  border-right: 1px solid black;
  border-left: 1px solid black;
  border-bottom: 1px solid black;
  padding: 5px 5px 5px 5px;
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
