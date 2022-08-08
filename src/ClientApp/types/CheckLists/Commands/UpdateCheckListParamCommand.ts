import { CheckListParamType } from '../Models/CheckListParamType'

export interface UpdateCheckListParamCommand {
  idCheckList: number | null
  idCheckListItem: number | null
  id: number
  key: string
  value: string
  type: CheckListParamType
}
