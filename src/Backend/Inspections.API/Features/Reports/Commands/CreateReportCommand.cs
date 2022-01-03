using MediatR;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Ardalis.GuardClauses;

namespace Inspections.API.Features.Inspections.Commands
{
    public class CreateReportCommand : IRequest<int>
    {
        public int ConfigurationId { get; set; }
        public ReportType ReportType { get; set; }

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
