import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { ReportConfiguration } from '~/types'

export const state = () => ({
  configurations: [] as ReportConfiguration[]
})

export type ReportConfigurationState = ReturnType<typeof state>

export const mutations: MutationTree<ReportConfigurationState> = {
  SET_CONFIGURATIONS: (state, value: ReportConfiguration[]) => (state.configurations = value)
}

export const actions: ActionTree<ReportConfigurationState, RootState> = {
  async getConfigurations ({ commit }, payload) {
    const configs = await this.$axios.$get(`reportconfiguration?${payload ?? ''}`)
    commit('SET_CONFIGURATIONS', configs)
  }
}
