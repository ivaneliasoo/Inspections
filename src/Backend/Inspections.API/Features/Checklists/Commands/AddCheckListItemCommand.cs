using System.Runtime.Serialization;
using Inspections.Core.Domain.CheckListAggregate;
using MediatR;

namespace Inspections.API.Features.Checklists.Commands
{
    [DataContract]
    public class AddCheckListItemCommand : IRequest<bool>
    {
        [DataMember]
        public int IdCheckList { get; set; }
        [DataMember]
        public string Text { get; set; } = default!;
        [DataMember]
        public CheckValue Checked { get; set; }
        [DataMember]
        public bool Editable { get; set; }
        [DataMember]
        public bool Required { get; set; }
        [DataMember]
        public string? Remarks { get; set; }
        private AddCheckListItemCommand() { }

        public AddCheckListItemCommand(string text, CheckValue @checked, bool editable, bool required, string? remarks)
        {
            Text = text;
            Checked = @checked;
            Editable = editable;
            Required = required;
            Remarks = remarks;
        }
    }
}
