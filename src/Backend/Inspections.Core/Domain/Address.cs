using System;
using System.Collections.Generic;
using System.Text;
using Inspections.Shared;

namespace Inspections.Core.Domain
{
    public class Address : Entity<int>
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public override string ToString()
        {
            return $"{AddressLine} {AddressLine2}, {City}, {Province}";
        }

    }
}
