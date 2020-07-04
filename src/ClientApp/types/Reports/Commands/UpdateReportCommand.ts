import { EMALicenseType } from '..';


export interface UpdateReportCommand {
    id: number;
    name: string;
    address: string;
    licenseType: EMALicenseType;
    licenseNumber: string;
    date: string;
    isClosed: boolean;
}
