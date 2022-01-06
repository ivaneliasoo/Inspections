using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Inspections.Infrastructure.Data.Conversions
{
    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }),
                v => JsonSerializer.Deserialize<T>(v, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonSerializer.Serialize(l, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }) == JsonSerializer.Serialize(r, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }),
                v => v == null ? 0 : JsonSerializer.Serialize(v, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }), new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull })
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }
    }
}
