import { ResponsableType } from '../Models'

export interface SignatureDTO {
    title: string;
    annotation: string;
    responsableType: ResponsableType;
    responsableName: string;
    designation: string;
    remarks: string;
    date: string;
    principal: boolean;
}
