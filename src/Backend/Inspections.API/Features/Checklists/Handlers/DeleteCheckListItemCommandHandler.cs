﻿using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.Checklists.Handlers;

public class DeleteCheckListItemCommandHandler : IRequestHandler<DeleteCheckListItemCommand, bool>
{
    private readonly ICheckListsRepository _checkListsRepository;

    public DeleteCheckListItemCommandHandler(ICheckListsRepository checkListsRepository)
    {
        _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
    }

    public async Task<bool> Handle(DeleteCheckListItemCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var item = await _checkListsRepository.GetItemByIdAsync(request!.IdCheckListItem).ConfigureAwait(false);
        if (item is null)
            return false;

        var checklist = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);
        checklist.RemoveCheckItems(item);
        await _checkListsRepository.UpdateAsync(checklist).ConfigureAwait(false);

        return true;
    }

}
