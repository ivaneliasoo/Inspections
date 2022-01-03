using Ardalis.GuardClauses;
using Inspections.Shared;

namespace Inspections.Core.Domain.ReportsAggregate
{
    public class PhotoRecord : Entity<int>
    {
        public int ReportId { get; private set; }
        public string FileName { get; set; } = default!;
        public string FileNameResized { get; set; } = default!;
        public string? Label { get; set; }

        public string? PhotoStorageId { get; set; }
        public string? ThumbnailStorageId { get; set; }
        public string? PhotoUrl { get; set; } = default!;
        public string? ThumbnailUrl { get; set; } = default!;

        private PhotoRecord() { }

        public PhotoRecord(int reportId, string path, string pathResized, string? label)
        {
            ReportId = reportId;
            Label = label;
            FileName = path;
            FileNameResized = pathResized;
        }

        public void SetMetadata(string photoStorageId, string thumbnailStorageId, string photoUrl, string thumbnailUrl)
        {
            Guard.Against.NullOrWhiteSpace(photoStorageId, nameof(photoStorageId));
            Guard.Against.NullOrWhiteSpace(thumbnailStorageId, nameof(thumbnailStorageId));
            Guard.Against.NullOrWhiteSpace(photoUrl, nameof(photoUrl));
            Guard.Against.NullOrWhiteSpace(thumbnailUrl, nameof(thumbnailUrl));

            PhotoStorageId = photoStorageId;
            ThumbnailStorageId = thumbnailStorageId;
            PhotoUrl = photoUrl;
            ThumbnailUrl = thumbnailUrl;
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
    }
}
