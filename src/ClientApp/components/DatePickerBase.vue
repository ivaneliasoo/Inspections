<template>
  <div>
    <v-menu
      v-model="menu1"
      :disabled="disabled"
      :close-on-content-click="false"
      max-width="290"
      offset-y
    >
      <template #activator="{ on }">
        <v-text-field
          v-model="fechaComputed"
          :label="titulo"
          readonly
          :clearable="!disabled"
          :prepend-icon="showIcon ? 'event' : ''"
          autocomplete="nope"
          v-on="on"
          @input="emitEvent(fechaPickerModel)"
          @click:clear="limpiar()"
        >
          <template #append-outer="">
            <slot name="append-outer" />
          </template>
        </v-text-field>
      </template>
      <v-date-picker
        v-model="fechaPickerModel"
        locale="en-Us"
        :max="max || max === '' ? max : fechaHoy"
        :min="min ? min : ''"
        first-day-of-week="1"
        @change="menu1 = false"
      />
    </v-menu>
  </div>
</template>

<script lang="ts">
import moment from 'moment'
import { defineComponent, computed, ref } from '@nuxtjs/composition-api'

export default defineComponent({
  props: {
    value: {
      type: [Object, Array, String],
      required: false,
      default: ''
    },
    titulo: {
      type: String,
      default: 'Date'
    },
    min: {
      type: [Object, Array, Number, String],
      default: ''
    },
    max: {
      type: [Object, Array, Number, String],
      default: ''
    },
    showIcon: {
      type: Boolean,
      default: false
    },
    disabled: {
      type: Boolean,
      default: false
    },
  },
  // @Prop({ required: true }) value:any
  // @Prop({ default: 'Date' }) titulo:any

  // @Prop() min:any
  // @Prop() max:any
  // @Prop({ default: false }) showIcon:Boolean | undefined
  // @Prop({ default: false }) disabled:Boolean | undefined
  setup (props, { emit }) {
    const menu1 = ref(false)

    const fechaPickerModel = computed({
      get: (): string | null => {
        if (props.value) {
          return fDateToYMD(props.value)
        } else {
          emit('input', fechaHoy.value)
          return null
        }
      },
      set: (valor) => {
        if (valor !== '') { emit('input', fDateToYMD(valor)) }

        emit('input', valor)
      }
    })

    const fechaHoy = computed(() => {
      return fDateToYMD(new Date())
    })

    const fechaComputed = computed(() => {
      return (fechaPickerModel.value !== null && fechaPickerModel.value !== undefined) ? fDateToDMY(fechaPickerModel.value) : ''
    })

    const emitEvent = (valor:any) => {
      emit('input', valor ?? '')
    }

    const fDateToDMY = (dateValue:any) => {
      return moment(dateValue).format('DD/MM/YYYY')
    }

    const fDateToYMD = (dateValue:any) => {
      return moment(dateValue).format('YYYY-MM-DD')
    }

    const limpiar = () => {
      emit('input', '')
    }

    return {
      menu1,
      fechaPickerModel,
      fechaHoy,
      fechaComputed,
      emitEvent,
      limpiar,
    }
  }

})
</script>

<style>
</style>
