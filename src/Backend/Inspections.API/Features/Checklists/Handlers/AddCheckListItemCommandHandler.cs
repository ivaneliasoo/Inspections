using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.Checklists.Handlers
{
    public class AddCheckListItemCommandHandler : IRequestHandler<AddCheckListItemCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;

        public AddCheckListItemCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public async Task<bool> Handle(AddCheckListItemCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var checkList = await _checkListsRepository.GetByIdAsync(request.IdCheckList).ConfigureAwait(false);

            var newItem = new CheckListItem(request.IdCheckList, request.Text, request.Checked, request.Editable, request.Required, request.Remarks);

            checkList.AddCheckItems(newItem);

            await _checkListsRepository.UpdateAsync(checkList).ConfigureAwait(false);

            return true;
        }

    }
}
