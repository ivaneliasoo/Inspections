using Inspections.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.InspectionReportAggregate
{
    public class EMALicense : ValueObject
    {
        public EMALIcenseType Type { get; set; }
        public string Number { get; set; }
        public DateTimeRange Validity { get; set; }
    }
}