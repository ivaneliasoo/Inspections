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
import { Component, Vue, Prop } from 'nuxt-property-decorator'
// import DatePickerBase from '@/components/DatePickerBase.vue'

@Component
export default class DateRangeBase extends Vue {
    @Prop() value:any;
    @Prop({ default: false }) showIcon:Boolean | undefined;
    @Prop({ default: 'Desde' }) tituloDesde:String | undefined;
    @Prop({ default: 'Hasta' }) tituloHasta:String | undefined;
    @Prop() max:String | undefined;

    get desdeMax() {
      return this.value && this.value.hasta ? this.value.hasta : ''
    }

    get hastaMin() {
      return this.value && this.value.desde ? this.value.desde : ''
    }

    get maxHasta () {
      return this.max || this.max === '' ? this.max : undefined
    }

    updateFechas() {
      this.$emit('input', this.value)
    }
}

</script>

<style>

</style>
