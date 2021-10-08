<template>
  <div class="tw-text-left">
    <h4 class="tw-font-semibold">Voltage</h4>
    <div class="tw-flex tw-flex-row tw-flex-wrap tw-mx-1 tw-justify-evenly">
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL1N"
          type="number"
          label="L1-N"
          placeholder="230"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL2N"
          type="number"
          label="L2-N"
          placeholder="230"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL3N"
          type="number"
          label="L3-N"
          placeholder="230"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
    </div>
    <div class="tw-flex tw-flex-row tw-flex-wrap tw-mx-1 tw-justify-evenly">
      <div class="tw-flex tw-items-center tw-mx-2">
        <v-text-field type="number" placeholder="400" label="L1-L2">
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL1L3"
          type="number"
          label="L1-L3"
          placeholder="400"
        >
          <template #append>
            <span>V</span>
          </template>
        </v-text-field>
      </div>
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsVoltageL2L3"
          type="number"
          label="L2-L3"
          placeholder="400"
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
          placeholder="400"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
      </div>
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsRunningLoadL2"
          type="number"
          label="L2"
          placeholder="400"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
      </div>
      <div class="tw-flex tw-items-center">
        <v-text-field
          v-model="report.operationalReadingsRunningLoadL3"
          type="number"
          label="L3"
          placeholder="400"
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
          placeholder="400"
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
        <v-select
          v-model="report.operationalReadingsMainBreakerCapacity"
          placeholder="Breaking Capacity (lsc)"
          :items="[{ id: 100 }, { id: 200 }, { id: 300 }, { id: 400 }]"
          item-value="id"
          item-text="id"
        >
          <template #append> kA </template>
        </v-select>
      </v-col>
      <v-col>
        <v-text-field
          v-model="report.operationalReadingsMainBreakerRating"
          type="number"
          placeholder="400"
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
      <v-simple-checkbox
        v-model="report.operationalReadingsOverCurrentDirectActingEnabled"
      ></v-simple-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          :readonly="!report.operationalReadingsOverCurrentDirectActingEnabled"
          label="Direct Acting"
        />
      </div>
      <v-simple-checkbox
        v-model="report.operationalReadingsOverCurrentDTLEnabled"
      ></v-simple-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          :readonly="!report.operationalReadingsOverCurrentDTLEnabled"
          readonly
          label="DTL"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
          :readonly="!report.operationalReadingsOverCurrentDTLEnabled"
          label="@"
        >
          <template #append>
            <span>sec</span>
          </template>
        </v-text-field>
      </div>
      <v-simple-checkbox
        v-model="report.operationalReadingsOverCurrentIDTMLEnabled"
      ></v-simple-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          :readonly="!report.operationalReadingsOverCurrentIDTMLEnabled"
          label="IDTML"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
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
      <v-simple-checkbox
        v-model="report.operationalReadingsEarthFaultRoobEnabled"
      ></v-simple-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-select
          :readonly="!report.operationalReadingsEarthFaultRoobEnabled"
          label="RooB"
        >
          <template #append>
            <span>mA</span>
          </template>
        </v-select>
      </div>
      <v-simple-checkbox
        v-model="report.operationalReadingsEarthFaultEIREnabled"
      ></v-simple-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          :readonly="!report.operationalReadingsEarthFaultEIREnabled"
          readonly
          label="EIR"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
          :readonly="!report.operationalReadingsEarthFaultEIREnabled"
          label="@"
        >
          <template #append>
            <span>sec</span>
          </template>
        </v-text-field>
      </div>
      <v-simple-checkbox
        v-model="report.operationalReadingsEarthFaultEFEnabled"
      ></v-simple-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          :readonly="!report.operationalReadingsEarthFaultEFEnabled"
          label="E/F"
        >
          <template #append>
            <span>A</span>
          </template>
        </v-text-field>
        <v-text-field
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
import { defineComponent, ref } from "@nuxtjs/composition-api";
import { ReportQueryResult } from "~/services/api";

export default defineComponent({
  props: {
    reportData: {
      type: Object as () => ReportQueryResult,
      required: true,
    },
  },
  setup(props) {
    const report = ref({ ...props.reportData });
    return {
      report,
    };
  },
});
</script>

<style scoped>
</style>
