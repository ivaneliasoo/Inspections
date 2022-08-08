<template>
  <div class="tw-text-left">
    <h4 class="tw-font-semibold">Voltage</h4>
    <div class="tw-flex tw-flex-row tw-flex-wrap tw-mx-1 tw-justify-evenly">
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL1N"
          type="number"
          label="L1-N"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div v-if="isMultiline" class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL2N"
          type="number"
          label="L2-N"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div v-if="isMultiline" class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL3N"
          type="number"
          label="L3-N"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
    </div>
    <div class="tw-flex tw-flex-row tw-flex-wrap tw-mx-1 tw-justify-evenly">
      <div v-if="isMultiline" class="tw-flex tw-items-center tw-mx-2">
        <v-text-field
          v-model="report.operationalReadingsVoltageL1L2"
          type="number"
          placeholder="400"
          label="L1-L2"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div v-if="isMultiline" class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL1L3"
          type="number"
          label="L1-L3"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div v-if="isMultiline" class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL2L3"
          type="number"
          label="L2-L3"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
    </div>
    <h4 class="tw-font-semibold">Running Load</h4>
    <div class="tw-flex tw-flex-row tw-flex-wrap tw-mx-1 tw-justify-evenly">
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsRunningLoadL1"
          type="number"
          label="L1"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
      </div>
      <div v-if="isMultiline" class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsRunningLoadL2"
          type="number"
          label="L2"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
      </div>
      <div v-if="isMultiline" class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsRunningLoadL3"
          type="number"
          label="L3"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
      </div>
    </div>
    <h4 class="tw-font-semibold">Main Breaker Details</h4>
    <v-row align="end">
      <v-col>
        <v-text-field
          v-model="report.operationalReadingsMainBreakerAmp"
          type="number"
          label="Main Breaker"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
      </v-col>
      <v-col>
        <v-select
          v-model="report.operationalReadingsMainBreakerPoles"
          placeholder="Poles Capacity"
          :items="[{ id: 2 }, { id: 3 }, { id: 4 }]"
          item-value="id"
          item-text="id"
        >
          <template #append> Poles </template>
        </v-select>
      </v-col>
      <v-col>
        <v-text-field
          v-model="report.operationalReadingsMainBreakerCapacity"
          placeholder="Breaking Capacity (lsc)"
          item-value="id"
          item-text="id"
        >
          <template #append> kA </template>
        </v-text-field>
      </v-col>
      <v-col>
        <v-text-field
          v-model="report.operationalReadingsMainBreakerRating"
          type="number"
          label="Rating"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
      </v-col>
    </v-row>
    <h4 class="tw-font-semibold">Over Current</h4>
    <div class="tw-flex tw-flex-wrap tw-justify-between tw-items-center">
      <v-checkbox
        v-model="report.operationalReadingsOverCurrentDirectActingEnabled"
      />
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          v-model="report.operationalReadingsOverCurrentDirectActing"
          type="number"
          :readonly="!report.operationalReadingsOverCurrentDirectActingEnabled"
          label="Direct Acting"
        />
      </div>
      <v-checkbox v-model="report.operationalReadingsOverCurrentDTLEnabled" />
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          v-model="report.operationalReadingsOverCurrentDTLA"
          type="number"
          :readonly="!report.operationalReadingsOverCurrentDTLEnabled"
          label="DTL"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
          v-model="report.operationalReadingsOverCurrentDTLSec"
          type="number"
          :readonly="!report.operationalReadingsOverCurrentDTLEnabled"
          label="@"
        >
          <template #append>
            <span>sec</span>
          </template>
        </v-text-field>
      </div>
      <v-checkbox v-model="report.operationalReadingsOverCurrentIDTMLEnabled" />
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          v-model="report.operationalReadingsOverCurrentIDMTLA"
          type="number"
          :readonly="!report.operationalReadingsOverCurrentIDTMLEnabled"
          label="IDTML"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
          v-model="report.operationalReadingsOverCurrentIDMTLTm"
          type="number"
          :readonly="!report.operationalReadingsOverCurrentIDTMLEnabled"
          label="@"
        >
          <template #append>
            <span>Tm</span>
          </template>
        </v-text-field>
      </div>
    </div>
    <h4 class="tw-font-semibold">Earth Fault</h4>
    <div class="tw-flex tw-flex-wrap tw-justify-between tw-items-center">
      <v-checkbox v-model="report.operationalReadingsEarthFaultRoobEnabled" />
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-select
          v-model="report.operationalReadingsEarthFaultMA"
          type="number"
          :readonly="!report.operationalReadingsEarthFaultRoobEnabled"
          :items="[10, 30, 100, 300]"
          label="RooB"
        >
          <template #append>
            <span>mA</span>
          </template>
        </v-select>
      </div>
      <v-checkbox v-model="report.operationalReadingsEarthFaultEIREnabled" />
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          v-model="report.operationalReadingsEarthFaultELRA"
          type="number"
          :readonly="!report.operationalReadingsEarthFaultEIREnabled"
          label="EIR"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
          v-model="report.operationalReadingsEarthFaultELRSec"
          type="number"
          :readonly="!report.operationalReadingsEarthFaultEIREnabled"
          label="@"
        >
          <template #append>
            <span>sec</span>
          </template>
        </v-text-field>
      </div>
      <v-checkbox v-model="report.operationalReadingsEarthFaultEFEnabled" />
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          v-model="report.operationalReadingsEarthFaultA"
          type="number"
          :readonly="!report.operationalReadingsEarthFaultEFEnabled"
          label="E/F"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
          v-model="report.operationalReadingsEarthFaultSec"
          type="number"
          :readonly="!report.operationalReadingsEarthFaultEFEnabled"
          label="@"
        >
          <template #append>
            <span>Tm</span>
          </template>
        </v-text-field>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from '@nuxtjs/composition-api'
import { useVModel } from '@vueuse/core'
import { ReportQueryResult } from '~/services/api'

export default defineComponent({
  props: {
    value: {
      type: Object as () => ReportQueryResult,
      required: true,
    },
    isMultiline: {
      type: Boolean,
      default: true,
    },
  },
  setup(props, { emit }) {
    const report = useVModel(props, 'value', emit)
    return {
      report,
    }
  },
})
</script>
