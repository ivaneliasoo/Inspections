<template>
  <div>
    <div v-for="(signature, index) in signatures" :key="`signature${index}`">
      <v-row>
          <v-col class="text-left">
              <h3>{{ signature.title }}</h3>
              <h5>{{ signature.annotation }}</h5>
              <v-chip v-if="signature.principal">Principal Sign</v-chip>
          </v-col>
          <v-col class="text-right">
            <v-btn v-if="!signature.viewSign || isClosed" class="mx-2" fab dark small color="primary" @click="viewSign(index, signature);">
              <v-icon dark>{{ signature.viewSign ? 'mdi-close' : 'mdi-draw' }}</v-icon>
            </v-btn>
          </v-col>
      </v-row>
      <v-row align="center" justify="space-around">
        <v-col cols="6" xl="2">
            <v-select 
            :id="`type${index}`"
            :name="`type${index}`"
            v-model="signature.responsable.type" 
            :readonly="isClosed"
            :items="responsableTypes" 
            label="Responsable Type" />
        </v-col>
        <v-col cols="6" xl="4">
          <v-text-field :id="`name${index}`" :name="`name${index}`" v-model="signature.responsable.name" :readonly="isClosed" label="Responsable" />
        </v-col>
        <v-col cols="12" xl="4">
          <v-text-field :id="`designation${index}`" :name="`designation${index}`" v-model="signature.designation" :readonly="isClosed" label="Designation" />
        </v-col>
        <v-col cols="12" md="6" xl="2">
          <DatePickerBase :id="`date${index}`" :name="`date${index}`" v-model="signature.date" :disabled="isClosed" label="Date" max="" min="" />
        </v-col>
        <v-col cols="12" sm="6" md="6" xl="12">
          <v-text-field :id="`remarks${index}`" :name="`remarks${index}`" v-model="signature.remarks" :readonly="isClosed" label="Remarks (if any)" />
        </v-col>
        <SignaturePad v-if="signature.viewSign" v-model="signature.drawedSign" :signature-id="signature.id" 
          :savedData="signature.drawedSign"
          @input="viewSign(index, signature);"
          @close="viewSign(index, signature);" />
      </v-row>
    </div>
  </div>
</template>

<script lang="ts">
  import { Component, Vue, Model, Prop } from 'vue-property-decorator';
import { SignatureDTO } from '~/types/Signatures/ViewModels/SignatureDTO';
import { ResponsableType } from "@/types";

  @Component
  export default class SignaturesForm extends Vue {
    @Model('input') signaturesModel: SignatureDTO[] | undefined
    @Prop({ required: true, type: Boolean, default: () => false }) isClosed: boolean | undefined

    get signatures() {
      return this.signaturesModel
    }

    viewSign(index: number, item: SignatureDTO) {
      item.viewSign = !item.viewSign
      item.drawedSign = item.drawedSign
      if(this.signaturesModel)
        this.signaturesModel.splice(index,1, item)
      this.$emit('input', this.signaturesModel)
    }

    responsableTypes: any = Object.keys(ResponsableType)
    .map((key: any) => {
      if (!isNaN(Number(key.toString()))) return;

      return { id: ResponsableType[key], text: key };
    })
    .filter(i => i !== undefined)
  }
</script>

<style scoped>

</style>