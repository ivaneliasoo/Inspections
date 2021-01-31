using Inspections.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.ReportsAggregate
{
    public class License : ValueObject
    {
        public string Number { get; set; }
        public DateTimeRange Validity { get; set; }
        public decimal Amp { get; set; }
        public decimal Volt { get; set; }
        public decimal Kva { get; set; }
    }
}
