using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.API.Features.Checklists.Mapping;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var checklist = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);
            await _checkListsRepository.DeleteAsync(checklist).ConfigureAwait(false);
            return true;
        }
    }
}
