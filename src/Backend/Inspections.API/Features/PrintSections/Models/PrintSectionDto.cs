using Ardalis.GuardClauses;
using Inspections.Core.Domain.PrintSectionsAggregate;
using JetBrains.Annotations;

namespace Inspections.API.Features.PrintSections.Models;

public class PrintSectionDto
{
    public int Id { [UsedImplicitly] get; }
    public string Code { [UsedImplicitly] get; }
    public string Description { [UsedImplicitly] get; }
    public string Content { [UsedImplicitly] get; }
    public bool IsMainReport { [UsedImplicitly] get; }
    public PrintSectionStatus Status { [UsedImplicitly] get; }

    public PrintSectionDto(PrintSection printSection)
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
