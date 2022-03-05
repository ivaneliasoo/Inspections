using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class EditNoteCommand : IRequest<bool>
    {
        public int ReportId { get; }
        public int Id { get; }
        public string Text { get; } = default!;
        public bool Checked { get; }

        internal EditNoteCommand()
        {

        }

        public EditNoteCommand(int id, string text, bool @checked, int reportId)
        {
            Id = id;
            Text = text;
            Checked = @checked;
            ReportId = reportId;
        }
    }
}
