using FluentAssertions;
using Infrastructure.Common.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;

namespace Infrastructure.UnitTests.Tests.Common.Services;

public class AuthenticationServiceTests
{
    /* - - - Preparation - - - */
    private const string ValidRawPasswordOne = "Password1!";
    private const string ValidRawPasswordTwo = "Password2!";
    private const string InvalidRawPassword = "";
    
    private readonly AuthenticationService _sut = new();


    /* - - - Method: CheckPassword - - - */
    [Fact]
    public void CheckPassword_WithNullPasswordHash_ThrowsArgumentNullException()
    {
        // Act
        var act = () => _sut.CheckPassword(null!, ValidRawPasswordOne);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void CheckPassword_WithNullRawPassword_ThrowsArgumentNullException()
    {
        // Arrange
        var passwordHash = _sut.ValidateAndHashPassword(ValidRawPasswordOne);
        
        // Act
        var act = () => _sut.CheckPassword(passwordHash, null!);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void CheckPassword_WithNonMatchingPasswords_ReturnsFalse()
    {
        // Arrange
        var passwordHash = _sut.ValidateAndHashPassword(ValidRawPasswordOne);
        
        // Act
        var result = _sut.CheckPassword(passwordHash, ValidRawPasswordTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void CheckPassword_WithMatchingPasswords_ReturnsTrue()
    {
        // Arrange
        var passwordHash = _sut.ValidateAndHashPassword(ValidRawPasswordOne);
        
        // Act
        var result = _sut.CheckPassword(passwordHash, ValidRawPasswordOne);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void CheckPassword_WithV2Hash_ReturnsTrue()
    {
        // Arrange
        var passwordHasher = new PasswordHasher<object>(
            Options.Create(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
            }));
        var passwordHash = passwordHasher.HashPassword(new object(),  ValidRawPasswordOne);
        
        // Act
        var result = _sut.CheckPassword(passwordHash, ValidRawPasswordOne);
        
        // Assert
        result.Should().BeTrue();
    }

    /* - - - Method: ValidateAndHashPassword - - - */
    [Theory]
    [InlineData(null, ErrorCode.MISSING_VALUE)]
    [InlineData(InvalidRawPassword, ErrorCode.PATTERN_MISMATCH)]
    public void ValidateAndHashPassword_WithInvalidRawPassword_ThrowsValidationException(string? rawPassword, ErrorCode errorCode)
    {
        // Act
        var act = () => _sut.ValidateAndHashPassword(rawPassword!);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = "Password",
            ErrorCode = errorCode
        });
    }

    [Fact]
    public void ValidateAndHashPassword_WithValidRawPassword_ReturnsPasswordHash()
    {
        // Act
        var result = _sut.ValidateAndHashPassword(ValidRawPasswordOne);
        
        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        _sut.CheckPassword(result, ValidRawPasswordOne).Should().BeTrue();
    }


    /* - - - Method: HashToken - - - */
    [Fact]
    public void HashToken_WithNullToken_ThrowsArgumentNullException()
    {
        // Act
        var act = () => _sut.HashToken(null!);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HashToken_WithValidToken_ReturnsTokenHash()
    {
        // Arrange
        const string validToken = "test-token-one";
        const string expectedTokenHash = "5brimu8/fAKRjaiSw+HUqprpdpUy77HAW2tijMCqWew=";
        
        // Act
        var result = _sut.HashToken(validToken);
        
        // Assert
        result.Should().Be(expectedTokenHash);
    }
}