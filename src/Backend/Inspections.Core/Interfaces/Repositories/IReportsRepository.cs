﻿using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.QueryModels;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces
{
    public interface IReportsRepository : IAsyncRepository<Report>
    {
        Task<IEnumerable<Report>> GetAll(string? filter, bool? closed, bool myReports = true);
        Task<ReportQueryResult> GetByIdAsync(int id, bool projected);
    }
}
