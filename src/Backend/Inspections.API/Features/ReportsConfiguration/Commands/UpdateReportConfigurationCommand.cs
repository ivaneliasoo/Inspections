using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                                             List<int> signatureDefinitions)
        {
            Id = id;
            Type = type;
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
            ChecksDefinition = checksDefinition;
            SignatureDefinitions = signatureDefinitions;
        }

        public int Id { get; set; }
        public ReportType Type { get;  set; }
        public string Title { get;  set; }
        public string FormName { get;  set; }
        public string RemarksLabelText { get;  set; }
        public List<int> ChecksDefinition { get;  set; }
        public List<int> SignatureDefinitions { get;  set; }
    }
}
