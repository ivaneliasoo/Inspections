using Ardalis.GuardClauses;
using Inspections.Core.Domain.CheckListAggregate;
using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class BulkUpdateCheckItemsCommand : IRequest<bool>
    {
        public BulkUpdateCheckItemsCommand(int reportId, int checkListId, int newValue)
        {
            Guard.Against.NegativeOrZero(reportId, nameof(reportId));
            Guard.Against.NegativeOrZero(checkListId, nameof(checkListId));
            Guard.Against.EnumOutOfRange<CheckValue>(newValue, nameof(newValue));
            ReportId = reportId;
            CheckListId = checkListId;
            NewValue = (CheckValue)newValue;
        }

        public int ReportId { get; }
        public int CheckListId { get; }
        public CheckValue NewValue { get; }
    }
}
