namespace Inspections.Core.QueryModels
{
    public class ResumenCheckList
    {
        public int Id { get; set; }
        public string Text { get; private set; } = default!;
        public string? Annotation { get; private set; }
        public int TotalItems { get; set; }
    }
}
