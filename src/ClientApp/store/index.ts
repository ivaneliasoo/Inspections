import { GetterTree, ActionTree, MutationTree } from 'vuex'

export const state = () => ({
  isApiAvaliable: false as Boolean
})

export type RootState = ReturnType<typeof state>

export const getters: GetterTree<RootState, RootState> = {
  apiStatus: state => state.isApiAvaliable
}

export const mutations: MutationTree<RootState> = {
  CHANGE_ISAPIAVAILABLE: (state, value: boolean) => (state.isApiAvaliable = value)
}

export const actions: ActionTree<RootState, RootState> = {
//   checkApi ({ commit }) {
//     const things = this.$axios.$get('/api/health')
//     console.log(things)
//     commit('CHANGE_NAME', things)
//   }
}
