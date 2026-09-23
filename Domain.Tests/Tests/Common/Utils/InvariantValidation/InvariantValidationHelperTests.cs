using Domain.Common.Utils.InvariantValidation;
using FluentAssertions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Common.Utils.InvariantValidation;

public class InvariantValidationHelperTests
{
    /* - - - Preparation - - - */ 
    private const string ValidationField = "test-field";
    private const string ValidationMessage = "test-validation-message";
    
    private static readonly InvariantValidationResult ValidInvariantValidationResultOne = new(
            true,
            $"{ValidationField}-one",
            ErrorCode.UNKNOWN_ERROR,
            $"{ValidationMessage}-one");
    private static readonly InvariantValidationResult ValidInvariantValidationResultTwo = new(
            true,
            $"{ValidationField}-two",
            ErrorCode.UNKNOWN_ERROR,
            $"{ValidationMessage}-two");
    
    private static readonly InvariantValidationResult InvalidInvariantValidationResultOne = new(
            false,
            $"{ValidationField}-one",
            ErrorCode.UNKNOWN_ERROR,
            $"{ValidationMessage}-one");
    private static readonly InvariantValidationResult InvalidInvariantValidationResultTwo = new(
            false,
            $"{ValidationField}-two",
            ErrorCode.UNKNOWN_ERROR,
            $"{ValidationMessage}-two");
    
    
    /* - - - Method: ConstructInvariantValidation - - - */
    [Fact]
    public void ConstructInvariantValidation_WithSingleInvalidResult_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => InvariantValidationHelper.ConstructInvariantValidation(() => InvalidInvariantValidationResultOne);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        
        exception.FieldErrors.Should().ContainSingle(fieldError => 
            fieldError.Field == InvalidInvariantValidationResultOne.Field &&
            fieldError.ErrorCode == InvalidInvariantValidationResultOne.ErrorCode);
        
        exception.Message.Should().Be(InvalidInvariantValidationResultOne.ValidationMessage);
    }
    
    [Fact]
    public void ConstructInvariantValidation_WithMultipleInvalidResults_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => InvariantValidationHelper.ConstructInvariantValidation(
            () => InvalidInvariantValidationResultOne,
            () => InvalidInvariantValidationResultTwo);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        
        exception.FieldErrors.Should().ContainSingle(fieldError => 
            fieldError.Field == InvalidInvariantValidationResultOne.Field &&
            fieldError.ErrorCode == InvalidInvariantValidationResultOne.ErrorCode);
        exception.FieldErrors.Should().NotContain(fieldError => 
            fieldError.Field == InvalidInvariantValidationResultTwo.Field &&
            fieldError.ErrorCode == InvalidInvariantValidationResultTwo.ErrorCode);
        
        exception.Message.Should().Be(InvalidInvariantValidationResultOne.ValidationMessage);
        exception.Message.Should().NotBe(InvalidInvariantValidationResultTwo.ValidationMessage);
    }
    
    [Fact]
    public void ConstructInvariantValidation_WithMixOfValidAndInvalidResults_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => InvariantValidationHelper.ConstructInvariantValidation(
            () => ValidInvariantValidationResultOne,
            () => InvalidInvariantValidationResultOne);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == InvalidInvariantValidationResultOne.Field &&
            fieldError.ErrorCode == InvalidInvariantValidationResultOne.ErrorCode);
        
        exception.Message.Should().Be(InvalidInvariantValidationResultOne.ValidationMessage);
    }
    
    [Fact]
    public void ConstructInvariantValidation_WithNoResults_DoesNotThrow()
    {
        // Act
        var act = () => InvariantValidationHelper.ConstructInvariantValidation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ConstructInvariantValidation_WithAllValidResults_DoesNotThrow()
    {
        // Act
        var act = () => InvariantValidationHelper.ConstructInvariantValidation( 
            () => ValidInvariantValidationResultOne, 
            () => ValidInvariantValidationResultTwo);
        
        // Assert
        act.Should().NotThrow();
    }
}