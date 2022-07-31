<template>
  <ul id="signatures" class="tw-mx-2 page-break-before">
    <br>
    <li class="tw-mt-5 tw-font-bold">
      <PrintingSubTitle>{{ lastChecksCount + 1 }}. OPERATIONAL READINGS</PrintingSubTitle>
    </li>
    <li class="text-line tw-text-sm tw-font-bold tw-text-gray-600">
      {{ lastChecksCount + 1 }}.1 Voltage
      <ul class="text-line tw-text-sm tw-font-normal tw-text-black">
        <li>L1-N: {{ readings.L1N }} V  L2-N: {{ readings.L2N }} V  L3-N: {{ readings.L3N }} V</li>
        <li>L1-L2: {{ readings.L1L2 }} V  L1-L3: {{ readings.L1L3 }} V  L2-L3: {{ readings.L2L3 }} V</li>
      </ul>
    </li>
    <li class="text-line tw-text-sm tw-font-bold tw-text-gray-600">
      {{ lastChecksCount + 1 }}.2 Running Load
      <ul class="text-line tw-text-sm tw-font-normal tw-text-black">
        <li>L1: {{ readings.RunningLoadL1 }} A  L2-N: {{ readings.RunningLoadL2 }} A  L3-N: {{ readings.RunningLoadL3 }} A</li>
      </ul>
    </li>
    <li class="text-line tw-text-sm tw-font-bold tw-text-gray-600">
      {{ lastChecksCount + 1 }}.3 Main Breaker Details
      <ul class="text-line tw-text-sm tw-font-normal tw-text-black">
        <li>a) Main Breaker: {{ readings.MainBreakerDetailsMainBreaker }} A, Rating {{ readings.MainBreakerDetailsRating }} A, {{ readings.MainBreakerDetailsPolesCapacity }} Poles, Braking Capacity {{ readings.MainBreakerDetailsBreakingCapacity }} kA</li>
        <li
          v-if="readings.OvercurrentProtectionDirectActingEnabled ||
            readings.OvercurrentProtectionDTLEnabled||
            readings.OvercurrentProtectionIDTMLEnabled"
        >
          b) Over-current Settings:
        </li>
        <li v-if="readings.OvercurrentProtectionDirectActingEnabled" class="tw-p-3">
          Direct Acting: {{ readings.OvercurrentProtectionDirectActing }} A
        </li>
        <li v-if="readings.OvercurrentProtectionDTLEnabled" class="tw-p-3">
          <span>DTL: OC: {{ readings.OvercurrentProtectionDTLA }} A @ {{ readings.OvercurrentProtectionDTLSec }} sec</span>
        </li>
        <li v-if="readings.OvercurrentProtectionIDTMLEnabled" class="tw-p-3">
          <span>IDMTL: OC: {{ readings.OvercurrentProtectionIDMTLA }} A TMS {{ readings.OvercurrentProtectionIDMTLTm }}</span>
        </li>
        <li
          v-if="readings.EarthFaultProtectionRCCBEnabled ||
            readings.EarthFaultProtectionELREnabled ||
            readings.EarthFaultProtectionEFREnabled"
        >
          c) Earth Fault Settings:
        </li>
        <li v-if="readings.EarthFaultProtectionRCCBEnabled" class="tw-p-3">
          RCCB: {{ readings.EarthFaultProtectionProtectionRCCB }} mA
        </li>
        <li v-if="readings.EarthFaultProtectionELREnabled" class="tw-p-3">
          <span>ELR: {{ readings.EarthFaultProtectionELRA }} A @ {{ readings.EarthFaultProtectionELRSEC }} sec</span>
        </li>
        <li v-if="readings.EarthFaultProtectionEFREnabled" class="tw-p-3">
          <span>EFR: OC: {{ readings.EarthFaultProtectionA }} A {{ readings.EarthFaultProtectionEFRTm }}s</span>
        </li>
      </ul>
    </li>
  </ul>
</template>

<script>
import moment from 'moment'
export default {
  props: {
    readings: {
      type: Object,
      default: () => {},
      required: true,
    },
    lastChecksCount: {
      type: Number,
      required: true,
    },
  },
  methods: {
    formatDate (date) {
      if (date) {
        return moment(date).format('DD-MM-YYYY')
      }
      return ''
    },
  },
}
</script>

<style lang="scss" scoped>
.signature {
  width: 200px;
}
.text-line {
  line-height: 2;
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
