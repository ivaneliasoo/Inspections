using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers;

public class AddNoteCommandHandler : IRequestHandler<AddNoteCommand, int>
{
    private readonly IReportsRepository _reportsRepository;

    public AddNoteCommandHandler(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
    }

    public async Task<int> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);
        report.AddNote(new Note()
        {
            Text = request.Text,
            Checked = request.Checked,
            ReportId = request.ReportId,
            NeedsCheck = request.NeedsCheck
        });
        await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);

        var updatedReport = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);

        return updatedReport.Notes.Last()?.Id ?? 0;
    }
}
