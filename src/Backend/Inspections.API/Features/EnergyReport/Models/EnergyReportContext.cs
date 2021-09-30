using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.EnergyReport.Models {

    public class EnergyReportContext : DbContext
    {
        public EnergyReportContext(DbContextOptions<EnergyReportContext> options): base(options) {
        }
        
        public DbSet<CurrentTable> CurrentTable { get; set; }
    }   

}