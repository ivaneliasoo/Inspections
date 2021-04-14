using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class CompleteReportCommandHandler : IRequestHandler<CompleteReportCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;

        public CompleteReportCommandHandler(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }

        public async Task<bool> Handle(CompleteReportCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));


            var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);

            if (!report.Completed || !report.Signatures.Any(s => s.ResponsableName?.Length > 0 && s.DrawedSign?.Length > 0))
            {
                throw new InvalidOperationException("Can't close a report that has pending checks or principal signature is empty. Please verify.");
            }
            else
            {
                report.Close();
                await _reportsRepository.UpdateAsync(report);
                return true;
            }

        }
    }
}
