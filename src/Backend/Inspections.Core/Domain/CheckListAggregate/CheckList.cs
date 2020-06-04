using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckList : Entity<int>
    {
        public string Text { get; set; }
        public List<CheckListParam> TextParams { get; set; }
        public bool Completed => !Checks.Any(c => c.Checked==CheckValue.True || c.Checked == CheckValue.NA);
        public string Annotation { get; set; }
        private readonly List<CheckListItem> _checks = new List<CheckListItem>();
        public IReadOnlyList<CheckListItem> Checks => _checks.AsReadOnly();
    }
}
