namespace ReportsApp.Models
{
    public class Note
    {
        public int reportId { get; set; }
        public string text { get; set; }
        public bool @checked { get; set; }
        public bool needsCheck { get; set; }
        public int id { get; set; }
        public object domainEvents { get; set; }
    }
}
