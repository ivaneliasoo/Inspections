using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class DeletePhotoRecordCommandHandler : IRequestHandler<DeletePhotoRecordCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;

        public DeletePhotoRecordCommandHandler(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }
        public async Task<bool> Handle(DeletePhotoRecordCommand request, CancellationToken cancellationToken)
        {
            var report = await _reportsRepository.GetByIdAsync(request.ReportId).ConfigureAwait(false);

            var photo = report.PhotoRecords.Where(n => n.Id == request.Id).FirstOrDefault();
            report.RemovePhoto(photo);
            await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);
            return true;
        }
    }
}
