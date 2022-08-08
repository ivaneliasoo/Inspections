import { PrintSectionStatus } from '../Models'
export interface PrintSectionDTO {
  id: number
  code: string
  description: string
  content: string
  isMainReport: boolean
  status: PrintSectionStatus
}
