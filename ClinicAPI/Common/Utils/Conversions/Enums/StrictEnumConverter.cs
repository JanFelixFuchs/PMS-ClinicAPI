using System.Text.Json;
using System.Text.Json.Serialization;

namespace PMS_ClinicAPI.Common.Utils.Conversions.Enums;

public class StrictEnumConverter<T> : JsonConverter<T?> where T : struct, Enum
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Returnig null if token is null
        if (reader.TokenType == JsonTokenType.Null)
            return null;
        
        // Checking string-based enums
        if (reader.TokenType == JsonTokenType.String)
        {
            // Reading string
            var stringValue = reader.GetString();
            
            // Trying to parse string value
            if (Enum.TryParse<T>(stringValue, true, out var parsedResult))
                return parsedResult;

            // Returning null if parsing fails
            return null;
        }
        
        // Checking integer-based enums
        if (reader.TokenType == JsonTokenType.Number)
        {
            // Reading integer
            var integerValue = reader.GetInt32();
            
            // Trying to parse integer value
            if (Enum.IsDefined(typeof(T), integerValue))
                return (T)(object)integerValue;
            
            // Returning null if parsing fails
            return null;
        }

        // Returning null for all other token types
        return null;
    }
    
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        // Writing null or converted value
        if (value is null)
            writer.WriteNullValue();
        else 
            writer.WriteStringValue(value.ToString());
    }
}