using System;

namespace Inspections.Shared
{
    public abstract class BaseDomainEvent
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.Now;
    }
}