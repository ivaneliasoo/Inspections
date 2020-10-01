<template>
  <div  class="mx-20">
    <v-card v-for="(signature, index) in signatures" :key="`signature${index}`" :color="signature.drawedSign? 'teal lighten-5' : ''" class="mx-20">
      <v-card-text>
        <Signature v-model="signatures[index]" :is-report-closed="isClosed" :index="index" @sign="viewSign(index, signature)"/>
      </v-card-text>
      <v-card-actions class="text-right">
        
      </v-card-actions>
    </v-card>
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
      
      if(this.signaturesModel)
        this.signaturesModel.splice(index,1, item)
      this.$emit('input', this.signaturesModel)
    }

    
  }
</script>

<style scoped>
::v-deep .v-card{
  margin: 20px;
}
</style>