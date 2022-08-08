import { ResponsableType } from '../Models'

export interface SignatureDTO {
  id: number
  title: string
  annotation: string
  responsableType: ResponsableType
  responsableName: string
  designation: string
  remarks: string
  date: string
  principal: boolean
  drawedSign: string
  viewSign?: boolean
}
