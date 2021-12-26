using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
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
            Guard.Against.Null(reportConfiguration, nameof(Report));
            Id = reportConfiguration.Id;
            Type = reportConfiguration.Type;
            Title = reportConfiguration.Title;
            FormName = reportConfiguration.FormName;
            RemarksLabelText = reportConfiguration.RemarksLabelText;
            ChecksDefinition = reportConfiguration.ChecksDefinition.Select(cd => cd.Id);
            SignatureDefinitions= reportConfiguration.SignatureDefinitions.Select(cd => cd.Id);
            PrintSectionId = reportConfiguration.PrintSectionId;

        }

        public int Id { get; set; }
        public ReportType Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
        public IEnumerable<int> ChecksDefinition { get; set; }
        public IEnumerable<int> SignatureDefinitions { get; set; }
        public int? PrintSectionId { get; set; }
    }
}
