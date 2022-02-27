using MediatR;

namespace Inspections.API.Features.Checklists.Commands;

public class DeleteCheckListItemCommand : IRequest<bool>
{
    public int IdCheckList { get; }
    public int IdCheckListItem { get; }
    public DeleteCheckListItemCommand(int idCheckList, int idCheckListItem)
    {
        IdCheckList = idCheckList;
        IdCheckListItem = idCheckListItem;
    }
}
