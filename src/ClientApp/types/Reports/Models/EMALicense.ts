import { EMALicenseType } from './EMALicenseType'

export interface EMALicense {
    licenseType: EMALicenseType;
    number: string;
    validity: string;
}
