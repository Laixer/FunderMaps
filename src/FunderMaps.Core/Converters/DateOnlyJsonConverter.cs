using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FunderMaps.Core.Converters;

/// <summary>
///     JSON converter for <see cref="DateOnly"/> that accepts both
///     "yyyy-MM-dd" and full ISO 8601 datetime strings.
/// </summary>
public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? throw new JsonException("Expected a date string.");

        if (DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnly))
        {
            return dateOnly;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateTime))
        {
            return DateOnly.FromDateTime(dateTime);
        }

        throw new JsonException($"Unable to parse '{value}' as a date.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}
