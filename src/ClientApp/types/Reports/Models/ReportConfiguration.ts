
import { CheckListPrintingMetadata } from '../../PrintSections/index'
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
    inactive: boolean;
    printSectionId: number;
    templateName: string;
    checkListPrintingMetadata: CheckListPrintingMetadata;
    display: string;
    useNotes: boolean;
    useCheckList: boolean;
    usePhotoRecord: boolean;
}
