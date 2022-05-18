using Ardalis.GuardClauses;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Reports.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Inspections.API.Features.Reports.Handlers;

public class DeletePhotoRecordCommandHandler : IRequestHandler<DeletePhotoRecordCommand, bool>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly PhotoRecordManager _photoManager;
    private readonly ClientSettings _settings;

    public DeletePhotoRecordCommandHandler(IReportsRepository reportsRepository, PhotoRecordManager photoManager, IOptions<ClientSettings> options)
    {
        _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        _photoManager = photoManager ?? throw new ArgumentNullException(nameof(photoManager));
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }
    
    public async Task<bool> Handle(DeletePhotoRecordCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);

        if (report is null) return false;
        var photo = report.PhotoRecords.FirstOrDefault(n => n.Id == request.Id);
        if (photo is not null)
        {
            await _photoManager.RemovePhoto(photo.FileName);
            await _photoManager.RemovePhoto(photo.FileNameResized);
            report.RemovePhoto(photo);
        }
        await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);

        return true;
    }
}
