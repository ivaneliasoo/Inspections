import { CheckListParam } from './CheckListParam'
import { CheckValue } from '~/services/api'

export interface CheckListItem {
  id: number
  checkListId: number
  text: string
  checked: CheckValue
  editable: boolean
  required: boolean
  remarks: string
  textParams: CheckListParam[]
}
