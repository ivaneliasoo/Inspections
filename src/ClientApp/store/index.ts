import { GetterTree, ActionTree, MutationTree } from 'vuex'

export const state = () => ({
  isApiAvaliable: false as Boolean,
  showFabSaveButton: false as Boolean,
  canGoBack: false as Boolean
})

export type RootState = ReturnType<typeof state>

export const getters: GetterTree<RootState, RootState> = {
  apiStatus: state => state.isApiAvaliable
}

export const mutations: MutationTree<RootState> = {
  CHANGE_ISAPIAVAILABLE: (state, value: boolean) => (state.isApiAvaliable = value),
  CHANGE_SHOWFABSAVEBUTTON: (state, value: boolean) => (state.showFabSaveButton = value),
  CHANGE_CANGOBACK: (state, value: boolean) => (state.canGoBack = value)
}

export const actions: ActionTree<RootState, RootState> = {
  hasPendingChanges ({ commit }, payload) {
    commit('CHANGE_SHOWFABSAVEBUTTON', payload)
  },
  allowToGoBack ({ commit }, payload) {
    commit('CHANGE_CANGOBACK', payload)
  }
}
