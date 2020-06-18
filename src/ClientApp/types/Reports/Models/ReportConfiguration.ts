
import { ReportType } from './ReportType'
import { CheckList } from '~/types/CheckLists/Models/CheckList'
import { Signature } from '~/types/Signatures/Models/Signature'

export interface ReportConfiguration {
    id: number;
    type: ReportType;
    title: string;
    formName: string;
    remarksLabelText: string;
    checksDefinition: CheckList[];
    signatureDefinitions: Signature[];
}
