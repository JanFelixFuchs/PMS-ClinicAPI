using Domain.Common.Utils.Policies;
using FluentAssertions;
using TestUtils.Constants;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Common.Utils.Policies;

public class PasswordValidationPolicyTests
{
    /* - - - Method: Validate - - - */
    [Fact]
    public void Constructor_WithNullPassword_ThrowsValidationException()
    {
        // Act
        var act = () => PasswordValidationPolicy.Validate(null!);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = PasswordValidationPolicy.PasswordPropertyName,
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidPasswordsNotMatchingRegex), MemberType = typeof(TestConstants))]
    public void Validate_WithInvalidPassword_ThrowsValidationException(string password)
    {
        // Act
        var act = () => PasswordValidationPolicy.Validate(password);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = PasswordValidationPolicy.PasswordPropertyName,
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        }); 
    }
    
    [Fact]
    public void Validate_WithValidPassword_DoesNotThrow()
    {
        // Act
        var act = () => PasswordValidationPolicy.Validate("TestPassword1!");
        
        // Assert
        act.Should().NotThrow();
    }
}