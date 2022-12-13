using Ardalis.GuardClauses;
using Inspections.Shared;

namespace Inspections.Core.Domain.ReportsAggregate;

public class PhotoRecord : Entity<int>
{
    public int ReportId { get; private set; }
    public string FileName { get; set; } = default!;
    public string FileNameResized { get; set; } = default!;
    public string? Label { get; set; }

    public string? PhotoStorageId { get; set; }
    public string? ThumbnailStorageId { get; set; }

    public string? BaseURL { get; set; } = default;

    public string? PhotoUrl
    {
        get => $"{BaseURL}api/reports/{Id}/photo";
    }

    public string? ThumbnailUrl
    {
        get => $"{BaseURL}api/reports/{Id}/thumbnail";
    }

    public class Date
    {
        private int _month = 7;  // Backing store

        public int Month
        {
            get => _month;
            set
            {
                if ((value > 0) && (value < 13))
                {
                    _month = value;
                }
            }
        }
    }

    public byte[]? Photo { get; set; }
    public byte[]? Thumbnail { get; set; }

    private PhotoRecord() { }

    public PhotoRecord(int reportId, string path, string pathResized, string? label)
    {
        ReportId = reportId;
        Label = label;
        FileName = path;
        FileNameResized = pathResized;
    }

    public PhotoRecord(int reportId, string baseUrl, string? label, byte[] photo, byte[] thumbnail)
    {
        ReportId = reportId;
        BaseURL = baseUrl;
        FileName = "";
        FileNameResized = "";
        Label = label;
        Photo = photo;
        Thumbnail = thumbnail;
    }

    public PhotoRecord(int reportId, string path, string pathResized, string? label, byte[] photo, byte[] thumbnail)
    {
        ReportId = reportId;
        Label = label;
        FileName = path;
        FileNameResized = pathResized;
        Photo = photo;
        Thumbnail = thumbnail;
    }

    public void SetMetadata(string photoStorageId, string thumbnailStorageId, string photoUrl, string thumbnailUrl)
    {
        Guard.Against.NullOrWhiteSpace(photoStorageId, nameof(photoStorageId));
        Guard.Against.NullOrWhiteSpace(thumbnailStorageId, nameof(thumbnailStorageId));
        Guard.Against.NullOrWhiteSpace(photoUrl, nameof(photoUrl));
        Guard.Against.NullOrWhiteSpace(thumbnailUrl, nameof(thumbnailUrl));

        PhotoStorageId = photoStorageId;
        ThumbnailStorageId = thumbnailStorageId;
    }

    public override string ToString()
    {
        return FileName;
    }
}

public class PhotoRecordResult : Entity<int>
{
    public int ReportId { get; private set; }
    public string FileName { get; set; } = default!;
    public string FileNameResized { get; set; } = default!;
    public string? Label { get; set; }
    public string? PhotoUrl { get; set; } = default!;
    public string? ThumbnailUrl { get; set; } = default!;
    public string? PhotoStorageId { get; set; }
    public string? ThumbnailStorageId { get; set; }
    public string? PhotoBase64 { get; set; } = default!;
    public string? ThumbnailBase64 { get; set; } = default!;
    public byte[]? Photo { get; set; } = default;
    public byte[]? Thumbnail { get; set; } = default;
    public DateTimeOffset Timestamp { get; set; }
}
