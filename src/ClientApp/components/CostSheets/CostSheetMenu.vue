<template>
  <div>
    <v-menu
      right
      bottom
      fixed
    >
      <template #activator="{ on, attrs }">
        <v-btn
          icon
          v-bind="attrs"
          v-on="on"
        >
          <v-icon>mdi-menu</v-icon>
        </v-btn>
      </template>
      <v-list class="menu">
        <v-list-item>
          <v-list-item-title @click="$emit('show-cost-sheets')">
            Cost Sheets
          </v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title @click="$emit('show-templates')">
            Templates
          </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </div>
</template>
<script>
import { defineComponent } from '@vue/composition-api'

export default defineComponent({
  setup () {

  },
})
</script>
