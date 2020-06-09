using Inspections.API.Features.Checklists.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    [DataContract]
    public class AddCheckListItemParamCommand : IRequest<bool>
    {
        public AddCheckListItemParamCommand(int idCheckList, int idCheckListItem, List<CheckListParamDTO> checkListParams)
        {
            IdCheckList = idCheckList;
            IdCheckListItem = idCheckListItem;
            CheckListParams = checkListParams;
        }
        private AddCheckListItemParamCommand()
        {

        }

        [DataMember]
        public int IdCheckList { get; set; }
        [DataMember]
        public int IdCheckListItem { get; set; }
        [DataMember]
        public List<CheckListParamDTO> CheckListParams { get; set; }
    }
}
