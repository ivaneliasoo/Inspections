using Inspections.Shared;

namespace Inspections.Core.Domain.CheckListAggregate;

public class CheckListItem : Entity<int>
{
    public int CheckListId { get; set; }
    public string Text { get; set; } = default!;
    public CheckValue Checked { get; set; }
    public bool Editable { get; set; }
    public bool Required { get; set; }
    public string? Remarks { get; set; }
    public bool Touched { get; set; }

    private CheckListItem() { } //Required by EF

    public CheckListItem(int checkListId, string text, CheckValue @checked, bool editable, bool required, string? remarks, bool touched = false)
    {
        CheckListId = checkListId;
        Text = text;
        Checked = @checked;
        Editable = editable;
        Required = required;
        Remarks = remarks;
        Touched = touched;
    }
}
