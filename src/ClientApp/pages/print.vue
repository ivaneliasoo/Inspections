<template>
  <div style="display: block; border: 2px">
    <div style="display: block;" class="page-break-after">
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
          v-if="reportData.checkLists && configuration.checkListMetadata.display === 'Numbered'"
          :check-lists="reportData.checkLists"
        />
        <div v-if="reportData.checkLists && configuration.checkListMetadata.display === 'Inline'">
          {{ reportData.checkLists.map(c => c.text) }}
        </div>
        <PrintingReportsOperationalReadings
          :readings="operationalReadings"
          :last-checks-count="lastChecksCount"
        />
        <PrintingReportsSignatures
          v-if="reportData.signatures"
          :signatures="reportData.signatures"
          :last-checks-count="lastChecksCount + 1"
        />
      </article>
    </div>
    <!-- TODO-IVAN Hacer Encabezado del Compounded Photo Record -->
    <PrintingReportsPhotoRecord v-if="printPhotos" :items="photoRecordsPages" />
  </div>
</template>

<script>
import moment from 'moment'
export default {
  layout: 'printlayout',
  async asyncData ({ route, $axios }) {
    if (route && route.query && route.query.id) {
      const id = parseInt(route.query.id)
      const token = route.query.token

      const printPhotos = route.query && route.query.printPhotos ? route.query.printPhotos === 'true' : false
      const isCompoundedPhotoRecord = route.query && route.query.compoundedPhotoRecord ? route.query.compoundedPhotoRecord === 'true' : false
      const result = await $axios.$get(`reports/${id}`, {
        headers: {
          Authorization: `bearer ${token}`
        }
      })
      const photoRecords = await $axios.$get(`reports/${id}/photorecord`, {
        headers: {
          Authorization: `bearer ${token}`
        }
      })

      const configuration = await $axios.$get('reportconfiguration/1', {
        headers: {
          Authorization: `bearer ${token}`
        }
      })

      console.log({ configuration })

      const pagesLength = Math.ceil(photoRecords.length / 8)
      const photoRecordsPages = [[]]
      let currentPage = 0
      photoRecords.forEach((photo, index) => {
        if (index > 0 && (index) % 8 === 0) { photoRecordsPages.push([]); currentPage++ }
        if (photoRecordsPages[currentPage]) { photoRecordsPages[currentPage].push(photo) }
      })

      if (pagesLength % 2 !== 0) {
        photoRecordsPages[currentPage].push({ thumbnailBase64: '', label: '' })
      }

      let operationalReadings = {}
      if (result) {
        const {
          operationalReadingsId,
          operationalReadingsVoltageL1N,
          operationalReadingsVoltageL2N,
          operationalReadingsVoltageL3N,
          operationalReadingsVoltageL1L2,
          operationalReadingsVoltageL1L3,
          operationalReadingsVoltageL2L3,
          operationalReadingsRunningLoadL1,
          operationalReadingsRunningLoadL2,
          operationalReadingsRunningLoadL3,
          operationalReadingsMainBreakerAmp,
          operationalReadingsMainBreakerPoles,
          operationalReadingsMainBreakerCapacity,
          operationalReadingsOverCurrentDTLA,
          operationalReadingsOverCurrentDTLSec,
          operationalReadingsOverCurrentIDMTLA,
          operationalReadingsOverCurrentIDMTLTm,
          operationalReadingsEarthFaultMA,
          operationalReadingsEarthFaultELRA,
          operationalReadingsEarthFaultELRSec,
          operationalReadingsEarthFaultA,
          operationalReadingsEarthFaultSec,
          operationalReadingsMainBreakerRating,
          operationalReadingsOverCurrentDirectActingEnabled,
          operationalReadingsOverCurrentDirectActing,
          operationalReadingsOverCurrentDTLEnabled,
          operationalReadingsOverCurrentIDTMLEnabled,
          operationalReadingsEarthFaultRoobEnabled,
          operationalReadingsEarthFaultEIREnabled,
          operationalReadingsEarthFaultEFEnabled
        } = result

        operationalReadings = {
          operationalReadingsId,
          operationalReadingsVoltageL1N,
          operationalReadingsVoltageL2N,
          operationalReadingsVoltageL3N,
          operationalReadingsVoltageL1L2,
          operationalReadingsVoltageL1L3,
          operationalReadingsVoltageL2L3,
          operationalReadingsRunningLoadL1,
          operationalReadingsRunningLoadL2,
          operationalReadingsRunningLoadL3,
          operationalReadingsMainBreakerAmp,
          operationalReadingsMainBreakerPoles,
          operationalReadingsMainBreakerCapacity,
          operationalReadingsOverCurrentDTLA,
          operationalReadingsOverCurrentDTLSec,
          operationalReadingsOverCurrentIDMTLA,
          operationalReadingsOverCurrentIDMTLTm,
          operationalReadingsEarthFaultMA,
          operationalReadingsEarthFaultELRA,
          operationalReadingsEarthFaultELRSec,
          operationalReadingsEarthFaultA,
          operationalReadingsEarthFaultSec,
          operationalReadingsMainBreakerRating,
          operationalReadingsOverCurrentDirectActingEnabled,
          operationalReadingsOverCurrentDirectActing,
          operationalReadingsOverCurrentDTLEnabled,
          operationalReadingsOverCurrentIDTMLEnabled,
          operationalReadingsEarthFaultRoobEnabled,
          operationalReadingsEarthFaultEIREnabled,
          operationalReadingsEarthFaultEFEnabled
        }
      }

      return {
        reportId: id,
        reportData: result,
        photoRecordsPages,
        printPhotos,
        isCompoundedPhotoRecord,
        isPrintable: false,
        operationalReadings,
        configuration
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
  auth: false,
  mounted () {
    window.isPrintable = true
  }
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
