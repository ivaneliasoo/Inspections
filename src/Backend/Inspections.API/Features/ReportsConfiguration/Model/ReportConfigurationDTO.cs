using Inspections.Core.Domain.ReportConfigurationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.ReportsConfiguration.Model
{
    public class ReportConfigurationDTO
    {
        public ReportConfigurationDTO(ReportConfiguration reportConfiguration)
        {
            Type = reportConfiguration.Type;
            Title = reportConfiguration.Title;
            FormName = reportConfiguration.FormName;
            RemarksLabelText = reportConfiguration.RemarksLabelText;
        }

        public ReportType Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
    }
}
