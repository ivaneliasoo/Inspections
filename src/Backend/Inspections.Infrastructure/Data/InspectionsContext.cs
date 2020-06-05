using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.InspectionReportAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Infrastructure.Data
{
    public class InspectionsContext : DbContext
    {
        public InspectionsContext(DbContextOptions options)
            :base(options)
        {

        }
        public ICollection<InspectionReport> Inspections { get; set; }
        public ICollection<CheckList> CheckLists { get; set; }
        public ICollection<CheckListItem> CheckListItems { get; set; }
        public ICollection<CheckListParam> CheckListParams { get; set; }
        public ICollection<CheckListParamType> CheckListParamTypes { get; set; }
        public ICollection<CheckValue> CheckValues { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<PhotoRecord> Photos { get; set; }
        public ICollection<ReportConfiguration> ReportConfigurations { get; set; }
        public ICollection<Signature> Signatures { get; set; }
        public ICollection<ResponsableType> ResponsableTypes { get; set; }
        public ICollection<ReportType> ReportTypes { get; set; }

    }
}
