using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.ReportsConfiguration.Commands
{
    public class AddReportConfigurationCommand : IRequest<bool>
    {
        public AddReportConfigurationCommand(ReportType type,
                                             string title,
                                             string formName,
                                             string remarksLabelText,
                                             List<int> checksDefinition,
                                             List<int> signatureDefinitions)
        {
            Type = type;
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
            ChecksDefinition = checksDefinition;
            SignatureDefinitions = signatureDefinitions;
        }

        public ReportType Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
        public List<int> ChecksDefinition { get; set; }
        public List<int> SignatureDefinitions { get; set; }
    }
}
