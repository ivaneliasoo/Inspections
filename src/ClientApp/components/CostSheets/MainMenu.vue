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
            <v-list-item-title class="font-weight-bold">
              COST SHEETS
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
