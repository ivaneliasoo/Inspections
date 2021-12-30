using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace Inspections.Core.Domain
{
    [Table("Team")]
    public class Team
    {
        [Key]
        public int id { get; set; }

        public string? vehicle { get; set; }
        
        public string? foreman { get; set; }

        public int position { get; set; }

        string? status { get; set; }

        [NotMapped]
        public bool updated { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? lastUpdate { get; set; }

        // [Column("team_members", TypeName = "jsonb")]
        public string? teamMembers { get; set; }

        public DateTimeOffset LastEdit { get; set; }
        
        public string LastEditUser { get; set; }

        public Team() {
            this.id = 0;
            this.vehicle = "";
            this.status = "";
            this.foreman = "";
            this.position = 0;
            //this.teamMembers = null;
            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";            
        }

        public Team(int id, string vehicle, string foreman) {
            this.id = id;
            this.vehicle = vehicle;
            this.foreman = foreman;
            this.status = "";
            this.position = 0;
            //this.teamMembers = null;
            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";            
        }
    }
}
