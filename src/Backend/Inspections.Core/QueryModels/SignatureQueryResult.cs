using System;

namespace Inspections.Core.QueryModels
{
    public class SignatureQueryResult
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Annotation { get; set; }
        public string? Designation { get; set; }
        public string? DrawnSign { get; set; }
        public bool Principal { get; set; }
        public string? Remarks { get; set; }
        public int? ResponsibleType { get; set; }
        public string? ResponsibleName { get; set; }
        public string Title { get; set; } = default!;
        public short Order { get; set; }
    }
}
