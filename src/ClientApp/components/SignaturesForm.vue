<template>
  <div class="mx-20">
    <v-card v-for="(signature, index) in signatures" :key="`signature${index}`" :color="signature.drawedSign? 'teal lighten-5' : ''" class="mx-20">
      <v-card-text>
        <v-row dense>
          <v-col class="text-left">
            <h3>{{ signature.title }}</h3>
            <h5>{{ signature.annotation }}</h5>
            <v-chip v-if="signature.principal">
              Principal Sign
            </v-chip>
            <v-chip v-if="signature.drawedSign" color="success">
              Signed
            </v-chip>
          </v-col>
        </v-row>
        <v-row align="center" justify="space-around" dense>
          <v-col v-if="!signature.title.includes('LEW')" cols="6" xl="2">
            <v-select
              :id="`type${index}`"
              v-model="signature.responsable.type"
              :name="`type${index}`"
              :readonly="isClosed"
              :items="responsableTypes"
              label="Representation Type"
            />
          </v-col>
          <v-col cols="6" xl="4">
            <ValidationProvider v-slot="{ errors }" :rules="signature.principal ? 'required':''" immediate>
              <v-text-field
                :id="`name${index}`"
                v-model="signature.responsableName"
                :name="`name${index}`"
                :error-messages="errors[0]"
                :readonly="isClosed"
                label="Name"
                @blur="signature.responsable.name = signature.responsableName"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12" xl="4">
            <v-text-field :id="`designation${index}`" v-model="signature.designation" :name="`designation${index}`" :readonly="isClosed" label="Designation" />
          </v-col>
          <v-col cols="12" md="6" xl="2">
            <DatePickerBase
              :id="`date${index}`"
              v-model="signature.date"
              :name="`date${index}`"
              :disabled="isClosed"
              label="Date"
              max=""
              min=""
            />
          </v-col>
          <v-col cols="12" sm="6" md="6" xl="12">
            <v-text-field :id="`remarks${index}`" v-model="signature.remarks" :name="`remarks${index}`" :readonly="isClosed" label="Remarks (if any)" />
          </v-col>
          <SignaturePad
            v-if="signature.viewSign"
            v-model="signature.drawedSign"
            :signature-id="signature.id"
            :saved-data="signature.drawedSign"
            @input="viewSign(index, signature);"
            @close="viewSign(index, signature);"
          />
        </v-row>
      </v-card-text>
      <v-card-actions class="text-right">
        <v-row>
          <v-col class="text-right">
            <v-btn v-if="!signature.viewSign || isClosed" class="mx-2" dark color="primary" @click="viewSign(index, signature);">
              <v-icon dark>
                {{ signature.viewSign ? 'mdi-close' : 'mdi-draw' }}
              </v-icon>
              Sign
            </v-btn>
          </v-col>
        </v-row>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Model, Prop } from 'vue-property-decorator'
import { ValidationProvider } from 'vee-validate'
import { SignatureDTO } from '~/types/Signatures/ViewModels/SignatureDTO'
import { ResponsableType } from '@/types'

  @Component({
    components: {
      ValidationProvider
    }
  })
export default class SignaturesForm extends Vue {
    @Model('input') signaturesModel: SignatureDTO[] | undefined
    @Prop({ required: true, type: Boolean, default: () => false }) isClosed: boolean | undefined

    get signatures () {
      return this.signaturesModel
    }

    viewSign (index: number, item: SignatureDTO) {
      item.viewSign = !item.viewSign
      item.drawedSign = item.drawedSign
      if (this.signaturesModel) { this.signaturesModel.splice(index, 1, item) }
      this.$emit('input', this.signaturesModel)
    }

    responsableTypes: any = Object.keys(ResponsableType)
      .map((key: any) => {
        if (!isNaN(Number(key.toString()))) { return }

        return { id: ResponsableType[key], text: key }
      })
      .filter(i => i !== undefined)
}
</script>

<style scoped>
::v-deep .v-card{
  margin: 20px;
}
</style>
