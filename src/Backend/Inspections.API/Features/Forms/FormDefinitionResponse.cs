using System.Text.Json;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.Forms;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;

namespace Inspections.API.Features.Forms;

public class FormDefinitionResponse
{
    public static FormDefinitionResponse CreateFromEntity(FormDefinition entity)
    {
        Guard.Against.Null(entity, nameof(entity));
        var instance = new FormDefinitionResponse();

        instance.Id = entity.Id;
        instance.Name = entity.Name;
        instance.Title = entity.Title;
        instance.Fields = entity.Fields;
        instance.DefaultValues = entity.DefaultValues;
        instance.Enabled = entity.Enabled;
        instance.Reports = entity.Reports.ToList();
        instance.ReportConfigurations = entity.ReportConfigurations.ToList();
        return instance;
    }

    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DynamicFields Fields { get; set; } = new();
    public JsonDocument? DefaultValues { get; set; }
    public bool Enabled { get; set; } = true;
    public List<Report> Reports { get; set; } = new();
    public List<ReportConfiguration> ReportConfigurations { get; set; } = new();
}
