<template>
  <div>
    <v-row dense>
            <v-col class="text-left">
                <h3>{{ signature.title }}</h3>
                <h5>{{ signature.annotation }}</h5>
                <v-chip v-if="signature.principal">Principal Sign</v-chip>
                <v-chip color="success" v-if="signature.drawedSign">Signed</v-chip>
            </v-col>
        </v-row>
        <v-row align="center" justify="space-around" dense>
          <v-col cols="6" xl="2">
              <v-select
              :id="`type${index}`"
              :name="`type${index}`"
              v-model="signature.responsable.type"
              :readonly="isReportClosed"
              :items="responsableTypes"
              label="Representation Type" />
          </v-col>
          <v-col cols="6" xl="4">
            <ValidationProvider :rules="signature.principal ? 'required':''" v-slot="{ errors }" immediate>
              <v-text-field :id="`name${index}`" :name="`name${index}`" v-model="name" @blur="signature.responsable.name = name"  :readonly="isReportClosed" label="Name" />
            </ValidationProvider>
          </v-col>
          <v-col cols="12" xl="4">
            <v-text-field :id="`designation${index}`" :name="`designation${index}`" v-model="signature.designation" :readonly="isReportClosed" label="Designation" />
          </v-col>
          <v-col cols="12" md="6" xl="2">
            <DatePickerBase :id="`date${index}`" :name="`date${index}`" v-model="signature.date" :disabled="isReportClosed" label="Date" max="" min="" />
          </v-col>
          <v-col cols="12" sm="6" md="6" xl="12">
            <v-text-field :id="`remarks${index}`" :name="`remarks${index}`" v-model="signature.remarks" :readonly="isReportClosed" label="Remarks (if any)" />
          </v-col>
          <SignaturePad v-model="signature.drawedSign" :signature-id="signature.id"
            :savedData="signature.drawedSign"
            @input="viewSign(index, signature);"
            @close="viewSign(index, signature);" />
        </v-row>
        <v-row>
          <v-col class="text-right">
            <v-btn v-if="!signature.viewSign || isReportClosed" class="mx-2" dark color="primary" @click="viewSign(index, signature);">
              <v-icon dark>{{ signature.viewSign ? 'mdi-close' : 'mdi-draw' }}</v-icon>
              Sign
            </v-btn>
          </v-col>
        </v-row>
  </div>
</template>

<script lang="ts">
import { ValidationProvider } from 'vee-validate'

  import { Component, Prop, Model, Vue } from 'vue-property-decorator';
import { ResponsableType, Signature } from '~/types';
import { SignatureDTO } from '~/types/Signatures/ViewModels/SignatureDTO';

  @Component({
    components: {
      ValidationProvider
    }
  })
  export default class SignatureComponent extends Vue {
    @Model("input") signature: Signature | undefined;
    @Prop({ required: true }) isReportClosed: Signature | undefined;
    @Prop({ required: true }) index: Signature | undefined;

    name: string = ''

    mounted() {
      this.name = this.signature!.responsable.name
    }

    responsableTypes: any = Object.keys(ResponsableType)
    .map((key: any) => {
      if (!isNaN(Number(key.toString()))) return;

      return { id: ResponsableType[key], text: key };
    })
    .filter(i => i !== undefined)

    viewSign(index: number, item: SignatureDTO) {
      item!.viewSign = !item!.viewSign
      item!.drawedSign = item!.drawedSign
      this.$emit('sign', {index, item})
    }

  }
</script>

<style scoped>

</style>
