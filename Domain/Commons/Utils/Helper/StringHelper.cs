namespace Domain.Commons.Utils.Helper;

public static class StringHelper
{
    public static string Normalize(string value)
    {
        // Returning normalized string
        return value.ToUpperInvariant();
    }
}