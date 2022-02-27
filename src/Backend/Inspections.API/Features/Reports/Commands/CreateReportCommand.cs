using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using JetBrains.Annotations;
using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class CreateReportCommand : IRequest<int>
    {
        public int ConfigurationId { get; init; }
        public ReportType ReportType { [UsedImplicitly] get; init; }

        private CreateReportCommand()
        {

        }
        public CreateReportCommand(int configurationId, ReportType reportType)
        {
            Guard.Against.NegativeOrZero(configurationId, nameof(configurationId));
            ConfigurationId = configurationId;
            ReportType = reportType;
        }
    }
}
