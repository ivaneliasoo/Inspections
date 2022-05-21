<template>
    <div>
      <v-dialog 
        v-model="selectSectionDialog" 
        @click:outside="close"
        max-width="840px">
        <v-card>
            <v-card-title class="text-body-1">
                Select Section/Item
            </v-card-title>
            <v-card-text>
                <div style="width: 800px;">
                    <table
                        width="98%">
                        <tbody>
                            <tr>
                                <searchField
                                    :data="searchData"
                                    fieldName="description"
                                    placeHolder="Search"
                                    :size=50
                                    compareFunc="includes"
                                    @scroll_to="scrollTo">
                                </searchField>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div style="width: 800px; overflow-y: scroll;">
                    <table width="100%">
                        <tbody>
                            <tr class="table-header">
                                <td class="font-weight-bold text-center header-row" style="width:20%;">Number</td>
                                <td class="font-weight-bold text-center header-row" style="width:60%;">Description</td>
                                <td class="font-weight-bold text-center header-row" style="width:20%;">
                                    <v-btn small plain color="white">
                                        Select
                                    </v-btn>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div  class="costsheet-table" style="height: 390px; width: 800px; overflow-y: scroll;">
                    <table width="100%" height="100%"
                        id="select-section">
                        <tbody v-for="(section, secIndex) in template.sections" :key="secIndex">
                            <tr>
                                <td class="text-caption text-left table-row" style="width:20%;">
                                    {{section.secNumber}}
                                </td>
                                <td class="text-caption text-left table-row" style="width:60%;">
                                    {{section.description}}
                                </td>
                                <td class="text-caption text-center table-row" style="width:20%;">
                                    <v-btn small @click="selectSection(secIndex)">
                                        Select
                                    </v-btn>
                                </td>
                            </tr>
                            <tr v-for="(item, itemIndex) in section.items" :key="itemIndex">
                                <td class="text-caption text-left table-row" style="width:20%;">
                                    {{item.itemNumber}}
                                </td>
                                <td class="text-caption text-left table-row" style="width:60%;">
                                    {{item.description}}
                                </td>
                                <td class="text-caption text-center table-row" style="width:20%;">
                                    <v-btn small @click="selectItem(secIndex, itemIndex)">
                                        Select
                                    </v-btn>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </v-card-text>
            <v-card-actions>
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
</style>

<script>
  import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';
  import { endpoint } from '../../composables/costsheets/util.js'

export default {
    props: {
        template: Object,
        selectSectionDialog: Boolean
    },
    data: () => ({
        searchString: "",
        curRow: null
    }),
    computed: {
        searchData() {
            const data = [];
            const template = this.template;
            for (let i=0; i<template.sections.length; i++) {
                const section = template.sections[i];
                data.push({
                    description: section.description,
                    secIndex: i,
                    itemIndex: -1
                });
                for (let j=0; j<section.items.length; j++) {
                    const item = section.items[j];
                    data.push({
                        description: item.description,
                        secIndex: i,
                        itemIndex: j
                    })
                }
            }
            return data;
        }
    },
    methods: {
        selectSection(secIndex) {
            this.$emit('select-section', secIndex);
        },
        selectItem(secIndex, itemIndex) {
            this.$emit('select-item', secIndex, itemIndex);
        },
        close() {
            this.$emit("close");
        },
        scrollTo(line) {
            console.log("scrollTo", line)
            var rows = document.querySelectorAll('#select-section tr');
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