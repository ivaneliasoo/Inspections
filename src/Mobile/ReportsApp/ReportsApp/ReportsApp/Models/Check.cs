using System.Collections.Generic;

namespace ReportsApp.Models
{
    public class Check
    {
        public int checkListId { get; set; }
        public string text { get; set; }
        public int @checked { get; set; }
        public bool editable { get; set; }
        public bool required { get; set; }
        public string remarks { get; set; }
        public List<object> textParams { get; set; }
        public int id { get; set; }
        public object domainEvents { get; set; }
    }
}
