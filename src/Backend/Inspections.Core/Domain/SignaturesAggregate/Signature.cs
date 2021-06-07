using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.Domain.SignaturesAggregate
{
    public class Signature : Entity<int>, IAggregateRoot
    {
        public string Title { get; set; } = default!;
        public string? Annotation { get; set; }
        public Responsable Responsable { get; set; } = new Responsable();
        public string? ResponsableName => Responsable.Name;
        public string? Designation { get; set; }
        public string? Remarks { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Principal { get; set; }
        public bool IsConfiguration { get; set; }
        public int? ReportId { get; set; }
        public int? ReportConfigurationId { get; set; }
        public string? DrawnSign { get; set; }
        public short Order { get; set; }

        public Signature PreparteForNewReport()
        {
            Signature newSignature = (Signature)this.MemberwiseClone();
            if (newSignature is null) throw new Exception("error trying to add signatures to the report");
            newSignature.Id = 0;
            newSignature.ReportId = 0;
            newSignature.ReportConfigurationId = null;
            newSignature.Date = DateTime.Now;
            newSignature.IsConfiguration = false;
            newSignature.Responsable = new Responsable { Name = "" };

            return newSignature;
        }

        public Signature PreparteForNewReportConfiguration()
        {
            var newSignature = (Signature)this.MemberwiseClone();
            if (newSignature is null) throw new Exception("error trying to add signatures to the report");
            newSignature.Id = 0;
            newSignature.ReportId = null;
            newSignature.ReportConfigurationId = 0;
            newSignature.IsConfiguration = true;

            return newSignature;
        }

    }
}
