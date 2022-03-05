
import { CheckListParam } from './CheckListParam'
import { CheckListItem } from './CheckListItem'
import { ReportConfiguration } from '~/types/Reports/Models/ReportConfiguration'

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
