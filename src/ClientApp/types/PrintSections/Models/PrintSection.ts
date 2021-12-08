
import { PrintSectionStatus } from './PrintSectionStatus'
export interface PrintSection {
    id: number;
    code: string;
    content: string;
    isMainReport: boolean;
    status: PrintSectionStatus;
}
