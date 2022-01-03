using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.QueryModels;
using Inspections.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces
{
    public interface IReportsRepository : IAsyncRepository<Report>
    {
        Task<IEnumerable<ReportListItem>> GetAll(string? filter, bool? closed, bool myReports = true, string orderBy = "date", bool descending = true);
        Task<ReportQueryResult> GetByIdAsync(int id, bool projected);
    }
}
