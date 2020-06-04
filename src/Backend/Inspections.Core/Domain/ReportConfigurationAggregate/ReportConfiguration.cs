using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.Domain.ReportConfigurationAggregate
{
    public class ReportConfiguration : Entity<int>
    {
        public ReportType Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
        public List<CheckList> ChecksDefinition { get; set; }
        public List<Signature> SignatureDefinitions { get; set; }
    }
}
