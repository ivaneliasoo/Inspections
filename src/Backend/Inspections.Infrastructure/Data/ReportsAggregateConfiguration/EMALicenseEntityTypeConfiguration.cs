using System;
using System.Collections.Generic;
using System.Text;
using Inspections.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration
{
    public class EMALicenseEntityTypeConfiguration : IEntityTypeConfiguration<EMALicense>
    {
        public void Configure(EntityTypeBuilder<EMALicense> builder)
        {
            builder.OwnsOne(p => p.Validity, l =>
            {
                l.Property(p => p.Start).IsRequired();
                l.Property(p => p.End).IsRequired();
            });
        }
    }
}
