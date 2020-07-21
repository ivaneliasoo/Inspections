using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.Domain
{
    public class User
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int? LastEditedReport { get; set; }
        public bool IsAdmin { get; set; }
    }
}
