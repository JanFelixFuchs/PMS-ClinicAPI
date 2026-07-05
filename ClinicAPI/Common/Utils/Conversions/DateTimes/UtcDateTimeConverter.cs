using System.Text.Json;
using System.Text.Json.Serialization;

namespace PMS_ClinicAPI.Common.Utils.Conversions.DateTimes;

public class UtcDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTime().EnsureUtc();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.EnsureUtc());
}