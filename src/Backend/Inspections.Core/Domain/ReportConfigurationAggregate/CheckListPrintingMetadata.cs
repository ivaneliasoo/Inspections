using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.ReportConfigurationAggregate;

[NotMapped]
public class CheckListPrintingMetadata
{
    public CheckListDisplay Display { get; set; } = CheckListDisplay.Numbered;
}
