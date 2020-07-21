﻿using Inspections.Core;
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
        private readonly IUserNameResolver _userNameResolver;

        public ReportEntityTypeConfiguration(IUserNameResolver userNameResolver)
        {
            this._userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
        }

        public void Configure(EntityTypeBuilder<Report> builder)
        {

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

            builder.Property(p => p.IsClosed).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.FormName).IsRequired();
            builder.Property(p => p.RemarksLabelText).IsRequired();

            var navigationChecks = builder.Metadata.FindNavigation(nameof(Report.CheckList));
            navigationChecks.SetField("checkList");
            navigationChecks.SetPropertyAccessMode(PropertyAccessMode.Field);

            var navsPhotos = builder.Metadata.FindNavigation(nameof(Report.PhotoRecords));
            navsPhotos.SetField("photoRecords");
            navsPhotos.SetPropertyAccessMode(PropertyAccessMode.Field);

            var navigationNotes = builder.Metadata.FindNavigation(nameof(Report.Notes));
            navigationNotes.SetField("notes");
            navigationNotes.SetPropertyAccessMode(PropertyAccessMode.Field);

            var navigationSignatures = builder.Metadata.FindNavigation(nameof(Report.Signatures));
            navigationSignatures.SetField("signatures");
            navigationSignatures.SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
