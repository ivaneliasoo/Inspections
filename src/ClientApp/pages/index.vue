<template>
  <div>
    <v-row class="small">
      <v-col cols="6">
        <ReportStatus style="height: 150px" />
      </v-col>
      <v-col cols="6">
        <ReportsByInspector id="prueba" />
      </v-col>
    </v-row>
    <v-row>
      <v-col><v-btn @click="print">Print</v-btn></v-col>
      <v-col></v-col>
    </v-row>
  </div>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'

@Component
export default class IndexPage extends Vue {
  
}
</script>

<style>
.small {
    max-width: 600px;
    margin:  150px auto;
  }
</style>
