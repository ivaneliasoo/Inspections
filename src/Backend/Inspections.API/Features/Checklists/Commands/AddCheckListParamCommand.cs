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
    public class AddCheckListParamCommand : IRequest<bool>
    {
        [DataMember]
        public int IdCheckList { get; set; }
        [DataMember]
        public List<CheckListParamDTO> ChecklistParams { get; set; }

        private AddCheckListParamCommand() { }

        public AddCheckListParamCommand(int idCheckList, List<CheckListParamDTO> checklistParams)
        {
            IdCheckList = idCheckList;
            ChecklistParams = checklistParams;
        }
    }
}
