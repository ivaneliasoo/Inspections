<template>
  <ul class="tw-list-disc tw-mx-2">
    <li
      v-for="(itemList, index) in checkLists"
      :key="itemList.id"
      class="tw-mt-5"
    >
      <div class="tw-flex tw-justify-between">
        <div class="tw-inline-block">
          <PrintingSubTitle>
            {{ index + 1 }}. {{ itemList.text }}
          </PrintingSubTitle>
        </div>
        <div v-if="itemList.annotation !== ''" class="tw-inline-block">
          <PrintingAnnotation>{{ itemList.annotation }}</PrintingAnnotation>
        </div>
        <div v-if="index === 0" class="tw-text-left tw-inline-block">
          <PrintingSubTitle>REMARKS</PrintingSubTitle>
        </div>
      </div>
      <ul class="tw-mt-2">
        <li
          v-for="(check, checkIndex) in itemList.checks"
          :key="check.id"
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
            <div class="text-line tw-text-sm">
              {{ index + 1 }}.{{ checkIndex + 1 }}. {{ check.text }}
            </div>
          </div>
          <div class="tw-flex-col tw-text-left">
            <div class="tw-w-20" />
          </div>
          <div class="tw-flex-col tw-text-right tw-w-90">
            <div class="tw-inline-flex tw-align-middle">
              <div class="tw-mr-12 tw-border tw-w-6 tw-h-6 tw-rounded-md tw-min-h-min tw-min-w-max tw-border-gray-600">
                <div class="tw-block tw-top-0">
                  <span v-if="check.checked === 2" class="tw-text-xs">N.A.</span>
                  <svg
                    v-if="check.checked === 0"
                    class="tw-fill-current tw--top-5"
                    xmlns="http://www.w3.org/2000/svg"
                    xmlns:xlink="http://www.w3.org/1999/xlink"
                    version="1.1"
                    width="24"
                    height="24"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M19,6.41L17.59,5L12,10.59L6.41,5L5,6.41L10.59,12L5,17.59L6.41,19L12,13.41L17.59,19L19,17.59L13.41,12L19,6.41Z"
                    />
                  </svg>
                  <svg
                    v-if="check.checked === 1"
                    xmlns="http://www.w3.org/2000/svg"
                    xmlns:xlink="http://www.w3.org/1999/xlink"
                    version="1.1"
                    width="24"
                    height="24"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M21,7L9,19L3.5,13.5L4.91,12.09L9,16.17L19.59,5.59L21,7Z"
                    />
                  </svg>
                </div>
              </div>
              <div class="tw-text-xs tw-text-left tw-w-40 tw-max-w-screen-lg">
                {{ !check.remarks ? "____________________________________" : check.remarks }}
              </div>
            </div>
          </div>
        </li>
      </ul>
    </li>
  </ul>
</template>

<script>
export default {
  props: {
    checkLists: {
      type: Array,
      default: () => [],
      required: true,
    },
  },
}
</script>

<style lang="scss" scoped>
</style>
