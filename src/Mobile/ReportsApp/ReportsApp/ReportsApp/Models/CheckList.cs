using System.Collections.Generic;

namespace ReportsApp.Models
{
    public class CheckList
    {
        public int reportId { get; set; }
        public int reportConfigurationId { get; set; }
        public object reportConfiguration { get; set; }
        public string text { get; set; }
        public List<object> textParams { get; set; }
        public string annotation { get; set; }
        public bool isConfiguration { get; set; }
        public bool completed { get; set; }
        public bool @checked { get; set; }
        public List<Check> checks { get; set; }
        public int id { get; set; }
        public object domainEvents { get; set; }
    }


    public class GroupedChecks:List<Check>
    {
        public int reportId { get; set; }
        public int id { get; set; }
        public bool completed { get; set; }
        public bool @checked { get; set; }
        public string Name { get; set; }

        public GroupedChecks(int reportId, int id, bool completed, bool @checked, List<Check> checks, string name) : base(checks)
        {
            this.reportId = reportId;
            this.id = id;
            this.completed = completed;
            this.@checked = @checked;
            Name = name;
        }
    }
}
