using Inspections.Core.Domain.PrintSectionsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration
{
    public class PrintSectionEntityTypeConfiguration : IEntityTypeConfiguration<PrintSection>
    {
        public void Configure(EntityTypeBuilder<PrintSection> builder)
        {
            builder.Ignore(p => p.DomainEvents);

            builder.ToTable("PrintSections", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Code).IsRequired();
            builder.Property(p => p.Content).IsRequired();
            builder.Property(p => p.IsMainReport).HasDefaultValue(false);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(PrintSectionStatus.Active);
        }
    }

}
