using System.Text.RegularExpressions;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Common.Utils.Validation;

public static class PropertyValidationConditions
{
    // Generic conditions
    public static PropertyValidationResult IsNotNull<T>(T input, string propertyName) => new(
        input != null,
        propertyName,
        ErrorCode.MISSING_VALUE,
        $"{propertyName} must not be null");


    // String conditions
    public static PropertyValidationResult IsNotNullEmptyOrWhitespace(string stringInput, string propertyName) => new(
        !string.IsNullOrWhiteSpace(stringInput),
        propertyName,
        ErrorCode.MISSING_VALUE,
        $"{propertyName} must not be null, empty or whitespace");

    public static PropertyValidationResult IsNullNotEmptyOrWhitespace(string? stringInput, string propertyName) => new(
        stringInput == null || !string.IsNullOrWhiteSpace(stringInput),
        propertyName,
        ErrorCode.EMPTY_VALUE,
        $"{propertyName} must not be empty or whitespace");

    public static PropertyValidationResult HasMaximumLength(string stringInput, int maxLength, string propertyName) => new(
        stringInput.Length <= maxLength,
        propertyName,
        ErrorCode.MAX_LENGTH_EXCEEDED,
        $"{propertyName} must have {maxLength} or fewer characters");

    public static PropertyValidationResult IsNullOrHasMaximumLength(string? stringInput, int maxLength, string propertyName) => new(
        stringInput == null || stringInput.Length <= maxLength,
        propertyName,
        ErrorCode.MAX_LENGTH_EXCEEDED,
        $"{propertyName} must have {maxLength} or fewer characters");

    public static PropertyValidationResult IsMatchingRegex(string stringInput, string regexPattern, string propertyName) => new(
        new Regex(regexPattern).IsMatch(stringInput),
        propertyName,
        ErrorCode.PATTERN_MISMATCH,
        $"{propertyName} must match the required pattern");


    // Enum conditions
    public static PropertyValidationResult IsDefinedEnum<T>(T enumInput, string propertyName) where T : struct, Enum => new(
        Enum.IsDefined(enumInput),
        propertyName,
        ErrorCode.INVALID_ENUM_VALUE,
        $"{propertyName} must be a valid option");


    // Date and time conditions
    public static PropertyValidationResult IsDateTimeInTheFuture(DateTime dateTimeInput, DateTime currentDateTime, string propertyName) => new(
        dateTimeInput >= currentDateTime,
        propertyName,
        ErrorCode.DATETIME_NOT_IN_FUTURE,
        $"{propertyName} must be in the future");

    public static PropertyValidationResult IsNullOrDateTimeInTheFuture(DateTime? dateTimeInput, DateTime currentDateTime, string propertyName) => new(
        dateTimeInput == null || dateTimeInput.Value >= currentDateTime,
        propertyName,
        ErrorCode.DATETIME_NOT_IN_FUTURE,
        $"{propertyName} must be in the future");

    public static PropertyValidationResult IsDateTimeInThePast(DateTime dateTimeInput, DateTime currentDateTime, string propertyName) => new(
        dateTimeInput <= currentDateTime,
        propertyName,
        ErrorCode.DATETIME_NOT_IN_PAST,
        $"{propertyName} must be in the past");

    public static PropertyValidationResult IsNullOrDateTimeInThePast(DateTime? dateTimeInput, DateTime currentDateTime, string propertyName) => new(
        dateTimeInput == null || dateTimeInput.Value <= currentDateTime,
        propertyName,
        ErrorCode.DATETIME_NOT_IN_PAST,
        $"{propertyName} must be in the past");

    public static PropertyValidationResult AreIdenticalDates(DateTime firstDateTimeInput, DateTime secondDateTimeInput, string firstPropertyName, string secondPropertyName) => new(
        firstDateTimeInput.Date == secondDateTimeInput.Date,
        firstPropertyName,
        ErrorCode.DATETIME_OUT_OF_RANGE,
        $"{firstPropertyName} and {secondPropertyName} must be equal dates");

    public static PropertyValidationResult AreDateTimesInOrder(DateTime firstDateTimeInput, DateTime secondDateTimeInput, string firstPropertyName, string secondPropertyName) => new(
        firstDateTimeInput < secondDateTimeInput,
        firstPropertyName,
        ErrorCode.DATETIME_OUT_OF_RANGE,
        $"{firstPropertyName} must be earlier than {secondPropertyName}");


    // Collection conditions
    public static PropertyValidationResult IsNotEmpty<T>(ICollection<T> collectionInput, string propertyName) => new(
        collectionInput.Count > 0,
        propertyName,
        ErrorCode.MISSING_VALUE,
        $"{propertyName} must not be empty");

    public static PropertyValidationResult HasMaximumLength<T>(ICollection<T> collectionInput, int maxLength, string propertyName) => new(
        collectionInput.Count <= maxLength,
        propertyName,
        ErrorCode.MAX_LENGTH_EXCEEDED,
        $"{propertyName} must have {maxLength} or less values");

    public static PropertyValidationResult IsNotContainingDuplicates<T>(ICollection<T> collectionInput, string propertyName) => new(
        collectionInput.Count == collectionInput.Distinct().Count(),
        propertyName,
        ErrorCode.CONTAINS_DUPLICATE_ELEMENTS,
        $"{propertyName} must not contain duplicate values");
}