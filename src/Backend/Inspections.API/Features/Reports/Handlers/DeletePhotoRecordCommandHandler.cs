using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Inspections.API.Features.Reports.Handlers
{
    public class DeletePhotoRecordCommandHandler : IRequestHandler<DeletePhotoRecordCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IStorageHelper _storageHelper;
        private readonly ClientSettings _settings;

        public DeletePhotoRecordCommandHandler(IReportsRepository reportsRepository, IStorageHelper storageHelper, IOptions<ClientSettings> options)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task<bool> Handle(DeletePhotoRecordCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);


            try
            {
                if (report is null) return false;
                var photo = report.PhotoRecords.Where(n => n.Id == request.Id).FirstOrDefault();
                if (photo is not null)
                {
                    _storageHelper.DeleteFile(Path.Combine(Directory.GetCurrentDirectory(), _settings.ReportsImagesFolder, photo.FileName.Replace("/ReportsImages/", "", StringComparison.InvariantCulture)));
                    _storageHelper.DeleteFile(Path.Combine(Directory.GetCurrentDirectory(), _settings.ReportsImagesFolder, photo.FileNameResized.Replace("/ReportsImages/", "", StringComparison.InvariantCulture)));
                    report.RemovePhoto(photo);
                }
                await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
