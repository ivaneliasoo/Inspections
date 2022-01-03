using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;
using System.Collections.Generic;

namespace Inspections.API.Features.ReportsConfiguration.Commands
{
    public class AddReportConfigurationCommand : IRequest<int>
    {
        public AddReportConfigurationCommand(ReportType type,
                                             string title,
                                             string formName,
                                             string remarksLabelText,
                                             List<int> checksDefinition,
                                             List<int> signatureDefinitions,
                                             int printSectionId)
        {
            Type = type;
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
            ChecksDefinition = checksDefinition;
            SignatureDefinitions = signatureDefinitions;
            PrintSectionId = printSectionId;
        }
        private AddReportConfigurationCommand() { }

        public ReportType Type { get; set; }
        public string Title { get; set; } = default!;
        public string FormName { get; set; } = default!;
        public string RemarksLabelText { get; set; } = default!;
        public List<int> ChecksDefinition { get; } = default!;
        public List<int> SignatureDefinitions { get; } = default!;
        public int PrintSectionId { get; } = default!;
    }
}
