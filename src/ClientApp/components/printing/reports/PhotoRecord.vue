<template>
  <div style="display: block; border: 2px">
    <div
      v-for="(page, index) in items"
      :key="index"
      style="display: block; border: 2px"
      class="page-break-before"
    >
      <div class="tw-block tw-text-center" style="width: 100%">
        <div class="tw-grid tw-grid-cols-2 tw-gap-2">
          <div
            v-for="photo in page"
            :key="photo.id"
            class="tw-border-2 tw-border-indigo-200 tw-mx-2 tw-my-2"
          >
            <span class="tw-text-xs tw-text-center">{{ photo.timestamp }}</span>
            <img style="width: 360px; max-height: 400px" :src="photo.photoBase64" />
            <span class="tw-font-bold tw-text-center">{{ photo.label }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    items: {
      type: Array,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
@media print {
  .page-break-after {
    page-break-after: always;
  }
  .page-break-before {
    page-break-before: always;
  }
}
</style>
