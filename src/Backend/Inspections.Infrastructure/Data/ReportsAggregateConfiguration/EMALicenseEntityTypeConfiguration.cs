using Inspections.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration;

public class EMALicenseEntityTypeConfiguration : IEntityTypeConfiguration<EMALicense>
{
    public void Configure(EntityTypeBuilder<EMALicense> builder)
    {
        builder.ToTable("Licenses", InspectionsContext.DEFAULT_SCHEMA);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.PersonInCharge).IsRequired();
        builder.Property(p => p.Contact).IsRequired();
        builder.Property(p => p.Email).IsRequired();
        builder.Property(p => p.Amp).IsRequired();
        builder.Property(p => p.Volt).IsRequired();
        builder.Property(p => p.KVA).IsRequired();
        builder.OwnsOne(p => p.Validity, l =>
        {
            l.Property(p => p.Start).IsRequired();
            l.Property(p => p.End).IsRequired();
        });
    }
}
