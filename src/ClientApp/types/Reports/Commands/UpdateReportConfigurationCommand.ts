import { ReportType } from '../Models/ReportType'
import { CheckList } from '../../CheckLists/Models/CheckList';
import { Signature } from '../../Signatures/Models/Signature';

export interface UpdateReportConfigurationCommand {
    id: number;
    type: ReportType;
    title: string;
    formName: string;
    remarksLabelText: string;
    inactive: boolean;
    checksDefinition: number[] | CheckList[];
    signatureDefinitions: number[] | Signature[];
    printSectionId: number;
}
