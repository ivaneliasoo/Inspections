using System;
using System.Data.Common;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class AddPhotoRecordCommandHandler : IRequestHandler<AddPhotoRecordCommand, bool>
    {
        private readonly PhotoRecordManager _photoManager;
        private readonly IReportsRepository _reportsRepository;

        public AddPhotoRecordCommandHandler(PhotoRecordManager photoManager
            , IReportsRepository reportsRepository
            , PhotoRecordManager storageHelper)
        {
            _photoManager = photoManager ?? throw new ArgumentNullException(nameof(photoManager));
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }

        public async Task<bool> Handle(AddPhotoRecordCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var savedFilesPaths = new string[request!.FormFiles.Count];
            try
            {
                var report = await _reportsRepository.GetByIdAsync(request!.ReportId).ConfigureAwait(false);

                bool isLabelInHeader = request.Label?.Length > 0;

                int createdFilesCount = 0;
                foreach (var file in request!.FormFiles)
                {
                    var nameAndLabel = !isLabelInHeader ? file.FileName.Split('|', StringSplitOptions.RemoveEmptyEntries) : null;
                    var name = nameAndLabel is null ? file.FileName : nameAndLabel[0];
                    var label = nameAndLabel is null ? request.Label!.ToUpperInvariant() : nameAndLabel.Length > 1 ? nameAndLabel[1] : ""; 
                    var filePath = await _photoManager.AddPhoto(file.OpenReadStream(), request!.ReportId.ToString(CultureInfo.InvariantCulture), name, file.ContentType).ConfigureAwait(false);
                    var photo = new PhotoRecord(request.ReportId, filePath.PhotoPath, filePath.ThumbnailPath, label);
                    photo.SetMetadata(filePath.PhotoStorageId!, filePath.ThumbnailStorageId!, filePath.PhotoUrl!, filePath.ThumbnailUrl!);
                    report.AddPhoto(photo);
                    savedFilesPaths[createdFilesCount] = filePath.PhotoPath;
                    createdFilesCount++;
                }
                await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);
                return true;
            }
            catch (DbException)
            {
                foreach (var filePath in savedFilesPaths)
                {
                    if (!string.IsNullOrWhiteSpace(filePath))
                        await _photoManager.RemovePhoto(filePath).ConfigureAwait(false);
                }
                return false;
            }
        }
    }
}
