using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain
{
    [Table("Template")]
    public class Template
    {
        [Key]
        public int id { get; set; }

        public string? templateDef { get; set; }

        public override string ToString()
        {
            return $"{id} {templateDef}";
        }

        public DateTimeOffset LastEdit { get; set; }

        public string LastEditUser { get; set; }

        public Template()
        {
            id = 0;
            templateDef = "";
            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";
        }
    }
}
