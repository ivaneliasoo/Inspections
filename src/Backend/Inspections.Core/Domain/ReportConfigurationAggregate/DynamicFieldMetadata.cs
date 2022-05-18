using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.ReportConfigurationAggregate;

[NotMapped]
public class DynamicFieldMetadata
{
    [Key] public string FieldName { get; set; } = default!;
    public string SectionTitle { get; set; } = default!;
    public string? SwitchableSection { get; set; }
    public string Label { get; set; } = default!;
    public string InputType { get; set; } = default!;
    public string? SelectOptions { get; set; }
    public string Suffix { get; set; } = default!;
    public string Prefix { get; set; } = default!;
    public int Min { get; set; } = 0;
    public int Max { get; set; } = 999;
    public float Step { get; set; } = 1;
    public short MaxLength { get; set; } = 0;
    public bool Enabled { get; set; } = true;
    public bool RollerOnMobile { get; set; }
    public short RollerDigits { get; set; } = 3;
    public bool Visible { get; set; } = true;
    public object DefaultValue { get; set; } = default!;
    public int Order { get; set; }
}

[NotMapped]
public class DynamicFields
{
    public DynamicFieldMetadata[]? FieldsDefinitions { get; set; }
}
