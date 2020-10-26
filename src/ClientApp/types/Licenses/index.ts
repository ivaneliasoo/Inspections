export interface LicenseDTO {
  licenseId: number;
  number: string;
  validity: DateTimeRange;
}

export interface DateTimeRange {
  start: string;
  end: string;
}