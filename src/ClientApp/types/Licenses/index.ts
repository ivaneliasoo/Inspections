export interface LicenseDTO {
  licenseId: number;
  number: string;
  validityStart: string;
  validityEnd: string;
}

export interface DateTimeRange {
  start: string;
  end: string;
}