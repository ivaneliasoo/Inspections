using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Reports.Handlers;

public class UpdateOperationreadingsCommandHandler : IRequestHandler<UpdateOperationalReadingsCommand, bool>
{
    private readonly InspectionsContext _context;
    private readonly IReportsRepository _reportsRepository;

    public UpdateOperationreadingsCommandHandler(InspectionsContext context, IReportsRepository reportsRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        this._reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
    }
    public async Task<bool> Handle(UpdateOperationalReadingsCommand request, CancellationToken cancellationToken)
    {
        var report = await _context.Set<Report>().AsNoTracking().Include("OperationalReadings").AsNoTracking().SingleOrDefaultAsync(r => r.Id == request.ReportId, cancellationToken: cancellationToken);
        // if (report?.OperationalReadings == null)
        // {
        //     return false;
        // }
        //
        // var op = new OperationalReadings()
        // {
        //     Id = request.Id,
        //     VoltageL1N = report.OperationalReadings.VoltageL1N != request.VoltageL1N ? request.VoltageL1N : report.OperationalReadings.VoltageL1N,
        //     VoltageL2N = report.OperationalReadings.VoltageL2N != request.VoltageL2N ? request.VoltageL2N : report.OperationalReadings.VoltageL2N,
        //     VoltageL3N = report.OperationalReadings.VoltageL3N != request.VoltageL3N ? request.VoltageL3N : report.OperationalReadings.VoltageL3N,
        //     VoltageL1L2 = report.OperationalReadings.VoltageL1L2 != request.VoltageL1L2 ? request.VoltageL1L2 : report.OperationalReadings.VoltageL1L2,
        //     VoltageL1L3 = report.OperationalReadings.VoltageL1L3 != request.VoltageL1L3 ? request.VoltageL1L3 : report.OperationalReadings.VoltageL1L3,
        //     VoltageL2L3 = report.OperationalReadings.VoltageL2L3 != request.VoltageL2L3 ? request.VoltageL2L3 : report.OperationalReadings.VoltageL2L3,
        //     RunningLoadL1 = report.OperationalReadings.RunningLoadL1 != request.RunningLoadL1 ? request.RunningLoadL1 : report.OperationalReadings.RunningLoadL1,
        //     RunningLoadL2 = report.OperationalReadings.RunningLoadL2 != request.RunningLoadL2 ? request.RunningLoadL2 : report.OperationalReadings.RunningLoadL2,
        //     RunningLoadL3 = report.OperationalReadings.RunningLoadL3 != request.RunningLoadL3 ? request.RunningLoadL3 : report.OperationalReadings.RunningLoadL3,
        //     MainBreakerAmp = report.OperationalReadings.MainBreakerAmp != request.MainBreakerAmp ? request.MainBreakerAmp : report.OperationalReadings.MainBreakerAmp,
        //     MainBreakerPoles = report.OperationalReadings.MainBreakerPoles != request.MainBreakerPoles ? request.MainBreakerPoles : report.OperationalReadings.MainBreakerPoles,
        //     MainBreakerCapacity = report.OperationalReadings.MainBreakerCapacity != request.MainBreakerCapacity ? request.MainBreakerCapacity : report.OperationalReadings.MainBreakerCapacity,
        //     OverCurrentDTLA = report.OperationalReadings.OverCurrentDTLA != request.OverCurrentDTLA ? request.OverCurrentDTLA : report.OperationalReadings.OverCurrentDTLA,
        //     OverCurrentDTLSec = report.OperationalReadings.OverCurrentDTLSec != request.OverCurrentDTLSec ? request.OverCurrentDTLSec : report.OperationalReadings.OverCurrentDTLSec,
        //     OverCurrentIDMTLA = report.OperationalReadings.OverCurrentIDMTLA != request.OverCurrentIDMTLA ? request.OverCurrentIDMTLA : report.OperationalReadings.OverCurrentIDMTLA,
        //     OverCurrentIDMTLTm = report.OperationalReadings.OverCurrentIDMTLTm != request.OverCurrentIDMTLTm ? request.OverCurrentIDMTLTm : report.OperationalReadings.OverCurrentIDMTLTm,
        //     EarthFaultMA = report.OperationalReadings.EarthFaultMA != request.EarthFaultMA ? request.EarthFaultMA : report.OperationalReadings.EarthFaultMA,
        //     EarthFaultELRA = report.OperationalReadings.EarthFaultELRA != request.EarthFaultELRA ? request.EarthFaultELRA : report.OperationalReadings.EarthFaultELRA,
        //     EarthFaultELRSec = report.OperationalReadings.EarthFaultELRSec != request.EarthFaultELRSec ? request.EarthFaultELRSec : report.OperationalReadings.EarthFaultELRSec,
        //     EarthFaultA = report.OperationalReadings.EarthFaultA != request.EarthFaultA ? request.EarthFaultA : report.OperationalReadings.EarthFaultA,
        //     EarthFaultSec = report.OperationalReadings.EarthFaultSec != request.EarthFaultSec ? request.EarthFaultSec : report.OperationalReadings.EarthFaultSec,
        //     MainBreakerRating = report.OperationalReadings.MainBreakerRating != request.MainBreakerRating ? request.MainBreakerRating : report.OperationalReadings.MainBreakerRating,
        //     OverCurrentDirectActingEnabled = report.OperationalReadings.OverCurrentDirectActingEnabled != request.OverCurrentDirectActingEnabled ? request.OverCurrentDirectActingEnabled : report.OperationalReadings.OverCurrentDirectActingEnabled,
        //     OverCurrentDirectActing = report.OperationalReadings.OverCurrentDirectActing != request.OverCurrentDirectActing ? request.OverCurrentDirectActing : report.OperationalReadings.OverCurrentDirectActing,
        //     OverCurrentDTLEnabled = report.OperationalReadings.OverCurrentDTLEnabled != request.OverCurrentDTLEnabled ? request.OverCurrentDTLEnabled : report.OperationalReadings.OverCurrentDTLEnabled,
        //     OverCurrentIDTMLEnabled = report.OperationalReadings.OverCurrentIDTMLEnabled != request.OverCurrentIDTMLEnabled ? request.OverCurrentIDTMLEnabled : report.OperationalReadings.OverCurrentIDTMLEnabled,
        //     EarthFaultRoobEnabled = report.OperationalReadings.EarthFaultRoobEnabled != request.EarthFaultRoobEnabled ? request.EarthFaultRoobEnabled : report.OperationalReadings.EarthFaultRoobEnabled,
        //     EarthFaultEIREnabled = report.OperationalReadings.EarthFaultEIREnabled != request.EarthFaultEIREnabled ? request.EarthFaultEIREnabled : report.OperationalReadings.EarthFaultEIREnabled,
        //     EarthFaultEFEnabled = report.OperationalReadings.EarthFaultEFEnabled != request.EarthFaultEFEnabled ? request.EarthFaultEFEnabled : report.OperationalReadings.EarthFaultEFEnabled,
        // };
        // report.AddOperationalReadings(op);
        await _reportsRepository.UpdateAsync(report);
        return true;
    }
}
