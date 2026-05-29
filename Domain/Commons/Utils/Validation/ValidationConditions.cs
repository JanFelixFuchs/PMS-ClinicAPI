using System.Text.RegularExpressions;
using Domain.Commons.Interfaces;

namespace Domain.Commons.Utils.Validation;

public static class ValidationConditions
{
    // Generic conditions
    public static (bool, string) IsNotNull<T>(T input, string propertyName, string? customErrorMessage = null) =>
        (input != null, customErrorMessage ?? $"{propertyName} must not be null");
    
    public static (bool, string) IsNotDeleted<T>(T input, string propertyName, string? customErrorMessage = null) where T : IDeletable =>
        (!input.IsDeleted, customErrorMessage ?? $"{propertyName} must not be deleted");
    
    public static (bool, string) IsNullOrNotDeleted<T>(T? input, string propertyName, string? customErrorMessage = null) where T : IDeletable =>
        (input == null || !input.IsDeleted, customErrorMessage ?? $"{propertyName} must not be deleted");
    
    public static (bool, string) IsNotArchived<T>(T input, string propertyName, string? customErrorMessage = null) where T: IArchivable =>
        (!input.IsArchived, customErrorMessage ?? $"{propertyName} must not be archived");
    
    public static (bool, string) IsNullOrNotArchived<T>(T? input, string propertyName, string? customErrorMessage = null) where T : IArchivable =>
        (input == null || !input.IsArchived, customErrorMessage ?? $"{propertyName} must not be archived");
    
    
    // String conditions
    public static (bool, string) IsNotNullEmptyOrWhitespace(string stringInput, string propertyName, string? customErrorMessage = null) =>
        (!string.IsNullOrWhiteSpace(stringInput), customErrorMessage ?? $"{propertyName} must not be null, empty or whitespace");

    public static (bool, string) IsNullNotEmptyOrWhitespace(string? stringInput, string propertyName, string? customErrorMessage = null) =>
        (stringInput == null || !string.IsNullOrWhiteSpace(stringInput), customErrorMessage ?? $"{propertyName} must not be empty or whitespace");
    
    public static (bool, string) HasMaximumLength(string stringInput, int maxLength, string propertyName, string? customErrorMessage = null) =>
        (stringInput.Length <= maxLength, customErrorMessage ?? $"{propertyName} must have {maxLength} or fewer characters");
    
    public static (bool, string) IsNullOrHasMaximumLength(string? stringInput, int maxLength, string propertyName, string? customErrorMessage = null) =>
        (stringInput == null || stringInput.Length <= maxLength, customErrorMessage ?? $"{propertyName} must have {maxLength} or fewer characters");
    
    public static (bool, string) IsMatchingRegex(string stringInput, string regexPattern, string propertyName, string? customErrorMessage = null) =>
        (new Regex(regexPattern).IsMatch(stringInput), customErrorMessage ?? $"{propertyName} must match the required pattern");
    
    
    // Enum conditions
    public static (bool, string) IsDefinedEnum<T>(T enumInput, string propertyName, string? customErrorMessage = null) where T : struct, Enum =>
        (Enum.IsDefined(enumInput), customErrorMessage ?? $"{propertyName} must be a valid option");
    
    public static (bool, string) IsExactEnumValue<T>(T enumInput, T expectedValue, string propertyName, string? customErrorMessage = null) where T : struct, Enum =>
        (EqualityComparer<T>.Default.Equals(enumInput, expectedValue), customErrorMessage ?? $"{propertyName} must be {expectedValue}");

    
    // Date and time conditions
    public static (bool, string) IsDateTimeInTheFuture(DateTime dateTimeInput, string propertyName, string? customErrorMessage = null) =>
        (dateTimeInput >= DateTime.UtcNow, customErrorMessage ?? $"{propertyName} must be in the future");
    
    public static (bool, string) IsNullOrDateTimeInTheFuture(DateTime? dateTimeInput, string propertyName, string? customErrorMessage = null) =>
        (dateTimeInput == null || dateTimeInput.Value >= DateTime.UtcNow, customErrorMessage ?? $"{propertyName} must be in the future");
    
    public static (bool, string) IsDateInThePast(DateTime dateTimeInput, string propertyName, string? customErrorMessage = null) =>
        (dateTimeInput.Date <= DateTime.UtcNow.Date, customErrorMessage ?? $"{propertyName} must be in the past");
    
    public static (bool, string) IsNullOrDateInThePast(DateTime? dateTimeInput, string propertyName, string? customErrorMessage = null) =>
        (dateTimeInput == null || dateTimeInput.Value.Date <= DateTime.UtcNow.Date, customErrorMessage ?? $"{propertyName} must be in the past");
    
    public static (bool, string) AreIdenticalDates(DateTime firstDateTimeInput, DateTime secondDateTimeInput, string firstPropertyName, string secondPropertyName, string? customErrorMessage = null) =>
        (firstDateTimeInput.Date == secondDateTimeInput.Date, customErrorMessage ?? $"{firstPropertyName} and {secondPropertyName} must be equal dates");
    
    public static (bool, string) AreDateTimesInOrder(DateTime firstDateTimeInput, DateTime secondDateTimeInput, string firstPropertyName, string secondPropertyName, string? customErrorMessage = null) => 
        (firstDateTimeInput < secondDateTimeInput, customErrorMessage ?? $"{firstPropertyName} must be earlier than {secondPropertyName}");
    
    
    // Collection conditions
    public static (bool, string) IsNotEmpty<T>(ICollection<T> collectionInput, string propertyName, string? customErrorMessage = null) =>
        (collectionInput.Count > 0, customErrorMessage ?? $"{propertyName} must not be empty");
    
    public static (bool, string) HasMaximumLength<T>(ICollection<T> collectionInput, int maxLength, string propertyName, string? customErrorMessage = null) =>
        (collectionInput.Count <= maxLength, customErrorMessage ?? $"{propertyName} must have {maxLength} or less values");
    
    public static (bool, string) IsNotContainingDuplicates<T>(ICollection<T> collectionInput, string propertyName, string? customErrorMessage = null) =>
        (collectionInput.Count == collectionInput.Distinct().Count(), customErrorMessage ?? $"{propertyName} must not contain duplicate values");
    
    public static (bool, string) IsNotContainingDeletedElements<T>(ICollection<T> collectionInput, string propertyName, string? customErrorMessage = null) where T : IDeletable =>
        (collectionInput.All(element => !element.IsDeleted), customErrorMessage ?? $"{propertyName} must not contain deleted elements");
    
    public static (bool, string) IsNotContainingArchivedElements<T>(ICollection<T> collectionInput, string propertyName, string? customErrorMessage = null) where T : IArchivable =>
        (collectionInput.All(element => !element.IsArchived), customErrorMessage ?? $"{propertyName} must not contain archived elements");

    public static (bool, string) IsContainingElementsWithExactEnumValue<TElement, TEnum>(ICollection<TElement> collectionInput, Func<TElement, TEnum> enumSelector, TEnum expectedValue, string propertyName, string? customErrorMessage = null) where TEnum : struct, Enum =>
        (collectionInput.All(element => EqualityComparer<TEnum>.Default.Equals(enumSelector(element), expectedValue)), customErrorMessage ?? $"{propertyName} must be {expectedValue}");
}