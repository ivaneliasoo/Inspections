using Inspections.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.ReportsAggregate
{
    public class EMALicense : ValueObject
    {
        public EMALicenseType LicenseType { get; set; }
        public string Number { get; set; }
        public DateTimeRange Validity { get; set; }
    }
}
