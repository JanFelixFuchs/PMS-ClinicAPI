using Domain.Common.Utils.PropertyValidation;
using FluentAssertions;
using TestUtils.Constants;
using TestUtils.Models;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Common.Utils.PropertyValidation;

public class PropertyValidationConditionsTests
{
    /* - - - Preparation - - - */
    private const string PropertyName = "test-property";
    private const string ComparingPropertyNameOne = "test-comparing-property-one";
    private const string ComparingPropertyNameTwo = "test-comparing-property-two";

    private const string NonEmptyStringValue = "test-non-empty-string-value";
    
    private const string RegexPattern = "^[a-z-]+$";
    private const string MatchingStringValue = NonEmptyStringValue;
    private const string NonMatchingStringValue = $"{NonEmptyStringValue}-!";
    
    private readonly DateTime _dateTime = new(2026, 1, 1);
    
    private readonly List<object> _emptyCollection = [];
    private readonly List<object> _nonEmptyCollection = [new(), new(), new()];
    private readonly List<string> _nonEmptyCollectionWithNullElements = [null!, null!];
    private readonly List<string> _nonEmptyCollectionWithDuplicates = [NonEmptyStringValue, NonEmptyStringValue];
    
    /* - - - Method: IsNotNull - - - */
    [Fact]
    public void IsNotNull_WithNull_ReturnsInvalidPropertyValidationResultWithMissingValueErrorCode()
    {
        // Act 
        var result = PropertyValidationConditions.IsNotNull<object>(null!, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.MISSING_VALUE);
    }

