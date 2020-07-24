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
    public class UpdateCheckListItemCommandHandler : IRequestHandler<UpdateCheckListItemCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;

        public UpdateCheckListItemCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public async Task<bool> Handle(UpdateCheckListItemCommand request, CancellationToken cancellationToken)
        {
            var checkList = await _checkListsRepository.GetByIdAsync(request.CheckListId).ConfigureAwait(false);
            var item = await _checkListsRepository.GetItemByIdAsync(request.Id).ConfigureAwait(false);

            checkList.RemoveCheckItems(item);
            item.Checked = request.Checked;
            item.Remarks = request.Remarks;
            item.Required = request.Required;
            item.Editable = request.Editable;
            item.Text = request.Text;
            checkList.AddCheckItems(item);

            await _checkListsRepository.UpdateAsync(checkList).ConfigureAwait(false);

            return true;
        }
       
    }
}
