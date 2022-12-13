using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Shared;
using Inspections.Core.Domain;

namespace Inspections.Core.QueryModels;

public class ReportQueryResult
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTimeOffset Date { get; set; }
    public EMALicense? License { get; set; }
    public string? LicenseNumber { get; set; } = "No Licensed";
    public string? LicenseName { get; set; } = "No Licensed";
    public decimal? LicenseKVA { get; set; } = 0;
    public decimal? LicenseVolt { get; set; } = 0;
    public decimal? LicenseAmp { get; set; } = 0;
    public int ReportConfigurationId { get; set; }
    public DateTimeRange? LicenseValidity { get; set; } = new()!;
    public string Title { get; set; } = default!;
    public string FormName { get; set; } = default!;
    public string? RemarksLabelText { get; set; }
    public bool IsClosed { get; set; }
    public bool NeedsPhotoRecord { get; set; }
    public IEnumerable<FormResult> Forms { get; set; } = default!;
    public IEnumerable<SignatureQueryResult> Signatures { get; set; } = default!;
    public IEnumerable<CheckListQueryResult> CheckLists { get; set; } = default!;
    public IEnumerable<NoteQueryResult> Notes { get; set; } = default!;
}
