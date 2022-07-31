/* eslint-disable no-empty-pattern */
import { defineStore } from 'pinia'
import {
  CheckList,
  UpdateCheckListCommand,
  DeleteCheckListItem,
  UpdateCheckListItemCommand,
  AddCheckListItemCommand,
  AddCheckListCommand
} from '~/types'

export interface State {
  checkLists: CheckList[],
  currentCheckList: CheckList
}

export const useChecklistStore = defineStore('checklists', {
  state: (): State => ({
    checkLists: [] as CheckList[],
    currentCheckList: {} as CheckList
  }),
  actions: {
    async getChecklists (payload) {
      const checks = await window.$nuxt.$axios.$get('checklists', { params: payload })
      this.SET_CHECKLISTS(checks)
    },
    async getCheckListItemsById (payload) {
      const checkListResult = await window.$nuxt.$axios.$get(`checklists/${payload}`)
      this.SET_CURRENT_CHECKLIST(checkListResult)
      return checkListResult
    },
    async updateCheckList (payload: UpdateCheckListCommand) {
      await window.$nuxt.$axios.$put(`checklists/${payload.idCheckList}`, payload)
      this.UPDATE_CHECKLIST(payload)
    },
    async createCheckList (payload: AddCheckListCommand) {
      return await window.$nuxt.$axios.$post('checklists', payload)
    },
    async deleteCheckList (payload) {
      await window.$nuxt.$axios.$delete(`checklists/${payload.idCheckList}`)
      this.DELETE_CHECKLIST(payload.id)
    },
    async createCheckListItem (payload: AddCheckListItemCommand) {
      await window.$nuxt.$axios.$post(
        `checklists/${payload.idCheckList}/items/`,
        payload
      )
    },
    async updateCheckListItem (payload) {
      await window.$nuxt.$axios.$put(
        `checklists/${payload.checkListId}/items/${payload.id}`,
        payload
      )
      this.UPDATE_CHECKLIST_ITEM(payload)
    },
    async deleteCheckListItem (payload: DeleteCheckListItem) {
      await window.$nuxt.$axios.$delete(
        `checklists/${payload.idCheckList}/items/${payload.idCheckListItem}`
      )
      this.DELETE_CHECKLIST_ITEM(payload.idCheckListItem)
    },
    async updateCheckListItemParams (payload) {
      await window.$nuxt.$axios.$put(
        `checklists/${payload.idCheckList}/items/${payload.idCheckListItem}`,
        payload
      )
    },
    SET_CHECKLISTS (value: CheckList[]) { this.checkLists = value },
    SET_CURRENT_CHECKLIST (value: CheckList) {
      this.currentCheckList = value
    },
    UPDATE_CHECKLIST (value: CheckList) {
      this.currentCheckList.annotation = value.annotation
      this.currentCheckList.text = value.text
    },
    DELETE_CHECKLIST_ITEM (value: number) {
      this.currentCheckList.checks = this.currentCheckList.checks.filter(
        ci => ci.id !== value
      )
    },
    DELETE_CHECKLIST (value: number) {
      this.checkLists = this.checkLists.filter(c => c.id !== value)
    },
    UPDATE_CHECKLIST_ITEM (value: UpdateCheckListItemCommand) {
      const index: number = this.currentCheckList.checks.findIndex(
        ci => ci.id === value.id
      )
      const temp = this.currentCheckList.checks[index]
      temp.checked = value.checked
      temp.editable = value.editable
      temp.remarks = value.remarks
      temp.required = value.required
      temp.text = value.text
      this.currentCheckList.checks.splice(index, 1, temp)
    }
  }
})
