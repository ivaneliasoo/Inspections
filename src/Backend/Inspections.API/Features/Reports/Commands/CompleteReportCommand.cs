using Ardalis.GuardClauses;
using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class CompleteReportCommand : IRequest<bool>
    {
        public CompleteReportCommand(int reportId)
        {
            Guard.Against.NegativeOrZero(reportId, nameof(reportId));
            ReportId = reportId;
        }

        public int ReportId { get; }
    }
}
