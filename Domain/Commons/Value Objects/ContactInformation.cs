using Domain.Commons.Enums;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Validation;

namespace Domain.Commons.Value_Objects;

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
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(email, nameof(Email)),
            ValidationConditions.IsMatchingRegex(email, RegexPatterns.Email, nameof(Email)));
    }

    // Method to validate the phone number
    private static void ValidatePhoneNumber(string phoneNumber, Country country)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(phoneNumber, nameof(PhoneNumber)),
            ValidationConditions.IsMatchingRegex(phoneNumber,  RegexPatterns.GetPhoneNumberRegexPattern(country), nameof(PhoneNumber)));
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