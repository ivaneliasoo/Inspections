using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspections.Core.Domain
{
    [Table("current")]
    public class CurrentTable
    {
        [Key]
        public int id { get; set; }

        public string circuit { get; set; }

        [Column("start_date")]
        public string startDate { get; set; }

        [Column("end_date")]
        public string endDate { get; set; }

        [Column("current_data", TypeName = "jsonb")]
        public string currentData { get; set; }

        public override string ToString() {
            return $"{id} {circuit} {startDate} {endDate} {currentData}";
        }

        public CurrentTable() {
            id = 0;
            circuit = "";
            startDate = "";
            endDate = "";
            currentData = "";
        }        
    }
}
