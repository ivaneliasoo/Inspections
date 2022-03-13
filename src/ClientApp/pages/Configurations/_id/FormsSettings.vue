<template>
  <div v-if="form">
    <h3>Forms Settings for Report Configuration</h3>
    <h6>Let's configurate some fields</h6>
    <ValidationObserver tag="form">
      <v-row justify="space-between" align="center">
        <v-col>
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtName"
              v-model="form.name"
              :disabled="form.id > 0"
              :error-messages="errors"
              label="Form Name"
            />
          </ValidationProvider>
        </v-col>
        <v-col>
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtTitle"
              v-model="form.title"
              :error-messages="errors"
              label="Form Title"
            />
          </ValidationProvider>
        </v-col>
        <v-col>
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-text-field
              id="txtIcon"
              v-model="form.icon"
              :error-messages="errors"
              label="Form Icon"
            >
              <template #append-outer>
                <a href="https://materialdesignicons.com/" target="_blank">
                  <v-icon>mdi-help</v-icon>
                </a>
              </template>
            </v-text-field>
          </ValidationProvider>
        </v-col>
        <v-col cols="1">
          <ValidationProvider v-slot="{ errors }" rules="required">
            <v-checkbox
              id="chkEnabled"
              v-model="form.enabled"
              :error-messages="errors"
              label="Enabled"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="1" class="text-right">
          <v-btn small text title="Save" @click="handleSubmit">
            <v-icon color="success">
              mdi-content-save
            </v-icon>
            Save
          </v-btn>
        </v-col>
      </v-row>
    </ValidationObserver>
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
        <v-list v-if="form.fields.fieldsDefinitions" max-height="10" dense>
          <v-list-item-group
            v-model="selectedListItem"
            active-class="indigo--text"
          >
            <template v-for="(item, index) in form.fields.fieldsDefinitions">
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
                  <v-btn icon small color="red" @click="removeField(index)">
                    <v-icon small>
                      mdi-delete
                    </v-icon>
                  </v-btn>
                  <v-btn icon small color="info" @click="duplicateField(index)">
                    <v-icon small>
                      mdi-content-duplicate
                    </v-icon>
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
import { AxiosResponse } from 'axios'
import {
  defineComponent,
  ref,
  useRoute,
  computed,
  useFetch,
  useContext,
  useRouter,
  watchEffect,
  watch
} from '@nuxtjs/composition-api'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import useGoBack from '~/composables/useGoBack'
import { FormDefinitionResponse, NewFormDefinitionCommand } from '~/services/api'

export default defineComponent({
  components: {
    ValidationObserver,
    ValidationProvider
  },
  setup () {
    useGoBack()
    const route = useRoute()
    const router = useRouter()

    const id = computed(() => parseInt(route.value.query.id.toString()))
    const reportConfigurationId = computed(() => parseInt(route.value.params.id.toString()))

    const { $formsApi } = useContext()

    const form = ref<FormDefinitionResponse>({
      id: 0,
      name: '',
      title: '',
      icon: '',
      enabled: true,
      fields: {
        fieldsDefinitions: []
      }
    })

    useFetch(async () => {
      const result = await $formsApi.getFormDefinition(id.value) as unknown as AxiosResponse<FormDefinitionResponse>
      if (
        result.data.fields &&
        result.data.fields.fieldsDefinitions
      ) {
        form.value = result.data
      } else {
        form.value = {
          ...result.data,
          fields: {
            fieldsDefinitions: []
          }
        }
      }
    })

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
        name: 'switchableSection',
        label: 'SubSection (Enabled/Disabled)',
        enabled: true
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
          { value: 'textarea', label: 'Text Area' },
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
        type: 'number',
        name: 'order',
        label: 'Field Painting Order'
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
      form.value.fields.fieldsDefinitions.unshift({
        sectionTitle: '',
        switchableSection: '',
        fieldName: '',
        label: '',
        inputType: '',
        suffix: '',
        prefix: '',
        min: 0,
        max: 999,
        step: 1,
        maxLength: 3,
        enabled: true,
        rollerOnMobile: true,
        rollerDigits: 3,
        visible: true,
        defaultValue: '',
        order: form.value.fields.fieldsDefinitions.length + 1
      })
    }

    const removeField = (index) => {
      form.value.fields.fieldsDefinitions.splice(index, 1)
    }

    const duplicateField = (index) => {
      form.value.fields.fieldsDefinitions.unshift(form.value.fields.fieldsDefinitions[index])
      selectedListItem.value = 0
    }

    const selectedListItem = ref()

    const selectedField = computed({
      get: () => {
        return form.value.fields.fieldsDefinitions[selectedListItem.value]
      },
      set: (value) => {
        const index = selectedListItem.value
        form.value.fields.fieldsDefinitions.splice(index, 1, value)
      }
    })

    watch(() => selectedField.value, (value, oldValue) => {
      if (value && value.inputType === 'select') {
        const optionsIndex = schema.findIndex(s => s.name === 'selectOptions')
        const inputTypeIndex = schema.findIndex(s => s.name === 'inputType')
        if (optionsIndex > -1 && oldValue && oldValue.inputType === 'select') {
          schema.splice(optionsIndex, 1)
        } else if (optionsIndex > -1 && oldValue && oldValue.inputType !== 'select') {
          return
        }

        schema.splice(inputTypeIndex + 1, 0, {
          type: 'text',
          name: 'selectOptions',
          label: 'Select Options',
          enabled: true,
          validation: 'required'
        })
      }
    })

    const handleSubmit = async (data: any) => {
      const payload: NewFormDefinitionCommand = {
        ...form.value,
        reportConfigurations: [reportConfigurationId.value]
      }
      if (id.value > 0) {
        await $formsApi.updateFormDefinition(id.value, payload)
      } else {
        await $formsApi.addFormDefinition(payload)
        router.back()
      }
    }

    return {
      id,
      schema,
      form,
      handleSubmit,
      selectedListItem,
      selectedField,
      addField,
      removeField,
      duplicateField
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
