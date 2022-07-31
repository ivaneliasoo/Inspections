import { defineStore } from 'pinia'
import { AddressDto } from '~/types'

interface State {
  addressList: AddressDto[];
}

export const useAddressStore = defineStore('addresses', {
  state: (): State => ({
    addressList: [] as AddressDto[],
  }),
  actions: {
    async getAddresses (payload) {
      const sign = await window.$nuxt.$axios.$get('addresses', { params: payload })
      this.loadAddresses(sign)
    },
    async getAddressById (payload) {
      return await window.$nuxt.$axios.$get(`addresses/${payload}`)
    },
    async deleteAddress (payload) {
      await window.$nuxt.$axios.$delete(`addresses/${payload}`)
      this.removeAddress(payload)
    },
    async createAddress (payload) {
      await window.$nuxt.$axios.$post('addresses', payload)
      this.addAddress(payload)
    },
    async updateAddress (payload) {
      await window.$nuxt.$axios.$put(`addresses/${payload.id}`, payload)
      this.setAddress(payload)
    },
    loadAddresses (value: AddressDto[]) {
      this.addressList = value
    },
    removeAddress (value: number) {
      this.addressList = this.addressList.filter(s => s.id !== value)
    },
    addAddress (value: AddressDto) {
      this.addressList.splice(0, 0, value)
    },
    setAddress (value: AddressDto) {
      const index = this.addressList.findIndex(s => s.id === value.id)
      this.addressList.splice(index, 1, value)
    },
  },
})
