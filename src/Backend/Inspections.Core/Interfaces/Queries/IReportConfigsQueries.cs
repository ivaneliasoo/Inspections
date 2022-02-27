using Inspections.Core.QueryModels;

namespace Inspections.Core.Interfaces.Queries;

public interface IReportConfigurationsQueries
{
    IEnumerable<ResumenReportConfiguration> GetByFilter(string? filter);
}
