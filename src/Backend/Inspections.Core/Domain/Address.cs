using System;
using System.Collections.Generic;
using System.Text;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain
{
    public class Address : Entity<int>
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string Unit { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string LicenseNumber { get; set; }

        public override string ToString()
        {
            return $"{AddressLine} {AddressLine2}, {Unit}, {Country}, {PostalCode}";
        }

    }
}
