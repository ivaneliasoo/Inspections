using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class EditNoteCommand : IRequest<bool>
    {
        public int ReportId { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }

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
