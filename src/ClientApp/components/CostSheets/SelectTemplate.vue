<template>
  <div>
    <v-dialog
      v-model="selectTemplateDialog"
      max-width="840px"
      @click:outside="close"
    >
      <v-card>
        <v-card-title class="text-body-1">
          Select Template
        </v-card-title>
        <v-card-text>
          <div style="width: 800px;">
            <table
              width="98%"
            >
              <tbody>
                <tr>
                  <td colspan="2" class="search-box">
                    <searchField
                      :data="templates"
                      :field-names="['project', 'dateCreated']"
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
          <div style="width: 800px; overflow-y: scroll;">
            <table width="100%">
              <tbody>
                <tr class="table-header">
                  <td
                    class="font-weight-bold text-center header-row"
                    style="width:65%;"
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
                    style="width:15%;"
                    title="Click for original sort order"
                    @click="sortById"
                  />
                </tr>
              </tbody>
            </table>
          </div>
          <div class="costsheet-table" style="height: 390px; width: 800px; overflow-y: scroll;">
            <table
              id="select-template"
              width="100%"
              height="100%"
            >
              <tbody>
                <tr v-for="(sheet, index) in templates" :key="index" style="height: 0px;">
                  <td class="text-caption text-left table-row" style="width:65%;">
                    {{ sheet.project }}
                  </td>
                  <td class="text-caption text-center table-row" style="width:20%;">
                    {{ sheet.dateCreated }}
                  </td>
                  <td class="text-caption text-center table-row" style="width:15%;">
                    <v-btn small @click="selectTemplate(index)">
                      Select
                    </v-btn>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </v-card-text>
        <v-card-actions>
          Search mode: &nbsp;&nbsp;
          <v-radio-group
            v-model="compareFunc"
            row
            mandatory
          >
            <v-radio
              label="Row includes ..."
              value="includes"
            />
            <v-radio
              label="Row starts with ..."
              value="startsWith"
            />
          </v-radio-group>
          <v-spacer />
          <v-btn @click="close">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { Section, Item, CostSheet }
  from '../../composables/costsheets/entity.js'
import { endpoint } from '../../composables/costsheets/util.js'
import { toIsoDate } from '~/composables/jp_util.js'

export default {
  props: {
    templates: Array,
    selectTemplateDialog: Boolean,
    action: String
  },
  data: () => ({
    templates_: null,
    sortField: 'none',
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
  watch: {
    selectTemplateDialog (oldValue, newValue) {
      if (newValue) {
        this.nameSearch = ''
        this.dateSearch = ''
        this.sortField = 'none'
        if (this.curRow) {
          this.scrollTo(0)
          this.curRow.style.background = 'white'
          this.curRow = null
        }
      }
    }
  },
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
    selectTemplate (index) {
      this.$emit('select-template', index, this.action)
    },
    close () {
      this.$emit('close')
    },
    sortIcon (field) {
      return (this.sorted[field] && this.descending[field]) ? 'mdi-arrow-down-thin' : 'mdi-arrow-up-thin'
    },
    sortById () {
      this.sortField = 'id'
      this.resetSortState()
      this.templates.sort((t1, t2) => t1.id - t2.id)
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
      this.templates.sort((r1, r2) => {
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
      const rows = document.querySelectorAll('#select-template tr')
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

.table-header {
  border-top: 1px solid black;
  border-left: 1px solid black;
  border-right: 1px solid black;
}

.header-row {
  border-right: 1px solid black;
}

.costsheet-table {
  border: 1px solid black;
}

.table-row {
  border-right: 1px solid black;
  border-bottom: 1px solid black;
  padding: 5px 5px 5px 5px;
}

.invisible-scrollbar::-webkit-scrollbar {
  display: none;
}
</style>
