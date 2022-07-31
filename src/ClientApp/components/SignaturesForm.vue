<template>
  <div class="mx-20">
      <v-card
        v-for="(signature, index) in signatures"
        :key="`signature${index}`"
        :color="signature.drawnSign ? 'teal lighten-5' : ''"
        class="mx-20"
      >
        <v-card-text>
          <v-row dense>
            <v-col class="text-left">
              <h3>{{ signature.title }}</h3>
              <h5>{{ signature.annotation }}</h5>
              <v-chip v-if="signature.principal"> Principal Sign </v-chip>
              <v-chip v-if="signature.drawnSign" color="success"> Signed </v-chip>
            </v-col>
          </v-row>
          <v-row align="center" justify="space-around" dense>
            <v-col v-if="!isLEW(signature)" cols="6" xl="2">
              <ValidationProvider
                v-slot="{ errors }"
                :rules="signature.principal ? 'oneOf:0,1,2,3,4' : ''"
                immediate
              >
                <v-select
                  :id="`type${index}`"
                  v-model="signature.responsibleType"
                  :name="`type${index}`"
                  :error-messages="errors[0]"
                  :readonly="canEdit"
                  :items="responsibleTypes"
                  item-text="text"
                  item-value="id"
                  label="Representation Type"
                />
              </ValidationProvider>
            </v-col>
            <v-col cols="6" xl="4">
              <ValidationProvider
                v-slot="{ errors }"
                :rules="signature.principal ? 'required' : ''"
                immediate
              >
                <v-text-field
                  :id="`name${index}`"
                  v-model="signature.responsibleName"
                  :name="`name${index}`"
                  :error-messages="errors[0]"
                  :readonly="canEdit"
                  label="Name"
                />
              </ValidationProvider>
            </v-col>
            <v-col cols="12" xl="4">
              <ValidationProvider>
                <v-text-field
                  :id="`designation${index}`"
                  v-model="signature.designation"
                  :name="`designation${index}`"
                  :readonly="canEdit"
                  label="Designation"
                />
              </ValidationProvider>
            </v-col>
            <v-col cols="12" md="6" xl="2">
              <ValidationProvider>
                <DatePickerBase
                  :id="`date${index}`"
                  v-model="signature.date"
                  :name="`date${index}`"
                  :disabled="canEdit"
                  label="Date"
                  max=""
                  min=""
                />
              </ValidationProvider>
            </v-col>
            <v-col cols="12" sm="6" md="6" xl="12">
              <ValidationProvider>
                <v-text-field
                  :id="`remarks${index}`"
                  v-model="signature.remarks"
                  :name="`remarks${index}`"
                  :readonly="canEdit"
                  label="Remarks (if any)"
                />
              </ValidationProvider>
            </v-col>
            <ValidationProvider>
              <SignaturePad
                v-if="signature.viewSign"
                v-model="signature.drawnSign"
                :saved-data="signature.drawnSign"
                @input="viewSign(index, signature)"
                @close="viewSign(index, signature)"
              />
            </ValidationProvider>
          </v-row>
        </v-card-text>
        <v-card-actions class="text-right">
          <v-row>
            <v-col class="text-right">
              <v-btn
                v-if="!signature.viewSign || canEdit"
                class="mx-2"
                dark
                color="primary"
                @click="viewSign(index, signature)"
              >
                <v-icon dark>
                  {{ signature.viewSign ? "mdi-close" : "mdi-draw" }}
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
import { defineComponent } from '@nuxtjs/composition-api';
import { oneOf } from 'vee-validate/dist/rules';
import { ValidationProvider, extend } from 'vee-validate';
import { responsibleTypes } from '@/types/Signatures';
import { SignatureQueryResult } from "~/services/api";
import { useVModel } from "@vueuse/core";

extend('oneOf', { ...oneOf }, )


export default defineComponent({
  components: {
    ValidationProvider,
  },
  props: {
    value: {
      type: Array as () => SignatureQueryResult[] | undefined,
      required: true,
    },
    canEdit: {
      type: Boolean,
      default: false,
    },
  },
  setup(props, { emit }) {
    const signatures = useVModel(props, 'value', emit);

    const isLEW = (signature: SignatureQueryResult) => {
      return signature.title && signature.title.includes("LEW");
    };

    const viewSign = (index: number, item: SignatureQueryResult) => {
      item.viewSign = !item.viewSign;
      if (signatures.value) {
        signatures.value.splice(index, 1, item);
      }
    };

    return {
      signatures,
      isLEW,
      viewSign,
      responsibleTypes,
    };
  },
});
</script>

<style scoped>
:deep(.v-card) {
  margin: 20px;
}
</style>
