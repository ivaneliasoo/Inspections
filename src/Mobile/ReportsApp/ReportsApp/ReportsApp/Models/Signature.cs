using System;

namespace ReportsApp.Models
{
    public class Signature
    {
        public string title { get; set; }
        public string annotation { get; set; }
        public Responsable responsable { get; set; }
        public object responsableName { get; set; }
        public object designation { get; set; }
        public object remarks { get; set; }
        public DateTime date { get; set; }
        public bool principal { get; set; }
        public bool isConfiguration { get; set; }
        public int reportId { get; set; }
        public int reportConfigurationId { get; set; }
        public object reportConfiguration { get; set; }
        public object drawedSign { get; set; }
        public int id { get; set; }
        public object domainEvents { get; set; }
    }
}
