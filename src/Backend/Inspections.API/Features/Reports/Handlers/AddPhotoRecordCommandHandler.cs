using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Reports.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Inspections.API.Features.Reports.Handlers
{
    public class AddPhotoRecordCommandHandler : IRequestHandler<AddPhotoRecordCommand, bool>
    {
        private readonly FileUploadService _fileUploadService;
        private readonly IReportsRepository _reportsRepository;
        private readonly IStorageHelper _storageHelper;

        public AddPhotoRecordCommandHandler(FileUploadService fileUploadService
            , IReportsRepository reportsRepository
            , IStorageHelper storageHelper)
        {
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
        }

        public async Task<bool> Handle(AddPhotoRecordCommand request, CancellationToken cancellationToken)
        {
            var savedFilesPaths = new string[request!.FormFiles.Count];
            try
            {
                var report = await _reportsRepository.GetByIdAsync(request!.ReportId).ConfigureAwait(false);

                int createdFilesCount = 0;
                foreach (var file in request!.FormFiles)
                {
                    var filePath = await _fileUploadService.UploadAttachments(file, request!.ReportId.ToString()).ConfigureAwait(false);
                    report.AddPhoto(new PhotoRecord(request.ReportId, $"/ReportsImages/{report.Id}/{Path.GetFileName(filePath)}" , request.Label));
                    savedFilesPaths[createdFilesCount] = filePath;
                    createdFilesCount++;
                }
                await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                foreach (var filePath in savedFilesPaths)
                {
                    if (!string.IsNullOrWhiteSpace(filePath))
                        _storageHelper.DeleteFile(filePath);
                }
                return false;
            }
        }
    }
}
