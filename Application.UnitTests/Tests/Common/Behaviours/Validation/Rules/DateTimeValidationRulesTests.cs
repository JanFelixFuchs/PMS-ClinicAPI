using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;
using FluentValidation.TestHelper;
using TestUtils.Constants;
using Utils.Exceptions.Errors.Codes;

namespace Application.UnitTests.Tests.Common.Behaviours.Validation.Rules;

public class DateTimeValidationRulesTests
{
    /* - - - Preparation - - - */
    private class DateTimeValidationRulesTestModel
    {
        public DateTime DateTimeValue { get; init; }
        public DateTime MaxDateTimeValue { get; init; }
        public DateTime? NullableDateTimeValue { get; init; }
        public DateTime MinDateTimeValue { get; init; }
    }

    private class RequiredDateTimeValidator : AbstractValidator<DateTimeValidationRulesTestModel>
    {
        public RequiredDateTimeValidator() => 
            RuleFor(testModel => testModel.DateTimeValue).ValidRequiredDateTime();
    }

    private class RequiredPastDateTimeValidator : AbstractValidator<DateTimeValidationRulesTestModel>
    {
        public RequiredPastDateTimeValidator(DateTime currentDateTime) => 
            RuleFor(testModel => testModel.DateTimeValue).ValidRequiredPastDateTime(currentDateTime);
    }
    
    private class RequiredBeforeDateTimeValidator : AbstractValidator<DateTimeValidationRulesTestModel>
    {
        public RequiredBeforeDateTimeValidator() => 
            RuleFor(testModel => testModel.DateTimeValue).ValidRequiredBeforeDateTime(testModel => testModel.MaxDateTimeValue);
    }
    
    private class RequiredFutureDateTimeValidator : AbstractValidator<DateTimeValidationRulesTestModel>
    {
        public RequiredFutureDateTimeValidator(DateTime currentDateTime) => 
            RuleFor(testModel => testModel.DateTimeValue).ValidRequiredFutureDateTime(currentDateTime);
    }
    
    private class RequiredAfterDateTimeValidator : AbstractValidator<DateTimeValidationRulesTestModel>
    {
        public RequiredAfterDateTimeValidator() =>
            RuleFor(testModel => testModel.DateTimeValue).ValidRequiredAfterDateTime(testModel => testModel.MinDateTimeValue);
    }
    
    private class OptionalDateTimeValidator : AbstractValidator<DateTimeValidationRulesTestModel>
    {
        public OptionalDateTimeValidator() => 
            RuleFor(testModel => testModel.NullableDateTimeValue).ValidOptionalDateTime();
    }
    
