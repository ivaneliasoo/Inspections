import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { User, ChangePasswordDTO } from '~/types/Users'

export const state = () => ({
  users: [] as User[]
})

export type UserState = ReturnType<typeof state>

export const mutations: MutationTree<UserState> = {
  SET_USERS: (state, value: User[]) => (state.users = value),
  REMOVE_USER: (state, value: string) => {
    state.users = state.users.filter(c => c.userName !== value)
  }
}

export const actions: ActionTree<UserState, RootState> = {
  async getUsers ({ commit }, payload) {
    const users = await this.$axios.$get('users')
    commit('SET_USERS', users)
  },
  async getUserByName ({ commit }, payload) {
    return await this.$axios.$get(`users/${payload}`)
  },
  async createUser ({ commit }, payload: User) {
    const reportId: number = await this.$axios.$post('users', payload)
    return reportId
  },
  async updateUser ({ commit }, payload: User) {
    return await this.$axios.$put(`users/${payload.userName}`, payload)
  },
  async setUserLastEditedReport ({ commit }, payload: { userName: string, lastEditedReport: number}) {
    const userData: User = await this.$axios.$get(`users/${payload.userName}`)
    userData.lastEditedReport = payload.lastEditedReport
    return await this.$axios.$put(`users/${userData.userName}`, userData)
  },
  async changePassword ({ commit }, payload: ChangePasswordDTO) {
    return await this.$axios.$patch(`users/${payload.userName}`, payload)
  },
  async deleteUser ({ commit }, payload: string) {
    return await this.$axios.$delete(`users/${payload ?? 0}`, { data: { userName: payload } })
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      .then((resp) => {
        commit('REMOVE_USER', payload)
      })
  }
}
