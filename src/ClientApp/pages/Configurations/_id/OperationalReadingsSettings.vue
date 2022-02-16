<template>
  <div>
    <h3>Reports Configuration .- Operational Readings Configuration</h3>
    <v-row align="start" justify="space-between">
      <v-col cols="8" align-self="start">
        <FormulateForm
          v-if="selectedField"
          v-model="selectedField"
          :schema="schema"
          @submit="handleSubmit"
        />
        <h6 v-else>
          Select a Field or create a new one in the list to edit
        </h6>
      </v-col>
      <v-col cols="4">
        <v-toolbar color="indigo" dark>
          <v-toolbar-title>Fields</v-toolbar-title>
          <v-spacer />
          <v-btn icon @click="addField">
            <v-icon>mdi-plus</v-icon>
          </v-btn>
        </v-toolbar>
        <v-list v-if="values" max-height="10" dense>
          <v-list-item-group
            v-model="selectedListItem"
            active-class="indigo--text"
          >
            <template v-for="(item, index) in values">
              <v-list-item :key="`item-${index}`" :value="index">
                <v-list-item-content>
                  <v-list-item-subtitle
                    class="text--primary text-left font-weight-bold"
                    v-text="`${item.fieldName} - ${item.sectionTitle}`"
                  />
                  <v-list-item-subtitle
                    class="text-left"
                    v-text="`${item.label} (type: ${item.inputType})`"
                  />
                </v-list-item-content>
                <v-list-item-action>
                  <v-btn icon color="red" @click="removeField(index)">
                    <v-icon>mdi-delete</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
              <v-divider :key="`divider-${item.fieldName}`" />
            </template>
          </v-list-item-group>
        </v-list>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import {
  defineComponent,
  ref,
  useRoute,
  computed,
  useFetch,
  useContext
} from '@nuxtjs/composition-api'
import useGoBack from '~/composables/useGoBack'

export default defineComponent({
  layout: '',
  setup () {
    useGoBack()
    const route = useRoute()

    const id = computed(() => route.value.params.id)

    const { $axios } = useContext()

    useFetch(async () => {
      const result = await $axios.$get(`reportconfiguration/${id.value}`)
      if (
        result.operationalReadings &&
        result.operationalReadings.fieldsDefinitions
      ) {
        values.value = result.operationalReadings.fieldsDefinitions
      } else {
        values.value = []
      }
    })

    const values = ref([])

    const schema = [
      {
        type: 'text',
        name: 'sectionTitle',
        label: 'Section Title',
        enabled: true,
        validation: 'required'
      },
      {
        type: 'text',
        name: 'fieldName',
        label: 'Field Name',
        enabled: true,
        validation: 'required'
      },
      {
        type: 'text',
        name: 'label',
        label: 'Label',
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
        name: 'inputType',
        placeholder: 'Select an input type',
        validation: 'required'
      },
      {
        type: 'text',
        name: 'preffix',
        label: 'Preffix'
      },
      {
        type: 'text',
        name: 'suffix',
        label: 'Suffix'
      },
      {
        type: 'number',
        name: 'min',
        label: 'Min'
      },
      {
        type: 'number',
        name: 'max',
        label: 'Max'
      },
      {
        type: 'number',
        name: 'step',
        label: 'Step'
      },
      {
        type: 'number',
        name: 'maxLength',
        label: 'Max Length'
      },
      {
        type: 'checkbox',
        name: 'enabled',
        label: 'Enabled'
      },
      {
        type: 'checkbox',
        name: 'visible',
        label: 'Visible'
      },
      {
        type: 'text',
        name: 'defaultValue',
        label: 'Default Value'
      },
      {
        component: 'div',
        class: 'double-wide',
        children: [
          {
            type: 'checkbox',
            name: 'isRollerOnMobile',
            label: 'Is Roller On Mobile',
            cols: 1
          },
          {
            type: 'number',
            clasification: 'number',
            name: 'rollerDigits',
            label: 'Roller Digits',
            cols: 1
          }
        ]
      },
      {
        type: 'submit',
        label: 'Save Field'
      }
    ]

    const addField = () => {
      values.value.unshift({
        sectionTitle: '',
        dieldName: '',
        label: '',
        inputType: '',
        suffix: '',
        preffix: '',
        min: 0,
        max: 999,
        step: 0.1,
        maxLength: '3',
        enabled: true,
        rollerOnMobile: true,
        rollerDigits: '3',
        visible: true,
        defaultValue: 0
      })
    }

    const removeField = (index) => {
      values.value.splice(index, 1)
      $axios.$put(`reportconfiguration/${id.value}/operationalreadings`, {
        id: id.value,
        fieldsDefinitions: {
          fieldsDefinitions: values.value
        }
      })
    }

    const selectedListItem = ref()

    const selectedField = computed({
      get: () => {
        return values.value[selectedListItem.value]
      },
      set: (value) => {
        const index = selectedListItem.value
        values.value.splice(index, 1, value)
      }
    })

    const handleSubmit = (data: any) => {
      $axios.$put(`reportconfiguration/${id.value}/operationalreadings`, {
        id: id.value,
        fieldsDefinitions: {
          fieldsDefinitions: values.value
        }
      })
    }

    return {
      id,
      schema,
      values,
      handleSubmit,
      selectedListItem,
      selectedField,
      addField,
      removeField
    }
  }
})
</script>
<style lang="scss">
@import "../../../node_modules/@braid/vue-formulate/themes/snow/snow.scss";
.double-wide {
  display: flex;
}
.double-wide .formulate-input {
  flex-grow: 1;
  width: calc(50% - 0.5em);
}
.double-wide .formulate-input:first-child {
  margin-right: 0.5em;
}
.double-wide .formulate-input:last-child {
  margin-left: 0.5em;
}
</style>
