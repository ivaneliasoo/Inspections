import { defineStore } from 'pinia'
import { User, ChangePasswordDTO } from '~/types/Users'

export interface State {
  users: User[]
}

export const useUsersStore = defineStore('users', {
  state: (): State => ({
    users: [] as User[],
  }),
  actions: {
    async getUsers() {
      const users = await window.$nuxt.$axios.$get('users')
      this.loadUsers(users)
    },
    async getUserByName(payload) {
      return await window.$nuxt.$axios.$get(`users/${payload}`)
    },
    async createUser(payload: User) {
      const reportId: number = await window.$nuxt.$axios.$post('users', payload)
      return reportId
    },
    async updateUser(payload: User) {
      return await window.$nuxt.$axios.$put(
        `users/${payload.userName}`,
        payload
      )
    },
    async setUserLastEditedReport(payload: {
      userName: string
      lastEditedReport: number
    }) {
      const userData: User = await window.$nuxt.$axios.$get(
        `users/${payload.userName}`
      )
      userData.lastEditedReport = payload.lastEditedReport
      return await window.$nuxt.$axios.$put(
        `users/${userData.userName}`,
        userData
      )
    },
    async changePassword(payload: ChangePasswordDTO) {
      return await window.$nuxt.$axios.$patch(
        `users/${payload.userName}`,
        payload
      )
    },
    async deleteUser(payload: string) {
      return await window.$nuxt.$axios
        .$delete(`users/${payload ?? 0}`, { data: { userName: payload } })
        .then(() => {
          this.removeUser(payload)
        })
    },
    loadUsers(value: User[]) {
      this.users = value
    },
    removeUser(value: string) {
      this.users = this.users.filter((c) => c.userName !== value)
    },
  },
})
