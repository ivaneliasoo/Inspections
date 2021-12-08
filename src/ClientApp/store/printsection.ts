import { ActionTree, MutationTree } from 'vuex'
import { RootState } from 'store'
import { PrintSectionDTO } from '~/types/PrintSections/ViewModels/PrintSectionDTO'

export const state = () => ({
  printSectionsList: [] as PrintSectionDTO[]
})

export type PrintSectionState = ReturnType<typeof state>

export const mutations: MutationTree<PrintSectionState> = {
  SET_PRINT_SECTION_LIST: (state, value: PrintSectionDTO[]) => (state.printSectionsList = value),
  REMOVE_PRINT_SECTION: (state, value: number) => (state.printSectionsList = state.printSectionsList.filter(s => s.id !== value)),
  ADD_PRINT_SECTION: (state, value: PrintSectionDTO) => (state.printSectionsList.splice(0, 0, value)),
  UPDATE_PRINT_SECTION: (state, value: PrintSectionDTO) => {
    const index = state.printSectionsList.findIndex(s => s.id === value.id)
    state.printSectionsList.splice(index, 1, value)
  }
}

export const actions: ActionTree<PrintSectionState, RootState> = {
  async getPrintSections ({ commit }, payload) {
    const sign = await this.$axios.$get('printsection', { params: payload })
    commit('SET_PRINT_SECTION_LIST', sign)
  },

  async getPrintSectionById (payload) {
    return await this.$axios.$get(`printsection/${payload}`)
  },

  async deletePrintSection ({ commit }, payload) {
    await this.$axios.$delete(`printsection/${payload}`)
    commit('REMOVE_PRINT_SECTION', payload)
  },
  async createPrintSection ({ commit }, payload) {
    await this.$axios.$post('printsection', payload)
    commit('ADD_PRINT_SECTION', payload)
  },
  async updatePrintSection ({ commit }, payload) {
    payload.responsibleName = ''
    await this.$axios.$put(`printsection/${payload.id}`, payload)
    commit('UPDATE_PRINT_SECTION', payload)
  }
}
