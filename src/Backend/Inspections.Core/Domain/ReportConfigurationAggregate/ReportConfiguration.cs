﻿using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.Domain.ReportConfigurationAggregate
{
    public class ReportConfiguration : Entity<int>, IAggregateRoot
    {
        public ReportType Type { get; set; }
        public string Title { get; set; } = default!;
        public string FormName { get; set; } = default!;
        public string RemarksLabelText { get; set; } = default!;
        public List<CheckList> ChecksDefinition { get; set; } = default!;
        public List<Signature> SignatureDefinitions { get; set; } = default!;
        public bool Inactive { get; set; }
    }
}
