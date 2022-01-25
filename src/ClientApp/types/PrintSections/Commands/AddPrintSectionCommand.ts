import { PrintSectionStatus } from '../Models'
export interface AddPrintSectionCommand {
    code: string;
    description: string;
    content: string;
    isMainReport: boolean;
    responsableName: string;
    status: PrintSectionStatus;
}
