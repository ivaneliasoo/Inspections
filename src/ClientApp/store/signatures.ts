import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { SignatureDTO } from '~/types/Signatures/ViewModels/SignatureDTO'

export const state = () => ({
  signaturesList: [] as SignatureDTO[]
})

export type SignatureState = ReturnType<typeof state>

export const mutations: MutationTree<SignatureState> = {
  SET_SIGNATURES_LIST: (state, value: SignatureDTO[]) => (state.signaturesList = value),
  REMOVE_SIGNATURE: (state, value: number) => (state.signaturesList = state.signaturesList.filter(s=>s.id !== value))
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
  }
}
