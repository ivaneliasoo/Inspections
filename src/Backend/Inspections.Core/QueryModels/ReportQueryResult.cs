using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Shared;

namespace Inspections.Core.QueryModels;

public class ReportQueryResult
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTimeOffset Date { get; set; }
    public string? LicenseNumber { get; set; } = default!;
    public string? LicenseName { get; set; } = default!;
    public decimal? LicenseKVA { get; set; }
    public decimal? LicenseVolt { get; set; }
    public decimal? LicenseAmp { get; set; }
    public int ReportConfigurationId { get; set; }
    public DateTimeRange? LicenseValidity { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string FormName { get; set; } = default!;
    public string? RemarksLabelText { get; set; }
    public bool IsClosed { get; set; }
    public IEnumerable<FormResult> Forms { get; set; } = default!;
    public IEnumerable<SignatureQueryResult> Signatures { get; set; } = default!;
    public IEnumerable<CheckListQueryResult> CheckLists { get; set; } = default!;
    public IEnumerable<NoteQueryResult> Notes { get; set; } = default!;
}
