using Inspections.Core.Domain.ReportsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration
{
    public class ReportEntityTypeConfiguration : IEntityTypeConfiguration<Report>
    {

        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.Ignore(p => p.CheckList);
            builder.Ignore(p => p.PhotoRecords);
            builder.Ignore(p => p.Notes);
            builder.Ignore(p => p.Signatures);

            builder.ToTable("Reports", InspectionsContext.DEFAULT_SCHEMA);
            builder.Ignore(p => p.DomainEvents);
            builder.HasKey(p => p.Id);
            builder.OwnsOne(p => p.License, ir =>
            {
                ir.Property(p => p.Number).IsRequired();
                ir.Property(p => p.LicenseType).IsRequired();
                ir.OwnsOne(p => p.Validity, l =>
                {
                    l.Property(p => p.Start).IsRequired();
                    l.Property(p => p.End).IsRequired();
                });
            });
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Address).IsRequired();
            builder.Property(p => p.Date).IsRequired();

            builder.Metadata.FindNavigation(nameof(Report.CheckList));
            builder.Metadata.FindNavigation(nameof(Report.PhotoRecords));
            builder.Metadata.FindNavigation(nameof(Report.Notes));
            builder.Metadata.FindNavigation(nameof(Report.Signatures));
        }
    }
}
