using System.Text.Json;
using System.Text.Json.Serialization;

namespace PMS_ClinicAPI.Common.Utils.Conversions;

public class StrictEnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Determining underlying type
        var underlyingType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
        
        // Returning if underlying type is an enum
        return underlyingType.IsEnum;
    }
    
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        // Determining underlying type
        var underlyingType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
        
        // Determining if underlying type is nullable
        var underlyingTypeIsNullable = Nullable.GetUnderlyingType(typeToConvert) != null;
        
        // Returning default enum converter for non-nullable enums
        if (!underlyingTypeIsNullable) 
            return new JsonStringEnumConverter().CreateConverter(typeToConvert, options);
        
        // Creating generic strict enum converter for underlying type
        var converterType = typeof(StrictEnumConverter<>).MakeGenericType(underlyingType);
        
        // Returning strict enum converter for nullable enums
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}