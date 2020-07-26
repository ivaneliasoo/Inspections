using Ardalis.GuardClauses;
using Inspections.API.Features.Inspections.Commands;
using Inspections.Core;
using Inspections.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Inspections.Handlers
{
    public class CreateInspectionHandler : IRequestHandler<CreateReportCommand, int>
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IReportConfigurationsRepository _reportConfigurationsRepository;
        

        public CreateInspectionHandler(IReportsRepository reportsRepository, IReportConfigurationsRepository reportConfigurationsRepository)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            this._reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
        }

        public async Task<int> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var cfg = await _reportConfigurationsRepository.GetByIdAsync(request.ConfigurationId).ConfigureAwait(false);

            IReportsBuilder _reportsBuilder = new ReportsBuilder(cfg);
            var newReport = _reportsBuilder
                .WithDefaultNotes(true)
                .WithName(request.Name)
                .Build();

            var result = await _reportsRepository.AddAsync(newReport).ConfigureAwait(false);
            return result.Id;                              ;
        }
    }
}
