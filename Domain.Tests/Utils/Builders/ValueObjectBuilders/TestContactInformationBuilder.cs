using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using TestUtils.Constants;

namespace Domain.Tests.Utils.Builders.ValueObjectBuilders;

public class TestContactInformationBuilder
{
    // Properties
    public string? Email { get; private set; }
    private string? PhoneNumber { get; set; }
    private Country Country { get; set; }
    
    // Constructor
    private TestContactInformationBuilder()
    {
        // Setting default values
        Email = "test@email.com";
        PhoneNumber = TestConstants.ValidGermanPhoneNumber;
        Country = Country.De;
    }
    
    
    /* - - - Factory methods - - - */
    public static TestContactInformationBuilder Create()
    {
        return new TestContactInformationBuilder();
    }

    public ContactInformation Build()
    {
        return new ContactInformation(
            Email!,
            PhoneNumber!,
            Country);
    }
    
    
    /* - - - Override methods - - - */
    public TestContactInformationBuilder WithEmail(string? email)
    {
        Email = email;
        return this;
    }

    public TestContactInformationBuilder WithPhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber;
        return this;
    }

    public TestContactInformationBuilder WithCountry(Country country)
    {
        Country = country;
        return this;
    }
}