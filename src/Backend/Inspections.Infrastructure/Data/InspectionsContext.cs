#nullable disable
using Inspections.Core;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.QueryModels;
using Inspections.Infrastructure.Data.InspectionReportsAggregateConfiguration;
using Inspections.Infrastructure.Data.ReportsAggregateConfiguration;
using Inspections.Shared;
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
        private readonly IUserNameResolver _userNameResolver;


        public InspectionsContext(DbContextOptions options)
            : base(options)
        {

        }


        public InspectionsContext(DbContextOptions options, IMediator mediator, IUserNameResolver userNameResolver) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));

        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<CheckList> CheckLists { get; set; }
        public DbSet<CheckListItem> CheckListItems { get; set; }
        public DbSet<Note> Notes { get; set; }  
        public DbSet<PhotoRecord> Photos { get; set; }
        public DbSet<ReportConfiguration> ReportConfigurations { get; set; }
        public DbSet<Signature> Signatures { get; set; }

        public DbSet<Core.Domain.User> Users { get; set; }
        public DbSet<Core.Domain.Address> Addresses { get; set; }
        public DbSet<Core.Domain.EMALicense> Licenses { get; set; }
        public DbSet<OperationalReadings> OperationalReadings { get; set; }


        //Queries

        public DbSet<ResumenCheckList> ResumenCheckLists { get; set; }
        public DbSet<ResumenReportConfiguration> ResumenReportConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PhotoRecordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SignatureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CheckListItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CheckListEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AddressEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EMALicenseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfigurationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportEntityTypeConfiguration());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()))
            {
                if (entityType is ResumenReportConfiguration)
                    continue;

                if (entityType is Core.Domain.EMALicense)
                    continue;

                if (entityType is DateTimeRange)
                    continue;

                modelBuilder.Entity(entityType.Name).Property<DateTimeOffset>("LastEdit").IsRequired();
                modelBuilder.Entity(entityType.Name).Property<string>("LastEditUser").IsRequired().HasMaxLength(20);
            }


            modelBuilder.Entity<Core.Domain.User>()
                .HasKey(p => p.UserName);

            modelBuilder.Entity<Core.Domain.User>()
            .Property(p => p.UserName)
                .HasMaxLength(20);
            modelBuilder.Entity<Core.Domain.User>()
            .Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Core.Domain.User>()
            .Property(p => p.LastName).IsRequired().HasMaxLength(50);


            modelBuilder.Entity<ResumenCheckList>().HasNoKey().ToTable("ResumenCheckList", m=>m.ExcludeFromMigrations());
            modelBuilder.Entity<ResumenReportConfiguration>().HasNoKey().ToTable("ResumenReportConfiguration", m => m.ExcludeFromMigrations());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entity.CurrentValues.EntityType.DisplayName() == "Responsible")
                    continue;

                if (entity.CurrentValues.EntityType.DisplayName() == "License")
                    continue;

                if (entity.CurrentValues.EntityType.DisplayName() == "License.Validity#DateTimeRange")
                    continue;

                if (entity.CurrentValues.EntityType.DisplayName() == "DateTimeRange")
                    continue;
                
                if (entity.CurrentValues.EntityType.DisplayName() == "EMALicense.Validity#DateTimeRange")
                    continue;

                entity.Property("LastEdit").CurrentValue = DateTimeOffset.UtcNow;
                entity.Property("LastEditUser").CurrentValue = _userNameResolver.UserName ?? "Seed";
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
