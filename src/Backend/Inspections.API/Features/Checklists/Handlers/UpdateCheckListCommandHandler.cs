﻿using Ardalis.GuardClauses;
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
            checkList.Edit(request.Text, request.Annotation);
            await _checkListsRepository.UpdateAsync(checkList).ConfigureAwait(false);

            return true;
        }
    }
}
