using Inspections.Shared;
using System.Collections.Generic;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckListItem : Entity<int>
    {
        public int CheckListId { get; set; }
        public string Text { get; set; } = default!;
        public CheckValue Checked { get; set; }
        public bool Editable { get; set; }
        public bool Required { get; set; }
        public string? Remarks { get; set; }

        private CheckListItem() { } //Required by EF

        public CheckListItem(int checkListId, string text, CheckValue @checked, bool editable, bool required, string? remarks)
        {
            CheckListId = checkListId;
            Text = text;
            Checked = @checked;
            Editable = editable;
            Required = required;
            Remarks = remarks;
        }
    }
}
