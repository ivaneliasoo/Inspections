using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Inspections.Commands;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using Inspections.Shared;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;

        public UpdateReportCommandHandler(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }

        public async Task<bool> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var report = await _reportsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

            report.Edit(request.Name, request.Address, new EMALicense
            {
                LicenseType = request.LicenseType,
                Number = request.LicenseNumber,
                Validity = new DateTimeRange(DateTime.Now, DateTime.Now.AddYears(1))
            }, request.Date);

            if (request.IsClosed)
            {
                report.Close();
            }

            await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);

            return true;
        }
    }
}
