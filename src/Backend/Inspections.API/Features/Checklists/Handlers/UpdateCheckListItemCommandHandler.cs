using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.Checklists.Handlers;

public class UpdateCheckListItemCommandHandler : IRequestHandler<UpdateCheckListItemCommand, bool>
{
    private readonly ICheckListsRepository _checkListsRepository;

    public UpdateCheckListItemCommandHandler(ICheckListsRepository checkListsRepository)
    {
        _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
    }

    public async Task<bool> Handle(UpdateCheckListItemCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var checkList = await _checkListsRepository.GetByIdAsync(request.CheckListId).ConfigureAwait(false);
        var item = await _checkListsRepository.GetItemByIdAsync(request.Id).ConfigureAwait(false);

        checkList.RemoveCheckItems(item);
        item.Checked = request.Checked;
        item.Remarks = request.Remarks;
        item.Required = request.Required;
        item.Editable = request.Editable;
        item.Text = request.Text;
        item.Touched = true;
        checkList.AddCheckItems(item);

        await _checkListsRepository.UpdateAsync(checkList).ConfigureAwait(false);

        return true;
    }

}
