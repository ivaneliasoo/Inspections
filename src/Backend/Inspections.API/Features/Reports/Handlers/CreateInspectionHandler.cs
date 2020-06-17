using Inspections.API.Features.Inspections.Commands;
using Inspections.Core;
using Inspections.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Inspections.Handlers
{
    public class CreateInspectionHandler : IRequestHandler<Create, bool>
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IReportConfigurationsRepository _reportConfigurationsRepository;
        

        public CreateInspectionHandler(IReportsRepository reportsRepository, IReportConfigurationsRepository reportConfigurationsRepository)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            this._reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
        }

        public async Task<bool> Handle(Create request, CancellationToken cancellationToken)
        {
            // must be one configuratio per type
            var cfg = await _reportConfigurationsRepository.GetByIdAsync(1).ConfigureAwait(false);

            IReportsBuilder _reportsBuilder = new ReportsBuilder(cfg);
            var newReport = _reportsBuilder.WithDefaultNotes(true).Build();

            await _reportsRepository.AddAsync(newReport).ConfigureAwait(false);
            return true;                              ;
        }
    }
}
