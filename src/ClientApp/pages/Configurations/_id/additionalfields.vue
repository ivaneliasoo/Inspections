<template>
  <div>
    <h3>Reports Configuration .- Operational Readings Configuration</h3>
    <FormulateForm
      v-model="values"
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
    const values = ref({
      operationalReadingConfiguration: [
        {
          SectionTitle: 'Voltage',
          FieldName: 'L1N',
          Label: 'L1-N',
          InputType: 'numericinput',
          Suffix: 'V',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 230
        },
        {
          SectionTitle: 'Voltage',
          FieldName: 'L2N',
          Label: 'L2-N',
          InputType: 'numericinput',
          Suffix: 'V',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 230
        },
        {
          SectionTitle: 'Voltage',
          FieldName: 'L3N',
          Label: 'L3-N',
          InputType: 'numericinput',
          Suffix: 'V',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 230
        },
        {
          SectionTitle: 'Voltage',
          FieldName: 'L1L2',
          Label: 'L1-L2',
          InputType: 'numericinput',
          Suffix: 'V',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 400
        },
        {
          SectionTitle: 'Voltage',
          FieldName: 'L1L3',
          Label: 'L1-L3',
          InputType: 'numericinput',
          Suffix: 'V',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 400
        },
        {
          SectionTitle: 'Voltage',
          FieldName: 'L2L3',
          Label: 'L2-L3',
          InputType: 'numericinput',
          Suffix: 'V',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 400
        },
        {
          SectionTitle: 'Running Load',
          FieldName: 'L1',
          Label: 'L1',
          InputType: 'numericinput',
          Suffix: 'A',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 10
        },
        {
          SectionTitle: 'Running Load',
          FieldName: 'L2',
          Label: 'L2',
          InputType: 'numericinput',
          Suffix: 'A',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 10
        },
        {
          SectionTitle: 'Running Load',
          FieldName: 'L3',
          Label: 'L3',
          InputType: 'numericinput',
          Suffix: 'A',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 10
        },
        {
          SectionTitle: 'Main Breaker Details',
          FieldName: 'MainBreaker',
          Label: 'Main Breaker',
          InputType: 'numericinput',
          Suffix: 'A',
          Preffix: '',
          Min: 0,
          Max: 999,
          Step: 0.1,
          MaxLength: '3',
          Enabled: true,
          RollerOnMobile: true,
          RollerDigits: '3',
          Visible: true,
          DefaultValue: 10
        }
      ]
    })
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
            type: 'text',
            name: 'FieldName',
            label: 'Field Name',
            enabled: true,
            validation: 'required'
          },
          {
            type: 'select',
            label: 'Field Input Type',
            options: [
              { value: 'text', label: 'Text input' },
              { value: 'number', label: 'Numeric Input' },
              { value: 'select', label: 'Select' },
              { value: 'checkbox', label: 'Checkbox' }
            ],
            name: 'InputType',
            placeholder: 'Select an input type'
          },
          {
            type: 'text',
            name: 'Label',
            label: 'Label'
          },
          {
            type: 'number',
            name: 'MaxLength',
            label: 'Max Length'
          },
          {
            type: 'text',
            name: 'Preffix',
            label: 'Preffix'
          },
          {
            type: 'text',
            name: 'Suffix',
            label: 'Suffix'
          },
          {
            type: 'text',
            name: 'Placeholder',
            label: 'Placehoder'
          },
          {
            component: 'div',
            class: 'double-wide',
            children: [
              {
                type: 'checkbox',
                name: 'IsRollerOnMobile',
                label: 'Is Roller On Mobile'
              },
              {
                type: 'number',
                clasification: 'number',
                name: 'RollerDigits',
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
      values,
      handleSubmit
    }
  }
})
</script>
<style lang="scss">
  @import '../../../node_modules/@braid/vue-formulate/themes/snow/snow.scss';
  .formulate-input-element.formulate-input-element--group.formulate-input-group{
    max-width: 100%;
  }
  .double-wide {
    display: flex;
  }
  .double-wide .formulate-input {
    flex-grow: 1;
    width: calc(50% - .5em);
  }
  .double-wide .formulate-input:first-child {
    margin-right: .5em;
  }
  .double-wide .formulate-input:last-child {
    margin-left: .5em;
  }
</style>
