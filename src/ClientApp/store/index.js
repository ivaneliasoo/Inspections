export const state = () => ({
  isApiAvaliable: false,
  showFabSaveButton: false,
  canGoBack: false
})

export const getters = {
  apiStatus: state => state.isApiAvaliable
}

export const mutations = {
  CHANGE_ISAPIAVAILABLE: (state, value) => (state.isApiAvaliable = value),
  CHANGE_SHOWFABSAVEBUTTON: (state, value) => (state.showFabSaveButton = value),
  CHANGE_CANGOBACK: (state, value) => (state.canGoBack = value)
}

export const actions = {
  hasPendingChanges ({ commit }, payload) {
    commit('CHANGE_SHOWFABSAVEBUTTON', payload)
  },
  allowToGoBack ({ commit }, payload) {
    commit('CHANGE_CANGOBACK', payload)
  }
}
