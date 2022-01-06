using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable enable
namespace Inspections.Core.Domain
{
    public class JobScope
    {
        public string? scope { get; set; }

        public JobScope(string scp)
        {
            this.scope = scp;
        }
    }

    [Table("SchedJob")]
    public class SchedJob
    {
        public int id { get; set; }

        public int team { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        [Required]
        public DateTime? date { get; set; }

        public string? shift { get; set; }

        public bool splitShift { get; set; }

        public string? job1 { get; set; }

        public string? job2 { get; set; }

        public string? teamMembers { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? lastUpdate { get; set; }

        [NotMapped]
        public bool updated { get; set; }

        public DateTimeOffset LastEdit { get; set; }

        public string LastEditUser { get; set; }

        public bool excludeSunday { get; set; }

        public SchedJob(int id, int team, DateTime date, string shift, bool splitShift, string job1, string job2)
        {
            this.id = id;
            this.team = team;
            this.date = date;
            this.shift = shift;
            this.splitShift = splitShift;
            this.job1 = job1;
            this.job2 = job2;
            this.teamMembers = null;
            this.excludeSunday = true;
            this.LastEditUser = "";
            this.LastEdit = new DateTimeOffset();
        }

        public SchedJob()
        {
            this.id = 0;
            this.team = 0;
            this.date = new DateTime();
            this.shift = "";
            this.splitShift = false;
            this.job1 = "";
            this.job2 = "";
            this.teamMembers = null;
            this.excludeSunday = true;
            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";
        }
    }

}
