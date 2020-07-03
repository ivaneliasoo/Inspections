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
            Id = reportConfiguration.Id;
            Type = reportConfiguration.Type;
            Title = reportConfiguration.Title;
            FormName = reportConfiguration.FormName;
            RemarksLabelText = reportConfiguration.RemarksLabelText;
            ChecksDefinition = reportConfiguration.ChecksDefinition.Select(cd => cd.Id);
            SignatureDefinitions= reportConfiguration.SignatureDefinitions.Select(cd => cd.Id);
        }

        public int Id { get; set; }
        public ReportType Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
        public IEnumerable<int> ChecksDefinition { get; set; }
        public IEnumerable<int> SignatureDefinitions { get; set; }
    }
}
