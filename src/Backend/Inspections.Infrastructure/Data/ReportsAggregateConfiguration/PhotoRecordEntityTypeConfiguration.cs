using Inspections.Core.Domain.ReportsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration
{
    public class PhotoRecordEntityTypeConfiguration : IEntityTypeConfiguration<PhotoRecord>
    {
        public void Configure(EntityTypeBuilder<PhotoRecord> builder)
        {
            builder.ToTable("Photos", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Label).IsRequired();
            builder.Property(p => p.FilePath).IsRequired();
            builder.Property(p => p.ReportId).IsRequired();

            builder.Ignore(p => p.DomainEvents);

        }
    }
}
