using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
{
    private readonly IReportsRepository _reportsRepository;

    public DeleteNoteCommandHandler(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository;
    }
    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);

        if (report == null)
        {
            return false;
        }

        var note = report.Notes.FirstOrDefault(n => n.Id == request.Id);
        if (note == null)
        {
            return false;
        }

        report.RemoveNote(note);
        await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);
        return true;
    }
}
