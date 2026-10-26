using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;
using FluentValidation.TestHelper;
using TestUtils.Constants;
using Utils.Exceptions.Errors.Codes;

namespace Application.UnitTests.Tests.Common.Behaviours.Validation.Rules;

public class StringValidationRulesTests
{
    /* - - - Preparation - - - */
    private const string NonEmptyStringValue = "test-non-empty-string-value";
    private const string TestRegexPattern = "^[a-z-]+$";
    private const string TestRegexPatternMatchingStringValue = NonEmptyStringValue;
    private const string TestRegexPatternNonMatchingStringValue = $"{NonEmptyStringValue}-!";
    
    private class StringValidationRulesTestModel
    {
        public string StringValue { get; init; } = string.Empty;
        public string? NullableStringValue { get; init; }
        public string RegexPattern { get; init; } = string.Empty;
    }

    private class RequiredStringValidator : AbstractValidator<StringValidationRulesTestModel>
    {
        public RequiredStringValidator() => 
            RuleFor(testModel => testModel.StringValue).ValidRequiredString();
    }
    
    private class OptionalStringValidator : AbstractValidator<StringValidationRulesTestModel>
    {
        public OptionalStringValidator() => 
            RuleFor(testModel => testModel.NullableStringValue).ValidOptionalString();
    }
    
    private class RequiredMaximumStringLengthValidator : AbstractValidator<StringValidationRulesTestModel>
    {
        public RequiredMaximumStringLengthValidator(int maxLength) =>
            RuleFor(testModel => testModel.StringValue).ValidRequiredMaximumStringLength(maxLength);
    }
    
    private class OptionalMaximumStringLengthValidator : AbstractValidator<StringValidationRulesTestModel>
    {
        public OptionalMaximumStringLengthValidator(int maxLength) =>
            RuleFor(testModel => testModel.NullableStringValue).ValidOptionalMaximumStringLength(maxLength);
    }

    private class RequiredRegexValidator : AbstractValidator<StringValidationRulesTestModel>
    {
        public RequiredRegexValidator(string regex) => 
            RuleFor(testModel => testModel.StringValue).ValidRequiredRegex(regex);
    }
    
    private class RequiredRegexWithSelectorValidator : AbstractValidator<StringValidationRulesTestModel>
    {
        public RequiredRegexWithSelectorValidator() =>
            RuleFor(model => model.StringValue).ValidRequiredRegex(model => model.RegexPattern);
    }
    
    
    /* - - - Method: ValidRequiredString - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void ValidRequiredString_WithNullEmptyOrWhitespaceString_HasValidationErrorWithMissingValueState(string? value)
    {
        // Arrange
        var sut = new RequiredStringValidator();
        var model = new StringValidationRulesTestModel { StringValue = value! };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.StringValue)
            .WithCustomState(ErrorCode.MISSING_VALUE);
    }

    [Fact]
    public void ValidRequiredString_WithNonEmptyString_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredStringValidator();
        var model = new StringValidationRulesTestModel { StringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.StringValue);
    }
    
    
    /* - - - Method: ValidOptionalString - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void ValidOptionalString_WithEmptyOrWhitespaceString_HasValidationErrorWithEmptyValueState(string value)
    {
        // Arrange
        var sut = new OptionalStringValidator();
        var model = new StringValidationRulesTestModel { NullableStringValue = value };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.NullableStringValue)
            .WithCustomState(ErrorCode.EMPTY_VALUE);
    }
    
    [Fact]
    public void ValidOptionalString_WithNull_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalStringValidator();
        var model = new StringValidationRulesTestModel { NullableStringValue = null };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableStringValue);
    }

    [Fact]
    public void ValidOptionalString_WithNonEmptyString_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalStringValidator();
        var model = new StringValidationRulesTestModel { NullableStringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableStringValue);
    }
    
    
    /* - - - Method: ValidRequiredMaximumStringLength - - - */
    [Fact]
    public void ValidRequiredMaximumStringLength_WithStringAboveMaxLength_HasValidationErrorWithMaxLengthExceededState()
    {
        // Arrange
        var sut = new RequiredMaximumStringLengthValidator(maxLength: NonEmptyStringValue.Length - 1);
        var model = new StringValidationRulesTestModel { StringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.StringValue)
            .WithCustomState(ErrorCode.MAX_LENGTH_EXCEEDED);
    }
    
