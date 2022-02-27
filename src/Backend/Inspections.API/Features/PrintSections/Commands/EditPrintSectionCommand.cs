using Inspections.Core.Domain.PrintSectionsAggregate;
using MediatR;

namespace Inspections.API.Features.PrintSections.Commands;

public class EditPrintSectionCommand : IRequest<bool>
{
    public EditPrintSectionCommand(int id,
        string code,
        string description,
        string content,
        bool isMainReport,
        PrintSectionStatus status)
    {
        Id = id;
        Code = code;
        Description = description;
        Content = content;
        IsMainReport = isMainReport;
        Status = status;
    }

    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public bool IsMainReport { get; set; }
    public PrintSectionStatus Status { get; set; }
}
