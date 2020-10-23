﻿using System;
using System.Collections.Generic;
using System.Text;
using Inspections.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration
{
    class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Ignore(p => p.DomainEvents);

            builder.ToTable("Addresses", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.AddressLine).IsRequired();
            builder.Property(p => p.Unit).IsRequired();
            builder.Property(p => p.Country).IsRequired();
            builder.Property(p => p.PostalCode).IsRequired();
            builder.Property(p => p.LicenseId).IsRequired();
            builder.HasOne(p => p.License).WithOne(a => a.Address).HasForeignKey<Address>(f => f.LicenseId);
        }
    }
}
