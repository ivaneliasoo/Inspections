﻿using Ardalis.GuardClauses;
using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Handlers;

public class DeleteReportConfigurationCommandHandler : IRequestHandler<DeleteReportConfigurationCommand, bool>
{
    private readonly IReportConfigurationsRepository _reportConfigurationsRepository;

    public DeleteReportConfigurationCommandHandler(IReportConfigurationsRepository reportConfigurationsRepository)
    {
        _reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
    }

    public async Task<bool> Handle(DeleteReportConfigurationCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var reportConfig = await _reportConfigurationsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

        if (reportConfig is null)
            return false;

        await _reportConfigurationsRepository.DeleteAsync(reportConfig).ConfigureAwait(false);
        return true;
    }
}
