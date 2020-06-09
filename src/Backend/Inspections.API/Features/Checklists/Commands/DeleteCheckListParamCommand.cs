using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    public class DeleteCheckListParamCommand : IRequest<bool>
    {
        public int IdCheckList { get; set; }
        public int IdCheckListItem { get; set; }
        public int IdCheckListParam { get; set; }
        public DeleteCheckListParamCommand(int idCheckList, int idCheckListItem, int idCheckListParam)
        {
            IdCheckList = idCheckList;
            IdCheckListItem = idCheckListItem;
            IdCheckListParam = idCheckListParam;
        }
    }
}
