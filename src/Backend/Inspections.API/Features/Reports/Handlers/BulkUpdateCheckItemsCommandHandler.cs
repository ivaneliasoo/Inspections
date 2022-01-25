using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using MediatR;

namespace Inspections.API.Features.Reports.Handlers
{
    public class BulkUpdateCheckItemsCommandHandler : IRequestHandler<BulkUpdateCheckItemsCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;
        private readonly InspectionsContext _context;

        public BulkUpdateCheckItemsCommandHandler(ICheckListsRepository checkListsRepository, InspectionsContext context)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Handle(BulkUpdateCheckItemsCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var report = _context.Set<Report>().Any(r => r.Id == request.ReportId);

            var checkList = await _checkListsRepository.GetByIdAsync(request.CheckListId).ConfigureAwait(false);
            checkList.SetValue(request.NewValue);
            await _checkListsRepository.UpdateAsync(checkList).ConfigureAwait(false);
            return true;
        }
    }
}
