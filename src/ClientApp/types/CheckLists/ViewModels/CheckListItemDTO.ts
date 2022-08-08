import { CheckValue } from '../Models/CheckValue'
import { CheckListParamDTO } from './CheckListParamDTO'

export interface CheckListItemDTO {
  id: number
  checkListId: number
  text: string
  checked: CheckValue
  required: boolean
  remarks: string
  textParams: CheckListParamDTO[]
}
