using System.Collections.Generic;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Commands
{
    public class UpdateReportConfigurationCommand : IRequest<bool>
    {
        public UpdateReportConfigurationCommand(int id,
                                             ReportType type,
                                             string title,
                                             string formName,
                                             string remarksLabelText,
                                             List<int> checksDefinition,
                                             List<int> signatureDefinitions,
                                             int printSectionId)
        {
            Id = id;
            Type = type;
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
            ChecksDefinition = checksDefinition;
            SignatureDefinitions = signatureDefinitions;
            PrintSectionId = printSectionId;
        }

        private UpdateReportConfigurationCommand() { }

        public int Id { get; set; }
        public ReportType Type { get; set; }
        public string Title { get; set; } = default!;
        public string FormName { get; set; } = default!;
        public string RemarksLabelText { get; set; } = default!;
        public List<int>? ChecksDefinition { get; }
        public List<int>? SignatureDefinitions { get; }
        public int PrintSectionId { get; } = default!;
    }
}
