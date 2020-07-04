import { GetterTree, ActionTree, MutationTree } from 'vuex'

export const state = () => ({
  isApiAvaliable: false as Boolean,
  showFabSaveButton: false as Boolean
})

export type RootState = ReturnType<typeof state>

export const getters: GetterTree<RootState, RootState> = {
  apiStatus: state => state.isApiAvaliable
}

export const mutations: MutationTree<RootState> = {
  CHANGE_ISAPIAVAILABLE: (state, value: boolean) => (state.isApiAvaliable = value),
  CHANGE_SHOWFABSAVEBUTTON: (state, value: boolean) => ( state.showFabSaveButton = value )
}

export const actions: ActionTree<RootState, RootState> = {
  hasPendingChanges({commit}, payload) {
    commit('CHANGE_SHOWFABSAVEBUTTON', payload)
  }
}
