
import { CheckListParam } from './CheckListParam'
import { ReportConfiguration } from '~/types/Reports/Models/ReportConfiguration'
import { CheckListItem } from './CheckListItem'

export interface CheckList {
    id: number;
    reportId: number | null;
    // report: Report;
    reportConfigurationId: number | null;
    reportConfiguration: ReportConfiguration;
    text: string;
    checks: CheckListItem[];
    textParams: CheckListParam[];
    annotation: string;
    isConfiguration: boolean;
}
