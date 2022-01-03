using Inspections.Core.QueryModels;
using System.Collections.Generic;

namespace Inspections.Infrastructure.Queries
{
    public interface IReportConfigurationsQueries
    {
        IEnumerable<ResumenReportConfiguration> GetByFilter(string? filter);
    }
}
