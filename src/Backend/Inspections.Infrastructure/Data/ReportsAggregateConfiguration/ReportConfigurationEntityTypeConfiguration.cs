﻿using Inspections.Core.Domain.ReportConfigurationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration;

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
        builder.Property(p => p.Inactive).IsRequired().HasDefaultValue(false);
        builder.Property(p => p.Footer).IsRequired();
        builder.Property(p => p.MarginTop).IsRequired();
        builder.Property(p => p.MarginBottom).IsRequired();
        builder.Property(p => p.MarginLeft).IsRequired();
        builder.Property(p => p.MarginRight).IsRequired();
        builder.Property(p => p.PrintSectionId).IsRequired();
        builder.Property(p => p.CheckListMetadata).HasColumnType("jsonb").HasDefaultValueSql("'{ \"Display\": 0 }'::jsonb");
        builder.Property(p => p.TemplateName).IsRequired().HasDefaultValue("print");
        builder.Ignore(p => p.DomainEvents);
    }
}
