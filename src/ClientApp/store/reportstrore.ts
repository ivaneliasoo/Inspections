import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { Report, CreateReport } from '~/types'

export const state = () => ({
  reportList: [] as Report[]
})

export type ReportsState = ReturnType<typeof state>

export const mutations: MutationTree<ReportsState> = {
  SET_REPORT_LIST: (state, value: Report[]) => (state.reportList = value),
  REMOVE_REPORT: (state, value: number) => (state.reportList = state.reportList.filter(r=>r.id !== value))
}

export const actions: ActionTree<ReportsState, RootState> = {
  async getReports ({ commit }, payload) {
    const configs = await this.$axios.$get(`reports`, { params: payload })
    commit('SET_REPORT_LIST', configs)
  },
  async getReportById ({ }, payload) {
    const report = await this.$axios.$get(`reports/${payload}`)
    return report
  },
  async getReportPhotos ({ }, payload) {
    const photos = await this.$axios.$get(`reports/${payload}/photorecord`)
    return photos
  },
  // eslint-disable-next-line no-empty-pattern
  async createReport ({}, payload: CreateReport) {
    const reportId = await this.$axios.$post('reports', payload)
    return reportId
  },
  async deleteReport ({ commit }, payload: number) {
    await this.$axios.$delete(`reports/${payload}`)
    commit('REMOVE_REPORT', payload)
  }
}
