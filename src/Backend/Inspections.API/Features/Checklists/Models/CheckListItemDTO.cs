using Inspections.Core.Domain.CheckListAggregate;

namespace Inspections.API.Features.Checklists.Models;

public class CheckListItemDTO
{
    public int Id { get; set; }
    public int CheckListId { get; set; }
    public string Text { get; set; } = default!;
    public CheckValue Checked { get; set; }
    public bool Editable { get; set; }
    public bool Required { get; set; }
    public string? Remarks { get; set; }
}
