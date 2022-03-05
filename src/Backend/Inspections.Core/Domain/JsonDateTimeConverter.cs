using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inspections.Core.Domain;

public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.ParseExact(reader.GetString(),
            //"yyyy-MM-ddTHH:mm:ss.sssZ", CultureInfo.InvariantCulture);
            "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(
            //"yyyy-MM-ddTHH:mm:ss.sssZ", CultureInfo.InvariantCulture));
            "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture));
}
