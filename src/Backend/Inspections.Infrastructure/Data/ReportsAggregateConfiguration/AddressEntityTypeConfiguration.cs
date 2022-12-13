using Inspections.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration;

class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Ignore(p => p.DomainEvents);

        builder.ToTable("Addresses", InspectionsContext.DEFAULT_SCHEMA);
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();
        builder.Property(p => p.Company).IsRequired(false);
        builder.Property(p => p.AddressLine).IsRequired();
        builder.Property(p => p.Unit);
        builder.Property(p => p.Country).IsRequired();
        builder.Property(p => p.PostalCode).IsRequired();
        builder.Property(p => p.LicenseId).IsRequired(false);
        builder.Property(p => p.AttentionTo).IsRequired(false);
        builder.HasOne(p => p.License);
    }
}
