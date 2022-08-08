import { ReportType } from '../Models/ReportType'

export interface ReportConfigurationDTO {
  id: number
  type: ReportType
  title: string
  formName: string
  remarksLabelText: string
  inactive: boolean
}
