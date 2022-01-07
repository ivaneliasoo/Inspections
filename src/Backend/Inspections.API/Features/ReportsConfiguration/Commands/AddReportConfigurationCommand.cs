using System.Collections.Generic;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;

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
                                             int printSectionId, CheckListPrintingMetadata checklistMetadata)
        {
            Guard.Against.Null(checklistMetadata, nameof(checklistMetadata));
            
            Type = type;
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
            ChecksDefinition = checksDefinition;
            SignatureDefinitions = signatureDefinitions;
            PrintSectionId = printSectionId;
            ChecklistMetadata = checklistMetadata;
        }
        private AddReportConfigurationCommand()
        {
            ChecklistMetadata = new CheckListPrintingMetadata();
        }

        public ReportType Type { get; }
        public string Title { get; } = default!;
        public string FormName { get; } = default!;
        public string RemarksLabelText { get; } = default!;
        public List<int> ChecksDefinition { get; } = default!;
        public List<int> SignatureDefinitions { get; } = default!;
        public CheckListPrintingMetadata ChecklistMetadata { get; }
        public int PrintSectionId { get; }
    }
}
