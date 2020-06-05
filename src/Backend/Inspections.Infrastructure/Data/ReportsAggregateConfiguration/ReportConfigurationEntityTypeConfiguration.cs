using Inspections.Core.Domain.ReportConfigurationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration
{
    public class ReportConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<ReportConfiguration>
    {
        public void Configure(EntityTypeBuilder<ReportConfiguration> builder)
        {
            builder.ToTable("ReportsConfiguration", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.RemarksLabelText).IsRequired(false);
            builder.Property(p => p.FormName).IsRequired();

            builder.HasMany(p => p.ChecksDefinition)
                .WithOne()
                .IsRequired(false)
                .HasForeignKey("ReportConfigurationId");

            builder.HasMany(p => p.SignatureDefinitions)
                .WithOne()
                .IsRequired(false)
                .HasForeignKey("ReportConfigurationId");


            builder.Ignore(p => p.DomainEvents);
        }
    }
}
