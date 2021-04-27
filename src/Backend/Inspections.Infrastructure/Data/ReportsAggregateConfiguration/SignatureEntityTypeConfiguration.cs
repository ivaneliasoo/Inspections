using Inspections.Core.Domain.SignaturesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration
{
    public class SignatureEntityTypeConfiguration : IEntityTypeConfiguration<Signature>
    {
        public void Configure(EntityTypeBuilder<Signature> builder)
        {
            builder.ToTable("Signatures", InspectionsContext.DEFAULT_SCHEMA);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.Annotation).IsRequired(false);
            builder.Property(p => p.Designation).IsRequired(false);
            builder.Property(p => p.Remarks).IsRequired(false);
            builder.Property(p => p.Date).IsRequired();
            builder.Property(p => p.Principal).IsRequired();
            builder.OwnsOne(p => p.Responsable
            , s =>
            {
                s.Property(p => p.Name).IsRequired();
                s.Property(p => p.Type).IsRequired();
                s.Ignore("LastEdit");
                s.Ignore("LastEditUser");
            });

            builder.Property(p => p.Order).IsRequired();

            builder.Ignore(p => p.DomainEvents);

        }
    }

}
