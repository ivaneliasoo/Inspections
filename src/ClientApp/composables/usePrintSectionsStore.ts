import { defineStore } from 'pinia'
import { PrintSectionDTO } from '~/types/PrintSections/ViewModels/PrintSectionDTO'

export interface State {
  printSectionsList: PrintSectionDTO[]
}

export const usePrintSectionsStore = defineStore('printsections', {
  state: (): State => ({
    printSectionsList: [],
  }),
  actions: {
    async getPrintSections(payload) {
      const printSect = await window.$nuxt.$axios.$get('printsection', {
        params: payload,
      })
      this.setList(printSect)
    },
    async getPrintSectionById(payload) {
      return await window.$nuxt.$axios.$get(`printsection/${payload}`)
    },
    async deletePrintSection(payload) {
      await window.$nuxt.$axios.$delete(`printsection/${payload}`)
      this.remove(payload)
    },
    async createPrintSection(payload) {
      await window.$nuxt.$axios.$post('printsection', payload)
      this.add(payload)
    },
    async updatePrintSection(payload) {
      await window.$nuxt.$axios.$put(`printsection/${payload.id}`, payload)
      this.udpate(payload)
    },
    setList(value: PrintSectionDTO[]) {
      this.printSectionsList = value
    },
    remove(value: number) {
      this.printSectionsList = this.printSectionsList.filter(
        (s) => s.id !== value
      )
    },
    add(value: PrintSectionDTO) {
      this.printSectionsList.splice(0, 0, value)
    },
    udpate(value: PrintSectionDTO) {
      const index = this.printSectionsList.findIndex((s) => s.id === value.id)
      this.printSectionsList.splice(index, 1, value)
    },
  },
})
