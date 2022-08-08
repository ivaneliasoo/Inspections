import { defineStore } from 'pinia'
import { Report, CreateReport } from '~/types'

export interface State {
  reportList: Report[]
}

export const useReportsStore = defineStore('reports', {
  state: (): State => ({
    reportList: [] as Report[],
  }),
  actions: {
    async getReports(payload) {
      const reports = await window.$nuxt.$axios.$get('reports', {
        params: payload,
      })
      this.SET_REPORT_LIST(reports)
    },
    async getReportById(payload) {
      const report = await window.$nuxt.$axios.$get(`reports/${payload}`)
      return report
    },
    async getReportPhotos(payload) {
      const photos = await window.$nuxt.$axios.$get(
        `reports/${payload}/photorecord`
      )
      return photos
    },
    async createReport(payload: CreateReport) {
      const reportId = await window.$nuxt.$axios.$post('reports', payload)
      return reportId
    },
    async deleteReport(payload: number) {
      await window.$nuxt.$axios.$delete(`reports/${payload}`)
      this.REMOVE_REPORT(payload)
    },
    SET_REPORT_LIST(value: Report[]) {
      this.reportList = value
    },
    REMOVE_REPORT(value: number) {
      this.reportList = this.reportList.filter((r) => r.id !== value)
    },
  },
})
