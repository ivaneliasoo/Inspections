using Inspections.Shared;

namespace Inspections.Core.Domain.PrintSectionsAggregate
{
    public class PrintSection : Entity<int>, IAggregateRoot
    {
        public string Code { get; set; } = default!;
        public string Content { get; set; } = default!;
        public bool IsMainReport { get; set; }
        public PrintSectionStatus Status { get; set; }
    }
}
