<template>
  <div>
    <v-menu
      left
      bottom
      fixed
    >
      <template #activator="{ on, attrs }">
        <v-btn
          icon
          x-small
          v-bind="attrs"
          v-on="on"
        >
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </template>
      <v-list dense class="menu">
        <v-list-item>
          <v-list-item-title @click="$emit('add-section-t', 0, 'above')">
            Add section below
          </v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title @click="$emit('add-section', 0, 'above')">
            Select from template
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
