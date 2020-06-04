using Inspections.Shared;

namespace Inspections.Core.Domain.InspectionReportAggregate
{
    public class Note : Entity<int>
    {
        public string Text { get; set; }
        public bool Checked { get; set; }
        public bool ShouldCheck { get; set; }
    }
}
