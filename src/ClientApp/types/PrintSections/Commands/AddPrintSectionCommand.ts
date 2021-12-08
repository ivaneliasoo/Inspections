import { PrintSectionStatus } from '../Models'
export interface AddPrintSectionCommand {
    code: string;
    content: string;
    isMainReport: boolean;
    responsableName: string;
    status: PrintSectionStatus;
}
