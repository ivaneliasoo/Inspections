using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain.Forms;

public sealed class FormDefinition : Entity<int>
{
#pragma warning disable CS8618
    private FormDefinition()
#pragma warning restore CS8618
    {
            
    }
    public FormDefinition(string name, string title, string icon, DynamicFields fields)
    {
        Name = name;
        Title = title;
        Icon = icon;
        Fields = fields;
    }
    public string Name { get;  }
    public string Title { get; private set; }
    public string? Icon { get; set; }
    public DynamicFields Fields { get; private set; }
    public bool Enabled { get; set; } = true;
    
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
    
    public void AssociateReportConfiguration(IList<ReportConfiguration> reportConfigurations)
    {
        Guard.Against.Null(reportConfigurations, nameof(reportConfigurations));
        
        this.ReportConfigurations.AddRange(reportConfigurations);
    }
}
