using System;
using System.Collections.Generic;
using System.Text;
using Inspections.Shared;

namespace Inspections.Core.Domain
{
    public class EMALicense : Entity<int>
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string PersonInCharge { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public decimal Amp { get; set; }
        public decimal Volt { get; set; }
        public decimal KVA { get; set; }
        public DateTimeRange Validity { get; set; }
    }
}
