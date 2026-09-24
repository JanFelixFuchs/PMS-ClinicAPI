using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.Tests.Utils.Builders.ValueObjectBuilders;
using FluentAssertions;
using TestUtils.Constants;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Common.ValueObjects;

public class ContactInformationTests
{
    /* - - - Preparation - - - */ 
    private const string TestEmailBase = "test@email";
    private const string TestEmailDomain = "com";
    private const string TestPhoneNumberBase = "0123456789";
    
    
    /* - - - Constructor - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceEmail_ThrowsValidationException(string? email)
    {
        // Act
        var act = () => TestContactInformationBuilder
            .Create()
            .WithEmail(email)
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(ContactInformation.Email),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmailsNotMatchingRegex), MemberType = typeof(TestConstants))]
    public void Constructor_WithEmailNotMatchingRegex_ThrowsValidationException(string email)
    {
        // Act
        var act = () => TestContactInformationBuilder
            .Create()
            .WithEmail(email)
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(ContactInformation.Email),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        }); 
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespacePhoneNumber_ThrowsValidationException(string? phoneNumber)
    {
        // Act
        var act = () => TestContactInformationBuilder
            .Create()
            .WithPhoneNumber(phoneNumber)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(ContactInformation.PhoneNumber),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidPhoneNumbersNotMatchingRegex), MemberType = typeof(TestConstants))]
    public void Constructor_WithPhoneNumberNotMatchingRegex_ThrowsValidationException(string phoneNumber, Country country)
    {
        // Act
        var act = () => TestContactInformationBuilder
            .Create()
            .WithPhoneNumber(phoneNumber)
            .WithCountry(country)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(ContactInformation.PhoneNumber),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        }); 
    }
    
    [Fact]
    public void Constructor_WithUndefinedCountry_ThrowsValidationException()
    {
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithCountry((Country)999)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Country),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        }); 
    }
    
    [Theory]
    [InlineData(TestConstants.ValidGermanPhoneNumber, Country.De)]
    [InlineData(TestConstants.ValidFinnishPhoneNumber, Country.Fi)]
    public void Constructor_WithValidArguments_SetsAllProperties(string phoneNumber, Country country)
    {
        // Arrange
        var contactInformationBuilder = TestContactInformationBuilder
            .Create()
            .WithPhoneNumber(phoneNumber)
            .WithCountry(country);
        
        // Act
        var sut = contactInformationBuilder.Build();
        
        // Assert
        sut.Email.Should().Be(contactInformationBuilder.Email);
        sut.PhoneNumber.Should().Be(phoneNumber);
    }
    
    
    /* - - - Method: ToString - - - */
    [Fact]
    public void ToString_ReturnsStringContainingAllProperties()
    {
        // Arrange
        var sut = TestContactInformationBuilder.Create().Build();
        
        // Act
        var result = sut.ToString();
    
        // Assert
        result.Should()
            .Contain(sut.Email).And
            .Contain(sut.PhoneNumber);
    }
    
    
    /* - - - Method: Equals and GetHashCode - - - */
    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var sut = TestContactInformationBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(null);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        // Arrange
        var sut = TestContactInformationBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(new object());
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentEmail_ReturnsFalse()
    {
        // Arrange
        var contactInformationOne = TestContactInformationBuilder
            .Create()
            .WithEmail($"{TestEmailBase}.{TestEmailDomain}")
            .Build();
        var contactInformationTwo = TestContactInformationBuilder
            .Create()
            .WithEmail($"{new string(TestEmailBase.Reverse().ToArray())}.{TestEmailDomain}")
            .Build();
        
        // Act
        var result = contactInformationOne.Equals(contactInformationTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData("+49", Country.De)]
    [InlineData("+358", Country.Fi)]
    public void Equals_WithDifferentPhoneNumber_ReturnsFalse(string phoneNumberPrefix, Country country)
    {
        // Arrange
        var contactInformationOne = TestContactInformationBuilder
            .Create()
            .WithPhoneNumber($"{phoneNumberPrefix}{TestPhoneNumberBase}")
            .WithCountry(country)
            .Build();
        var contactInformationTwo = TestContactInformationBuilder
            .Create()
            .WithPhoneNumber($"{phoneNumberPrefix}{new string(TestPhoneNumberBase.Reverse().ToArray())}")
            .WithCountry(country)
            .Build();
        
        // Act
        var result = contactInformationOne.Equals(contactInformationTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithIdenticalValues_ReturnsTrue()
    {
        // Arrange
        var contactInformationOne = TestContactInformationBuilder.Create().Build();
        var contactInformationTwo = TestContactInformationBuilder.Create().Build();
    
        // Act
        var equalsResult = contactInformationOne.Equals(contactInformationTwo);
        var getHashCodeContactInformationOneResult = contactInformationOne.GetHashCode();
        var getHashCodeContactInformationTwoResult = contactInformationTwo.GetHashCode();
        
        // Assert
        equalsResult.Should().BeTrue();
        getHashCodeContactInformationOneResult.Should().Be(getHashCodeContactInformationTwoResult);
    }
}