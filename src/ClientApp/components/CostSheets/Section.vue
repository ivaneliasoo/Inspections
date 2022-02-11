<template>
    <tbody class="cs-section">
      <tr>
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
          <input
            class="text-caption"
            type="text"
            v-model="section.materialMarkup"
          />
        </td>
        <td class="col-group-2">
        </td>
        <td class="col-group-1">
        </td>
        <td class="col-group-1">
        </td>
        <td class="text-caption col-group-1">
          {{summation}}
        </td>
        <td class="text-caption col-group-1">
          {{finalMarkup}}
        </td>
        <td class="text-caption col-group-1">
          {{toQuotePrice}}
        </td>
      </tr>
      <tr v-for="(item, index) in section.items" :key="index"
          @contextmenu="showContextMenu($event, index)">
        <td class="col-group-1">
          <input v-if="index==0"
            class="text-caption table-input"
            type="text"
            v-model="section.subSection"
          >
          <div v-else>
            <span>&nbsp;</span>
          </div> 
        </td>
        <td class="col-group-3">
          <input
            class="text-caption table-input"
            type="text"
            v-model="item.description"
          >
        </td>
        <td class="col-group-2">
          <input
            class="text-caption table-input"
            type="text"
            v-model="item.noCables"
          />
        </td>
        <td class="col-group-2">
          <input
            class="text-caption table-input"
            type="text"
            v-model="item.unitCost"
          />
        </td>
        <td class="col-group-2">
          <input
            class="text-caption table-input"
            type="text"
            v-model="item.units"
          />
        </td>
        <td class="col-group-4 text-caption">
            {{ item.materialCost() }}
        </td>
        <td class="col-group-1  text-caption">
          {{ item.materialMarkup }}
        </td>
        <td class="col-group-2">
          <input
            class="text-caption table-input"
            type="text"
            v-model="item.labourCostUnit"
          />
        </td>
        <td class="col-group-1 text-caption" style="width: 100px;">
          {{ item.labourCost() }}
        </td>
        <td class="col-group-1 text-caption" style="width: 100px;">
          {{ item.workPrice() }}
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
          <v-list>
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
    materialMarkup: Number,
    finalMarkup: Number
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
    }
  },
};
</script>
