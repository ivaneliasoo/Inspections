using Inspections.Shared;
using System;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckListParam:Entity<int>
    {
        public int? CheckListId { get; set; }
        public int? CheckListItemId { get; set; }
        public CheckListItem CheckListItem { get; set; }
        public CheckList CheckList { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public CheckListParamType Type { get; set; }

        private CheckListParam() { } //Required by EF

        public CheckListParam(int? checkListId, int? checkListItemId, string key, string value, CheckListParamType type)
        {
            CheckListId = checkListId;
            CheckListItemId = checkListItemId;
            Key = key;
            Value = value;
            Type = type;
        }
    }
}
