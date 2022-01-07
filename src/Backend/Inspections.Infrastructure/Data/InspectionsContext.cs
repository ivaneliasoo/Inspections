using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inspections.Core.Domain;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.PrintSectionsAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.QueryModels;
using Inspections.Infrastructure.ApplicationServices;
using Inspections.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.Infrastructure.Data
{
    public class InspectionsContext : DbContext
    {
        internal const string DEFAULT_SCHEMA = "Inspections";
        private readonly IMediator _mediator = default!;
        private readonly IUserNameResolver _userNameResolver = default!;

        public InspectionsContext(DbContextOptions<InspectionsContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public InspectionsContext(DbContextOptions<InspectionsContext> options, IMediator mediator,
            IUserNameResolver userNameResolver) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
        }

        public DbSet<Report> Reports { get; } = default!;
        public DbSet<CheckList> CheckLists { get; set; } = default!;
        public DbSet<CheckListItem> CheckListItems { get; set; } = default!;
        public DbSet<Note> Notes { get; set; } = default!;
        public DbSet<PhotoRecord> Photos { get; set; } = default!;
        public DbSet<ReportConfiguration> ReportConfigurations { get; set; } = default!;
        public DbSet<Signature> Signatures { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<EMALicense> Licenses { get; set; } = default!;
        public DbSet<OperationalReadings> OperationalReadings { get; set; } = default!;
        public DbSet<PrintSection> PrintSections { get; set; } = default!;
        public DbSet<CurrentTable> CurrentTable { get; set; } = default!;
        public DbSet<Template> Template { get; set; } = default!;

        public DbSet<SchedJob> SchedJob { get; set; } = default!;
        public DbSet<Job> Job { get; set; } = default!;
        public DbSet<Team> Team { get; set; } = default!;
        public DbSet<Options> Options { get; set; } = default!;

        //Queries
        public DbSet<ResumenCheckList> ResumenCheckLists { get; set; } = default!;
        public DbSet<ResumenReportConfiguration> ResumenReportConfigurations { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()))
            {
                if (entityType is ResumenReportConfiguration or EMALicense or DateTimeRange)
                    continue;

                modelBuilder.Entity(entityType.Name).Property<DateTimeOffset>("LastEdit").IsRequired();
                modelBuilder.Entity(entityType.Name).Property<string>("LastEditUser").IsRequired().HasMaxLength(20);
            }


            modelBuilder.Entity<User>()
                .HasKey(p => p.UserName);

            modelBuilder.Entity<User>()
                .Property(p => p.UserName)
                .HasMaxLength(20);
            modelBuilder.Entity<User>()
                .Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(p => p.LastName).IsRequired().HasMaxLength(50);


            modelBuilder.Entity<ResumenCheckList>().HasNoKey()
                .ToTable("ResumenCheckList", m => m.ExcludeFromMigrations());
            modelBuilder.Entity<ResumenReportConfiguration>().HasNoKey()
                .ToTable("ResumenReportConfiguration", m => m.ExcludeFromMigrations());

            modelBuilder.Entity<SchedJob>()
                .HasKey(sj => new { sj.team, sj.date });

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries()
                         .Where(e => e.State is EntityState.Added or EntityState.Modified))
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
