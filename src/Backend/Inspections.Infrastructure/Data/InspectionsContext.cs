using Inspections.Core.Domain.CheckListAggregate;

using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Inspections.Infrastructure.Data
{
    public class InspectionsContext : DbContext
    {
        internal const string DEFAULT_SCHEMA = "Inspections";
        private readonly IMediator _mediator;


        public InspectionsContext(DbContextOptions options)
            :base(options)
        {

        }

        public InspectionsContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
               .EnableSensitiveDataLogging()
               .UseSqlServer("Data Source = .; User=sa; Password=15560367o%; Initial Catalog = InspectionsDb");
        }

        public DbSet<Report> Inspections { get; set; }
        public DbSet<CheckList> CheckLists { get; set; }
        public DbSet<CheckListItem> CheckListItems { get; set; }
        public DbSet<CheckListParam> CheckListParams { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<PhotoRecord> Photos { get; set; }
        public DbSet<ReportConfiguration> ReportConfigurations { get; set; }
        public DbSet<Signature> Signatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PhotoRecordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SignatureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CheckListParamEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CheckListItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CheckListEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfigurationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportEntityTypeConfiguration());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()))
            {
                modelBuilder.Entity(entityType.Name).Property<DateTimeOffset>("LastEdit").IsRequired();
            }

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entidad in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                entidad.Property("LastEdit").CurrentValue = DateTimeOffset.UtcNow;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await this.SaveChangesAsync();
            return true;
        }
    }
}
