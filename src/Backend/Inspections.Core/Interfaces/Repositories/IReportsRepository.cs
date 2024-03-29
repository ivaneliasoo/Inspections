﻿using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.QueryModels;
using Inspections.Shared;

namespace Inspections.Core.Interfaces.Repositories;

public interface IReportsRepository : IAsyncRepository<Report>
{
    Task<IEnumerable<ReportListItem>> GetAll(string? filter, bool? closed, bool myReports = true, string orderBy = "date", bool descending = true);
    Task<ReportQueryResult?> GetByIdAsync(int id, bool projected);
}
