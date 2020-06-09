using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Handlers
{
    public class AddCheckListParamCommandHandler : IRequestHandler<AddCheckListParamCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;

        public AddCheckListParamCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public async Task<bool> Handle(AddCheckListParamCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var checklist = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);

            if (checklist.Completed)
                return false;

            foreach (var param in request.ChecklistParams)
            {
                var newParam = new CheckListParam
                (
                    request.IdCheckList,
                    null,
                    param.Key,
                    param.Value,
                    param.Type
                );
                checklist.AddCheckListParams(newParam);
            }

            await _checkListsRepository.UpdateAsync(checklist).ConfigureAwait(false);

            return true;
        }
    }
}
