<template>
    <tbody ref="body" class="cs-section">
      <tr @contextmenu="showContextMenu($event, -1)">
        <td class="col-group-1">
          <TextField 
            v-model="section.secNumber" :col="0">
          </TextField>
        </td>
        <td class="col-group-2" colspan="4">
          <TextField class="font-weight-bold table-input"
            v-model="section.description" :col="1">
          </TextField>
        </td>
        <td class="col-group-4">
        </td>
        <td class="col-group-1">
          <NumberField
            v-model="section.materialMarkup" format="percent" @change="updateMaterialMarkup" :col="6">
          </NumberField>
        </td>
        <td class="col-group-2">
        </td>
        <td class="col-group-1">
        </td>
        <td class="col-group-1">
        </td>
        <td class="text-caption col-group-1">
          <NumberField 
            :value="section.summation()" format="currency" :readOnly="true" :col="10">
          </NumberField>
        </td>
        <td class="text-caption col-group-1">
          <NumberField
            v-model="section.finalMarkup" format="percent" @change="$emit('update-sheet')" :col="11">
          </NumberField>
        </td>
        <td class="text-caption col-group-1">
          <NumberField 
            :value="section.toQuotePrice()" format="currency" :readObly="true" :col="12">
          </NumberField>
        </td>
      </tr>
      <tr v-for="(item, index) in section.items" :key="index"
          @contextmenu="showContextMenu($event, index)">
        <td class="col-group-1" style="padding-left: 25px;">
          <TextField
            v-model="item.itemNumber">
          </TextField>
        </td>
        <td class="col-group-3">
          <TextArea 
            v-model="item.description" @change="$emit('update-sheet')">
          </TextArea>
        </td>
        <td class="col-group-2">
          <NumberField
            v-model="item.noCables" @change="$emit('update-sheet')">
          </NumberField>
        </td>
        <td class="col-group-2">
          <NumberField
            v-model="item.unitCost" format="currency" @change="$emit('update-sheet')">
          </NumberField>
        </td>
        <td class="col-group-2">
          <NumberField
            v-model="item.units" @change="$emit('update-sheet')">
          </NumberField>
        </td>
        <td class="col-group-4 text-caption">
         <NumberField 
            :value="item.materialCost()" format="currency" :readOnly="true">
          </NumberField>
        </td>
        <td class="col-group-1  text-caption">
          <NumberField
            v-model="item.materialMarkup" format="percent" @change="$emit('update-sheet')">
          </NumberField>
        </td>
        <td class="col-group-2">
          <NumberField
            v-model="item.labourCostUnit" format="currency"  @change="$emit('update-sheet')">
          </NumberField>
        </td>
        <td class="col-group-1 text-caption text-align-right">
         <NumberField 
            :value="item.labourCost()" format="currency" :readOnly="true">
          </NumberField>
        </td>
        <td class="col-group-1 text-caption">
         <NumberField 
            :value="item.workPrice()" format="currency" :readOnly="true">
          </NumberField>
        </td>
        <td class="col-group-1 text-caption" style="width: 100px;">
        </td>
        <td class="col-group-1 text-caption" style="width: 100px;">
        </td>
        <td class="col-group-1 text-caption" style="width: 100px;">
        </td>
      </tr>
      <tr v-show=false>
        <v-menu
          v-model="contextMenuVisible"
          :close-on-content-click="false"
          :position-x="xcoord"
          :position-y="ycoord"
          absolute
          offset-y
        >
          <v-list dense>
            <v-list-item>
              <v-list-item-title @click="addItem">
                Add item
              </v-list-item-title>
            </v-list-item>
            <v-list-item>
              <v-list-item-title @click="delItem">
                Delete item
              </v-list-item-title>
            </v-list-item>
            <v-divider></v-divider>
            <v-list-item>
              <v-list-item-title @click="addSection('below')">
                Add section
              </v-list-item-title>
            </v-list-item>
            <v-list-item>
              <v-list-item-title @click="delSection">
                Delete section
              </v-list-item-title>
            </v-list-item>
            <v-divider></v-divider>
            <v-list-item v-if="template!==null">
              <v-list-item-title @click="addFromTemplate">
                Select from template
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </tr>
    </tbody>
</template>

<style scoped>
input {
  width: 100%;
}

.cs-section {
  /* display: table; */
  border: 1px solid black;
}

.col-group-1 {
  background-color: #eae4ef;
  border-right: 1px solid;
  padding: 2px;
}

.col-group-2 {
  background-color: #e1f5f7;
  padding: 2px;
}

.col-group-3 {
  background-color: #e1f5f7;
  border-right: 1px solid;
  padding: 2px;
}

.col-group-4 {
  background-color: #eae4ef;
  padding: 2px;
}
</style>

<script>
import { Section, Item } 
    from '../../composables/costsheets/entity.js';

export default {
  props: {
    section: Object,
    index: Number,
    template: Object,
    triggerUpdate: Number
  },
  data: () => ({
    selected: null,
    fromTemplate: false,
    contextMenuVisible: false,
    selectSectionDialog: false,
    rowIndex: 0,
    xcoord: 0,
    ycoord: 0,
    newSectionPosition: "after"
  }),
  watch: { 
    triggerUpdate(newVal, oldVal) {
      this.$forceUpdate();
    }
  },
  methods: {
    showContextMenu(e, index) {
      e.preventDefault();
      this.fromTemplate = false;
      this.xcoord = e.clientX
      this.ycoord = e.clientY
      this.rowIndex = index;
      this.contextMenuVisible = true;
    },
    closeContextMenu() {
      this.contextMenuVisible = false;
    },
    openSelectSection() {
      this.selectSectionDialog = true;
    },
    selectSection(newSectionIndex) {
      this.closeSelectSection();
      this.$emit('add-section', this.index, this.newSectionPosition, newSectionIndex);
    },
    selectItem(newSecIndex, newItemIndex) {
      this.closeSelectSection();
      this.insertItem(newSecIndex, newItemIndex);
    },
    closeSelectSection() {
      this.selectSectionDialog = false;
    },
    addItem() {
      this.closeContextMenu();
      this.insertItem(-1, -1);
    },
    insertItem(newSecIndex, newItemIndex) {
      let newItem;
      if (newItemIndex > -1) {
        newItem = this.template.sections[newSecIndex].items[newItemIndex];
      } else {
        newItem = new Item({materialMarkup: this.section.materialMarkup});
      }
      this.section.items.splice(this.rowIndex+1, 0, newItem);
      this.section.renumberItems();
    },
    delItem() {
      this.closeContextMenu();
      this.section.items.splice(this.rowIndex, 1);
      this.section.renumberItems();
    },
    addFromTemplate() {
      this.closeContextMenu();
      if (this.template == null) {
        this.selectSectionDialog = false;
        return;
      }
      this.open
      this.fromTemplate = true;
      this.newSectionPosition = 'below';
      this.openSelectSection();
    },
    addSection(position) {
      this.closeContextMenu();
      this.$emit('add-section', this.index, position, -1);
    },
    delSection() {
      this.closeContextMenu();
      this.$emit('del-section', this.index);
    },
    updateMaterialMarkup() {
      this.section.updateMaterialMarkup();
      this.$emit('update-sheet')
    }
  },
};
</script>
