﻿using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.Checklists.Handlers;

public class UpdateCheckListCommandHandler : IRequestHandler<UpdateCheckListCommand, bool>
{
    private readonly ICheckListsRepository _checkListsRepository;

    public UpdateCheckListCommandHandler(ICheckListsRepository checkListsRepository)
    {
        _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
    }

    public async Task<bool> Handle(UpdateCheckListCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var checkList = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);
        checkList.Edit(request.Text, request.Annotation, request.IsConfiguration);
        await _checkListsRepository.UpdateAsync(checkList).ConfigureAwait(false);

        return true;
    }
}
