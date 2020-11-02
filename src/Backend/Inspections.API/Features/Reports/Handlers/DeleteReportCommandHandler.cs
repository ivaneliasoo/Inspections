using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Inspections.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Inspections.API.Features.Reports.Handlers
{
    public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IStorageHelper _storageHelper;
        private readonly ClientSettings _settings;

        public DeleteReportCommandHandler(IReportsRepository reportsRepository, IStorageHelper storageHelper, IOptions<ClientSettings> options)
        {
            this._reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            this._storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task<bool> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var report = await _reportsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (report is null)
                return false;

            if (!report.IsClosed)
            {
                _storageHelper.DeleteFolder(Path.Combine(Directory.GetCurrentDirectory(), _settings.ReportsImagesFolder, report.Id.ToString()));
                await _reportsRepository.DeleteAsync(report).ConfigureAwait(false);
                return true;
            }

            return false;

        }
    }
}
