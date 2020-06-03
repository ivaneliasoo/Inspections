using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain.InspectionsAggregate
{
    [ComplexType]
    public class EMALicense
    {
        public EMALIcenseType Type { get; set; }
        public string Number { get; set; }
    }
}