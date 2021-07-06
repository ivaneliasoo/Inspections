using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Inspections.Core.Domain.CheckListAggregate
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CheckValue
    {
        NotAcceptableFalse,
        Acceptable,
        NotApplicable,
        None
    }
}
