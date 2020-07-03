﻿using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckList : Entity<int>, IAggregateRoot
    {
        public int? ReportId { get; set; }
        public Report Report { get; set; }
        public int? ReportConfigurationId { get; set; }
        public ReportConfiguration ReportConfiguration { get; set; }
        public string Text { get; private set; }
        public List<CheckListParam> TextParams { get; private set; } = new List<CheckListParam>();
        public string Annotation { get; private set; }
        public bool IsConfiguration { get; set; }
        private readonly List<CheckListItem> _checks = new List<CheckListItem>();

        public CheckList(string text, List<CheckListParam> textParams, string annotation, bool isConfiguration)
        {
            Text = text;
            TextParams = textParams;
            Annotation = annotation;
            IsConfiguration = isConfiguration;
            ReportId = null;
        }

        private CheckList() { } //Required by EF

        public IReadOnlyList<CheckListItem> Checks => _checks.AsReadOnly();
        public bool Completed => !Checks.Any(c => c.Required && (c.Checked != CheckValue.False || c.Checked != CheckValue.NA));

        public void Edit(string text, string annotation, bool isConfiguration)
        {
            
            Text = text;
            Annotation = annotation;
            IsConfiguration = isConfiguration;
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

        public void AddCheckListParams(CheckListParam checkListParam)
        {
            Guard.Against.Null(checkListParam, nameof(checkListParam));
            
            TextParams.Add(checkListParam);
        }

        public void AddCheckListParams(IEnumerable<CheckListParam> checkListParams)
        {
            Guard.Against.Null(checkListParams, nameof(checkListParams));
            
            TextParams.AddRange(checkListParams);
        }

        public CheckList CloneForReport()
        {
            var newCheckList = new CheckList(this.Text, null, this.Annotation, false);
            newCheckList.ReportConfigurationId = this.ReportConfigurationId;

            foreach (var check in Checks)
            {
                var parameters = new List<CheckListParam>();
                foreach (var param in check.TextParams)
                {
                    parameters.Add(new CheckListParam(null, 0, param.Key, param.Value, param.Type));
                }
                newCheckList.AddCheckItems(new CheckListItem(0, check.Text, check.Checked, check.Required, check.Remarks, parameters));
            }
            return newCheckList;
        }

        public CheckList CloneForReportConfiguration()
        {
            var newCheckList = new CheckList(this.Text, null, this.Annotation, true);
            newCheckList.ReportConfigurationId = 0;
            newCheckList.ReportId = null;

            foreach (var check in Checks)
            {
                var parameters = new List<CheckListParam>();
                foreach (var param in check.TextParams)
                {
                    parameters.Add(new CheckListParam(null, 0, param.Key, param.Value, param.Type));
                }
                newCheckList.AddCheckItems(new CheckListItem(0, check.Text, check.Checked, check.Required, check.Remarks, parameters));
            }
            return newCheckList;
        }
    }
}
