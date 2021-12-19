<template>
  <div>
      <v-menu
        left
        bottom
        fixed
      >
        <template v-slot:activator="{ on, attrs }">
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
            <v-list-item-title @click="$emit('show-job-schedule')">
              Job schedule projection
            </v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="$emit('show-job-projection-overview')">
              Job projection overview
            </v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="$emit('show-man-power')">
              Manpower planning
            </v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="$emit('show-man-power-forecast')">
              Manpower forecast
            </v-list-item-title>
          </v-list-item>
          <v-divider></v-divider>
          <v-list-item>
            <v-list-item-title @click="$emit('edit-options')">
              Options
            </v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="$emit('save')">
              Save
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </div>
</template>
<script>
import { defineComponent } from '@vue/composition-api'

export default defineComponent({
  setup() {

  },
})
</script>
