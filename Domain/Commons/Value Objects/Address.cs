using Domain.Commons.Enums;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Validation;

namespace Domain.Commons.Value_Objects;

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
        Country = country;
    }
    
    
    /* - - - Validation methods - - - */
    private static void ValidateStreet(string street)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(street, nameof(Street)),
            ValidationConditions.HasMaximumLength(street, Lengths.Street, nameof(Street)));
    }
    
    private static void ValidateHouseNumber(string houseNumber)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(houseNumber, nameof(HouseNumber)),
            ValidationConditions.HasMaximumLength(houseNumber, Lengths.HouseNumber, nameof(HouseNumber)));
    }
    
    private static void ValidateCity(string city)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(city, nameof(City)),
            ValidationConditions.HasMaximumLength(city, Lengths.City, nameof(City)));
    }
    
    private static void ValidateZipCode(string zipCode, Country country)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(zipCode, nameof(ZipCode)),
            ValidationConditions.IsMatchingRegex(zipCode, RegexPatterns.GetZipCodeRegexPattern(country), nameof(ZipCode)));
    }

    private static void ValidateCountry(Country country)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsDefinedEnum(country, nameof(Country)));
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