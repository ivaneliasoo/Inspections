using Inspections.Core.Domain.ReportsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration;

public class ReportFormEntityTypeConfiguration : IEntityTypeConfiguration<ReportForm>
{
    public void Configure(EntityTypeBuilder<ReportForm> builder)
    {
        builder.ToTable("ReportFroms", InspectionsContext.DEFAULT_SCHEMA);
        builder.Ignore(p => p.DomainEvents);
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Enabled);
        builder.Property(p => p.Values).HasColumnType("jsonb");
        builder.Property(p => p.Fields).HasColumnType("jsonb").HasDefaultValueSql("'{ \"FieldsDefinitions\": null }'::jsonb");
        builder.Property(p => p.Icon).IsRequired(false);
        builder.Property(p => p.Name).HasMaxLength(50);
        builder.Property(p => p.Title).HasMaxLength(255);
    }
}
