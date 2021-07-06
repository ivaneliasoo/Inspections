<template>
  <div>
    <v-menu v-model="menu1" :disabled="disabled" :close-on-content-click="false" max-width="290" offset-y>
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
        @change="menu1 = false;"
      />
    </v-menu>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import moment from 'moment'

@Component
export default class DatePickerBase extends Vue {
  @Prop({ required: true }) value:any;
  @Prop({ default: 'Date' }) titulo:any;

  @Prop() min:any;
  @Prop() max:any;
  @Prop({ default: false }) showIcon:Boolean | undefined;
  @Prop({ default: false }) disabled:Boolean | undefined;

  menu1:Boolean = false

  get fechaPickerModel (): string | null {
    if (this.value) {
      return this.fDateToYMD(this.value)
    } else {
      this.$emit('input', this.fechaHoy)
      return null
    }
  }

  set fechaPickerModel (valor) {
    if (valor !== '') { this.$emit('input', this.fDateToYMD(valor)) }

    this.$emit('input', valor)
  }

  get fechaHoy () {
    return this.fDateToYMD(new Date())
  }

  get fechaComputed () {
    return (this.fechaPickerModel !== null && this.fechaPickerModel !== undefined) ? this.fDateToDMY(this.fechaPickerModel) : ''
  }

  emitEvent (valor:any) {
    this.$emit('input', valor ?? '')
  }

  fDateToDMY (dateValue:any) {
    return moment(dateValue).format('DD/MM/YYYY')
  }

  fDateToYMD (dateValue:any) {
    return moment(dateValue).format('YYYY-MM-DD')
  }

  limpiar () {
    this.$emit('input', '')
  }
}
</script>

<style>

</style>
