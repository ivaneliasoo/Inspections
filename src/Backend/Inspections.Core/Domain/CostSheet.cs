using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// Store Cost sheet data using JSON POCO mapping
// https://www.npgsql.org/efcore/mapping/json.html?tabs=data-annotations%2Cpoco
#nullable enable
namespace Inspections.Core.Domain
{
    [Table("CostSheet")]
    public class CostSheet
    {
        [Key]
        public long id { get; set; }

        public string project { get; set; }

        public string location { get; set; }

        public DateTime? dateCreated { get; set; }

        public double materialMarkup { get; set; }

        public double labourDailyRate { get; set; }

        public double labourNightMultiplier { get; set; }

        public double finalMarkup { get; set; }

        [Column(TypeName = "jsonb")]
        public Section[]? sections { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? lastUpdate { get; set; }

        public bool updated { get; set; }

        public DateTimeOffset LastEdit { get; set; }

        public string LastEditUser { get; set; }

        public CostSheet()
        {
            this.id = 0;
            this.project = "";
            this.location = "";

            this.LastEdit = new DateTimeOffset();
            this.LastEditUser = "";
        }

    }

    public class Section
    {
        public string secNumber { get; set; }

        public string subSection { get; set; }

        public string description { get; set; }

        public Item[]? items { get; set; }

        public double materialMarkup { get; set; }

        public double finalMarkup { get; set; }

        public Section() {
            this.secNumber = "";
            this.subSection = "";
            this.description = "";
        }

    }

    public class Item
    {
        public string itemNumber { get; set; }

        public string description { get; set; }

        public double materialMarkup { get; set; }

        public double noCables { get; set; }

        public double unitCost { get; set; }
        
        public double units { get; set; }
        
        public double labourCostUnit { get; set; }

        public Item() {
            this.itemNumber = "";
            this.description = "";
        }
    }
}
