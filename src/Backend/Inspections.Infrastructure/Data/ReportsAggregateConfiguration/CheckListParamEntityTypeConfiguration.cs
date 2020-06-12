using Inspections.Core.Domain.CheckListAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration
{
    public class CheckListParamEntityTypeConfiguration : IEntityTypeConfiguration<CheckListParam>
    {
        public void Configure(EntityTypeBuilder<CheckListParam> builder)
        {
            builder.ToTable("CheckListParams", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Key).IsRequired();
            builder.Property(p => p.Value).IsRequired();
            builder.Property(p => p.Type).IsRequired();
            builder.Ignore(p => p.DomainEvents);

            builder.HasOne(pt => pt.CheckList)
                .WithMany(p => p.TextParams)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(pt => pt.CheckListId).IsRequired(false);
            builder.HasOne(pt => pt.CheckListItem)
                .WithMany(t => t.TextParams)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(pt => pt.CheckListItemId).IsRequired(false);

        }
    }
}
