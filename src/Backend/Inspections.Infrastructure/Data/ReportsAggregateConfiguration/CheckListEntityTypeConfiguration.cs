using Inspections.Core.Domain.CheckListAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration
{
    public class CheckListEntityTypeConfiguration : IEntityTypeConfiguration<CheckList>
    {
        public void Configure(EntityTypeBuilder<CheckList> builder)
        {
            builder.ToTable("CheckLists", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Text).IsRequired();
            builder.Property(p => p.Annotation).IsRequired(false);
            builder.Property(p => p.IsConfiguration).IsRequired();

            builder.Ignore(p => p.DomainEvents);
        }
    }
}
