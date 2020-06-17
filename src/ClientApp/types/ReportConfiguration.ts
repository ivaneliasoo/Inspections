import { CheckList } from './CheckList'
import { ReportType } from './ReportType'
import { Signature } from './Signature'

export interface ReportConfiguration {
    id: number;
    type: ReportType;
    title: string;
    formName: string;
    remarksLabelText: string;
    checksDefinition: CheckList[];
    signatureDefinitions: Signature[];
}
