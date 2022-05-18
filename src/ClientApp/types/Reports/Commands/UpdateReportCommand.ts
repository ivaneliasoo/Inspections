import { EMALicenseType } from '../Models/EMALicenseType'

export interface UpdateReportCommand {
    id: number;
    name: string;
    address: string;
    licenseType: EMALicenseType;
    licenseNumber: string;
    date: string;
    isClosed: boolean;
}
