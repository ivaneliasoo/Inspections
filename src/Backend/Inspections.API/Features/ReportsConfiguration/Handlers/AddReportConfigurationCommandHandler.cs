using Ardalis.GuardClauses;
using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Interfaces;
using Inspections.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.ReportsConfiguration.Handlers
{
    public class AddReportConfigurationCommandHandler : IRequestHandler<AddReportConfigurationCommand, bool>
    {
        private readonly IReportConfigurationsRepository _reportConfigurationsRepository;

        public AddReportConfigurationCommandHandler(IReportConfigurationsRepository reportConfigurationsRepository)
        {
            _reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
        }

        public async Task<bool> Handle(AddReportConfigurationCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var repoConfig = new ReportConfiguration()
            { 
                Type = request.Type,
                RemarksLabelText = request.RemarksLabelText,
                Title = request.Title,
                FormName = request.FormName
            };

            var result = await _reportConfigurationsRepository.AddAsync(repoConfig).ConfigureAwait(false);

            return result.Id > 0;
        }
    }
}
