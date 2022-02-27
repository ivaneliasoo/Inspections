namespace Inspections.API.Models.Configuration;

public class ClientSettings
{
    public string ReportsImagesFolder { get; set; } = "MediaUpload/img/Reports";
    public string S3BucketName { get; set; } = default!;
}
