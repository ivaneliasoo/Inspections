﻿using Inspections.Core.Domain.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration;

public class FormDefinitionEntityTypeConfiguration : IEntityTypeConfiguration<FormDefinition>
{
    public void Configure(EntityTypeBuilder<FormDefinition> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Title).HasMaxLength(150).IsRequired();
        builder.Property(p => p.DefaultValues).HasColumnType("jsonb");
        builder.Property(p => p.Fields).HasColumnType("jsonb").HasDefaultValueSql("'{ \"FieldsDefinitions\": null }'::jsonb");
    }
}