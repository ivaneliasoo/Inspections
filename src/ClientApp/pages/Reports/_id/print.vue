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
    <!-- TODO-IVAN Hacer Encabezado del Compounded Photo Record -->
    <PrintingReportsPhotoRecord v-if="printPhotos || isCompoundedPhotoRecord" :items="photoRecordsPages" />
  </div>
</template>

<script>
import moment from 'moment'
export default {
  layout: 'printlayout',
  async asyncData ({ route, $axios, $reportsApi }) {
    if (route && route.params && route.params.id) {
      const id = parseInt(route.params.id)

      const printPhotos = route.query && route.query.printPhotos ? route.query.printPhotos === 'true' : false
      const isCompoundedPhotoRecord = route.query && route.query.compoundedPhotoRecord ? route.query.compoundedPhotoRecord === 'true' : false
      const result = await $axios.$get(`reports/${id}`)
      const photoRecords = await $reportsApi.apiReportsIdPhotorecordGet(id)
      const pagesLength = Math.ceil(photoRecords.data.length / 12)
      const photoRecordsPages = [[]]
      let currentPage = 0
      photoRecords.data.forEach((photo, index) => {
        if (index > 0 && (index) % 12 === 0) { photoRecordsPages.push([]); currentPage++ }
        if (photoRecordsPages[currentPage]) { photoRecordsPages[currentPage].push(photo) }
      })

      if (pagesLength % 2 !== 0) {
        photoRecordsPages[currentPage].push({ thumbnailBase64: '', label: '' })
      }
      return {
        reportId: id,
        reportData: result,
        photoRecordsPages,
        printPhotos,
        isCompoundedPhotoRecord
      }
    }
  },
  data () {
    return {
      reportId: -1,
      reportData: {},
      photoRecords: [],
      photoRecordsPages: []
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
