<template>
    <v-row>
      <v-col>
        <div
          v-for="checklistItem in checkList"
          :key="checklistItem.checkListId"
        >
          <div v-for="(check, index) in checklistItem.checks" :key="index">
            <check-item
              :item.sync="check"
              :index="`1.${index + 1}`"
              @update:item="enqueueCheckItem"
            />
          </div>
        </div>
      </v-col>
    </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
import { CheckList } from '~/types/CheckLists'

export default Vue.extend({
  props: {
     checkList: {
      required: true,
      type: Array as () => CheckList[]
    }
  }

})
</script>

<style scoped>

</style>