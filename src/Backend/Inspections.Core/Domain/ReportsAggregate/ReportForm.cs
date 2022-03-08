using System.Text.Json;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain.ReportsAggregate;

public class ReportForm : Entity<int>
{
#pragma warning disable CS8618
    private ReportForm() { }
#pragma warning restore CS8618
    public ReportForm(string name, string title, DynamicFields fields, string? icon, bool enabled)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.Null(fields, nameof(fields));
        
        Name = name;
        Title = title;
        Fields = fields;
        Icon = icon;
        Enabled = enabled;
    }

    public int ReportId { get; set; }
    public string Name { get;  }
    public string Title { get; }
    public string? Icon { get; }
    public DynamicFields Fields { get; }
    public JsonDocument? Values { get; private set; }
    public bool Enabled { get; }

    public void SetValues(JsonDocument newValues)
    {
        Guard.Against.Null(newValues, nameof(newValues));

        Values = newValues;
    }
}
