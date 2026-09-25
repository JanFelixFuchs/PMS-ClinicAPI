using Domain.Common.Enums;
using Xunit;

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

    public static TheoryData<string, string, Country> ValidCountryRelatedInformation =>
    [
        (ValidGermanZipCode, ValidGermanPhoneNumber, Country.De),
        (ValidFinnishZipCode, ValidFinnishPhoneNumber, Country.Fi)
    ];
    
    public static TheoryData<AppendixContentType, byte[]> ValidAppendixContentTypes => new()
    {
        { AppendixContentType.Pdf, [0x25, 0x50, 0x44, 0x46] },
        { AppendixContentType.Jpeg, [0xFF, 0xD8, 0xFF] },
        { AppendixContentType.Png, [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A] }
    };
    

    // Invalid values
    public static TheoryData<string?> InvalidNullEmptyOrWhitespaceString => 
    [
        null!,
        "",
        "   ",
    ];
    
    public static TheoryData<string> InvalidEmptyOrWhitespaceString => 
    [
        "",
        "   ",
    ];
    
    public static TheoryData<string, Country> InvalidZipCodesNotMatchingRegex =>
    [
        ("1234", Country.De),
        ("123456", Country.De),
        ("1234A", Country.De),
        ("1234", Country.Fi),
        ("123456", Country.Fi),
        ("1234A", Country.Fi),
    ];
    
    public static TheoryData<string> InvalidEmailsNotMatchingRegex =>
    [
        "@test-missing-local-part.test",
        "test-missing-at-sign.test",
        "test@test-missing-domain",
    ];
    
    public static TheoryData<string, Country> InvalidPhoneNumbersNotMatchingRegex =>
    [
        ("0123456789", Country.De),
        ("+4901234", Country.De),
        ("+4901234567891011", Country.De),
        ("+49 01234 56789", Country.De),
        ("+49-01234-56789", Country.De),
        ("490123456789", Country.Fi),
        ("0123456789", Country.Fi),
        ("+35801234", Country.Fi),
        ("+358012345678910", Country.Fi),
        ("+358 01234 56789", Country.Fi),
        ("+358-01234-56789", Country.Fi),
        ("3580123456789", Country.Fi),
    ];
}