using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckList : Entity<int>, IAggregateRoot
    {
        public int? ReportId { get; set; }
        public Report? Report { get; set; }
        public int? ReportConfigurationId { get; set; }
        public ReportConfiguration ReportConfiguration { get; set; } = default!;
        public string Text { get; private set; } = default!;
        public string? Annotation { get; private set; }
        public bool IsConfiguration { get; set; }
        public bool Completed => _checks.All(c => c.Checked != CheckValue.None);
        public bool @Checked => _checks.Any(c => c.Checked != CheckValue.None);
        private readonly List<CheckListItem> _checks = new();

        public CheckList(string text, string? annotation, bool isConfiguration)
        {
            Text = text;
            Annotation = annotation;
            IsConfiguration = isConfiguration;
            ReportId = null;
        }

        private CheckList() { } //Required by EF

        public IReadOnlyList<CheckListItem> Checks => _checks.AsReadOnly();

        public void Edit(string text, string? annotation, bool isConfiguration)
        {

            Text = text;
            Annotation = annotation;
            IsConfiguration = isConfiguration;
        }

        public void SetValue(CheckValue checkValue)
        {
            foreach (var check in Checks)
            {
                check.Checked = checkValue;
                check.Touched = true;
            }
        }

        public void AddCheckItems(CheckListItem checkListItem)
        {
            Guard.Against.Null(checkListItem, nameof(checkListItem));

            _checks.Add(checkListItem);
        }

        public void AddCheckItems(IEnumerable<CheckListItem> checkListItem)
        {
            Guard.Against.Null(checkListItem, nameof(checkListItem));

            _checks.AddRange(checkListItem);
        }

        public void RemoveCheckItems(CheckListItem checkListItem)
        {
            Guard.Against.Null(checkListItem, nameof(checkListItem));

            _checks.Remove(checkListItem);
        }

        public CheckList CloneForReport()
        {
            var newCheckList = new CheckList(this.Text, this.Annotation, false)
            {
                ReportConfigurationId = this.ReportConfigurationId
            };

            foreach (var check in Checks)
            {
                newCheckList.AddCheckItems(new CheckListItem(0, check.Text, CheckValue.NotApplicable, check.Editable, check.Required, check.Remarks));
            }
            return newCheckList;
        }

        public CheckList CloneForReportConfiguration()
        {
            var newCheckList = new CheckList(this.Text, this.Annotation, true)
            {
                ReportConfigurationId = 0,
                ReportId = null
            };

            foreach (var check in Checks)
            {
                newCheckList.AddCheckItems(new CheckListItem(0, check.Text, check.Checked, check.Editable, check.Required, check.Remarks));
            }
            return newCheckList;
        }
    }
}
