using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.Forms;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain.ReportConfigurationAggregate;

public class ReportConfiguration : Entity<int>, IAggregateRoot
{
    public ReportType Type { get; set; }
    public string Title { get; set; } = default!;
    public string FormName { get; set; } = default!;
    public string RemarksLabelText { get; set; } = default!;
    public CheckListPrintingMetadata CheckListMetadata { get; set; } = new();
    public List<CheckList>? ChecksDefinition { get; set; }
    public List<Signature> SignatureDefinitions { get; set; } = default!;
    public bool Inactive { get; set; }
    public string Footer { get; set; } = default!;
    public string MarginTop { get; set; } = default!;
    public string MarginBottom { get; set; } = default!;
    public string MarginLeft { get; set; } = default!;
    public string MarginRight { get; set; } = default!;
    public int? PrintSectionId { get; set; }
    public string? TemplateName { get; set; }
    public bool UseNotes { get; set; }
    public bool UsePhotoRecord { get; set; }
    public bool UseCheckList { get; set; }

    private readonly List<FormDefinition> forms = new();
    public IReadOnlyCollection<FormDefinition> Forms => forms;
}
