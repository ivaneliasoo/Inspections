<template>
    <div>
      <v-row>
        <v-col class="my-0 py-0 text-left" cols=1>
          <cost-sheet-menu
            @show-cost-sheets="showCostSheets"
            @show-templates="showTemplates"
          >
          </cost-sheet-menu>
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
                    width="100%">
                    <tbody>
                        <tr>
                            <td style="width: 34%;" class="search-box">
                                <searchField
                                    :data="costSheets"
                                    fieldName="project"
                                    placeHolder="Search project"
                                    :size=20
                                    :compareFunc="compareFunc"
                                    @scroll_to="scrollTo">
                                </searchField>
                            </td>
                            <td style="width: 34%;" class="search-box">
                                <searchField
                                    :data="costSheets"
                                    fieldName="location"
                                    placeHolder="Search location"
                                    :size=20
                                    :compareFunc="compareFunc"
                                    @scroll_to="scrollTo">
                                </searchField>
                            </td>
                            <td style="width: 32%;" class="search-box">
                                <searchField
                                    :data="costSheets"
                                    fieldName="dateCreated"
                                    placeHolder="Search date"
                                    :size=12
                                    :compareFunc="compareFunc"
                                    @scroll_to="scrollTo">
                                </searchField>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="width: 800px; overflow-y: scroll;"  class="invisible-scrollbar">
                <table width="100%">
                    <tbody>
                        <tr class="table-header">
                            <td class="font-weight-bold text-center header-row" style="width:34%;"
                                title="Click to sort by project"
                                @click="sortByProject" >
                                Project
                                <v-icon v-show="sortField==='project'">
                                    {{ sortIcon('project') }}
                                </v-icon>
                            </td>
                            <td class="font-weight-bold text-center header-row" style="width:34%;"
                                title="Click to sort by location"
                                @click="sortByLocation" >
                                Location
                                <v-icon v-show="sortField==='location'">
                                    {{ sortIcon('location') }}
                                </v-icon>
                            </td>
                            <td class="font-weight-bold text-center header-row" style="width:20%;"
                                title="Click to sort by date"
                                @click="sortByDate" >
                                Date created
                                <v-icon v-show="sortField==='date'">
                                    {{ sortIcon('date') }}
                                </v-icon>
                            </td>
                            <td class="font-weight-bold text-center header-row" style="width:12%;"
                                title="Click for original sort order"
                                @click="sortById">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div style="height: 75vh; width: 800px; overflow-y: scroll;">
                <table
                id="cost-sheets"
                class="costsheet-table">
                    <tbody>
                    <tr v-for="(sheet, index) in costSheets" :key="index" style="height: 0px;">
                        <td class="text-caption text-left table-row" style="width:34%;">
                        {{sheet.project}}
                        </td>
                        <td class="text-caption text-left table-row" style="width:34%;">
                        {{sheet.location}}
                        </td>
                        <td class="text-caption text-center table-row" style="width:20%;">
                        {{sheet.dateCreated}}
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

<script>
  import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';
  import { endpoint } from '../../composables/costsheets/util.js'

  export default {
    props: {
      costSheets: Array,
      triggerUpdate: Number
    },
    data: () => ({
        sortField: "",
        compareFunc: "includes",
        sortDescending: {
          project: false,
          location: false,
          date: false
        }
    }),
    methods: {
      showCostSheets() {
        this.$emit('show-cost-sheets');
      },
      showTemplates() {
        this.$emit('show-templates');
      },
      editCostSheet(index) {
        this.$emit('edit-sheet', index);
      },
      newCostSheet() {
        this.$emit('new-sheet');
      },
      newFromTemplate() {
        this.$emit('new-from-template', "loadTemplateAsSheet")
      },
      deleteCostSheet(index) {
        this.$emit('delete-sheet', index);
      },
      sortIcon(field) {
        return this.sortDescending[field] ? "mdi-arrow-up-thin" : "mdi-arrow-down-thin"
      },
      sortById() {
        this.sortField = "id";
        this.costSheets.sort((t1, t2) => t1.id - t2.id);
      },
      sortByProject() {
        this.sortField = "project";
        this.sortDescending.project = !this.sortDescending.project;
        this.sortTable("project", (row) => row.project);
      },
      sortByLocation() {
        this.sortField = "location";
        this.sortDescending.location = !this.sortDescending.location;
        this.sortTable("location", (row) => row.location);
      },
      toIsoDate(date) {
        const a = date.split("/");
        return a[2]+'-'+a[1]+'-'+a[0];
      },
      sortByDate() {
        this.sortField = "date";
        this.sortDescending.date = !this.sortDescending.date;
        this.sortTable("date", (row) => this.toIsoDate(row.dateCreated));
      },
      sortTable(field, getValue) {
        this.costSheets.sort((r1, r2) => {
          let v1, v2;
          if (this.sortDescending[field]) {
            v1 = getValue(r1);
            v2 = getValue(r2);
          } else {
            v1 = getValue(r2);
            v2 = getValue(r1);
          }
          if (v1 < v2) {
              return -1;
          } else if (v1 > v2) {
              return 1;
          } else {
              return 0;
          }
        })
      },
      scrollTo(line) {
          var rows = document.querySelectorAll('#cost-sheets tr');
          const row = rows[line];
          row.scrollIntoView({
              behavior: 'auto',
              block: 'start'
          });
          if (this.curRow) {
              this.curRow.style.background = "white";
          }
          row.style.background = "lightgrey";
          this.curRow = row;
      }
    }
  }
</script>