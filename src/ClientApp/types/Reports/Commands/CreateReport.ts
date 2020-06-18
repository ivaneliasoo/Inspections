import { ReportType } from '../Models/ReportType'

export interface CreateReport {
    name:string;
    configurationId: number;
    reportType: ReportType;
}
