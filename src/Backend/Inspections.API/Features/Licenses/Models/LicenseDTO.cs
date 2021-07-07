using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.Core.Domain;
using Inspections.Shared;

namespace Inspections.API.Features.Licenses.Models
{
    public class LicenseDTO
    {
        public LicenseDTO(int licenseId, string number, DateTimeRange validity)
        {
            Guard.Against.Null(validity, nameof(validity));

            LicenseId = licenseId;
            Number = number;
            ValidityStart = validity.Start;
            ValidityEnd = validity.End;
        }

        public LicenseDTO(EMALicense eMALicense)
        {
            Guard.Against.Null(eMALicense, nameof(eMALicense));

            LicenseId = eMALicense.Id;
            Number = eMALicense.Number;
            Name = eMALicense.Name;
            PersonInCharge = eMALicense.PersonInCharge;
            Contact = eMALicense.Contact;
            Email = eMALicense.Email;
            Amp = eMALicense.Amp;
            Volt = eMALicense.Volt;
            KVA = eMALicense.KVA;
            ValidityStart = eMALicense.Validity.Start;
            ValidityEnd = eMALicense.Validity.End;
        }

        internal LicenseDTO()
        {

        }

        public int LicenseId { get; set; }
        public string Number { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? PersonInCharge { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public decimal Amp { get; set; }
        public decimal Volt { get; set; }
        public decimal KVA { get; set; }
        public DateTime ValidityStart { get; set; }
        public DateTime ValidityEnd { get; set; }
    }
}
