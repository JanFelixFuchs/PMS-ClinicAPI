using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Common.Utils.PropertyValidation;

namespace Domain.Common.ValueObjects;

public class ContactInformation
{
    // Properties
    public string Email { get; } = string.Empty;
    public string PhoneNumber { get; } = string.Empty;

    // Constructor used by ef core and tests to initialize objects
    protected ContactInformation() { }

    // Standard constructor used to initialize objects
    public ContactInformation(
        string email,
        string phoneNumber,
        Country country)
    {
        // Validating properties
        ValidateCountry(country);
        ValidateEmail(email);
        ValidatePhoneNumber(phoneNumber, country);
        
        // Initializing properties
        Email = email;
        PhoneNumber = phoneNumber;
    }

    
    /* - - - Validation methods - - - */
    // Method to validate the email
    private static void ValidateEmail(string email)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNullEmptyOrWhitespace(email, nameof(Email)),
            () => PropertyValidationConditions.IsMatchingRegex(email, RegexPatterns.Email, nameof(Email)));
    }

    // Method to validate the phone number
    private static void ValidatePhoneNumber(string phoneNumber, Country country)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNullEmptyOrWhitespace(phoneNumber, nameof(PhoneNumber)),
            () => PropertyValidationConditions.IsMatchingRegex(phoneNumber,  RegexPatterns.GetPhoneNumberRegexPattern(country), nameof(PhoneNumber)));
    }

    // Method to validate the country
    private static void ValidateCountry(Country country)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsDefinedEnum(country, nameof(Country)));
    }

    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Email)}: {Email}, " +
               $"{nameof(PhoneNumber)}: {PhoneNumber}";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is ContactInformation other && 
               Email == other.Email &&
               PhoneNumber == other.PhoneNumber;
    }
    
    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return HashCode.Combine(Email, PhoneNumber);
    }
}