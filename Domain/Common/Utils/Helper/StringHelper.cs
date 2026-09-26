namespace Domain.Common.Utils.Helper;

public static class StringHelper
{
    public static string Normalize(string value)
    {
        // Validating arguments
        ArgumentNullException.ThrowIfNull(value);
        
        // Returning normalized string
        return value.ToUpperInvariant();
    }
}