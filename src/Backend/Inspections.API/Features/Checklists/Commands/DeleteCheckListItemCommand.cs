using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    public class DeleteCheckListItemCommand : IRequest<bool>
    {
        public int IdCheckList { get; set; }
        public int IdCheckListItem { get; set; }
        public DeleteCheckListItemCommand(int idCheckList, int idCheckListItem)
        {
            IdCheckList = idCheckList;
            IdCheckListItem = idCheckListItem;
        }
    }
}