    [Fact]
    public void ValidRequiredMaximumStringLength_WithStringAtMaxLength_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredMaximumStringLengthValidator(maxLength: NonEmptyStringValue.Length);
        var model = new StringValidationRulesTestModel { StringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.StringValue);
    }

    [Fact]
    public void ValidRequiredMaximumStringLength_WithStringBelowMaxLength_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredMaximumStringLengthValidator(maxLength: NonEmptyStringValue.Length + 1);
        var model = new StringValidationRulesTestModel { StringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.StringValue);
    }
    
    
    /* - - - Method: ValidOptionalMaximumStringLength - - - */
    [Fact]
    public void ValidOptionalMaximumStringLength_WithStringAboveMaxLength_HasValidationErrorWithMaxLengthExceededState()
    {
        // Arrange
        var sut = new OptionalMaximumStringLengthValidator(maxLength: NonEmptyStringValue.Length - 1);
        var model = new StringValidationRulesTestModel { NullableStringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.NullableStringValue)
            .WithCustomState(ErrorCode.MAX_LENGTH_EXCEEDED);
    }
    
    [Fact]
    public void ValidOptionalMaximumStringLength_WithNull_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalMaximumStringLengthValidator(maxLength: NonEmptyStringValue.Length);
        var model = new StringValidationRulesTestModel { NullableStringValue = null };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableStringValue);
    }

    [Fact]
    public void ValidOptionalMaximumStringLength_WithStringAtMaxLength_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalMaximumStringLengthValidator(maxLength: NonEmptyStringValue.Length);
        var model = new StringValidationRulesTestModel { NullableStringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableStringValue);
    }
    
    [Fact]
    public void ValidOptionalMaximumStringLength_WithStringBelowMaxLength_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalMaximumStringLengthValidator(maxLength: NonEmptyStringValue.Length + 1);
        var model = new StringValidationRulesTestModel { StringValue = NonEmptyStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableStringValue);
    }
    
    
    /* - - - Method: ValidRequiredRegex - - - */
    [Fact]
    public void ValidRequiredRegex_WithNonMatchingString_HasValidationErrorWithPatternMismatchState()
    {
        // Arrange
        var sut = new RequiredRegexValidator(regex: TestRegexPattern);
        var model = new StringValidationRulesTestModel { StringValue = TestRegexPatternNonMatchingStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.StringValue)
            .WithCustomState(ErrorCode.PATTERN_MISMATCH);
    }
    
    [Fact]
    public void ValidRequiredRegex_WithMatchingString_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredRegexValidator(regex: TestRegexPattern);
        var model = new StringValidationRulesTestModel { StringValue = TestRegexPatternMatchingStringValue };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.StringValue);
    }
    
    
    /* - - - Method: ValidRequiredRegexWithSelector - - - */
    [Fact]
    public void ValidRequiredRegexWithSelector_WithNonMatchingString_HasValidationErrorWithPatternMismatchState()
    {
        // Arrange
        var sut = new RequiredRegexWithSelectorValidator();
        var model = new StringValidationRulesTestModel
        {
            StringValue = TestRegexPatternNonMatchingStringValue, 
            RegexPattern = TestRegexPattern
        };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.StringValue)
            .WithCustomState(ErrorCode.PATTERN_MISMATCH);
    }
    
    [Fact]
    public void ValidRequiredRegexWithSelector_WithMatchingString_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredRegexWithSelectorValidator();
        var model = new StringValidationRulesTestModel
        {
            StringValue = TestRegexPatternMatchingStringValue, 
            RegexPattern = TestRegexPattern
        };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.StringValue);
    }
}