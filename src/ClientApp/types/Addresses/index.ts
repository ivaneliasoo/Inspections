import { DateTimeRange } from '../Licenses'

export interface AddressDto {
  id?: number
  company?: string
  addressLine?: string
  addressLine2?: string
  unit?: string
  country?: string
  postalCode?: string
  attentionTo?: string
  licenseId: number
  number: string
  validity: DateTimeRange
  formatedAddress: string
}
