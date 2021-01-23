using System;
using System.Collections.Generic;
using System.Text;

namespace ReportsApp.Models
{
    public class Validity
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class AddressDTO
    {
        public int Id { get; set; }
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string Unit { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public int LicenseId { get; set; }
        public string Number { get; set; }
        public Validity Validity { get; set; }
        public string FormatedAddress { get; set; }
        public override string ToString()
        {
            return FormatedAddress;
        }
    }
}
