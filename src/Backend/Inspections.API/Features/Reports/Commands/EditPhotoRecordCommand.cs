using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class EditPhotoRecordCommand : IRequest<bool>
    {
        public int ReportId { get; }
        public int Id { get; }
        public string Label { get; }

        public EditPhotoRecordCommand(int reportId, string label, int id)
        {
            ReportId = reportId;
            Label = label;
            Id = id;
        }
    }
}
