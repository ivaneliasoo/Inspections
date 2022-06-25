<template>
    <div>
      <v-dialog 
        v-model="selectTemplateDialog"
        @click:outside="close" 
        max-width="840px">
        <v-card>
            <v-card-title class="text-body-1">
                Select Template
            </v-card-title>
            <v-card-text>
                <div style="width: 800px;">
                    <table
                        width="98%">
                        <tbody>
                            <tr>
                                <td style="width: 60%;">
                                    <searchField
                                        :data="templates"
                                        fieldName="project"
                                        placeHolder="Search name"
                                        :size=45
                                        :compareFunc="compareFunc"
                                        @scroll_to="scrollTo">
                                    </searchField>
                                </td>
                                <td style="width: 30%;">
                                    <searchField
                                        :data="templates"
                                        fieldName="dateCreated"
                                        placeHolder="Search date"
                                        :size=10
                                        :compareFunc="compareFunc"
                                        @scroll_to="scrollTo">
                                    </searchField>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div style="width: 800px; overflow-y: scroll;">
                    <table width="100%">
                        <tbody>
                            <tr class="table-header">
                                <td class="font-weight-bold text-center header-row" style="width:65%;"
                                    title="Click to sort by name"
                                    @click="sortByProject" >
                                    Template Name
                                    <v-icon small v-show="sortField==='project'">
                                        {{ sortIcon('project') }}
                                    </v-icon>
                                </td>
                                <td class="font-weight-bold text-center header-row" style="width:20%;"
                                    title="Click to sort by date"
                                    @click="sortByDate" >
                                    Date created
                                    <v-icon small v-show="sortField==='date'">
                                        {{ sortIcon('date') }}
                                    </v-icon>
                                </td>
                                <td class="font-weight-bold text-center header-row" style="width:15%;"
                                    title="Click for original sort order">
                                    <v-btn small plain color="white"
                                        @click="sortById">
                                        Select
                                    </v-btn>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div  class="costsheet-table" style="height: 390px; width: 800px; overflow-y: scroll;">
                    <table
                        width="100%" height="100%"
                        id="select-template">
                        <tbody>
                            <tr v-for="(sheet, index) in templates" :key="index" style="height: 0px;">
                                <td class="text-caption text-left table-row" style="width:65%;">
                                    {{sheet.project}}
                                </td>
                                <td class="text-caption text-center table-row" style="width:20%;">
                                    {{sheet.dateCreated}}
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
                    mandatory>
                    <v-radio
                        label="Row includes ..."
                        value="includes"
                    ></v-radio>
                    <v-radio
                        label="Row starts with ..."
                        value="startsWith"
                    ></v-radio>
                    </v-radio-group>
                <v-spacer></v-spacer>
                <v-btn @click="close">Close</v-btn>
            </v-card-actions>
        </v-card>
      </v-dialog>
    </div>
</template>

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

<script>
import { toIsoDate } from '~/composables/jp_util.js';
  import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';
  import { endpoint } from '../../composables/costsheets/util.js'

export default {
    props: {
        templates: Array,
        selectTemplateDialog: Boolean,
        action: String
    },
    data: () => ({
        templates_: null,
        sortField: "none",
        compareFunc: "includes",
        sortDescending: {
          project: false,
          date: false
        }
    }),
    watch: {
        selectTemplateDialog(oldValue, newValue) {
            if (newValue) {
                this.nameSearch = "";
                this.dateSearch = "";
                this.sortField = "none";
                if (this.curRow) {
                    this.scrollTo(0);
                    this.curRow.style.background = "white";
                    this.curRow = null;
                }
            }
        }
    },
    methods: {
        selectTemplate(index) {
            this.$emit('select-template', index, this.action);
        },
        close() {
            this.$emit("close");
        },
        sortById() {
            this.sortField = "id";
            this.templates.sort((t1, t2) => t1.id - t2.id);
        },
        sortIcon(field) {
            return this.sortDescending[field] ? "mdi-arrow-up-thin" : "mdi-arrow-down-thin"
        },
        sortByProject() {
            this.sortField = "project";
            this.sortDescending.project = !this.sortDescending.project;
            this.sortTable("project", (row) => row.project);
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
            console.log("field", field)
            this.templates.sort((r1, r2) => {
            let v1, v2;
            console.log(v1, v2);
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
            var rows = document.querySelectorAll('#select-template tr');
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