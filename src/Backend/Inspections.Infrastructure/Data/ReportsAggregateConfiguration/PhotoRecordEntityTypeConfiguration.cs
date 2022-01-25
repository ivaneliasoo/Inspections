using Inspections.Core.Domain.ReportsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.ReportsAggregateConfiguration
{
    public class PhotoRecordEntityTypeConfiguration : IEntityTypeConfiguration<PhotoRecord>
    {
        public void Configure(EntityTypeBuilder<PhotoRecord> builder)
        {
            builder.ToTable("Photos", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Label).IsRequired();
            builder.Property(p => p.FileName).IsRequired();
            builder.Property(p => p.FileNameResized).IsRequired();
            builder.Ignore(p => p.DomainEvents);
        }
    }
}
