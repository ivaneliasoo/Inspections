import { ReportType } from '../Models/ReportType'

export interface AddReportConfigurationCommand {
    type: ReportType;
    title: string;
    formName: string;
    remarksLabelText: string;
    inactive: boolean;
    checksDefinition: number[];
    signatureDefinitions: number[];
}
