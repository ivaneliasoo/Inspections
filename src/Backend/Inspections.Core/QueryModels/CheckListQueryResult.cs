using System.Text.Json;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.Forms;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;

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


public class FormResult
{
    public static FormResult CreateFromEntity(ReportForm entity)
    {
        Guard.Against.Null(entity, nameof(entity));
        var instance = new FormResult();

        instance.Id = entity.Id;
        instance.Name = entity.Name;
        instance.Title = entity.Title;
        instance.Icon = entity.Icon;
        instance.Fields = entity.Fields;
        instance.Enabled = entity.Enabled;
        return instance;
    }

    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Icon { get; set; } 
    public DynamicFields Fields { get; set; } = new();
    public JsonDocument? Values { get; set; }
    public bool Enabled { get; set; } = true;
}
