using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Common.Utils.Validation;

namespace Domain.Common.Value_Objects;

public class Address
{
    // Properties
    public string Street { get; } = string.Empty;
    public string HouseNumber { get; } = string.Empty;
    public string City { get; } = string.Empty;
    public string ZipCode { get; } = string.Empty;
    public Country Country { get; } 
    
    // Constructor used by ef core and tests to initialize objects
    protected Address() { }
    
    // Standard constructor used to initialize objects
    public Address(
        string street,
        string houseNumber,
        string city,
        string zipCode,
        Country country)
    {
        // Validating properties
        ValidateCountry(country);
        ValidateStreet(street);
        ValidateHouseNumber(houseNumber);
        ValidateCity(city);
        ValidateZipCode(zipCode, country);
        
        // Initializing properties
        Street = street;
        HouseNumber = houseNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
    }
    
    
    /* - - - Validation methods - - - */
    // Method to validate the street
    private static void ValidateStreet(string street)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(street, nameof(Street)),
            PropertyValidationConditions.HasMaximumLength(street, Lengths.Street, nameof(Street)));
    }
    
    // Method to validate the house number
    private static void ValidateHouseNumber(string houseNumber)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(houseNumber, nameof(HouseNumber)),
            PropertyValidationConditions.HasMaximumLength(houseNumber, Lengths.HouseNumber, nameof(HouseNumber)));
    }
    
    // Method to validate the city
    private static void ValidateCity(string city)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(city, nameof(City)),
            PropertyValidationConditions.HasMaximumLength(city, Lengths.City, nameof(City)));
    }
    
    // Method to validate the zip code
    private static void ValidateZipCode(string zipCode, Country country)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(zipCode, nameof(ZipCode)),
            PropertyValidationConditions.IsMatchingRegex(zipCode, RegexPatterns.GetZipCodeRegexPattern(country), nameof(ZipCode)));
    }

    // Method to validate the country
    private static void ValidateCountry(Country country)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsDefinedEnum(country, nameof(Country)));
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Street)}: {Street}, " +
               $"{nameof(HouseNumber)}: {HouseNumber}, " +
               $"{nameof(City)}: {City}, " +
               $"{nameof(ZipCode)}: {ZipCode}, " +
               $"{nameof(Country)}: {Country}";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Address other && 
               Street == other.Street &&
               HouseNumber == other.HouseNumber &&
               City == other.City &&
               ZipCode == other.ZipCode &&
               Country == other.Country;
    }
    
    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return HashCode.Combine(Street, HouseNumber, City, ZipCode, Country);
    }
}