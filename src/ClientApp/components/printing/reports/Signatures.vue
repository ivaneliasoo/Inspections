<template>
  <ul id="signatures" class="tw-mx-2 page-break-after">
    <li
      v-for="(itemList, index) in signatures"
      :key="itemList.id"
      class="tw-mt-5"
    >
      <div
        class="
          tw-flex
          tw-flex-row
          tw-flex-nowrap
          tw-justify-between
          tw-align-middle
          tw-even
        "
      >
        <div class="tw-flex-col tw-text-left">
          <PrintingSubTitle>
            {{ lastChecksCount + 1 }}.{{ index + 1 }}. {{ itemList.title }}
          </PrintingSubTitle>
        </div>
        <div v-if="index === 0" class="tw-flex-col" />
      </div>
      <div
        class="
          tw-flex tw-flex-row tw-flex-nowrap tw-justify-between tw-align-middle
        "
      >
        <div class="tw-flex-col tw-flex-wrap tw-text-left">
          <span class="text-line tw-flex-col tw-text-center">
            Name of {{ itemList.responsibleTypeName }}:
            <span>{{
              itemList.responsibleName ? itemList.responsibleName.padEnd(
                45 - itemList.responsibleName.length,
                "_"
              ) : ''
            }}</span>
          </span>
        </div>
        <div class="tw-flex-col tw-flex-wrap tw-text-left">
          <span class="text-line tw-flex-col tw-text-center"
            >{{
              itemList.responsibleTypeName === "LEW"
                ? "License No"
                : "Designation"
            }}:
            {{
              itemList.designation ? itemList.designation.padEnd(30 - itemList.designation.length, "_") : ''
            }}</span
          >
        </div>
      </div>
      <div
        class="
          tw-flex tw-flex-row tw-flex-nowrap tw-justify-between tw-align-middle
        "
      >
        <div class="tw-flex-col tw-flex-wrap tw-text-left">
          <div class="tw-flex tw-flex-row ">
            <span class="text-line ">
              Signature:
            </span>
            <div class="tw--mt-8 tw-float-left">
              <img class="signature" :src="itemList.drawnSign">
            </div>
          </div>
        </div>
        <span class="text-line tw-flex-col tw-text-center"
          >Date: {{ formatDate(itemList.date) ? formatDate(itemList.date).padEnd(32 - formatDate(itemList.date).length, '_') : '' }}</span
        >
      </div>
      <br>
      <div class="tw-flex-col tw-flex-wrap tw-text-left tw--mt-10">
        <ul>
          <li class="tw-h-2 tw-mb-5 tw-mt-2">
            <span class="text-line tw-flex-col tw-text-center">{{ 'Remarks:'.padEnd(97 - 'Remarks:'.length, '_')}}</span>
          </li>
          <li>
            _________________________________________________________________________________________
          </li>
          <li>
            _________________________________________________________________________________________
          </li>
        </ul>
      </div>
    </li>
  </ul>
</template>

<script>
import moment from "moment";
export default {
  props: {
    signatures: {
      type: Array,
      default: () => [],
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
