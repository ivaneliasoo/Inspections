using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Checklists.Handlers
{
    public class DeleteCheckListCommandHandler : IRequestHandler<DeleteCheckListCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;

        public DeleteCheckListCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public async Task<bool> Handle(DeleteCheckListCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var checklist = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);
            await _checkListsRepository.DeleteAsync(checklist).ConfigureAwait(false);
            return true;
        }
    }
}
