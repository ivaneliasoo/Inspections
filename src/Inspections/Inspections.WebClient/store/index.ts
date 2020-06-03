import { actionTree, mutationTree, getAccessorType } from 'typed-vuex'

// Import all your submodules
import * as inspections from '~/store/inspections'

export const state = () => ({
  User: ''
})

type RootState = ReturnType<typeof state>

export const getters = {
  User: (state: RootState) => state.User,
  fullUser: (state: RootState) => state.User
}

export const mutations = mutationTree(state, {
  setUser(state, newValue: string) {
    state.User = newValue
  },

  initialiseStore() {
    console.log('initialised')
  }
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    // eslint-disable-next-line require-await
    async resetUser({ commit }) {
      commit('setUser', 'a@a.com')
    }
  }
)
// This compiles to nothing and only serves to return the correct type of the accessor
export const accessorType = getAccessorType({
  state,
  getters,
  mutations,
  actions,
  modules: {
    // The key (submodule) needs to match the Nuxt namespace (e.g. ~/store/submodule.ts)
    inspections
  }
})
