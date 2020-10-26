using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.Core.Domain;
using Inspections.Shared;

namespace Inspections.API.Features.Licenses.Models
{
    public class LicenseDTO
    {
        public LicenseDTO(int licenseId, string number, DateTimeRange validity)
        {
            LicenseId = licenseId;
            Number = number;
            Validity = validity;
        }

        public LicenseDTO(EMALicense eMALicense)
        {
            LicenseId = eMALicense.Id;
            Number = eMALicense.Number;
            Validity = eMALicense.Validity;
        }

        internal LicenseDTO()
        {

        }

        public int LicenseId { get; set; }
        public string Number { get; set; }
        public DateTimeRange Validity { get; set; }
    }
}
