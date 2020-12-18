export interface LicenseDTO {
  licenseId: number;
  number: string;
  Name: string;
  PersonInCharge: string;
  Contact: string;
  Email: string;
  Amp: number;
  Volt: number;
  KVA: number;
  validityStart: string;
  validityEnd: string;
}

export interface DateTimeRange {
  start: string;
  end: string;
}
