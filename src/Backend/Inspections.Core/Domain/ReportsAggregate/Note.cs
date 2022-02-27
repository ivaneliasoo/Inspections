using Inspections.Shared;

namespace Inspections.Core.Domain.ReportsAggregate;

public class Note : Entity<int>
{
    public int ReportId { get; set; }
    public string Text { get; set; } = default!;
    public bool Checked { get; set; }
    public bool NeedsCheck { get; set; }
}
