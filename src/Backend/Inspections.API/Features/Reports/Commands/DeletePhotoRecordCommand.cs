using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class DeletePhotoRecordCommand : IRequest<bool>
    {
        public int ReportId { get; }
        public int Id { get; }
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
