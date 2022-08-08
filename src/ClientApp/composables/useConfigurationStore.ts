import { defineStore } from 'pinia'
import {
  ReportConfiguration,
  AddReportConfigurationCommand,
  UpdateReportConfigurationCommand,
} from '~/types'

export interface State {
  configurations: ReportConfiguration[]
}

export const useConfigurationStore = defineStore('configuration', {
  state: (): State => ({
    configurations: [] as ReportConfiguration[],
  }),
  actions: {
    async getConfigurations(payload) {
      const configs = await window.$nuxt.$axios.$get(
        `reportconfiguration?${payload ?? ''}`
      )
      this.SET_CONFIGURATIONS(configs)
    },
    async getConfigurationById(payload) {
      return await window.$nuxt.$axios.$get(`reportconfiguration/${payload}`)
    },
    async createConfiguration(
      payload: AddReportConfigurationCommand | ReportConfiguration
    ) {
      return await window.$nuxt.$axios.$post('reportconfiguration', payload)
    },
    async updateConfiguration(payload: UpdateReportConfigurationCommand) {
      return await window.$nuxt.$axios.$put(
        `reportconfiguration/${payload.id}`,
        payload
      )
    },
    async deleteConfiguration(payload: number) {
      return await window.$nuxt.$axios
        .$delete(`reportconfiguration/${payload ?? 0}`)
        .then(() => {
          this.REMOVE_CONFIGURATION(payload)
        })
    },
    SET_CONFIGURATIONS(value: ReportConfiguration[]) {
      this.configurations = value
    },
    REMOVE_CONFIGURATION(value: number) {
      this.configurations = this.configurations.filter((c) => c.id !== value)
    },
  },
})
