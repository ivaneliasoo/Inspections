using Inspections.API.Features.Checklists.Models;
using Inspections.Core.Domain.CheckListAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    [DataContract]
    public class AddCheckListItemCommand : IRequest<bool>
    {
        [DataMember]
        public int IdCheckList { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public CheckValue Checked { get; set; }
        [DataMember]
        public bool Required { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public List<CheckListParamDTO> ChecklistParams { get; set; }
        private AddCheckListItemCommand() { }

        public AddCheckListItemCommand(string text, CheckValue @checked, bool required, string remarks, List<CheckListParamDTO> checklistParams)
        {
            Text = text;
            Checked = @checked;
            Required = required;
            Remarks = remarks;
            ChecklistParams = checklistParams;
        }
    }
}
