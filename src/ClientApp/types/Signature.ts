import { ReportConfiguration } from './ReportConfiguration'
import { Responsable } from './Responsable'

export interface Signature {
    id: number;
    title: string;
    annotation: string;
    responsable: Responsable;
    designation: string;
    remarks: string;
    date: string;
    principal: boolean;
    isConfiguration: boolean;
    reportId: number | null;
    // report: Report;
    reportConfigurationId: number | null;
    reportConfiguration: ReportConfiguration;
}
