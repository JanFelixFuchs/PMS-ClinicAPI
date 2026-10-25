using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;
using FluentValidation.TestHelper;
using TestUtils.Models;
using Utils.Exceptions.Errors.Codes;

namespace Application.UnitTests.Tests.Common.Behaviours.Validation.Rules;

public class EnumValidationRulesTests
{
    /* - - - Preparation - - - */
    private class EnumValidationRulesTestModel
    {
        public TestEnum EnumValue { get; init; }
    }

    private class RequiredEnumValidator : AbstractValidator<EnumValidationRulesTestModel>
    {
        public RequiredEnumValidator() =>
            RuleFor(testModel => testModel.EnumValue).ValidRequiredEnum();
    }
    
    
    /* - - - Method: Update - - - */
    [Fact]
    public void ValidRequiredEnum_WithUndefinedEnumValue_HasValidationErrorWithInvalidEnumValueState()
    {
        // Arrange
        var sut = new RequiredEnumValidator();
        var model = new EnumValidationRulesTestModel { EnumValue = (TestEnum)999 };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.EnumValue)
            .WithCustomState(ErrorCode.INVALID_ENUM_VALUE);
    }
    
    [Fact]
    public void ValidRequiredEnum_WithDefinedEnumValue_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredEnumValidator();
        var model = new EnumValidationRulesTestModel { EnumValue = TestEnum.ValueOne };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.EnumValue);
    }
}