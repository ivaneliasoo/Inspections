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
        <v-col cols="8" class="my-0 py-0 text-left text-h5">
          Templates
        </v-col>
        <v-col cols="3" class="my-0 py-0 text-center">
          <v-btn small @click="newTemplate">
              New template
          </v-btn>
        </v-col>
      </v-row>
      <v-row>
          <v-col cols="12">
            <div style="height: 80vh; width: 800px; overflow-y: auto;">
              <table
                id="templates"
                class="costsheet-table">
                  <thead>
                      <tr>
                        <th class="font-weight-bold" style="width:68%;">Template Name</th>
                        <th class="font-weight-bold" style="width:20%;">Date created</th>
                        <th class="font-weight-bold" style="width:12%;">Actions</th>
                      </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(sheet, index) in templates" :key="index" style="height: 0px;">
                      <td class="text-caption text-left" style="width:34%;">
                        {{sheet.project}}
                      </td>
                      <!--td class="text-caption text-left" style="width:34%;">
                        {{sheet.location}}
                      </td-->
                      <td class="text-caption text-center" style="width:20%;">
                        {{sheet.dateCreated}}
                      </td>
                      <td class="text-caption text-center" style="width:12%;">
                        <v-btn icon small @click="editTemplate(index)">
                          <v-icon>
                            mdi-pencil
                          </v-icon>
                        </v-btn>
                        <v-btn icon small @click="deleteTemplate(index)">
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
  border: 1px solid black;
}

.costsheet-table td, .costsheet-table th {
  padding: 5px 5px 5px 5px;
}
</style>

<script>
  import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';
  import { endpoint } from '../../composables/costsheets/util.js'

  export default {
    props: {
      templates: Array
    },
    // data: () => ({
    // }),
    methods: {
      showCostSheets() {
        this.$emit('show-cost-sheets');
      },
      showTemplates() {
        this.$emit('show-templates');
      },
      editTemplate(index) {
        this.$emit('open-template', index, "editTemplate");
      },
      newTemplate() {
        this.$emit('new-template');
      },
      deleteTemplate(index) {
        this.$emit('delete-template', index);
      },
    }
  }
</script>