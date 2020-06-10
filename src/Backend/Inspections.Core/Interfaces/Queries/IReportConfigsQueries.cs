using Inspections.Core.QueryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inspections.Infrastructure.Queries
{
    public interface IReportConfigurationsQueries
    {
        Task<IEnumerable<ResumenReportConfiguration>> GetByFilter(string filter);
    }
}