using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable enable
namespace Inspections.Core.Domain
{
    [Table("Options")]
    public class Options
    {
        [Key]
        public int id { get; set; }

        public int scheduleWeeks { get; set; }
        
        public int autosaveInterval { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime lastUpdate { get; set; }

        public DateTimeOffset LastEdit { get; set; }
        
        public string LastEditUser { get; set; }

        public Options() {
            this.id = 1;
            this.scheduleWeeks = 1;
            this.autosaveInterval = 120;
            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";            
        }

        public Options(int id, int scheduleWeeks, int autosaveInterval) {
            this.id = id;
            this.scheduleWeeks = scheduleWeeks;
            this.autosaveInterval = autosaveInterval;
            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";            
        }
    }
}
