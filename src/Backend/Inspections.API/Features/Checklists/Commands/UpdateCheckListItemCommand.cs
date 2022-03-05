using Inspections.Core.Domain.CheckListAggregate;
using MediatR;

namespace Inspections.API.Features.Checklists.Commands;

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

    public int Id { get; }
    public int CheckListId { get; }
    public string Text { get; }
    public CheckValue Checked { get; }
    public bool Editable { get; }
    public bool Required { get; }
    public string? Remarks { get; }
}
