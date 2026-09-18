using Domain.Common.Enums;
namespace TestUtils.Constants;

public static class TestConstants
{
    // Default current date time
    public static readonly DateTime DefaultCurrentDateTime = new(2026, 1, 1, 12, 0, 0);
    
    
    // Valid values
    public const string ValidGermanZipCode = "12345";
    public const string ValidFinnishZipCode = "12345";
    
    public const string ValidGermanPhoneNumber = "+49123456789";
    public const string ValidFinnishPhoneNumber = "+358123456789";
}