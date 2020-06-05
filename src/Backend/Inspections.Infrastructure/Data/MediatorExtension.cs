using Inspections.Shared;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.Infrastructure.Data
{
    internal static class MediatorExtension
    {
        internal static async Task DispatchDomainEventsAsync(this IMediator mediator, InspectionsContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity<int>>()
                .Where(o => o.Entity.DomainEvents != null && o.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(s => s.Entity.DomainEvents)
                .ToList();

            var tasks = domainEvents
                .Select(async (domainevent) =>
                {
                    await mediator.Publish(domainevent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
