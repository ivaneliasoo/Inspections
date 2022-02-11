<template>
  <div>
    <h3>Reports Configuration .- Operational Readings Configuration</h3>
    <FormulateForm
      v-model="model"
      :schema="schema"
      @submit="handleSubmit"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, useRoute, computed } from '@nuxtjs/composition-api'
import useGoBack from '~/composables/useGoBack'

export default defineComponent({
  layout: '',
  setup () {
    useGoBack()
    const route = useRoute()

    const id = computed(() => route.value.params.id)
    const model = ref({})
    const schema = [
      {
        componen: 'VCard',
        type: 'group',
        repeatable: true,
        name: 'operationalReadingConfiguration',
        addLabel: '+ New Operational Reading Field',
        children: [
          {
            type: 'materialinput',
            name: 'SectionTitle',
            label: 'Section Title',
            enabled: true,
            validation: 'required'
          },
          {
            type: 'materialinput',
            name: 'fieldName',
            label: 'Field Name',
            enabled: true,
            validation: 'required'
          },
          {
            type: 'select',
            label: 'Field Input Type',
            options: [
              { value: 'input', label: 'Text input' },
              { value: 'numericinput', label: 'Numeric Input' },
              { value: 'select', label: 'Select' },
              { value: 'checkbox', label: 'Checkbox' }
            ],
            name: 'inputType',
            placeholder: 'Select an input type'
          },
          {
            type: 'materialinput',
            name: 'label',
            label: 'Label'
          },
          {
            type: 'materialinput',
            name: 'MaxLength',
            label: 'Max Length'
          },
          {
            type: 'materialinput',
            name: 'preffix',
            label: 'Preffix'
          },
          {
            type: 'materialinput',
            name: 'suffix',
            label: 'Suffix'
          },
          {
            type: 'materialinput',
            name: 'placeholder',
            label: 'Placehoder'
          },
          {
            component: 'div',
            class: 'tw-border-2 double-row',
            children: [
              {
                type: 'checkbox',
                name: 'IsRollerOnMobile',
                label: 'Is Roller On Mobile'
              },
              {
                type: 'materialinput',
                clasification: 'number',
                name: 'rollerDigits',
                label: 'Roller Digits',
                cols: 1
              }
            ]
          }
        ]
      },
      {
        type: 'submit',
        label: 'Save Configuration'
      }
    ]

    const handleSubmit = (values: any) => {
      console.log(values)
    }

    return {
      id,
      schema,
      model,
      handleSubmit
    }
  }
})
</script>
<style lang="scss">
 @import '../../../node_modules/@braid/vue-formulate/themes/snow/snow.scss';
</style>
