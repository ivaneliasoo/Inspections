import { defineStore } from 'pinia'
import { SignatureDTO } from '~/types/Signatures/ViewModels/SignatureDTO'

export interface State {
  signaturesList: SignatureDTO[]
}

export const useSignaturesStore = defineStore('signatures', {
  state: (): State => ({
    signaturesList: [] as SignatureDTO[]
  }),
  actions: {
    async getSignatures (payload) {
      const sign = await window.$nuxt.$axios.$get('signatures', { params: payload })
      this.setList(sign)
    },
    async getSignatureById (payload) {
      return await window.$nuxt.$axios.$get(`signatures/${payload}`)
    },
    async deleteSignature (payload) {
      await window.$nuxt.$axios.$delete(`signatures/${payload}`)
      this.remove(payload)
    },
    async createSignature (payload) {
      payload.responsibleName = ''
      await window.$nuxt.$axios.$post('signatures', payload)
      this.add(payload)
    },
    async updateSignature (payload) {
      payload.responsibleName = ''
      await window.$nuxt.$axios.$put(`signatures/${payload.id}`, payload)
      this.udpate(payload)
    },
    setList (value: SignatureDTO[]) { this.signaturesList = value },
    remove (value: number) { this.signaturesList = this.signaturesList?.filter(s => s.id !== value) },
    add (value: SignatureDTO) { this.signaturesList?.splice(0, 0, value) },
    udpate (value: SignatureDTO) {
      const index = this.signaturesList.findIndex(s => s.id === value.id)
      this.signaturesList.splice(index, 1, value)
    }
  }
})
