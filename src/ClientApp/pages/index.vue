<template>
  <div>
    <v-row class="small">
      <v-col cols="6">
        <ReportStatus style="height: 150px" />
      </v-col>
      <v-col cols="6">
        <ReportsByInspector />
      </v-col>
    </v-row>
    <v-row>
      <v-col></v-col>
      <v-col></v-col>
    </v-row>
  </div>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import ReportStatus from '~/components/dashboard/ReportStatus.vue'
import ReportsByInspector from '~/components/dashboard/ReportsByInspector.vue'

@Component({
  components: {
    ReportStatus,
    ReportsByInspector
  }
})
export default class IndexPage extends Vue {

}
</script>

<style>
.small {
    max-width: 600px;
    margin:  150px auto;
  }
</style>
