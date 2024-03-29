import { DateTimeRange } from '../Licenses'

export interface AddressDto {
  id?: number;
  addressLine?: string;
  addressLine2?: string;
  unit?: string;
  country?: string;
  postalCode?: string;
  licenseId: number;
  number: string;
  validity: DateTimeRange;
  formatedAddress: string;
}
