import { ReportType } from '../Models/ReportType'

export interface UpdateReportConfigurationCommand {
    id: number;
    type: ReportType;
    title: string;
    formName: string;
    remarksLabelText: string;
    checksDefinition: number[];
    signatureDefinitions: number[];
}