    private class OptionalPastDateTimeValidator : AbstractValidator<DateTimeValidationRulesTestModel>
    {
        public OptionalPastDateTimeValidator(DateTime currentDateTime) => 
            RuleFor(testModel => testModel.NullableDateTimeValue).ValidOptionalPastDateTime(currentDateTime);
    }
    
    
    /* - - - Method: ValidRequiredDateTime - - - */
    [Fact]
    public void ValidRequiredDateTime_WithDefaultDateTime_HasValidationErrorWithMissingValueState()
    {
        // Arrange
        var sut = new RequiredDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = default };
        
        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.DateTimeValue)
            .WithCustomState(ErrorCode.MISSING_VALUE);
    }
    
    [Fact]
    public void ValidRequiredDateTime_WithNonDefaultDateTime_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = TestConstants.DefaultCurrentDateTime };
        
        // Act
        var result = sut.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }
    
    
    /* - - - Method: ValidRequiredPastDateTime - - - */
    [Fact]
    public void ValidRequiredPastDateTime_WithFutureDateTime_HasValidationErrorWithNotInPastState()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredPastDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = currentDateTime.AddDays(1) };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.DateTimeValue)
            .WithCustomState(ErrorCode.DATETIME_NOT_IN_PAST);
    }
    
    [Fact]
    public void ValidRequiredPastDateTime_WithPastDateTime_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredPastDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = currentDateTime.AddDays(-1) };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }

    [Fact]
    public void ValidRequiredPastDateTime_WithDateTimeEqualToCurrent_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredPastDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = currentDateTime };
        
        // Act
        var result = sut.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }
    
    
    /* - - - Method: ValidRequiredBeforeDateTime - - - */
    [Fact]
    public void ValidRequiredBeforeDateTime_WithDateTimeAfterMax_HasValidationErrorWithOutOfRangeState()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredBeforeDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel
        {
            DateTimeValue = currentDateTime.AddDays(1), 
            MaxDateTimeValue = currentDateTime
        };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.DateTimeValue)
            .WithCustomState(ErrorCode.DATETIME_OUT_OF_RANGE);
    }
    
    [Fact]
    public void ValidRequiredBeforeDateTime_WithDateTimeBeforeMax_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredBeforeDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel
        {
            DateTimeValue = currentDateTime.AddDays(-1),
            MaxDateTimeValue = currentDateTime
        };

        // Act
        var result = sut.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }

    [Fact]
    public void ValidRequiredBeforeDateTime_WithDateTimeEqualToMax_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredBeforeDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel
        {
            DateTimeValue = currentDateTime,
            MaxDateTimeValue = currentDateTime
        };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }


    /* - - - Method: ValidRequiredFutureDateTime - - - */
    [Fact]
    public void ValidRequiredFutureDateTime_WithPastDateTime_HasValidationErrorWithNotInFutureState()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredFutureDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = currentDateTime.AddDays(-1) };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.DateTimeValue)
            .WithCustomState(ErrorCode.DATETIME_NOT_IN_FUTURE);
    }

    [Fact]
    public void ValidRequiredFutureDateTime_WithFutureDateTime_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredFutureDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = currentDateTime.AddDays(1) };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }

    [Fact]
    public void ValidRequiredFutureDateTime_WithDateTimeEqualToCurrent_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredFutureDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { DateTimeValue = currentDateTime };
        
        // Act
        var result = sut.TestValidate(model);
        
        // Arrange
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }
    
    
    /* - - - Method: ValidRequiredAfterDateTime - - - */
    [Fact]
    public void ValidRequiredAfterDateTime_WithDateTimeBeforeMin_HasValidationErrorWithOutOfRangeState()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredAfterDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel
        {
            DateTimeValue = currentDateTime.AddDays(-1), 
            MinDateTimeValue = currentDateTime
        };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.DateTimeValue)
            .WithCustomState(ErrorCode.DATETIME_OUT_OF_RANGE);
    }
    
    [Fact]
    public void ValidRequiredAfterDateTime_WithDateTimeAfterMin_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredAfterDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel
        {
            DateTimeValue = currentDateTime.AddDays(1), 
            MinDateTimeValue = currentDateTime
        };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }

    [Fact]
    public void ValidRequiredAfterDateTime_WithDateTimeEqualToMin_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new RequiredAfterDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel
        {
            DateTimeValue = currentDateTime, 
            MinDateTimeValue = currentDateTime
        };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.DateTimeValue);
    }
    
    
    /* - - - Method: ValidOptionalDateTime - - - */
    [Fact]
    public void ValidOptionalDateTime_WithDefaultValue_HasValidationErrorWithEmptyValueState()
    {
        // Arrange
        var sut = new OptionalDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel { NullableDateTimeValue = default(DateTime) };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.NullableDateTimeValue)
            .WithCustomState(ErrorCode.EMPTY_VALUE);
    }
    
    [Fact]
    public void ValidOptionalDateTime_WithNull_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel { NullableDateTimeValue = null };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableDateTimeValue);
    }

    [Fact]
    public void ValidOptionalDateTime_WithNonDefaultValue_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalDateTimeValidator();
        var model = new DateTimeValidationRulesTestModel { NullableDateTimeValue = TestConstants.DefaultCurrentDateTime };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableDateTimeValue);
    }
    
    
    /* - - - Method: ValidOptionalPastDateTime - - - */
    [Fact]
    public void ValidOptionalPastDateTime_WithFutureValue_HasValidationErrorWithNotInPastState()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new OptionalPastDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { NullableDateTimeValue = currentDateTime.AddDays(1) };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.NullableDateTimeValue)
            .WithCustomState(ErrorCode.DATETIME_NOT_IN_PAST);
    }
    
    [Fact]
    public void ValidOptionalPastDateTime_WithNull_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalPastDateTimeValidator(TestConstants.DefaultCurrentDateTime);
        var model = new DateTimeValidationRulesTestModel { NullableDateTimeValue = null };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableDateTimeValue);
    }

    [Fact]
    public void ValidOptionalPastDateTime_WithPastValue_HasNoValidationError()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = new OptionalPastDateTimeValidator(currentDateTime);
        var model = new DateTimeValidationRulesTestModel { NullableDateTimeValue = currentDateTime.AddDays(-1) };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableDateTimeValue);
    }
}