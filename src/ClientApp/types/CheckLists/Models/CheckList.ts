
import { CheckListParam } from './CheckListParam'
import { ReportConfiguration } from '~/types/Reports/Models/ReportConfiguration'
import { CheckListItem } from './CheckListItem'

export interface CheckList {
    id: number;
    reportId: number | null;
    reportConfigurationId: number | null;
    reportConfiguration: ReportConfiguration;
    text: string;
    checks: CheckListItem[];
    checked: boolean;
    textParams: CheckListParam[];
    annotation: string;
    isConfiguration: boolean;
}
