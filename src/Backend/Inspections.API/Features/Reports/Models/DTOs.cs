namespace Inspections.API.Features.Reports.Models
{
    public record ExportDto(string PageUrl, int PhotosPerPage = 8, int ReportConfigurationId = 1);
}
