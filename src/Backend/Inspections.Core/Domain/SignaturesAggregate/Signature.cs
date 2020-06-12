﻿using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.Domain.SignaturesAggregate
{
    public class Signature : Entity<int>, IAggregateRoot
    {
        public string Title { get; set; }
        public string Annotation { get; set; }
        public Responsable Responsable { get; set; }
        public string Designation { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Principal { get; set; }
        public bool IsConfiguration { get; set; }
        public int? ReportId { get; set; }
        public Report Report { get; set; }
        public int? ReportConfigurationId { get; set; }
        public ReportConfiguration  ReportConfiguration { get; set; }

    }
}
