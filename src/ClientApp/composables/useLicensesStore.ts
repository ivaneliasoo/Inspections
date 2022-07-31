import { defineStore } from 'pinia'
import { LicenseDTO } from '~/types'

interface State {
  licensesList: LicenseDTO[];
  dashboard: { expiring: LicenseDTO[]; expired: LicenseDTO[] };
}

export const useLicensesStore = defineStore('licenses', {
  state: (): State => ({
    licensesList: [] as LicenseDTO[],
    dashboard: { expiring: [], expired: [] } as {
      expiring: LicenseDTO[];
      expired: LicenseDTO[];
    },
  }),
  actions: {
    async getLicenses (payload) {
      const licenses = await window.$nuxt.$axios.$get('EMALicenses', {
        params: payload,
      })
      this.loadLicensesList(licenses === '' ? [] : licenses)
    },

    async getLicensesDashboard () {
      const dashboard = await window.$nuxt.$axios.$get('EMALicenses/dashboard')
      this.setDashboard(dashboard)
      return dashboard
    },

    async getLicenseById (payload) {
      return await window.$nuxt.$axios.$get(`EMALicenses/${payload}`)
    },

    async deleteLicense (payload) {
      await window.$nuxt.$axios.$delete(`EMALicenses/${payload}`)
      this.removeLicense(payload)
    },
    async createLicense (payload) {
      await window.$nuxt.$axios.$post('EMALicenses', payload)
      this.addLicense(payload)
    },
    async updateLicense (payload) {
      await window.$nuxt.$axios.$put(`EMALicenses/${payload.licenseId}`, payload)
      this.update(payload)
    },
    loadLicensesList (value: LicenseDTO[]) {
      this.licensesList = value
    },
    removeLicense (value: number) {
      this.licensesList = this.licensesList.filter(
        s => s.licenseId !== value
      )
    },
    addLicense (value: LicenseDTO) {
      this.licensesList.splice(0, 0, value)
    },
    update (value: LicenseDTO) {
      const index = this.licensesList.findIndex(
        s => s.licenseId === value.licenseId
      )
      this.licensesList.splice(index, 1, value)
    },
    setDashboard (value: { expiring: LicenseDTO[]; expired: LicenseDTO[] }) {
      this.dashboard = value
    },
  },
})
