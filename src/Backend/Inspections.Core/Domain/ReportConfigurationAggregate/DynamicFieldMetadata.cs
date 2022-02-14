using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.ReportConfigurationAggregate;

[NotMapped]
public class DynamicFieldMetadata
{
    [Key]
    public string FieldName { get; set; }
    public string SectionTitle { get; set; }
    public string Label { get; set; }
    public string InputType { get; set; }
    public string Suffix { get; set; }
    public string Preffix { get; set; }
    public int Min { get; set; } = 0;
    public int Max { get; set; } = 999;
    public float Step { get; set; } = 1;
    public short MaxLength { get; set; } = 0;
    public bool Enabled { get; set; } = true;
    public bool RollerOnMobile { get; set; }
    public short RollerDigits { get; set; } = 3;
    public bool Visible { get; set; } = true;
    public int DefaultValue { get; set; }
}

[NotMapped]
public class DynamicFields
{
    public DynamicFieldMetadata[]? FieldsDefinitions { get; set; }
}

