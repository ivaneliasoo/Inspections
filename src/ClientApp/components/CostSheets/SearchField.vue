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
        fieldName: String,
        placeHolder: String,
        size: Number,
        compareFunc: String
    },
    data: () => ({
        searchValue: "",
        searchIndex: -1,
        curRow: null
    }),
    methods: {
        startsWith(dataValue, searchValue) {
            return dataValue.startsWith(searchValue);
        },
        includes(dataValue, searchValue) {
            return dataValue.includes(searchValue);
        },
        search() {
            const compareFunc = this.compareFunc;
            this.searchIndex = -1;
            for (let i=0; i<this.data.length; i++) {
                if (this[compareFunc](this.data[i][this.fieldName], this.searchValue)) {
                    this.scrollTo(i);
                    this.searchIndex = i;
                    break;
                }
            };
        },
        findNext() {
            if (this.searchIndex === -1) {
                return;
            }
            const compareFunc = this.compareFunc;
            for (let i=this.searchIndex+1; i<this.data.length; i++) {
                if (this[compareFunc](this.data[i][this.fieldName], this.searchValue)) {
                    this.scrollTo(i);
                    this.searchIndex = i;
                    break;
                }
                if (i >= this.data.length-1) {
                    i=0;
                }
            };
        },
        findPrevious() {
            if (this.searchIndex === -1) {
                return;
            }
            const compareFunc = this.compareFunc;
            for (let i=this.searchIndex-1; i>=0; i--) {
                if (this[compareFunc](this.data[i][this.fieldName], this.searchValue)) {
                    this.scrollTo(i);
                    this.searchIndex = i;
                    break;
                }
                if (i <= 0) {
                    i=this.data.length-1;
                }
            };
        },
        scrollTo(line) {
            // console.log("scrollTo", line);
            this.$emit('scroll_to', line);
        }
    }
}
</script>