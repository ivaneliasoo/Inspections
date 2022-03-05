namespace Inspections.Core.QueryModels;

public class CheckListItemQueryResult
{
    public int CheckListId { get; set; }
    public int @Checked { get; set; }
    public bool Editable { get; set; }
    public int Id { get; set; }
    public string? Remarks { get; set; }
    public bool Required { get; set; }
    public string Text { get; set; } = default!;
    public bool Touched { get; set; }
}
