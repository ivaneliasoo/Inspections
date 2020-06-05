using Ardalis.GuardClauses;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckList : Entity<int>
    {
        public string Text { get; private set; }
        public List<CheckListParam> TextParams { get; private set; }
        public bool Completed => !Checks.Any(c => c.Checked==CheckValue.True || c.Checked == CheckValue.NA);
        public string Annotation { get; private set; }
        private readonly List<CheckListItem> _checks = new List<CheckListItem>();
        public IReadOnlyList<CheckListItem> Checks => _checks.AsReadOnly();
        public bool IsConfiguration { get; private set; }


        public void AddCheckItems(CheckListItem checkListItem)
        {
            Guard.Against.Null(checkListItem, nameof(checkListItem));
            ValidateCanEdit();

            _checks.Add(checkListItem);
        }

        private void ValidateCanEdit()
        {
            if (Completed && !IsConfiguration)
                throw new InvalidOperationException($"add items to {Completed} {nameof(CheckList)} isn't allowed");
        }

        public void AddCheckItems(IEnumerable<CheckListItem> checkListItem)
        {
            Guard.Against.Null(checkListItem, nameof(checkListItem));
            ValidateCanEdit();

            _checks.AddRange(checkListItem);
        }
    }
}
