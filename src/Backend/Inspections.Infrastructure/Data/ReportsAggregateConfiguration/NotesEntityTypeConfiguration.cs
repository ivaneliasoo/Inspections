using Inspections.Core.Domain.PrintSectionsAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration
{
    public class NotesEntityTypeConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.Ignore(p => p.DomainEvents);

            builder.ToTable("Notes", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Text).IsRequired();
            builder.Property(p => p.Checked).IsRequired();
        }
    }


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
