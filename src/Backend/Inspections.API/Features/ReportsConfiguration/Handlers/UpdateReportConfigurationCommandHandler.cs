using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Handlers
{
    public class UpdateReportConfigurationCommandHandler : IRequestHandler<UpdateReportConfigurationCommand, bool>
    {
        private readonly IReportConfigurationsRepository _reportConfigurationsRepository;

        public UpdateReportConfigurationCommandHandler(IReportConfigurationsRepository reportConfigurationsRepository)
        {
            _reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
        }

        public async Task<bool> Handle(UpdateReportConfigurationCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var reportConfig = await _reportConfigurationsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (reportConfig is null)
                return false;

            reportConfig.Type = request.Type;
            reportConfig.RemarksLabelText = request.RemarksLabelText;
            reportConfig.Title = request.Title;
            reportConfig.FormName = request.FormName;
            reportConfig.PrintSectionId = request.PrintSectionId;
            reportConfig.CheckListMetadata.Display = request.Display;
            await _reportConfigurationsRepository.UpdateAsync(reportConfig).ConfigureAwait(false);
            return true;
        }
    }
}
