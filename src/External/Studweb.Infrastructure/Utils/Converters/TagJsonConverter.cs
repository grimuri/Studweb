using System.Text.Json;
using System.Text.Json.Serialization;
using Studweb.Domain.Aggregates.Note.ValueObjects;

namespace Studweb.Infrastructure.Utils.Converters;

public class TagJsonConverter : JsonConverter<Tag>
{
    public override Tag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Tag.Create(value ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, Tag value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}