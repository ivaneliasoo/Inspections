using System;
using System.Collections.Generic;
using System.Text;

namespace ReportsApp.Models
{
    public class PhotoRecord
    {
        public long Id { get; set; }
        public int ReportId { get; set; }
        public string FileName { get; set; }
        public string Label { get; set; }
    }
}
