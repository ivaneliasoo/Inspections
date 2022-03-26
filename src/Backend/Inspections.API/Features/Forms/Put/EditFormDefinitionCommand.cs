using System.Text.Json;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;

namespace Inspections.API.Features.Forms.Put;

public class EditFormDefinitionCommand : IRequest<bool>
{
    public EditFormDefinitionCommand(int id, string name, string title, DynamicFields fields, JsonDocument? defaultValues, bool enabled, short order, List<int> reports, List<int> reportConfigurations)
    {
        Id = id;
        Name = name;
        Title = title;
        Fields = fields;
        DefaultValues = defaultValues;
        Enabled = enabled;
        Order = order;
        Reports = reports;
        ReportConfigurations = reportConfigurations;
    }

    public int Id { get; }
    public string Name { get; }
    public string Title { get; }
    public DynamicFields Fields { get; }
    public JsonDocument? DefaultValues { get; }
    public bool Enabled { get; } = true;
    public short Order { get; set; }
    public List<int> Reports { get; } = new();
    public List<int> ReportConfigurations { get; } = new();
}
