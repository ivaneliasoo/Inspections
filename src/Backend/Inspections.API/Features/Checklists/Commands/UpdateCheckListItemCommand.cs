using Inspections.Core.Domain.CheckListAggregate;
using MediatR;

namespace Inspections.API.Features.Checklists.Commands
{
    public class UpdateCheckListItemCommand : IRequest<bool>
    {
        public UpdateCheckListItemCommand(
            int id,
            int checkListId,
            string text,
            CheckValue @checked,
            bool editable,
            bool required,
            string remarks)
        {
            Id = id;
            CheckListId = checkListId;
            Text = text;
            Checked = @checked;
            Editable = editable;
            Required = required;
            Remarks = remarks;
        }

        private UpdateCheckListItemCommand() { }

        public int Id { get; set; }
        public int CheckListId { get; set; }
        public string Text { get; set; } = default!;
        public CheckValue Checked { get; set; }
        public bool Editable { get; set; }
        public bool Required { get; set; }
        public string? Remarks { get; set; }
    }
}
