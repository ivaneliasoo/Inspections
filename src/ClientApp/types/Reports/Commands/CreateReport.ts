import { ReportType } from '../Models/ReportType'

export interface CreateReport {
  configurationId: number
  reportType: ReportType
}
