using Inspections.Core.Domain.ReportsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration
{
    public class ReportEntityTypeConfiguration : IEntityTypeConfiguration<Report>
    {
        public ReportEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Report> builder)
        {

            builder.ToTable("Reports", InspectionsContext.DEFAULT_SCHEMA);
            builder.Ignore(p => p.DomainEvents);
            builder.HasKey(p => p.Id);
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