    [Fact]
    public void IsNotNull_WithNonNullValue_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNotNull(new object(), PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNotNullEmptyOrWhitespace - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void IsNotNullEmptyOrWhitespace_WithNullEmptyOrWhitespaceValue_ReturnsInvalidPropertyValidationResultWithMissingValueErrorCode(string? value)
    {
        // Act 
        var result = PropertyValidationConditions.IsNotNullEmptyOrWhitespace(value!, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.MISSING_VALUE);
    }

    [Fact]
    public void IsNotNullEmptyOrWhitespace_WithNonEmptyValue_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNotNullEmptyOrWhitespace(NonEmptyStringValue, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNullNotEmptyOrWhitespace - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void IsNullNotEmptyOrWhitespace_WithEmptyOrWhitespaceValue_ReturnsInvalidPropertyValidationResultWithEmptyValueErrorCode(string value)
    {
        // Act 
        var result = PropertyValidationConditions.IsNullNotEmptyOrWhitespace(value, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.EMPTY_VALUE);
    }

    [Fact]
    public void IsNullNotEmptyOrWhitespace_WithNull_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullNotEmptyOrWhitespace(null, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullNotEmptyOrWhitespace_WithNonEmptyValue_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullNotEmptyOrWhitespace(NonEmptyStringValue, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - HasMaximumLength - - - */
    [Fact]
    public void HasMaximumLength_WithValueExceedingMaximumLength_ReturnsInvalidPropertyValidationResultWithMaxLengthExceededErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.HasMaximumLength(NonEmptyStringValue, NonEmptyStringValue.Length - 1, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.MAX_LENGTH_EXCEEDED);
    }
    
    [Fact]
    public void HasMaximumLength_WithValueAtMaximumLength_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.HasMaximumLength(NonEmptyStringValue, NonEmptyStringValue.Length, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void HasMaximumLength_WithValueBelowMaximumLength_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.HasMaximumLength(NonEmptyStringValue, NonEmptyStringValue.Length + 1, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsNullOrHasMaximumLength (string) - - - */
    [Fact]
    public void IsNullOrHasMaximumLength_WithValueExceedingMaximumLength_ReturnsInvalidPropertyValidationResultWithMaxLengthExceededErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrHasMaximumLength(NonEmptyStringValue, NonEmptyStringValue.Length - 1, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.MAX_LENGTH_EXCEEDED);
    }
    
    [Fact]
    public void IsNullOrHasMaximumLength_WithNull_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrHasMaximumLength(null, NonEmptyStringValue.Length, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrHasMaximumLength_WithValueAtMaximumLength_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrHasMaximumLength(NonEmptyStringValue, NonEmptyStringValue.Length, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrHasMaximumLength_WithValueBelowMaximumLength_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrHasMaximumLength(NonEmptyStringValue, NonEmptyStringValue.Length + 1, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsMatchingRegex - - - */
    [Fact]
    public void IsMatchingRegex_WithNonMatchingValue_ReturnsInvalidPropertyValidationResultWithPatternMismatchErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsMatchingRegex(NonMatchingStringValue, RegexPattern, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.PATTERN_MISMATCH);
    }
    
    [Fact]
    public void IsMatchingRegex_WithMatchingValue_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsMatchingRegex(MatchingStringValue, RegexPattern, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsDefinedEnum - - - */
    [Fact]
    public void IsDefinedEnum_WithUndefinedEnumValue_ReturnsInvalidPropertyValidationResultWithInvalidEnumValueErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsDefinedEnum((TestEnum)999, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.INVALID_ENUM_VALUE);
    }

    [Fact]
    public void IsDefinedEnum_WithDefinedEnumValue_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsDefinedEnum(TestEnum.ValueOne, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsDateTimeInTheFuture - - - */
    [Fact]
    public void IsDateTimeInTheFuture_WithDateTimeBeforeCurrentDateTime_ReturnsInvalidPropertyValidationResultWithDateTimeNotInFutureErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsDateTimeInTheFuture(_dateTime.AddDays(-1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.DATETIME_NOT_IN_FUTURE);
    }
    
    [Fact]
    public void IsDateTimeInTheFuture_WithDateTimeEqualToCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsDateTimeInTheFuture(_dateTime, _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsDateTimeInTheFuture_WithDateTimeAfterCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsDateTimeInTheFuture(_dateTime.AddDays(1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsNullOrDateTimeInTheFuture - - - */
    [Fact]
    public void IsNullOrDateTimeInTheFuture_WithDateTimeBeforeCurrentDateTime_ReturnsInvalidPropertyValidationResultWithDateTimeNotInFutureErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInTheFuture(_dateTime.AddDays(-1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.DATETIME_NOT_IN_FUTURE);
    }
    
    [Fact]
    public void IsNullOrDateTimeInTheFuture_WithNull_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInTheFuture(null, _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrDateTimeInTheFuture_WithDateTimeEqualToCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInTheFuture(_dateTime, _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrDateTimeInTheFuture_WithDateTimeAfterCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInTheFuture(_dateTime.AddDays(1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsDateTimeInThePast - - - */
    [Fact]
    public void IsDateTimeInThePast_WithDateTimeAfterCurrentDateTime_ReturnsInvalidPropertyValidationResultWithDateTimeNotInPastErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsDateTimeInThePast(_dateTime.AddDays(1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.DATETIME_NOT_IN_PAST);
    }
    
    [Fact]
    public void IsDateTimeInThePast_WithDateTimeEqualToCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsDateTimeInThePast(_dateTime, _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsDateTimeInThePast_WithDateTimeBeforeCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsDateTimeInThePast(_dateTime.AddDays(-1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsNullOrDateTimeInThePast - - - */
    [Fact]
    public void IsNullOrDateTimeInThePast_WithDateTimeAfterCurrentDateTime_ReturnsInvalidPropertyValidationResultWithDateTimeNotInPastErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInThePast(_dateTime.AddDays(1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.DATETIME_NOT_IN_PAST);
    }
    
    [Fact]
    public void IsNullOrDateTimeInThePast_WithNull_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInThePast(null, _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrDateTimeInThePast_WithDateTimeEqualToCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInThePast(_dateTime, _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrDateTimeInThePast_WithDateTimeBeforeCurrentDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNullOrDateTimeInThePast(_dateTime.AddDays(-1), _dateTime, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - AreIdenticalDates - - - */
    [Fact]
    public void AreIdenticalDates_WithDifferentDates_ReturnsInvalidPropertyValidationResultWithDateTimeOutOfRangeErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.AreIdenticalDates(_dateTime, _dateTime.AddDays(1), ComparingPropertyNameOne, ComparingPropertyNameTwo);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(ComparingPropertyNameOne);
        result.ErrorCode.Should().Be(ErrorCode.DATETIME_OUT_OF_RANGE);
    }

    [Fact]
    public void AreIdenticalDates_WithSameDateAndDifferentTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.AreIdenticalDates(_dateTime, _dateTime.AddHours(12), ComparingPropertyNameOne, ComparingPropertyNameTwo);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void AreIdenticalDates_WithSameDateAndSameTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.AreIdenticalDates(_dateTime, _dateTime, ComparingPropertyNameOne, ComparingPropertyNameTwo);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - AreDateTimesInOrder - - - */
    [Fact]
    public void AreDateTimesInOrder_WithFirstDateTimeAfterSecondDateTime_ReturnsInvalidPropertyValidationResultWithDateTimeOutOfRangeErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.AreDateTimesInOrder(_dateTime.AddHours(12), _dateTime, ComparingPropertyNameOne, ComparingPropertyNameTwo);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(ComparingPropertyNameOne);
        result.ErrorCode.Should().Be(ErrorCode.DATETIME_OUT_OF_RANGE);
    }

    [Fact]
    public void AreDateTimesInOrder_WithFirstDateTimeEqualToSecondDateTime_ReturnsInvalidPropertyValidationResultWithDateTimeOutOfRangeErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.AreDateTimesInOrder(_dateTime, _dateTime, ComparingPropertyNameOne, ComparingPropertyNameTwo);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(ComparingPropertyNameOne);
        result.ErrorCode.Should().Be(ErrorCode.DATETIME_OUT_OF_RANGE);
    }
    
    [Fact]
    public void AreDateTimesInOrder_WithFirstDateTimeBeforeSecondDateTime_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.AreDateTimesInOrder(_dateTime, _dateTime.AddHours(12), ComparingPropertyNameOne, ComparingPropertyNameTwo);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNotEmpty - - - */
    [Fact]
    public void IsNotEmpty_WithEmptyCollection_ReturnsInvalidPropertyValidationResultWithMissingValueErrorCode()
    {
        // Act 
        var result = PropertyValidationConditions.IsNotEmpty(_emptyCollection, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.MISSING_VALUE);
    }

    [Fact]
    public void IsNotEmpty_WithPopulatedCollection_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNotEmpty(_nonEmptyCollection, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - HasMaximumLength - - - */
    [Fact]
    public void HasMaximumLength_WithCollectionExceedingMaximumLength_ReturnsInvalidPropertyValidationResultWithMaxLengthExceededErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.HasMaximumLength(_nonEmptyCollection, _nonEmptyCollection.Count - 1, PropertyName);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.MAX_LENGTH_EXCEEDED);
    }
    
    [Fact]
    public void HasMaximumLength_WithEmptyCollection_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.HasMaximumLength(_emptyCollection, _emptyCollection.Count + 1, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void HasMaximumLength_WithCollectionAtMaximumLength_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.HasMaximumLength(_nonEmptyCollection, _nonEmptyCollection.Count, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void HasMaximumLength_WithCollectionBelowMaximumLength_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.HasMaximumLength(_nonEmptyCollection, _nonEmptyCollection.Count + 1, PropertyName);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - IsNotContainingNullElements - - - */
    [Fact]
    public void IsNotContainingNullElements_WithCollectionContainingNullElements_ReturnsInvalidPropertyValidationResultWithMissingValueErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsNotContainingNullElements(_nonEmptyCollectionWithNullElements, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.MISSING_VALUE);
    }

    [Fact]
    public void IsNotContainingNullElements_WithEmptyCollection_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNotContainingNullElements(_emptyCollection, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNotContainingNullElements_WithCollectionContainingNonNullElements_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNotContainingNullElements(_nonEmptyCollection, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();   
    }
    
    
    /* - - - IsNotContainingDuplicates - - - */
    [Fact]
    public void IsNotContainingDuplicates_WithCollectionContainingDuplicateElements_ReturnsInvalidPropertyValidationResultWithContainsDuplicateElementsErrorCode()
    {
        // Act
        var result = PropertyValidationConditions.IsNotContainingDuplicates(_nonEmptyCollectionWithDuplicates, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.CONTAINS_DUPLICATE_ELEMENTS);
    }

    [Fact]
    public void IsNotContainingDuplicates_WithEmptyCollection_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNotContainingDuplicates(_emptyCollection, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNotContainingDuplicates_WithCollectionContainingUniqueElements_ReturnsValidPropertyValidationResult()
    {
        // Act
        var result = PropertyValidationConditions.IsNotContainingDuplicates(_nonEmptyCollection, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();   
    }
}