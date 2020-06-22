import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { CheckList, CheckListItem } from '~/types'

export const state = () => ({
  checkLists: [] as CheckList[],
  checkListItems: [] as CheckListItem[]
})

export type CheckListsState = ReturnType<typeof state>

export const mutations: MutationTree<CheckListsState> = {
  SET_CHECKLISTS: (state, value: CheckList[]) => (state.checkLists = value),
  SET_CHECKLISTS_ITEMS: (state, value: CheckListItem[]) => (state.checkListItems = value)
}

export const actions: ActionTree<CheckListsState, RootState> = {
  async getChecklists ({ commit }, payload) {
    const checks = await this.$axios.$get('checklists', { params: payload })
    commit('SET_CHECKLISTS', checks)
  },
  async getCheckListItemsById ({ commit }, payload) {
    const checkListItems = await this.$axios.$get(`checklists/${payload}`)
    commit('SET_CHECKLISTS_ITEMS', checkListItems.checks)
  }
}
