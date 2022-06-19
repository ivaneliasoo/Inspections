<template>
  <v-card class="mx-auto" flat>
    <form @submit="handleSubmit">
      <div
        style="display: block"
        v-for="section in Object.keys(sections)"
        :key="`s-${section}`"
      >
        <h1 class="text-h6 text-left">{{ section }}</h1>
        <v-row align="center" justify="center">
          <v-col
            :cols="
              field.inputType.includes('textarea') ||
              field.inputType.includes('richtext')
                ? 12
                : 3
            "
            v-for="field in sections[section].sort((f) => f.order)"
            :key="field.fieldName"
          >
            <v-text-field
              v-if="['number', 'text'].some((l) => l === field.inputType)"
              v-model="values[field.fieldName]"
              v-show="field.visible"
              :min="field.min"
              :step="field.step"
              :max="field.max"
              :type="field.inputType"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
              @blur="handleSubmit"
            />
            <v-textarea
              v-if="field.inputType === 'textarea'"
              v-show="field.visible"
              v-model="values[field.fieldName]"
              :type="field.inputType"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
              @blur="handleSubmit"
            />
            <client-only>
              <VueEditor
                v-if="field.inputType === 'richtext'"
                v-model="values[field.fieldName]"
                v-show="field.visible"
                :label="field.label"
                :suffix="field.suffix"
                :prefix="field.preffix"
                @blur="handleSubmit"
              />
            </client-only>
            <v-select
              v-if="field.inputType === 'select'"
              v-show="field.visible"
              v-model="values[field.fieldName]"
              :items="field.selectOptions.split(',')"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
              @change="handleSubmit"
            />
            <v-checkbox
              v-if="field.inputType === 'checkbox'"
              v-show="field.visible"
              v-model="values[field.fieldName]"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
              @blur="handleSubmit"
              @checked="handleSubmit"
              @change="handleSubmit"
            />
            <v-range-slider
              v-if="field.inputType === 'slider-range'"
              v-show="field.visible"
              v-model="values[field.fieldName]"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
              @blur="handleSubmit"
              @checked="handleSubmit"
              @change="handleSubmit"
              :max="field.max"
              :min="field.min"
              hide-details
            ></v-range-slider>
          </v-col>
        </v-row>
      </div>
    </form>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "@nuxtjs/composition-api";
import { DynamicFieldMetadata } from "~/services/api";

export default defineComponent({
  props: {
    value: {
      type: Object,
    },
    formId: {
      type: Number,
      required: true,
    },
    schema: {
      type: Array as () => DynamicFieldMetadata[],
      required: true,
    },
  },
  setup(props, { emit }) {
    const defaultValues = props.schema
      .sort((a, b) => {
        if (a.order > b.order) {
          return 1;
        }
        if (a.order < b.order) {
          return -1;
        }
        return 0;
      })
      .reduce((acc, value) => {
        if (!acc[value.fieldName]) {
          acc[value.fieldName] = "";
        }
        acc[value.fieldName] = value.inputType !== "slider-range" ? value.defaultValue : value.defaultValue.split(",");
        return acc;
      }, {});

    const savedValues = props.value || defaultValues;

    const values = ref<any>(savedValues);

    const sections = computed(() => {
      return props.schema
        .filter((s) => s.enabled)
        .reduce((acc, value) => {
          if (!acc[value.sectionTitle]) {
            acc[value.sectionTitle] = [];
          }
          acc[value.sectionTitle].push(value);
          return acc;
        }, {});
    });

    const handleSubmit = () => {
      emit("handle-submit", { values: values.value, formId: props.formId });
      emit("input", values.value);
    };

    return { sections, values, handleSubmit };
  },
});
</script>

<style></style>
