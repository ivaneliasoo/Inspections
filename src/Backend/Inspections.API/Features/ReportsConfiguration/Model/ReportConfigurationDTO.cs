using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;

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
            SignatureDefinitions = reportConfiguration.SignatureDefinitions.Select(cd => cd.Id);
            PrintSectionId = reportConfiguration.PrintSectionId;
            CheckListMetadata = reportConfiguration.CheckListMetadata;
            OperationalReadings = reportConfiguration.OperationalReadings;
            AdditionalFileds = reportConfiguration.AdditionalFields;
            TemplateName = reportConfiguration.TemplateName;
        }

        public int Id { get; set; }
        public ReportType Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
        public IEnumerable<int> ChecksDefinition { get; set; }
        public IEnumerable<int> SignatureDefinitions { get; set; }
        public CheckListPrintingMetadata CheckListMetadata { get; set; }
        public DynamicFields? OperationalReadings { get; set; }
        public DynamicFields? AdditionalFileds { get; set; }
        public int? PrintSectionId { get; set; }
        public string? TemplateName { get; set; }
    }
}
