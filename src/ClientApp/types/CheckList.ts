import { ReportConfiguration } from './ReportConfiguration'
import { CheckListParam } from './CheckListParam'

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
