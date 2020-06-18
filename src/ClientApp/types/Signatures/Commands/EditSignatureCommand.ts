import { ResponsableType } from '../Models'

export interface EditSignatureCommand {
    id: number;
    title: string;
    annotation: string;
    responsableType: ResponsableType;
    responsableName: string;
    designation: string;
    remarks: string;
    date: string;
    principal: boolean;
}
