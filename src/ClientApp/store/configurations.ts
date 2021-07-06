import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { ReportConfiguration, AddReportConfigurationCommand, UpdateReportConfigurationCommand } from '~/types'

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
  async updateConfiguration ({ commit }, payload: UpdateReportConfigurationCommand) {
    return await this.$axios.$put(`reportconfiguration/${payload.id}`, payload )
  },
  async deleteConfiguration ({ commit }, payload: number) {
    return await this.$axios.$delete(`reportconfiguration/${payload ?? 0}`)
      .then(() => {
        commit('REMOVE_CONFIGURATION', payload)
      })
  }
}
