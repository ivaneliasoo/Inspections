using Inspections.Shared;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckListParam:Entity<int>
    {
        public int CheckListItemId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public CheckListParamType Type { get; set; }
    }
}
