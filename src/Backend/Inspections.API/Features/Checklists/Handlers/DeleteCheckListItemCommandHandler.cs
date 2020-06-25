using Inspections.API.Features.Checklists.Commands;
using Inspections.API.Features.Checklists.Mapping;
using Inspections.API.Features.Checklists.Models;
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
    public class DeleteCheckListItemCommandHandler : IRequestHandler<DeleteCheckListItemCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;

        public DeleteCheckListItemCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public async Task<bool> Handle(DeleteCheckListItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _checkListsRepository.GetItemByIdAsync(request!.IdCheckListItem).ConfigureAwait(false);
            if (item is null)
                return false;

            var checklist = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);
            checklist.RemoveCheckItems(item);
            await _checkListsRepository.UpdateAsync(checklist).ConfigureAwait(false);

            return true;
        }
       
    }
}
