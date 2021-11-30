<template>
  <div>
    <h3>Reports Configuration {{ id }}</h3>
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
        type: 'text',
        name: 'fieldName',
        label: 'Field Name',
        enabled: true,
        validation: 'required'
      },
      {
        componen: 'VCard',
        type: 'group',
        repeatable: true,
        name: 'addresses',
        addLabel: '+ New Field',
        children: [
          {
            type: 'select',
            label: 'Input Type',
            options: [
              { value: 'input', label: 'input' },
              { value: 'select', label: 'select' },
              { value: 'materialinput', label: 'materialinput' },
              { value: 'checkbox', label: 'checkbox' }
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
            name: 'placeholder',
            label: 'Placehoder'
          },
          {
            component: 'div',
            class: 'tw-border-2 doublw-row',
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
        component: 'VBtn',
        type: 'submit',
        label: 'Save'
      }
    ]

    const handleSubmit = (values) => {
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
