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
      <div class="tw-flex tw-items-center">
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
      <div class="tw-flex tw-items-center">
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
      <div class="tw-flex tw-items-center tw-mx-2">
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
      <div class="tw-flex tw-items-center">
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
      <div class="tw-flex tw-items-center">
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
      <div class="tw-flex tw-items-center">
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
      <div class="tw-flex tw-items-center">
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
      ></v-checkbox>
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
      <v-checkbox
        v-model="report.operationalReadingsOverCurrentDTLEnabled"
      ></v-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          v-model="report.operationalReadingsOverCurrentDTLA"
          type="number"
          :readonly="!report.operationalReadingsOverCurrentDTLEnabled"
          readonly
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
      <v-checkbox
        v-model="report.operationalReadingsOverCurrentIDTMLEnabled"
      ></v-checkbox>
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
      <v-checkbox
        v-model="report.operationalReadingsEarthFaultRoobEnabled"
      ></v-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-select
          v-model="report.operationalReadingsEarthFaultMA"
          type="number"
          :readonly="!report.operationalReadingsEarthFaultRoobEnabled"
          :items=[10,30,100,300]
          label="RooB"
        >
          <template #append>
            <span>mA</span>
          </template>
        </v-select>
      </div>
      <v-checkbox
        v-model="report.operationalReadingsEarthFaultEIREnabled"
      ></v-checkbox>
      <div
        class="tw-border tw-shadow-sm tw-shadow-sm tw-rounded-md tw-my-2 tw-p-3"
      >
        <v-text-field
          v-model="report.operationalReadingsEarthFaultELRA"
          type="number"
          :readonly="!report.operationalReadingsEarthFaultEIREnabled"
          readonly
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
      <v-checkbox
        v-model="report.operationalReadingsEarthFaultEFEnabled"
      ></v-checkbox>
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
import {
  defineComponent,
  useContext,
  watch,
  ref,
} from "@nuxtjs/composition-api";
import {
  ReportQueryResult,
  UpdateOperationalReadingsCommand,
} from "~/services/api";
import { debounce } from "lodash";

