using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class EditPhotoRecordCommandHandler : IRequestHandler<EditPhotoRecordCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;

        public EditPhotoRecordCommandHandler(IReportsRepository reportsRepository)
        {
            this._reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }

        public async  Task<bool> Handle(EditPhotoRecordCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);

            var photo = report.PhotoRecords.Where(n => n.Id == request.Id).FirstOrDefault();
            report.RemovePhoto(photo);
            photo.Label = request.Label;
            report.AddPhoto(photo);
            await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);
            return true;
        }
    }
}
