import { PrintSectionStatus } from '../Models'
export interface PrintSectionDTO {
    id:number;
    code: string;
    content: string;
    isMainReport: boolean;
    status: PrintSectionStatus;
}
