import { CheckValue } from '../Models/CheckValue'
import { CheckListParamDTO } from '../ViewModels/CheckListParamDTO'

export interface AddCheckListItemCommand {
  idCheckList: number
  text: string
  checked: CheckValue
  editable: boolean
  required: boolean
  remarks: string
  checklistParams: CheckListParamDTO[]
}
