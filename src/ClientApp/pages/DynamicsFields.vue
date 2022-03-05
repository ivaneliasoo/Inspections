<template>
  <div>
    <FormulateForm
      v-model="model"
      name="dynamicsforms"
      :schema="schema"
    />
    <pre class="text-left">{{ model }}</pre>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from '@nuxtjs/composition-api'

export default defineComponent({
  setup () {
    const schema = [
      {
        type: 'color',
        clasification: 'color',
        name: 'Color',
        label: 'Select a Color',
        validation: 'required'
      },
      {
        type: 'select',
        name: 'Address',
        label: 'Enter an Address',
        validation: 'required'
      },
      {
        component: 'VRow',
        class: 'tw-border-2 doublw-row',
        children: [
          {
            type: 'materialinput',
            name: 'date',
            label: 'Select a Date',
            validation: 'required'
          },
          {
            type: 'materialinput',
            name: 'license',
            label: 'Select a License',
            validation: 'required'
          }]
      }

    ]

    const model = ref({})
    return { schema, model }
  }
})
</script>

<style lang="scss">
@import '../node_modules/@braid/vue-formulate/themes/snow/snow.scss';
label.formulate-input-label {
  display: none;
}
</style>
