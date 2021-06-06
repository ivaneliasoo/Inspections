using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace Inspections.Core.Domain.ReportsAggregate
{
    public class PhotoRecord : Entity<int>
    {
        public int ReportId { get; private set; }
        public string FileName { get; private set; } = default!;
        public string FileNameResized { get; private set; } = default!;
        public string? Label { get; set; }
        private PhotoRecord() { }

        public PhotoRecord(int reportId, string path, string pathResized, string? label)
        {
            ReportId = reportId;
            Label = label;
            FileName = Path.Combine(AppContext.BaseDirectory, reportId.ToString(), path);
            FileNameResized = Path.Combine(AppContext.BaseDirectory, reportId.ToString(), pathResized);
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
