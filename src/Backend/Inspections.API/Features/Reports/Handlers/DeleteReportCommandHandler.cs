using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inspections.API.Features.Inspections.Commands;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, bool>
    {
        private readonly IReportsRepository _reportsRepository;

        public DeleteReportCommandHandler(IReportsRepository reportsRepository, ISignaturesRepository signaturesRepository, ICheckListsRepository checkListsRepository)
        {
            this._reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }
        public async Task<bool> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            var report = await _reportsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (!report.IsClosed)
            {
                await _reportsRepository.DeleteAsync(report).ConfigureAwait(false);
                return true;
            }

            return false;

        }
    }
}
