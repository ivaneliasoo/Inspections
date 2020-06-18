import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { Report, CreateReport } from '~/types'

export const state = () => ({
  reportList: [] as Report[]
})

export type ReportsState = ReturnType<typeof state>

export const mutations: MutationTree<ReportsState> = {
  SET_REPORT_LIST: (state, value: Report[]) => (state.reportList = value)
}

export const actions: ActionTree<ReportsState, RootState> = {
  async getReports ({ commit }, payload) {
    const configs = await this.$axios.$get(`reports?${payload ?? ''}`)
    commit('SET_REPORT_LIST', configs)
  },
  // eslint-disable-next-line no-empty-pattern
  async createReport ({}, payload: CreateReport) {
    await this.$axios.$post(`reports`, payload)
  }
}