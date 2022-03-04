import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { SignatureDTO } from '~/types/Signatures/ViewModels/SignatureDTO'

export const state = () => ({
  signaturesList: [] as SignatureDTO[]
})

export type SignatureState = ReturnType<typeof state>

export const mutations: MutationTree<SignatureState> = {
  SET_SIGNATURES_LIST: (state, value: SignatureDTO[]) => (state.signaturesList = value),
  REMOVE_SIGNATURE: (state, value: number) => (state.signaturesList = state.signaturesList.filter(s => s.id !== value)),
  ADD_SIGNATURE: (state, value: SignatureDTO) => (state.signaturesList.splice(0, 0, value)),
  UPDATE_SIGNATURE: (state, value: SignatureDTO) => {
    const index = state.signaturesList.findIndex(s => s.id === value.id)
    state.signaturesList.splice(index, 1, value)
  }
}

export const actions: ActionTree<SignatureState, RootState> = {
  async getSignatures ({ commit }, payload) {
    const sign = await this.$axios.$get('signatures', { params: payload })
    commit('SET_SIGNATURES_LIST', sign)
  },

  async getSignatureById ({ commit }, payload) {
    return await this.$axios.$get(`signatures/${payload}`)
  },

  async deleteSignature ({ commit }, payload) {
    await this.$axios.$delete(`signatures/${payload}`)
    commit('REMOVE_SIGNATURE', payload)
  },
  async createSignature ({ commit }, payload) {
    payload.responsibleName = ''
    await this.$axios.$post('signatures', payload)
    commit('ADD_SIGNATURE', payload)
  },
  async updateSignature ({ commit }, payload) {
    payload.responsibleName = ''
    await this.$axios.$put(`signatures/${payload.id}`, payload)
    commit('UPDATE_SIGNATURE', payload)
  }
}
