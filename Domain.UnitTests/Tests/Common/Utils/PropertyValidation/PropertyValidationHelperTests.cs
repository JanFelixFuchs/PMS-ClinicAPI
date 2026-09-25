using Domain.Common.Utils.PropertyValidation;
using FluentAssertions;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Common.Utils.PropertyValidation;

public class PropertyValidationHelperTests
{   
    /* - - - Preparation - - - */ 
    private const string ValidationField = "test-field";
    private const string ValidationMessage = "test-validation-message";

    private static readonly PropertyValidationResult ValidPropertyValidationResultOne = new(
        true,
        $"{ValidationField}-one",
        ErrorCode.MISSING_VALUE,
        $"{ValidationMessage}-one");
    private static readonly PropertyValidationResult ValidPropertyValidationResultTwo = new(
        true,
        $"{ValidationField}-two",
        ErrorCode.MISSING_VALUE,
        $"{ValidationMessage}-two");
    
    private static readonly PropertyValidationResult InvalidPropertyValidationResultOne = new(
        false,
        $"{ValidationField}-one",
        ErrorCode.MISSING_VALUE,
        $"{ValidationMessage}-one");
    private static readonly PropertyValidationResult InvalidPropertyValidationResultTwo = new(
        false,
        $"{ValidationField}-two",
        ErrorCode.MISSING_VALUE,
        $"{ValidationMessage}-two");


    /* - - - Method: ConstructPropertyValidation - - - */
    [Fact]
    public void ConstructPropertyValidation_WithSingleInvalidResult_ThrowsValidationException()
    {
        // Act
        var act = () => PropertyValidationHelper.ConstructPropertyValidation(() => InvalidPropertyValidationResultOne);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == InvalidPropertyValidationResultOne.Field &&
            fieldError.ErrorCode == InvalidPropertyValidationResultOne.ErrorCode);
        
        exception.Message.Should().Be(InvalidPropertyValidationResultOne.ValidationMessage);
    }
    
    [Fact]
    public void ConstructPropertyValidation_WithMultipleInvalidResults_ThrowsValidationException()
    {
        // Act
        var act = () => PropertyValidationHelper.ConstructPropertyValidation(
            () => InvalidPropertyValidationResultOne,
            () => InvalidPropertyValidationResultTwo);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == InvalidPropertyValidationResultOne.Field &&
            fieldError.ErrorCode == InvalidPropertyValidationResultOne.ErrorCode);
        exception.FieldErrors.Should().NotContain(fieldError =>
            fieldError.Field == InvalidPropertyValidationResultTwo.Field &&
            fieldError.ErrorCode == InvalidPropertyValidationResultTwo.ErrorCode);
        
        exception.Message.Should().Be(InvalidPropertyValidationResultOne.ValidationMessage);
        exception.Message.Should().NotBe(InvalidPropertyValidationResultTwo.ValidationMessage);
    }
    
    [Fact]
    public void ConstructPropertyValidation_WithMixOfValidAndInvalidResults_ThrowsValidationException()
    {
        // Act
        var act = () => PropertyValidationHelper.ConstructPropertyValidation(
            () => ValidPropertyValidationResultOne,
            () => InvalidPropertyValidationResultOne);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == InvalidPropertyValidationResultOne.Field &&
            fieldError.ErrorCode == InvalidPropertyValidationResultOne.ErrorCode);
        
        exception.Message.Should().Be(InvalidPropertyValidationResultOne.ValidationMessage);
    }
    
    [Fact]
    public void ConstructPropertyValidation_WithNoResults_DoesNotThrow()
    {
        // Act
        var act = () => PropertyValidationHelper.ConstructPropertyValidation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ConstructPropertyValidation_WithAllValidResults_DoesNotThrow()
    {
        // Act
        var act = () => PropertyValidationHelper.ConstructPropertyValidation( 
            () => ValidPropertyValidationResultOne, 
            () => ValidPropertyValidationResultTwo);
        
        // Assert
        act.Should().NotThrow();
    }
}