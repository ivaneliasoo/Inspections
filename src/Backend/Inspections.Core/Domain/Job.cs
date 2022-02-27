using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable enable
namespace Inspections.Core.Domain;

[Table("Job")]
public class Job
{
    [Key]
    public int id { get; set; }

    public string? status { get; set; }

    public string? value { get; set; }

    public string? salesPerson { get; set; }

    public int priority { get; set; }

    public string? scope { get; set; }

    public string? tag { get; set; }

    public string? comments { get; set; }

    public string? teams { get; set; }

    public string? teamCount { get; set; }

    public string? duration { get; set; }

    public string? shift { get; set; }

    [NotMapped]
    public bool updated { get; set; }

    [JsonConverter(typeof(JsonDateTimeConverter))]
    public DateTime? lastUpdate { get; set; }

    public DateTimeOffset LastEdit { get; set; }

    public string LastEditUser { get; set; }

    public Job(int id, string status, int priority, string scope, string tag)
    {
        this.id = id;
        this.status = status;
        this.priority = priority;
        this.scope = scope;
        this.tag = tag;
        this.comments = "";
        this.duration = "";
        this.teams = "";
        this.shift = "";
        this.value = "";
        this.teamCount = "";
        this.salesPerson = "";
        this.LastEdit = new DateTimeOffset();
        this.LastEditUser = "";
    }

    public Job()
    {
        this.id = 0;
        this.status = "";
        this.priority = 0;
        this.scope = "";
        this.tag = "";
        this.comments = "";
        this.duration = "";
        this.teams = "";
        this.teamCount = "";
        this.shift = "";
        this.value = "";
        this.teamCount = "";
        this.salesPerson = "";
        this.LastEdit = new DateTimeOffset();
        this.LastEditUser = "";
    }

}
