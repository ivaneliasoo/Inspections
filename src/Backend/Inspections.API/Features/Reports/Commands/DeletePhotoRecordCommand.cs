using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class DeletePhotoRecordCommand : IRequest<bool>
    {
        public int ReportId { get; set; }
        public int Id { get; set; }
        internal DeletePhotoRecordCommand()
        {

        }

        public DeletePhotoRecordCommand(int id, int reportId)
        {
            Id = id;
            ReportId = reportId;
        }
    }
}
