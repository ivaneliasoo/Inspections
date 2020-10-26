using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.Core.Domain;
using Inspections.Shared;

namespace Inspections.API.Features.Addresses.Models
{
    public class AddressDTO
    {
        public AddressDTO(Address address)
        {
            Id = address.Id;
            AddressLine = address.AddressLine;
            AddressLine2 = address.AddressLine2;
            Unit = address.Unit;
            Country = address.Country;
            PostalCode = address.PostalCode;
            LicenseId = address.LicenseId;
            Number = address.License.Number;
            Validity= address.License.Validity;
            FormatedAddress = address.ToString();
        }

        public AddressDTO()
        {

        }

        public int Id { get; set; }
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string Unit { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public int LicenseId { get; set; }
        public string Number { get; set; }
        public DateTimeRange Validity { get; set; }
        public string FormatedAddress { get; set; }
    }
}
