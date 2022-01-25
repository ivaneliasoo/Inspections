namespace Inspections.Core.QueryModels
{
    public class ResumenReportConfiguration
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = default!;
        public string FormName { get; set; } = default!;
        public string? RemarksLabelText { get; set; }
        public int DefinedCheckLists { get; set; }
        public int DefinedSignatures { get; set; }
        public int UsedByReports { get; set; }
        public bool Inactive { get; set; }
        public string PrintSection { get; set; } = default!;
    }
}
