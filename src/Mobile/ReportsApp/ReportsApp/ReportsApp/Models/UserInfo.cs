using System;
using System.Collections.Generic;
using System.Text;

namespace ReportsApp.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int? LastEditedReport { get; set; }
        public bool IsAdmin { get; set; }
        public UserInfo()
        {

        }
    }
}
