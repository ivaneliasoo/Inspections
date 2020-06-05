namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckListParam
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public CheckListParamType Type { get; set; }
    }
}
