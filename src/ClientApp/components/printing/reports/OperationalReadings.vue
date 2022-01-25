<template>
  <ul id="signatures" class="tw-mx-2 page-break-before">
    <br>
    <li class="tw-mt-5 tw-font-bold">
      <PrintingSubTitle>{{lastChecksCount + 1}}. OPERATIONAL READINGS</PrintingSubTitle>
    </li>
    <li class="text-line tw-text-sm tw-font-bold tw-text-gray-600">{{lastChecksCount + 1}}.1 Voltage
      <ul class="text-line tw-text-sm tw-font-normal tw-text-black">
        <li>L1-N: {{readings.operationalReadingsVoltageL1N}} V  L2-N: {{readings.operationalReadingsVoltageL2N}} V  L3-N: {{readings.operationalReadingsVoltageL3N}} V</li>
        <li>L1-L2: {{readings.operationalReadingsVoltageL1L2}} V  L1-L3: {{readings.operationalReadingsVoltageL1L3}} V  L2-L3: {{readings.operationalReadingsVoltageL2L3}} V</li>
      </ul>
    </li>
    <li class="text-line tw-text-sm tw-font-bold tw-text-gray-600">{{lastChecksCount + 1}}.2 Running Load
      <ul class="text-line tw-text-sm tw-font-normal tw-text-black">
        <li>L1: {{readings.operationalReadingsRunningLoadL1}} A  L2-N: {{readings.operationalReadingsRunningLoadL2}} A  L3-N: {{readings.operationalReadingsRunningLoadL3}} A</li>
      </ul>
    </li>
    <li class="text-line tw-text-sm tw-font-bold tw-text-gray-600">{{ lastChecksCount + 1}}.3 Main Breaker Details
      <ul class="text-line tw-text-sm tw-font-normal tw-text-black">
        <li>a) Main Breaker: {{readings.operationalReadingsMainBreakerAmp}} A, Rating {{readings.operationalReadingsMainBreakerRating}} A, {{readings.operationalReadingsMainBreakerPoles}} Poles, Braking Capacity {{readings.operationalReadingsMainBreakerCapacity}} kA</li>
        <li v-if="readings.operationalReadingsOverCurrentDirectActingEnabled ||
        readings.operationalReadingsOverCurrentDTLEnabled||
        readings.operationalReadingsOverCurrentIDTMLEnabled">b) Over-current Settings:</li>
        <li class="tw-p-3" v-if="readings.operationalReadingsOverCurrentDirectActingEnabled">   Direct Acting: {{readings.operationalReadingsOverCurrentDirectActing}} A</li>
        <li class="tw-p-3" v-if="readings.operationalReadingsOverCurrentDTLEnabled">   <span>DTL: OC: {{readings.operationalReadingsOverCurrentDTLA}} A @ {{readings.operationalReadingsOverCurrentDTLSec}} sec</span></li>
        <li class="tw-p-3" v-if="readings.operationalReadingsOverCurrentIDTMLEnabled">   <span>IDMTL: OC: {{readings.operationalReadingsOverCurrentIDMTLA}} A TMS {{readings.operationalReadingsOverCurrentIDMTLTm}}</span></li>
        <li v-if="readings.operationalReadingsEarthFaultRoobEnabled ||
        readings.operationalReadingsEarthFaultEIREnabled ||
        readings.operationalReadingsEarthFaultEFEnabled">c) Earth Fault Settings:</li>
        <li class="tw-p-3" v-if="readings.operationalReadingsEarthFaultRoobEnabled">   RCCB: {{readings.operationalReadingsEarthFaultMA}} mA</li>
        <li class="tw-p-3" v-if="readings.operationalReadingsEarthFaultEIREnabled">   <span>ELR: {{readings.operationalReadingsEarthFaultELRA}} A @ {{readings.operationalReadingsEarthFaultELRSec}} sec</span></li>
        <li class="tw-p-3" v-if="readings.operationalReadingsEarthFaultEFEnabled">   <span>EF: OC: {{readings.operationalReadingsEarthFaultA}} A {{readings.operationalReadingsEarthFaultSec}}s</span></li>
      </ul>
    </li>
    <!-- <li>5.4 Others
      <ul>
        <li>Earth loop impedance (Power Disruption might occur)</li>
        <li>L1-E: , L2-E: , L3</li>
        <li></li>
        <li></li>
      </ul>
    </li> -->
  </ul>
</template>

<script>
import moment from "moment";
import { Report } from '~/types';
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
    formatDate(date) {
      if (date) {
        return moment(date).format("DD-MM-YYYY");
      }
      return "";
    },
  },
};
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
