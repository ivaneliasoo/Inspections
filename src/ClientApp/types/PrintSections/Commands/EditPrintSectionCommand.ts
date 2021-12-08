import { PrintSectionStatus } from '../Models'
export interface EditPrintSectionCommand {
    id: number;
    code: string;
    content: string;
    isMainReport: boolean;
    responsableName: string;
    status: PrintSectionStatus;
}