export default defineComponent({
  props: {
    reportData: {
      type: Object as () => ReportQueryResult,
      required: true,
    },
  },
  setup(props) {
    const report = ref({ ...props.reportData });

    const { $reportsApi } = useContext();

    const updateCommand = ref<UpdateOperationalReadingsCommand>({
      id: report.value.operationalReadingsId!,
      reportId: report.value.id!,
      voltageL1N: report.value.operationalReadingsVoltageL1N!,
      voltageL2N: report.value.operationalReadingsVoltageL2N!,
      voltageL3N: report.value.operationalReadingsVoltageL3N!,
      voltageL1L2: report.value.operationalReadingsVoltageL1L2!,
      voltageL1L3: report.value.operationalReadingsVoltageL1L3!,
      voltageL2L3: report.value.operationalReadingsVoltageL2L3!,
      runningLoadL1: report.value.operationalReadingsRunningLoadL1!,
      runningLoadL2: report.value.operationalReadingsRunningLoadL2!,
      runningLoadL3: report.value.operationalReadingsRunningLoadL3!,
      mainBreakerAmp: report.value.operationalReadingsMainBreakerAmp!,
      mainBreakerPoles: report.value.operationalReadingsMainBreakerPoles!,
      mainBreakerCapacity: report.value.operationalReadingsMainBreakerCapacity!,
      overCurrentDTLA: report.value.operationalReadingsOverCurrentDTLA!,
      overCurrentDTLSec: report.value.operationalReadingsOverCurrentDTLSec!,
      overCurrentIDMTLA: report.value.operationalReadingsOverCurrentIDMTLA!,
      overCurrentIDMTLTm: report.value.operationalReadingsOverCurrentIDMTLTm!,
      earthFaultMA: report.value.operationalReadingsEarthFaultMA!,
      earthFaultELRA: report.value.operationalReadingsEarthFaultELRA!,
      earthFaultELRSec: report.value.operationalReadingsEarthFaultELRSec!,
      earthFaultA: report.value.operationalReadingsEarthFaultA!,
      earthFaultSec: report.value.operationalReadingsEarthFaultSec!,
      mainBreakerRating: report.value.operationalReadingsMainBreakerRating,
      overCurrentDirectActingEnabled:
        report.value.operationalReadingsOverCurrentDirectActingEnabled,
      overCurrentDirectActing:
        report.value.operationalReadingsOverCurrentDirectActing,
      overCurrentDTLEnabled:
        report.value.operationalReadingsOverCurrentDTLEnabled,
      overCurrentIDTMLEnabled:
        report.value.operationalReadingsOverCurrentIDTMLEnabled,
      earthFaultRoobEnabled:
        report.value.operationalReadingsEarthFaultRoobEnabled,
      earthFaultEIREnabled:
        report.value.operationalReadingsEarthFaultEIREnabled,
      earthFaultEFEnabled: report.value.operationalReadingsEarthFaultEFEnabled,
    });

    const update = debounce(() => {
      $reportsApi.updateOperationalReadings(
        updateCommand.value.reportId!,
        updateCommand.value
      );
    }, 500);

    watch(report.value, (newReport) => {
      updateCommand.value.id = newReport.operationalReadingsId!;
      updateCommand.value.reportId = newReport.id!;
      updateCommand.value.voltageL1N = newReport.operationalReadingsVoltageL1N!;
      updateCommand.value.voltageL2N = newReport.operationalReadingsVoltageL2N!;
      updateCommand.value.voltageL3N = newReport.operationalReadingsVoltageL3N!;
      updateCommand.value.voltageL1L2 =
        newReport.operationalReadingsVoltageL1L2!;
      updateCommand.value.voltageL1L3 =
        newReport.operationalReadingsVoltageL1L3!;
      updateCommand.value.voltageL2L3 =
        newReport.operationalReadingsVoltageL2L3!;
      updateCommand.value.runningLoadL1 =
        newReport.operationalReadingsRunningLoadL1!;
      updateCommand.value.runningLoadL2 =
        newReport.operationalReadingsRunningLoadL2!;
      updateCommand.value.runningLoadL3 =
        newReport.operationalReadingsRunningLoadL3!;
      updateCommand.value.mainBreakerAmp =
        newReport.operationalReadingsMainBreakerAmp!;
      updateCommand.value.mainBreakerPoles =
        newReport.operationalReadingsMainBreakerPoles!;
      updateCommand.value.mainBreakerCapacity =
        newReport.operationalReadingsMainBreakerCapacity!;
      updateCommand.value.overCurrentDTLA =
        newReport.operationalReadingsOverCurrentDTLA!;
      updateCommand.value.overCurrentDTLSec =
        newReport.operationalReadingsOverCurrentDTLSec!;
      updateCommand.value.overCurrentIDMTLA =
        newReport.operationalReadingsOverCurrentIDMTLA!;
      updateCommand.value.overCurrentIDMTLTm =
        newReport.operationalReadingsOverCurrentIDMTLTm!;
      updateCommand.value.earthFaultMA =
        newReport.operationalReadingsEarthFaultMA!;
      updateCommand.value.earthFaultELRA =
        newReport.operationalReadingsEarthFaultELRA!;
      updateCommand.value.earthFaultELRSec =
        newReport.operationalReadingsEarthFaultELRSec!;
      updateCommand.value.earthFaultA =
        newReport.operationalReadingsEarthFaultA!;
      updateCommand.value.earthFaultSec =
        newReport.operationalReadingsEarthFaultSec!;
      updateCommand.value.mainBreakerRating =
        newReport.operationalReadingsMainBreakerRating;
      updateCommand.value.overCurrentDirectActingEnabled =
        newReport.operationalReadingsOverCurrentDirectActingEnabled;
      updateCommand.value.overCurrentDirectActing =
        newReport.operationalReadingsOverCurrentDirectActing;
      updateCommand.value.overCurrentDTLEnabled =
        newReport.operationalReadingsOverCurrentDTLEnabled;
      updateCommand.value.overCurrentIDTMLEnabled =
        newReport.operationalReadingsOverCurrentIDTMLEnabled;
      updateCommand.value.earthFaultRoobEnabled =
        newReport.operationalReadingsEarthFaultRoobEnabled;
      updateCommand.value.earthFaultEIREnabled =
        newReport.operationalReadingsEarthFaultEIREnabled;
      updateCommand.value.earthFaultEFEnabled =
        newReport.operationalReadingsEarthFaultEFEnabled;

      update();
    });

    return {
      report,
    };
  },
});
</script>

<style scoped>
</style>
