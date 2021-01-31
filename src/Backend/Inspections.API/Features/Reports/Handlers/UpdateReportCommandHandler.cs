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
using Inspections.Infrastructure.Data;
using Inspections.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Reports.Handlers
{
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
            var license = _context.Licenses.AsNoTracking().Where(l => l.Number == request.LicenseNumber).Select(li => new { li.Validity, li.Amp, li.Volt, li.KVA }).FirstOrDefault();
            //TODO: verify licences when comming form UI
            report.Edit(reportName, request.Address, new License
            {
                Number = request.LicenseNumber,
                Validity = license.Validity,
                Amp =license.Amp,
                Volt = license.Volt,
                Kva = license.KVA,
            }, request.Date); ;

            if (!report.IsClosed && request.IsClosed)
            {
                report.Close();
            }

            await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);

            return true;
        }
    }
}
