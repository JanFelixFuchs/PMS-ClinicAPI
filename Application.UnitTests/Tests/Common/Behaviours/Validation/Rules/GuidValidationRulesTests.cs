using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;
using FluentValidation.TestHelper;
using Utils.Exceptions.Errors.Codes;

namespace Application.UnitTests.Tests.Common.Behaviours.Validation.Rules;

public class GuidValidationRulesTests
{
    /* - - - Preparation - - - */
    private class GuidValidationRulesTestModel
    {
        public Guid GuidValue { get; init; }
        public Guid? NullableGuidValue { get; init; }
    }
    
    private class RequiredGuidValidator : AbstractValidator<GuidValidationRulesTestModel>
    {
        public RequiredGuidValidator() => RuleFor(testModel => testModel.GuidValue).ValidRequiredGuid();
    }
    
    private class OptionalGuidValidator : AbstractValidator<GuidValidationRulesTestModel>
    {
        public OptionalGuidValidator() => RuleFor(testModel => testModel.NullableGuidValue).ValidOptionalGuid();
    }
    
    
    /* - - - Method: ValidRequiredGuid - - - */
    [Fact]
    public void ValidRequiredGuid_WithEmptyGuid_HasValidationErrorWithMissingValueState()
    {
        // Arrange
        var sut = new RequiredGuidValidator();
        var model = new GuidValidationRulesTestModel { GuidValue = Guid.Empty };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.GuidValue)
            .WithCustomState(ErrorCode.MISSING_VALUE);
    }
    
    [Fact]
    public void ValidRequiredGuid_WithNonEmptyGuid_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredGuidValidator();
        var model = new GuidValidationRulesTestModel { GuidValue = Guid.NewGuid() };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.GuidValue);
    }
    
    
    /* - - - Method: ValidOptionalGuid - - - */
    [Fact]
    public void ValidOptionalGuid_WithEmptyGuid_HasValidationErrorWithEmptyValueState()
    {
        // Arrange
        var sut = new OptionalGuidValidator();
        var model = new GuidValidationRulesTestModel { NullableGuidValue = Guid.Empty };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.NullableGuidValue)
            .WithCustomState(ErrorCode.EMPTY_VALUE);
    }
    
    [Fact]
    public void ValidOptionalGuid_WithNull_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalGuidValidator();
        var model = new GuidValidationRulesTestModel { NullableGuidValue = null };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableGuidValue);
    }

    [Fact]
    public void ValidOptionalGuid_WithNonEmptyGuid_HasNoValidationError()
    {
        // Arrange
        var sut = new OptionalGuidValidator();
        var model = new GuidValidationRulesTestModel { NullableGuidValue = Guid.NewGuid() };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.NullableGuidValue);
    }
}