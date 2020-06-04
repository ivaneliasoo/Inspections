using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace Inspections.Core.Domain.InspectionReportAggregate
{
    public class PhotoRecord : Entity<int>
    {
        private const string BasePath = "img/Reports";
        
        public Guid ReportId { get; private set; }
        public string FilePath { get; private set; }
        public string Label { get; private set; }

        internal PhotoRecord(Guid reportId, string path, string label)
        {
            ReportId = reportId;
            Label = label;
            FilePath = Path.Combine(AppContext.BaseDirectory, reportId.ToString(), BasePath, path);
        }

        public override string ToString()
        {
            return FilePath;
        }
    }
}
