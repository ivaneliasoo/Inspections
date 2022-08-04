using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// Store Energy Report data using JSON POCO mapping
// https://www.npgsql.org/efcore/mapping/json.html?tabs=data-annotations%2Cpoco
#nullable enable
namespace Inspections.Core.Domain
{
    [Table("PowerAnalyzerReport")]
    public class PowerAnalyzerReport
    {
        [Key]
        public long id { get; set; }

        public string name { get; set; }

        public string location { get; set; }

        public int template { get; set; }

        public string fileName { get; set; }

        public string? circuit { get; set; }
        
        public DateTime? dateCreated { get; set; }

        public string? chartLegendOption { get; set; }

        [Column(TypeName = "jsonb")]
        public ReportCover? cover { get; set; }

        //[Column(TypeName = "jsonb")]
        public string? rawCsvData { get; set; }

        //[Column(TypeName = "jsonb")]
        //public string? csvColumns { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? lastUpdate { get; set; }

        public bool updated { get; set; }

        public DateTimeOffset LastEdit { get; set; }

        public string LastEditUser { get; set; }

        public PowerAnalyzerReport()
        {
            this.id = 0;
            this.name = "";
            this.location = "";
            this.fileName = "";
            //this.csvColumns = "";
            this.rawCsvData = "";

            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";
        }

    }

    public class ReportCover
    {
        public string? title { get; set; }

        public string? client { get; set; }

        public int separation { get; set; }

        public String? reportAuthor { get; set; }

        public string? instrumentUsed { get; set; }

        public string? serialNumber { get; set; }

        public ReportCover() {
            this.title = "";
            this.client = "";
            this.instrumentUsed = "";
        }
    }

}
