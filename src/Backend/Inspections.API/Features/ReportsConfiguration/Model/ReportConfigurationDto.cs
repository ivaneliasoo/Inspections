using Ardalis.GuardClauses;
using Inspections.API.Features.Forms;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using JetBrains.Annotations;
// ReSharper disable MemberCanBePrivate.Global

namespace Inspections.API.Features.ReportsConfiguration.Model;

public class ReportConfigurationDto
{
    public ReportConfigurationDto(ReportConfiguration reportConfiguration)
    {
        Guard.Against.Null(reportConfiguration, nameof(Report));
        Id = reportConfiguration.Id;
        Type = reportConfiguration.Type;
        Title = reportConfiguration.Title;
        FormName = reportConfiguration.FormName;
        Forms = reportConfiguration.Forms.Select(FormDefinitionResponse.CreateFromEntity);
        RemarksLabelText = reportConfiguration.RemarksLabelText;
        ChecksDefinition = reportConfiguration.ChecksDefinition?.Select(cd => cd.Id);
        SignatureDefinitions = reportConfiguration.SignatureDefinitions.Select(cd => cd.Id);
        PrintSectionId = reportConfiguration.PrintSectionId;
        CheckListMetadata = reportConfiguration.CheckListMetadata;
        TemplateName = reportConfiguration.TemplateName;
        Display = reportConfiguration.CheckListMetadata.Display;
    }

    public int Id { [UsedImplicitly] get; }
    public ReportType Type { [UsedImplicitly] get; }
    public string Title { [UsedImplicitly] get; }
    public string FormName { [UsedImplicitly] get; }
    public string RemarksLabelText { [UsedImplicitly] get; }
    public IEnumerable<int>? ChecksDefinition { [UsedImplicitly] get; }
    public IEnumerable<int> SignatureDefinitions { [UsedImplicitly] get; }
    public IEnumerable<FormDefinitionResponse> Forms { [UsedImplicitly] get; }
    public CheckListPrintingMetadata CheckListMetadata { [UsedImplicitly] get; }
    public int? PrintSectionId { [UsedImplicitly] get; }
    public string? TemplateName { [UsedImplicitly] get; }
    public CheckListDisplay Display { [UsedImplicitly] get;  }
}
