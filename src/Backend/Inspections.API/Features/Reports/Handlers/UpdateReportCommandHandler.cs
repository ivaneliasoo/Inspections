using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Reports.Handlers;

public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, bool>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly InspectionsContext _context;

    public UpdateReportCommandHandler(IReportsRepository reportsRepository, InspectionsContext context)
    {
        _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var report = await _reportsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);
        var reportName = $"{request.Date:yyyyMMdd}-{report.Title}-{request.LicenseNumber}-{request.Address}";
        var license = _context.Licenses.AsNoTracking().FirstOrDefault(l => l.Number == request.LicenseNumber);
        if (license is null)
            throw new Exception($"Conflict. can't find License {request.LicenseNumber}");

        report.Edit(reportName, request.Address, license, request.Date);
        if (!report.IsClosed && request.IsClosed)
        {
            report.Close();
        }

        await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);

        return true;
    }
}
