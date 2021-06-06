using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;

namespace Inspections.API.Features.Reports.Handlers
{
    public class EditNoteCommandHandler : IRequestHandler<EditNoteCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;

        public EditNoteCommandHandler(IReportsRepository reportsRepository)
        {
            this._reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }
        public async Task<bool> Handle(EditNoteCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);

            var note = report.Notes.Where(n => n.Id == request.Id).FirstOrDefault();
            if (note is not null)
            {
                report.RemoveNote(note);

                note.Text = request.Text;
                note.Checked = request.Checked;

                report.AddNote(note);
            }
                
            await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);
            return true;
        }
    }
}
