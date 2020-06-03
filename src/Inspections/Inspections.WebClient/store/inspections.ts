import { getterTree, mutationTree, actionTree } from 'nuxt-typed-vuex'

export const state = () => ({
  name: ''
})

// eslint-disable-next-line prettier/prettier
export const getters = getterTree(state, {

})

export const mutations = mutationTree(state, {
  setName(state, newName: string) {
    state.name = newName
  }
})

export const actions = actionTree(
  { state },
  {
    initialise({ commit }) {
      commit('setName', 'Ivan')
    },
    setName({ commit }, newName: string) {
      commit('setName', newName)
    }
  }
)
