import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { AddressDto } from '~/types'

export const state = () => ({
  addressList: [] as AddressDto[]
})

export type AddressesState = ReturnType<typeof state>

export const mutations: MutationTree<AddressesState> = {
  SET_ADDRESSES_LIST: (state, value: AddressDto[]) => (state.addressList = value),
  REMOVE_ADDRESS: (state, value: number) => (state.addressList = state.addressList.filter(s => s.id !== value)),
  ADD_ADDRESS: (state, value: AddressDto) => (state.addressList.splice(0, 0, value)),
  UPDATE_ADDRESS: (state, value: AddressDto) => {
    const index = state.addressList.findIndex(s => s.id === value.id)
    state.addressList.splice(index, 1, value)
  }
}

export const actions: ActionTree<AddressesState, RootState> = {
  async getAddresses ({ commit }, payload) {
    const sign = await this.$axios.$get('addresses', { params: payload })
    commit('SET_ADDRESSES_LIST', sign)
  },

  async getAddressById ({ commit }, payload) {
    return await this.$axios.$get(`addresses/${payload}`)
  },

  async deleteAddress ({ commit }, payload) {
    await this.$axios.$delete(`addresses/${payload}`)
    commit('REMOVE_ADDRESS', payload)
  },
  async createAddress ({ commit }, payload) {
    await this.$axios.$post('addresses', payload)
    commit('ADD_ADDRESS', payload)
  },
  async updateAddress ({ commit }, payload) {
    await this.$axios.$put(`addresses/${payload.id}`, payload)
    commit('UPDATE_ADDRESS', payload)
  }
}
