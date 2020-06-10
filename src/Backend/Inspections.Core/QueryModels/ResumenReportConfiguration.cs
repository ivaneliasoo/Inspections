using Inspections.Core.Domain.ReportConfigurationAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.QueryModels
{
    public class ResumenReportConfiguration
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
        public int DefinedCheckLists { get; set; }
        public int DefinedSignatures { get; set; }
        public int UsedByReports { get; set; }
    }
}
