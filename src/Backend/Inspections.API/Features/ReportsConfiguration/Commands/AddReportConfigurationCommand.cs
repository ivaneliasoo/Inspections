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
                                             int printSectionId, CheckListDisplay display)
        {
            Guard.Against.Null(display, nameof(display));
            
            Type = type;
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
            ChecksDefinition = checksDefinition;
            SignatureDefinitions = signatureDefinitions;
            PrintSectionId = printSectionId;
            Display = display;
        }
        private AddReportConfigurationCommand()
        {
            Display = CheckListDisplay.Numbered;
        }

        public ReportType Type { get; }
        public string Title { get; } = default!;
        public string FormName { get; } = default!;
        public string RemarksLabelText { get; } = default!;
        public List<int> ChecksDefinition { get; } = default!;
        public List<int> SignatureDefinitions { get; } = default!;
        public CheckListDisplay Display { get; }
        public int PrintSectionId { get; }
    }
}
