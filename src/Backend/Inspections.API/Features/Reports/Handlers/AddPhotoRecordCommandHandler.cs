using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Reports.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

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
                    var label = nameAndLabel is null ? request.Label.ToUpperInvariant() : nameAndLabel[1]; 
                    var filePath = await _fileUploadService.UploadAttachments(file, request!.ReportId.ToString(CultureInfo.InvariantCulture),fileName: name).ConfigureAwait(false);
                    report.AddPhoto(new PhotoRecord(request.ReportId, $"/ReportsImages/{report.Id}/{Path.GetFileName(filePath)}", $"/ReportsImages/{report.Id}/{Path.GetFileNameWithoutExtension(filePath)}small{Path.GetExtension(filePath)}", label));
                    savedFilesPaths[createdFilesCount] = filePath;
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
                        _storageHelper.DeleteFile(filePath);
                }
                return false;
            }
        }

        
    }
}
