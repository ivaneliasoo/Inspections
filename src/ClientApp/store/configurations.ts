import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { ReportConfiguration, AddReportConfigurationCommand } from '~/types'

export const state = () => ({
  configurations: [] as ReportConfiguration[]
})

export type ReportConfigurationState = ReturnType<typeof state>

export const mutations: MutationTree<ReportConfigurationState> = {
  SET_CONFIGURATIONS: (state, value: ReportConfiguration[]) => (state.configurations = value),
  REMOVE_CONFIGURATION: (state, value: number) => {
    state.configurations = state.configurations.filter(c => c.id !== value)
  }
}

export const actions: ActionTree<ReportConfigurationState, RootState> = {
  async getConfigurations ({ commit }, payload) {
    const configs = await this.$axios.$get(`reportconfiguration?${payload ?? ''}`)
    commit('SET_CONFIGURATIONS', configs)
  },
  async getConfigurationById ({ commit }, payload) {
    return await this.$axios.$get(`reportconfiguration/${payload}`)
  },
  async createConfiguration ({ commit }, payload: AddReportConfigurationCommand) {
    return await this.$axios.$post(`reportconfiguration`, payload )
  },
  async updateConfiguration ({ commit }, payload: AddReportConfigurationCommand) {
    return await this.$axios.$put(`reportconfiguration`, payload )
  },
  async deleteConfiguration ({ commit }, payload: number) {
    return await this.$axios.$delete(`ReportConfiguration/${payload ?? 0}`, { data: { id: payload } } )
      .then(resp => {
        commit('REMOVE_CONFIGURATION', payload)
      })
  }
}
