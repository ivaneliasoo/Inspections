using System.Text.Json.Serialization;

namespace Inspections.Core.Domain.CheckListAggregate
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CheckValue
    {
        NotAcceptableFalse,
        Acceptable,
        NotApplicable,
        None
    }
}
