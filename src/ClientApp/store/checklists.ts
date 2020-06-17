import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { CheckList } from '~/types'

export const state = () => ({
  checkLists: [] as CheckList[]
})

export type CheckListsState = ReturnType<typeof state>

export const mutations: MutationTree<CheckListsState> = {
  SET_CHECKLISTS: (state, value: CheckList[]) => (state.checkLists = value)
}

export const actions: ActionTree<CheckListsState, RootState> = {
  async getChecklists ({ commit }, payload) {
    const checks = await this.$axios.$get(`checklists?${payload ?? ''}`)
    commit('SET_CHECKLISTS', checks)
  }
}
