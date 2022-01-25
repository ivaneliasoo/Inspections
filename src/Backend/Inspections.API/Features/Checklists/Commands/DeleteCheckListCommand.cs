using MediatR;

namespace Inspections.API.Features.Checklists.Commands
{
    public class DeleteCheckListCommand : IRequest<bool>
    {
        public int IdCheckList { get; set; }
        public DeleteCheckListCommand(int idCheckList)
        {
            IdCheckList = idCheckList;
        }
    }
}
