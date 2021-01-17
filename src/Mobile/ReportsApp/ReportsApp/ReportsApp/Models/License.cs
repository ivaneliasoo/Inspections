using System;
using System.Collections.Generic;
using System.Text;

namespace ReportsApp.Models
{
    public class License
    {
        public int LicenseId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string PersonInCharge { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public decimal Amp { get; set; }
        public decimal Volt { get; set; }
        public decimal KVA { get; set; }
        public DateTime ValidityStart { get; set; }
        public DateTime ValidityEnd { get; set; }
    }
}