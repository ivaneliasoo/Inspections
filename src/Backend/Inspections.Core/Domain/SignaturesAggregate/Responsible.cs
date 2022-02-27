using Inspections.Shared;

namespace Inspections.Core.Domain.SignaturesAggregate;

public class Responsible : ValueObject
{
    public ResponsibleType Type { get; set; }
    public string? Name { get; set; }
}
