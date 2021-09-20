<template>
  <div style="display: block; border: 2px">
    <div style="display: block;">
      <article class="tw-text-justify">
        <PrintingTitle> Particulars Of Instalation </PrintingTitle>
        <span class="text-line tw-font-medium">
          Name of Installation:
          <span class="tw-underline">{{ reportData.name }}</span>
        </span>
        <span class="text-line tw-font-medium">
          Addres of Installation:
          <span
            class="tw-underline"
          >{{ reportData.address }} ({{ reportData.licenseKVA }} kVA)</span>
        </span>
        <span class="text-line tw-font-medium">
          Approved Load:
          <span
            class="tw-underline"
          >{{ reportData.licenseAmp }} A /{{ reportData.licenseVolt }} V</span>
        </span>
        <div class="tw-flex tw-flex-row tw-justify-between">
          <span
            class="text-line tw-flex-col"
          >Electrical Installation License: {{ reportData.licenseNumber }}</span>
          <span
            class="text-line tw-flex-col"
          >Date & Time of Inspection: {{ formatedDate }}</span>
        </div>
        <PrintingReportsCheckLists
          v-if="reportData.checkLists"
          :check-lists="reportData.checkLists"
        />
        <PrintingReportsSignatures
          v-if="reportData.signatures"
          :signatures="reportData.signatures"
          :last-checks-count="lastChecksCount"
        />
      </article>
    </div>
    <div style="display: block; border: 2px; " class="page-break-before">
      <div class="tw-block tw-text-center">
        <div class="tw-grid tw-grid-cols-3">
          <div v-for="(photo, index) in photoRecords" :key="photo.id" class="tw-border-2 tw-border-indigo-200 tw-max-w-sm tw-min-h-max">
            <img v-if="index <= 5" style="width: 250px; top: 0" :src="photo.photoUrl" alt="">
            <span v-if="index <= 5" class="tw-font-bold tw-text-center">{{ photo.label }}</span>
          </div>
        </div>
      </div>
    </div>
    <!-- <div style="display: block; border: 2px; " class="page-break-before">
      <div class="tw-block tw-text-center">
        <div class="tw-grid tw-grid-cols-3">
          <div v-for="(photo, index) in photoRecords" :key="photo.id" class="tw-border-2 tw-border-indigo-200 tw-max-w-sm tw-min-h-max">
            <img v-if="index > 5 && index <= 11" style="width: 250px; top: 0" :src="photo.photoUrl" alt="">
            <span v-if="index > 5 && index <= 11" class="tw-font-bold tw-text-center">{{ photo.label }}</span>
          </div>
        </div>
      </div>
    </div> -->
  </div>
</template>

<script>
import moment from 'moment'
export default {
  layout: 'printlayout',
  async asyncData ({ route, $axios, $reportsApi }) {
    if (route && route.params && route.params.id) {
      const id = parseInt(route.params.id)
      const result = await $axios.$get(`reports/${id}`)
      const photoRecords = await $reportsApi.reportsIdPhotorecordGet(id)
      return {
        reportId: id,
        reportData: result,
        photoRecords: photoRecords.data
      }
    }
  },
  data () {
    return {
      reportId: -1,
      reportData: {},
      photoRecords: []
    }
  },
  computed: {
    formatedDate () {
      if (this.reportData && this.reportData.date) {
        return moment(this.reportData.date).format('DD-MM-YYYY HH:mm')
      }
      return ''
    },
    lastChecksCount () {
      if (this.reportData && this.reportData.checkLists) {
        return this.reportData.checkLists.length
      }

      return 0
    }
  },
  auth: false
}
</script>

<style lang="scss" scoped>
div {
  display: inline-block;
}

article {
  font-family: "Arial";
  display: flex;
  flex-flow: column;
  .text-line {
    font-size: 0.9em;
    line-height: 2;
  }
}
@media print {
  .page-break-after {
    page-break-after: always;
  }
  .page-break-before {
    page-break-before: always;
  }
}
</style>
