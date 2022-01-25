using Ardalis.GuardClauses;
using Inspections.Core.Domain.PrintSectionsAggregate;

namespace Inspections.API.Features.PrintSections.Models
{

    public class PrintSectionDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Content { get; set; } = default!;
        public bool IsMainReport { get; set; }
        public PrintSectionStatus Status { get; set; }

        public PrintSectionDTO(PrintSection printSection)
        {
            Guard.Against.Null(printSection, nameof(printSection));

            Id = printSection.Id;
            Code = printSection.Code;
            Description = printSection.Description;
            Content = printSection.Content;
            IsMainReport = printSection.IsMainReport;
            Status = printSection.Status;

        }
    }
}
