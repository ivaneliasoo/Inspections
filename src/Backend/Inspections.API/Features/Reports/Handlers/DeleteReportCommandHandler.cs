using System.Globalization;
using Ardalis.GuardClauses;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Reports.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Inspections.API.Features.Reports.Handlers;

public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, bool>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly PhotoRecordManager _photoManager;
    private readonly ClientSettings _settings;

    public DeleteReportCommandHandler(IReportsRepository reportsRepository, PhotoRecordManager photoManager, IOptions<ClientSettings> options)
    {
        this._reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        this._photoManager = photoManager ?? throw new ArgumentNullException(nameof(photoManager));
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<bool> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var report = await _reportsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

        if (report is null)
            return false;

        // if (report.IsClosed)
        // {
        //     return false;
        // }

        await _photoManager.RemovePhoto(Path.Combine(Directory.GetCurrentDirectory(), _settings.ReportsImagesFolder, report.Id.ToString(CultureInfo.InvariantCulture)));
        await _reportsRepository.DeleteAsync(report).ConfigureAwait(false);
        return true;

    }
}
