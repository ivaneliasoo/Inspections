export interface AddressDTO {
  id?: number;
  addressLine?: string;
  addressLine2?: string;
  unit?: string;
  country?: string;
  postalCode?: string;
  licenseNumber?: string;
  formatedAddress: string;
}