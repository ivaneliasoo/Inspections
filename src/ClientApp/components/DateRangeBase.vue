<template>
  <v-row>
    <v-col cols="6">
      <DatePickerBase v-model="value.desde" :titulo="tituloDesde" :max="desdeMax" :show-icon="showIcon" @input="updateFechas" />
    </v-col>
    <v-col cols="6">
      <DatePickerBase
        v-model="value.hasta"
        :titulo="tituloHasta"
        :min="hastaMin"
        :max="maxHasta"
        :show-icon="showIcon"
        @input="updateFechas"
      />
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, computed } from '@nuxtjs/composition-api'

export default defineComponent({
  props: {
    value: {
      type: Object,
      required: true
    },
    showIcon: {
      type: Boolean,
      default: false
    },
    tituloDesde: {
      type: String,
      default: 'Desde'
    },
    tituloHasta: {
      type: String,
      default: 'Hasta'
    },
    max: {
      type: String,
      required: false,
      default: ''
    },
  },
  setup (props, { emit }) {
    const desdeMax = computed(() => {
      return props.value && props.value.hasta ? props.value.hasta : ''
    })

    const hastaMin = computed(() => {
      return props.value && props.value.desde ? props.value.desde : ''
    })

    const maxHasta = computed(() => {
      return props.max || props.max === '' ? props.max : undefined
    })

    const updateFechas = () => {
      emit('input', props.value)
    }

    return {
      desdeMax,
      hastaMin,
      maxHasta,
      updateFechas
    }
  }
})
</script>

<style>

</style>
