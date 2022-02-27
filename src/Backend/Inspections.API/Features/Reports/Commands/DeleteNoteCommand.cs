using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class DeleteNoteCommand : IRequest<bool>
    {
        public int ReportId { get; }
        public int Id { get; }
        internal DeleteNoteCommand()
        {

        }

        public DeleteNoteCommand(int id, int reportId)
        {
            Id = id;
            ReportId = reportId;
        }
    }
}
