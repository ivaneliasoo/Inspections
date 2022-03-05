using System.Text.Json;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain.Forms;

public sealed class FormDefinition : Entity<int>
{
    private FormDefinition()
    {
            
    }
    public FormDefinition(string name, string title, DynamicFields fields, JsonDocument? defaultValues)
    {
        Name = name;
        Title = title;
        Fields = fields;
        DefaultValues = defaultValues;
    }
    public string Name { get;  }
    public string Title { get; private set; }
    public string? Icon { get; set; }
    public DynamicFields Fields { get; private set; }
    public JsonDocument? DefaultValues { get; private set; }
    public bool Enabled { get; set; } = true;
    
    public List<Report> Reports { get; } = new();

    public List<ReportConfiguration> ReportConfigurations { get; } = new();

    public void SetTitle(string title)
    {
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Title = title;
    } 
    
    public void SetFields(DynamicFields fields)
    {
        Guard.Against.Null(fields, nameof(fields));

        Fields = fields;
    }
    
    public void SetValues(JsonDocument values)
    {
        Guard.Against.Null(values, nameof(values));

        DefaultValues = values;
    }

    public void AssociateReport(IList<Report> reports)
    {
        Guard.Against.Null(reports, nameof(reports));
        
        this.Reports.AddRange(reports);
    }

    public void AssociateReportConfiguration(IList<ReportConfiguration> reportConfigurations)
    {
        Guard.Against.Null(reportConfigurations, nameof(reportConfigurations));
        
        this.ReportConfigurations.AddRange(reportConfigurations);
    }
}
