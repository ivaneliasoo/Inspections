using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.EnergyReport.Models {

    public class EnergyReportContext : DbContext
    {
        public EnergyReportContext(DbContextOptions<EnergyReportContext> options): base(options) {
        }
        
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<SchedJob>()
        //         .HasKey(sj => new { sj.team, sj.date });
        // }

        public DbSet<CurrentTable> CurrentTable { get; set; }
    }   

}