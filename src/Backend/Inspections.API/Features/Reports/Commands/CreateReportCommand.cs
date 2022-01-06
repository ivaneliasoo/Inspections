using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;

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
