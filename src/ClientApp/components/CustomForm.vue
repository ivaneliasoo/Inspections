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
            :cols="field.inputType === 'textarea' ? 12 : 3"
            v-for="field in sections[section].reverse()"
            :key="field.fieldName"
          >
            <v-text-field
              v-if="['number', 'text'].some((l) => l === field.inputType)"
              v-model="values[field.fieldName]"
              :min="field.min"
              :step="field.step"
              :max="field.max"
              :type="field.inputType"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
            />
            <v-textarea
              v-if="field.inputType === 'textarea'"
              v-model="values[field.fieldName]"
              :type="field.inputType"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
            />
            <v-select
              v-if="field.inputType === 'select'"
              v-model="values[field.fieldName]"
              :items="field.options"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
            />
            <v-checkbox
              v-if="field.inputType === 'checkbox'"
              v-model="values[field.fieldName]"
              :label="field.label"
              :suffix="field.suffix"
              :prefix="field.preffix"
            />
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
  model: {
    prop: "value",
    event: "input",
  },
  props: {
    schema: {
      type: Array as () => DynamicFieldMetadata[],
      required: true,
    },
  },
  setup(props, { emit }) {
    const defaultValues = props.schema.reverse().reduce((acc, value) => {
      if (!acc[value.fieldName]) {
        acc[value.fieldName] = "";
      }
      acc[value.fieldName] = value.defaultValue;
      return acc;
    }, {});

    const values = ref<any>(defaultValues);

    const sections = computed(() => {
      return props.schema.reduce((acc, value) => {
        if (!acc[value.sectionTitle]) {
          acc[value.sectionTitle] = [];
        }
        acc[value.sectionTitle].push(value);
        return acc;
      }, {});
    });

    const handleSubmit = () => emit("submit", values);

    return { sections, values, handleSubmit };
  },
});
</script>

<style></style>
