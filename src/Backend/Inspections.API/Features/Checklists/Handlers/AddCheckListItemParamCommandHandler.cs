using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Handlers
{
    public class AddCheckListItemParamCommandHandler : IRequestHandler<AddCheckListItemParamCommand, bool>
    {
        private const string DUPLCATED_PARAMS_MESSAGE = "Cant add duplicated params";
        private readonly ICheckListsRepository _checkListsRepository;

        public AddCheckListItemParamCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public async Task<bool> Handle(AddCheckListItemParamCommand request, CancellationToken cancellationToken)
        {
            //TODO: take care about domain
            Guard.Against.Null(request, nameof(request));

            var checkList = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);
            var checklistItem = await _checkListsRepository.GetItemByIdAsync(request.IdCheckListItem).ConfigureAwait(false);

            foreach (var param in request.CheckListParams)
            {
                if (checklistItem.TextParams.Count > 0)
                {
                    if (checklistItem.TextParams.Any(p => p.Key == param.Key))
                        throw new InvalidOperationException(DUPLCATED_PARAMS_MESSAGE);
                }

                var newParam = new CheckListParam
                (
                    null,
                    request.IdCheckListItem,
                    param.Key,
                    param.Value,
                    param.Type
                );
                checklistItem.TextParams.Add(newParam);
            }

            checkList.AddCheckItems(checklistItem);

            await _checkListsRepository.UpdateAsync(checkList).ConfigureAwait(false);

            return true;
        }
    }
}
