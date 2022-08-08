import { CheckListParamType } from '../Models/CheckListParamType'

export interface CheckListParamDTO {
  id: number
  key: string
  value: string
  type: CheckListParamType
}
