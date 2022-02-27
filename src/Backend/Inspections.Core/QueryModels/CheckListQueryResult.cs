namespace Inspections.Core.QueryModels;

public class CheckListQueryResult
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public string Text { get; set; } = default!;
    public string? Annotation { get; set; }
    public bool @Checked { get; set; }
    public IEnumerable<CheckListItemQueryResult> Checks { get; set; } = default!;
}
