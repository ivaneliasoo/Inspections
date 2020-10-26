using System;
using System.Collections.Generic;
using System.Text;
using Inspections.Shared;

namespace Inspections.Core.Domain
{
    public class EMALicense : Entity<int>
    {
        public string Number { get; set; }
        public DateTimeRange Validity { get; set; }
        //public Address Address { get; set; }
    }
}
