namespace Inspections.Core.QueryModels
{
    public class NoteQueryResult
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string Text { get; set; } = default!;
        public bool Checked { get; set; }
        public bool NeedsCheck { get; set; }
    }
}
