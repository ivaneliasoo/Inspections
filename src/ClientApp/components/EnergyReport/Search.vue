<template>
    <div width="width">
        <input class="text-body-1 text-left"
            type="search" :size="size"
            :placeholder="placeHolder"
            v-model="searchValue" 
            @input="search"
            style="border: none; border-color: transparent; outline: none;">
        <v-icon @click="findPrevious">
            mdi-chevron-up
        </v-icon>                                        
        <v-icon @click="findNext">
            mdi-chevron-down
        </v-icon>                                        
    </div>
</template>

<style scoped>
</style>

<script>
import { toIsoDate } from '~/composables/jp_util.js';
  import { Section, Item, CostSheet } 
    from '../../composables/costsheets/entity.js';
  import { endpoint } from '../../composables/costsheets/util.js'

export default {
    props: {
        data: Array,
        templates: Object,
        fieldNames: Array,
        placeHolder: String,
        size: Number,
        compareFunc: String
    },
    data: () => ({
        // fieldName: "project",
        searchValue: "",
        searchIndex: -1,
        curRow: null
    }),
    methods: {
        startsWith(dataValue, searchValue) {
            return dataValue.startsWith(searchValue);
        },
        includes(dataValue, searchValue) {
            if (!dataValue || typeof dataValue !== 'string') {
                return false;
            }
            return dataValue.toLowerCase().includes(searchValue.toLowerCase());
        },
        search() {
            const compareFunc = this.compareFunc;
            this.searchIndex = -1;
            for (let i=0; i<this.data.length; i++) {
                for (const field of this.fieldNames) {
                    const dataValue = (field === "template") 
                        ? this.templates[''+this.data[i].template].description
                        : this.data[i][field];
                    if (this[compareFunc](dataValue, this.searchValue)) {
                        this.scrollTo(i);
                        this.searchIndex = i;
                        return;
                    }
                }
            };
        },
        findNext() {
            if (this.searchIndex === -1) {
                return;
            }
            const compareFunc = this.compareFunc;
            for (let i=this.searchIndex+1; i<this.data.length; i++) {
                for (const field of this.fieldNames) {
                    const dataValue = (field === "template") 
                        ? this.templates[''+this.data[i].template].description
                        : this.data[i][field];
                     if (this[compareFunc](dataValue, this.searchValue)) {
                        this.scrollTo(i);
                        this.searchIndex = i;
                        return;
                    }
                }
            };
        },
        findPrevious() {
            if (this.searchIndex === -1) {
                return;
            }
            const compareFunc = this.compareFunc;
            for (let i=this.searchIndex-1; i>=0; i--) {
                for (const field of this.fieldNames) {
                    const dataValue = (field === "template") 
                        ? this.templates[''+this.data[i].template].description
                        : this.data[i][field];
                     if (this[compareFunc](dataValue, this.searchValue)) {
                        this.scrollTo(i);
                        this.searchIndex = i;
                        return;
                    }
                }
            };
        },
        scrollTo(line) {
            var rows = document.querySelectorAll('#reports tr');
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