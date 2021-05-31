import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { LicenseDTO } from '~/types'

export const state = () => ({
  licensesList: [] as LicenseDTO[],
  dashboard: { expiring: [], expired: [] } as { expiring: LicenseDTO[], expired: LicenseDTO[] }
})

export type LicensesState = ReturnType<typeof state>

export const mutations: MutationTree<LicensesState> = {
  SET_LICENSES_LIST: (state, value: LicenseDTO[]) => (state.licensesList = value),
  REMOVE_LICENSE: (state, value: number) => (state.licensesList = state.licensesList.filter(s=>s.licenseId !== value)),
  ADD_LICENSE: (state, value: LicenseDTO) => (state.licensesList.splice(0,0,value)),
  UPDATE_LICENSE: (state, value: LicenseDTO) => {
    const index = state.licensesList.findIndex(s=>s.licenseId === value.licenseId)
    state.licensesList.splice(index,1,value)
  },
  SET_DASHBOARD: (state, value: { expiring: LicenseDTO[], expired: LicenseDTO[] }) => (state.dashboard = value)
}

export const actions: ActionTree<LicensesState, RootState> = {
  async getLicenses ({ commit }, payload) {
    const sign = await this.$axios.$get('EMALicenses', { params: payload })
    commit('SET_LICENSES_LIST', sign)
  },

  async getLicensesDashboard({commit}, payload) {
    const dashboard = await this.$axios.$get('EMALicenses/dashboard')
    commit('SET_DASHBOARD', dashboard)
    return dashboard
  },

  async getLicenseById ({ commit }, payload) {
    return await this.$axios.$get(`EMALicenses/${payload}`)
  },

  async deleteLicense ({ commit }, payload) {
    await this.$axios.$delete(`EMALicenses/${payload}`)
    commit('REMOVE_LICENSE', payload)
  },
  async createLicense ({ commit }, payload) {
    await this.$axios.$post(`EMALicenses`, payload)
    commit('ADD_LICENSE', payload)
  },
  async updateLicense ({ commit }, payload) {
    await this.$axios.$put(`EMALicenses/${payload.licenseId}`, payload)
    commit('UPDATE_LICENSE', payload)
  }
}
