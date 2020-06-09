using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Shared;

namespace Inspections.Core.Interfaces
{
    public interface IReportConfigurationsRepository : IAsyncRepository<ReportConfiguration>
    {
    }
}
