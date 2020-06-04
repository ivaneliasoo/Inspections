using Inspections.Shared;

namespace Inspections.Core.Domain.SignaturesAggregate
{
    public class Responsable : ValueObject
    {
        public ResponsableType Type { get; set; }
        public string Name { get; set; }
    }

}
