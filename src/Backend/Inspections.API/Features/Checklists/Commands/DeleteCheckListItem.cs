using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    public class DeleteCheckListItem : IRequest<bool>
    {
        public int IdCheckList { get; set; }
        public int IdCheckListItem { get; set; }
        public DeleteCheckListItem(int idCheckList, int idCheckListItem)
        {
            IdCheckList = idCheckList;
            IdCheckListItem = idCheckListItem;
        }
    }
}
