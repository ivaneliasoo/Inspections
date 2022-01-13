using Inspections.Shared;

namespace Inspections.Core.Domain
{
    public class EMALicense : Entity<int>
    {
        public string Number { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? PersonInCharge { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public decimal Amp { get; set; }
        public decimal Volt { get; set; }
        public decimal KVA { get; set; }
        public DateTimeRange Validity { get; set; } = default!;
    }
}
