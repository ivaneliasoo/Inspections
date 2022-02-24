<template>
    <tbody class="cs-section">
      <tr @contextmenu="showContextMenu($event, -1)">
        <td class="col-group-1">
          <input
            class="text-caption table-input"
            type="text"
            v-model="section.secNumber"
          />
        </td>
        <td class="col-group-2" colspan="4">
          <input
            class="text-caption table-input"
            type="text"
            v-model="section.description"
          />
        </td>
        <td class="col-group-4">
        </td>
        <td class="col-group-1">
          <NumberField
            v-model="section.materialMarkup" format="percent" @change="$emit('update-sheet')">
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
            :value="section.summation()" format="currency" :readOnly="true">
          </NumberField>
        </td>
        <td class="text-caption col-group-1">
          <NumberField
            v-model="section.finalMarkup" format="percent" @change="$emit('update-sheet')">
          </NumberField>
        </td>
        <td class="text-caption col-group-1">
          <NumberField 
            :value="section.toQuotePrice()" format="currency" :readObly="true">
          </NumberField>
        </td>
      </tr>
      <tr v-for="(item, index) in section.items" :key="index"
          @contextmenu="showContextMenu($event, index)">
        <td class="col-group-1" style="padding-left: 25px;">
          <input
            class="text-caption table-input"
            type="text"
            v-model="item.itemNumber"
          >
        </td>
        <td class="col-group-3">
          <input
            class="text-caption table-input"
            type="text"
            v-model="item.description"
          >
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
          :close-on-content-click="true"
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
              <v-list-item-title @click="addSection('above')">
                Add section above
              </v-list-item-title>
            </v-list-item>
            <v-list-item>
              <v-list-item-title @click="addSection('below')">
                Add section below
              </v-list-item-title>
            </v-list-item>
            <v-list-item>
              <v-list-item-title @click="delSection">
                Delete section
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
    triggerUpdate: Number
  },
  data: () => ({
    selected: null,
    contextMenuVisible: false,
    rowIndex: 0,
    xcoord: 0,
    ycoord: 0
  }),
  computed: {
    toQuotePrice() {
      return 0;
    },
    summation() {
      return 0;
    }
  },
  watch: { 
    triggerUpdate(newVal, oldVal) {
      this.$forceUpdate();
    }
  },
  updated() {
  },
  methods: {
    showContextMenu(e, index) {
      e.preventDefault();
      this.xcoord = e.clientX
      this.ycoord = e.clientY
      this.rowIndex = index;
      this.contextMenuVisible = true;
    },
    addItem() {
      if (this.rowIndex == this.section.items.length-1) {
        this.section.items.push(new Item());
      } else {
        this.section.items.splice(this.rowIndex+1, 0, new Item());
      }
    },
    delItem() {
      this.section.items.splice(this.rowIndex, 1,);
    },
    addSection(position) {
      this.$emit('add-section', this.index, position);
    },
    delSection() {
      this.$emit('del-section', this.index);
    }
  },
};
</script>
