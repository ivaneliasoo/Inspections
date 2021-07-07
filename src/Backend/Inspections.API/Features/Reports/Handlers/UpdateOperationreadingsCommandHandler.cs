using System;
using System.Threading;
using System.Threading.Tasks;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class UpdateOperationreadingsCommandHandler : IRequestHandler<UpdateOperationalReadingsCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;

        public UpdateOperationreadingsCommandHandler(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }
        public async Task<bool> Handle(UpdateOperationalReadingsCommand request, CancellationToken cancellationToken)
        {

            var op = new OperationalReadings()
            {
                Id = request.Id,
                VoltageL1N = request.VoltageL1N,
                VoltageL2N = request.VoltageL2N,
                VoltageL3N = request.VoltageL3N,
                VoltageL1L2 = request.VoltageL1L2,
                VoltageL1L3 = request.VoltageL1L3,
                VoltageL2L3 = request.VoltageL2L3,
                RunningLoadL1 = request.RunningLoadL1,
                RunningLoadL2 = request.RunningLoadL2,
                RunningLoadL3 = request.RunningLoadL3,
                MainBreakerAmp = request.MainBreakerAmp,
                MainBreakerPoles = request.MainBreakerPoles,
                MainBreakerCapacity = request.MainBreakerCapacity,
                OverCurrentByMainBreaker = request.OverCurrentByMainBreaker,
                OverCurrentDTLA = request.OverCurrentDTLA,
                OverCurrentDTLSec = request.OverCurrentDTLSec,
                OverCurrentIDMTLA = request.OverCurrentIDMTLA,
                OverCurrentIDMTLTm = request.OverCurrentIDMTLTm,
                EarthFaultMA = request.EarthFaultMA,
                EarthFaultELRA = request.EarthFaultELRA,
                EarthFaultELRSec = request.EarthFaultELRSec,
                EarthFaultA = request.EarthFaultA,
                EarthFaultSec = request.EarthFaultSec
            };

            var report = await _reportsRepository.GetByIdAsync(request.ReportId);
            if (report is not null)
            {
                report.AddOperationalReadings(op);
                await _reportsRepository.UpdateAsync(report);
                return true;
            }

            return false;
        }
    }
}
