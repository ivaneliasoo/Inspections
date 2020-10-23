using Inspections.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.ReportsAggregate
{
    public class License : ValueObject
    {
        public string Number { get; set; }
        public DateTimeRange Validity { get; set; }
    }
}
