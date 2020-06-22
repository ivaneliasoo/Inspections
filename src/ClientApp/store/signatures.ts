import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { SignatureDTO } from '~/types/Signatures/ViewModels/SignatureDTO'

export const state = () => ({
  signaturesList: [] as SignatureDTO[]
})

export type SignatureState = ReturnType<typeof state>

export const mutations: MutationTree<SignatureState> = {
  SET_SIGNATURES_LIST: (state, value: SignatureDTO[]) => (state.signaturesList = value)
}

export const actions: ActionTree<SignatureState, RootState> = {
  async getSignatures ({ commit }, payload) {
    const sign = await this.$axios.$get('signatures', { params: payload })
    commit('SET_SIGNATURES_LIST', sign)
  }
}
