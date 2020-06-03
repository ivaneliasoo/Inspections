using Inspections.Shared;
using System;

namespace Inspections.Core.Domain.InspectionsAggregate
{
    public class Inspection : Entity<Guid>, IAggregateRoot
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public EMALicense License { get; set; }
    }
}
