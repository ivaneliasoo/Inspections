import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { CheckList, UpdateCheckListCommand, DeleteCheckListItem, UpdateCheckListItemCommand, AddCheckListItemCommand } from '~/types'

export const state = () => ({
  checkLists: [] as CheckList[],
  currentCheckList: {} as CheckList
})

export type CheckListsState = ReturnType<typeof state>

export const mutations: MutationTree<CheckListsState> = {
  SET_CHECKLISTS: (state, value: CheckList[]) => (state.checkLists = value),
  SET_CURRENT_CHECKLIST: (state, value: CheckList) => (state.currentCheckList = value),
  UPDATE_CHECKLIST: (state, value: CheckList) => {
    state.currentCheckList.annotation = value.annotation
    state.currentCheckList.text = value.text
  },
  DELETE_CHECKLIST_ITEM: (state, value: number) => {
    state.currentCheckList.checks = state.currentCheckList.checks.filter(ci=>ci.id !== value)
  },
  DELETE_CHECKLIST: (state, value: number) => {
    // const index: number = state.currentCheckList.checks.findIndex(ci=>ci.id === value)
    state.checkLists = state.checkLists.filter(c=>c.id !== value)
  },
  UPDATE_CHECKLIST_ITEM: (state, value: UpdateCheckListItemCommand) => {
    const index: number = state.currentCheckList.checks.findIndex(ci=>ci.id === value.id)
    let temp = state.currentCheckList.checks[index] 
      temp.checked = value.checked
      temp.remarks = value.remarks
      temp.required = value.required
      temp.text = value.text
    state.currentCheckList.checks.splice(index, 1, temp)
  }
}

export const actions: ActionTree<CheckListsState, RootState> = {
  async getChecklists ({ commit }, payload) {
    const checks = await this.$axios.$get('checklists', { params: payload })
    commit('SET_CHECKLISTS', checks)
  },
  async getCheckListItemsById ({ commit }, payload) {
    const checkListResult = await this.$axios.$get(`checklists/${payload}`)
    commit('SET_CURRENT_CHECKLIST', checkListResult)
    return checkListResult
  },
  async updateCheckList({commit}, payload: UpdateCheckListCommand) {
    await this.$axios.$put(`checklists/${payload.idCheckList}`, payload)
    commit('UPDATE_CHECKLIST', payload)
  },
  async deleteCheckList({commit}, payload) {
    await this.$axios.$delete(`checklists/${payload.idCheckList}`)
    commit('DELETE_CHECKLIST', payload.id)
  },
  async createCheckListItem({commit}, payload: AddCheckListItemCommand) {
    await this.$axios.$post(`checklists/${payload.idCheckList}/items/`, payload)
  },
  async updateCheckListItem({commit}, payload) {
    await this.$axios.$put(`checklists/${payload.checkListId}/items/${payload.id}`, payload)
    commit('UPDATE_CHECKLIST_ITEM', payload)
  },
  async deleteCheckListItem({commit}, payload:DeleteCheckListItem) {
    await this.$axios.$delete(`checklists/${payload.idCheckList}/items/${payload.idCheckListItem}`)
    commit('DELETE_CHECKLIST_ITEM', payload.idCheckListItem)
  },
  async updateCheckListItemParams({commit}, payload) {
    await this.$axios.$put(`checklists/${payload.idCheckList}/items/${payload.idCheckListItem}`, payload)
  }
}
