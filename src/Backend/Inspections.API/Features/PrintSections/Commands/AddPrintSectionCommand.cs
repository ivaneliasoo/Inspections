
using Inspections.Core.Domain.PrintSectionsAggregate;
using MediatR;

namespace Inspections.API.Features.PrintSections.Commands
{

    public class AddPrintSectionCommand : IRequest<bool>
    {
        public AddPrintSectionCommand(string code,
                                      string content,
                                      string description,
                                      bool isMainReport,
                                      PrintSectionStatus status)
        {
            Code = code;
            Content = content;
            Description = description;
            IsMainReport = isMainReport;
            Status = status;

        }

        private AddPrintSectionCommand() { }

        public string Code { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsMainReport { get; set; }
        public PrintSectionStatus Status { get; set; }

    }
}
