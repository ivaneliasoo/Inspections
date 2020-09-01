
import { Responsable } from './Responsable'
import { ReportConfiguration } from '~/types/Reports/Models/ReportConfiguration'
import { Report } from '~/types'

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
    report: Report;
    reportConfigurationId: number | null;
    reportConfiguration: ReportConfiguration;
    drawedSign?: string
}
