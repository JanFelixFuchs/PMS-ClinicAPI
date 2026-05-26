using Domain.Commons.Enums;

namespace Domain.Commons.Utils.Constants;

public static class RegexPatterns
{
    /* - - - Regex patterns - - - */
    public const string Code = "^[A-Z0-9]{8,64}$";
    public const string Username = "^[a-zA-Z0-9_]{8,64}$";
    public const string Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@$!%*?&])[A-Za-z0-9@$!%*?&]{8,}$";
    
    public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private const string PhoneNumberDe = @"^\+49[0-9]{6,12}$";
    private const string PhoneNumberFi = @"^\+358[0-9]{6,10}$";

    private const string ZipCodeDe = "^[0-9]{5}$";
    private const string ZipCodeFi = "^[0-9]{5}$";
    
    public const string Color = "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";
    
    
    /* - - - Dictionaries for country-related regex patterns - - - */
    private static readonly Dictionary<Country, string> ZipCodePatterns = new()
    {
        { Country.De, ZipCodeDe },
        { Country.Fi, ZipCodeFi }
    };

    private static readonly Dictionary<Country, string> PhoneNumberPatterns = new()
    {
        { Country.De, PhoneNumberDe },
        { Country.Fi, PhoneNumberFi }
    };
    
    
    /* - - - Methods for country-related regex patterns - - - */
    public static string GetZipCodeRegexPattern(Country country) 
        => ZipCodePatterns[country];
    
    public static string GetPhoneNumberRegexPattern(Country country) 
        => PhoneNumberPatterns[country];
}