﻿using Ardalis.GuardClauses;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.ApplicationServices;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers;

public class CreateInspectionHandler : IRequestHandler<CreateReportCommand, int>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly IReportConfigurationsRepository _reportConfigurationsRepository;
    private readonly PhotoRecordManager _photoRecordManager;
    private readonly IUserNameResolver _userNameResolver;

    public CreateInspectionHandler(IReportsRepository reportsRepository, IReportConfigurationsRepository reportConfigurationsRepository, PhotoRecordManager photoRecordManager, IUserNameResolver userNameResolver)
    {
        _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        _reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
        _photoRecordManager = photoRecordManager ?? throw new ArgumentNullException(nameof(photoRecordManager));
        _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
    }

    public async Task<int> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var cfg = await _reportConfigurationsRepository.GetByIdAsync(request.ConfigurationId).ConfigureAwait(false);

        var reportName = $"{DateTime.Now:yyyyMMdd}-{cfg.Title}";

        IReportsBuilder reportsBuilder = new ReportsBuilder(cfg, _userNameResolver.FullName);
        var newReport = reportsBuilder
            .WithDefaultNotes(false)
            .WithName(reportName)
            .Build();

        var result = await _reportsRepository.AddAsync(newReport).ConfigureAwait(false);
        await _photoRecordManager.CreateAlbum(result.Id.ToString());
        return result.Id;
    }
}
