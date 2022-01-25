using Inspections.Core.Domain.CheckListAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration
{
    public class CheckListItemEntityTypeConfiguration : IEntityTypeConfiguration<CheckListItem>
    {
        public void Configure(EntityTypeBuilder<CheckListItem> builder)
        {
            builder.ToTable("CheckListItems", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Text).IsRequired();
            builder.Property(p => p.Checked).IsRequired();
            builder.Property(p => p.Editable).IsRequired();
            builder.Property(p => p.Required).IsRequired();
            builder.Property(p => p.Remarks).IsRequired(false);

            builder.Ignore(p => p.DomainEvents);

        }
    }
}
