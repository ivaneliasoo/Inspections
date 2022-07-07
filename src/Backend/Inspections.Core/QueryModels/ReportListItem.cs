namespace Inspections.Core.QueryModels;

public class ReportListItem
{
    public int Id { get; set; }
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public DateTimeOffset Date { get; set; }
    public string? LicenseNumber { get; set; } = default!;
    public string? LicenseName { get; set; } = default!;
    public string? LicenseEmail { get; set; } = default!;
    public string? LicenseContact { get; set; } = default!;
    public decimal? LicenseKVA { get; set; }
    public decimal? LicenseVolt { get; set; }
    public decimal? LicenseAmp { get; set; }
    public string Title { get; set; }
    public DateTime? LicenseValidityStart { get; set; } = default!;
    public DateTime? LicenseValidityEnd { get; set; } = default!;
    public bool IsClosed { get; set; }
    public bool HasSignatures { get; set; }
    public int CompletedSignaturesCount { get; set; }
    public bool IsCheckListsCompleted { get; set; }
    public bool HasNotes { get; set; }
    public int SignaturesCount { get; set; }
    public bool HasPhotoRecords { get; set; }
    public int PhotosCount { get; set; }
    public int ReportConfigurationId { get; set; }
}
