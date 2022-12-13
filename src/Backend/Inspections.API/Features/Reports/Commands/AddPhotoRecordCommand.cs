using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class AddPhotoRecordCommand : IRequest<bool>
    {
        public int ReportId { get; }
        public IFormFileCollection FormFiles { get; }
        public string? Label { get; }
        public string? BaseUrl { get; }

        public AddPhotoRecordCommand(int reportId, IFormFileCollection formFiles, string? label, string? baseUrl)
        {
            ReportId = reportId;
            FormFiles = formFiles;
            Label = label ?? "";
            BaseUrl = baseUrl;
        }
    }
}
