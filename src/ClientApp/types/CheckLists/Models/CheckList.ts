
import { CheckListParam } from './CheckListParam'
import { ReportConfiguration } from '~/types/Reports/Models/ReportConfiguration'

export interface CheckList {
    id: number;
    reportId: number | null;
    // report: Report;
    reportConfigurationId: number | null;
    reportConfiguration: ReportConfiguration;
    text: string;
    textParams: CheckListParam[];
    annotation: string;
    isConfiguration: boolean;
}
