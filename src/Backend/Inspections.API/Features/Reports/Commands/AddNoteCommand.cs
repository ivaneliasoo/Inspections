using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class AddNoteCommand : IRequest<int>
    {
        public int ReportId { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
        public bool NeedsCheck { get; internal set; }

        internal AddNoteCommand()
        {

        }

        public AddNoteCommand(int reportId, string text, bool @checked, bool needsCheck)
        {
            ReportId = reportId;
            Text = text;
            Checked = @checked;
            NeedsCheck = needsCheck;
        }
    }
}
