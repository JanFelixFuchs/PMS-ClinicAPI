using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using TestUtils.Constants;

namespace TestUtils.Builders.ValueObjectBuilders;

public class TestAddressBuilder
{
    // Properties
    public string? Street { get; private set; }
    public string? HouseNumber { get; private set; }
    public string? City { get; private set; }
    private string? ZipCode { get; set; }
    private Country Country { get; set; }
    
    // Constructor
    private TestAddressBuilder()
    {
        // Setting default values
        Street = TestConstants.ValidInitialStreet;
        HouseNumber = TestConstants.ValidInitialHouseNumber;
        City = TestConstants.ValidInitialCity;
        ZipCode = TestConstants.ValidInitialZipCode;
        Country = TestConstants.ValidInitialCountry;
    }
    
    
    /* - - -  Factory methods - - - */
    public static TestAddressBuilder Create()
    {
        return new TestAddressBuilder();
    }

    public Address Build()
    {
        return new Address(
            Street!,
            HouseNumber!,
            City!,
            ZipCode!,
            Country);
    }
    
    
    /* - - - Override methods - - - */
    public TestAddressBuilder WithStreet(string? street)
    {
        Street = street;
        return this;
    }

    public TestAddressBuilder WithHouseNumber(string? houseNumber)
    {
        HouseNumber = houseNumber;
        return this;
    }

    public TestAddressBuilder WithCity(string? city)
    {
        City = city;
        return this;
    }

    public TestAddressBuilder WithZipCode(string? zipCode)
    {
        ZipCode = zipCode;
        return this;
    }

    public TestAddressBuilder WithCountry(Country country)
    {
        Country = country;
        return this;
    }
}