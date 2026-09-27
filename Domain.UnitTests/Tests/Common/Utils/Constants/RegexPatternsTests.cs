using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using FluentAssertions;

namespace Domain.Tests.Tests.Common.Utils.Constants;

public class RegexPatternsTests
{
    /* - - - Method: GetZipCodeRegexPattern - - - */
    [Fact]
    public void GetZipCodeRegexPattern_WithUndefinedCountry_ThrowsKeyNotFoundException()
    {
        // Act
        var act = () => RegexPatterns.GetZipCodeRegexPattern((Country)999);
        
        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }
    
    [Fact]
    public void GetZipCodeRegexPattern_HasEntryForEveryDefinedCountry()
    {
        foreach (var country in Enum.GetValues<Country>())
        {
            // Act
            var act = () => RegexPatterns.GetZipCodeRegexPattern(country);
            
            // Assert
            act.Should().NotThrow($"because {country} should have a configured zip code pattern");
        }
    }
    
    
    /* - - - Method: GetPhoneNumberRegexPattern - - - */
    [Fact]
    public void GetPhoneNumberRegexPattern_WithUndefinedCountry_ThrowsKeyNotFoundException()
    {
        // Act
        var act = () => RegexPatterns.GetPhoneNumberRegexPattern((Country)999);
        
        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }
    
    [Fact]
    public void GetPhoneNumberRegexPattern_HasEntryForEveryDefinedCountry()
    {
        foreach (var country in Enum.GetValues<Country>())
        {
            // Act
            var act = () => RegexPatterns.GetPhoneNumberRegexPattern(country);
            
            // Assert
            act.Should().NotThrow($"because {country} should have a configured phone number pattern");
        }
    }
}