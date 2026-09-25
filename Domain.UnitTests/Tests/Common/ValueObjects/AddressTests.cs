using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Common.ValueObjects;
using FluentAssertions;
using TestUtils.Builders.ValueObjectBuilders;
using TestUtils.Constants;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Common.ValueObjects;

public class AddressTests
{
    /* - - - Constructor - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceStreet_ThrowsValidationException(string? street)
    {
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithStreet(street)
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.Street),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }
    
    [Fact]
    public void Constructor_WithStreetExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var street = new string('*', Lengths.Street + 1);
        
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithStreet(street)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.Street),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceHouseNumber_ThrowsValidationException(string? houseNumber)
    {
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithHouseNumber(houseNumber)
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.HouseNumber),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }
    
    [Fact]
    public void Constructor_WithHouseNumberExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var houseNumber = new string('*', Lengths.HouseNumber + 1);
        
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithHouseNumber(houseNumber)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.HouseNumber),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceCity_ThrowsValidationException(string? city)
    {
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithCity(city)
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.City),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }
    
    [Fact]
    public void Constructor_WithCityExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var city = new string('*', Lengths.City + 1);
        
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithCity(city)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.City),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceZipCode_ThrowsValidationException(string? zipCode)
    {
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithZipCode(zipCode)
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.ZipCode),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidZipCodesNotMatchingRegex), MemberType = typeof(TestConstants))]
    public void Constructor_WithZipCodeNotMatchingRegex_ThrowsValidationException(string zipCode, Country country)
    {
        // Act
        var act = () => TestAddressBuilder
            .Create()
            .WithZipCode(zipCode)
            .WithCountry(country)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.ZipCode),
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
            Field = nameof(Address.Country),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        }); 
    }
    
    [Theory]
    [InlineData(TestConstants.ValidGermanZipCode, Country.De)]
    [InlineData(TestConstants.ValidFinnishZipCode, Country.Fi)]
    public void Constructor_WithValidArguments_SetsAllProperties(string zipCode, Country country)
    {
        // Arrange
        var addressBuilder = TestAddressBuilder
            .Create()
            .WithZipCode(zipCode)
            .WithCountry(country);
        
        // Act
        var sut = addressBuilder.Build();
        
        // Assert
        sut.Street.Should().Be(addressBuilder.Street);
        sut.HouseNumber.Should().Be(addressBuilder.HouseNumber);
        sut.City.Should().Be(addressBuilder.City);
        sut.ZipCode.Should().Be(zipCode);
        sut.Country.Should().Be(country);
    }
    
    
    /* - - - Method: ToString - - - */
    [Fact]
    public void ToString_ReturnsStringContainingAllProperties()
    {
        // Arrange
        var sut = TestAddressBuilder.Create().Build();

        // Act
        var result = sut.ToString();
        
        // Assert
        result.Should()
            .Contain(sut.Street).And
            .Contain(sut.HouseNumber).And
            .Contain(sut.City).And
            .Contain(sut.ZipCode).And
            .Contain(sut.Country.ToString());
    }
    
    
    /* - - - Method: Equals and GetHashCode - - - */
    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var sut = TestAddressBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(null);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        // Arrange
        var sut = TestAddressBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(new object());
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentStreet_ReturnsFalse()
    {
        // Arrange
        var addressBuilder = TestAddressBuilder.Create();
        var addressOne = addressBuilder.Build();
        var addressTwo = addressBuilder
            .WithStreet(new string(addressOne.Street.Reverse().ToArray()))
            .Build();
        
        // Act
        var result = addressOne.Equals(addressTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentHouseNumber_ReturnsFalse()
    {
        // Arrange
        var addressBuilder = TestAddressBuilder.Create();
        var addressOne = addressBuilder.Build();
        var addressTwo = addressBuilder
            .WithHouseNumber(new string(addressOne.HouseNumber.Reverse().ToArray()))
            .Build();
        
        // Act
        var result = addressOne.Equals(addressTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentCity_ReturnsFalse()
    {
        // Arrange
        var addressBuilder = TestAddressBuilder.Create();
        var addressOne = addressBuilder.Build();
        var addressTwo = addressBuilder
            .WithCity(new string(addressOne.City.Reverse().ToArray()))
            .Build();
        
        // Act
        var result = addressOne.Equals(addressTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(TestConstants.ValidGermanZipCode, Country.De)]
    [InlineData(TestConstants.ValidFinnishZipCode, Country.Fi)]
    public void Equals_WithDifferentZipCode_ReturnsFalse(string zipCode, Country country)
    {
        // Arrange
        var addressOne = TestAddressBuilder
            .Create()
            .WithZipCode(zipCode)
            .WithCountry(country)
            .Build();
        var addressTwo = TestAddressBuilder
            .Create()
            .WithZipCode(new string(zipCode.Reverse().ToArray()))
            .WithCountry(country)
            .Build();
        
        // Act
        var result = addressOne.Equals(addressTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentCountry_ReturnsFalse()
    {
        // Arrange
        var addressOne = TestAddressBuilder
            .Create()
            .WithCountry(Country.De)
            .Build();
        var addressTwo = TestAddressBuilder
            .Create()
            .WithCountry(Country.Fi)
            .Build();
        
        // Act
        var result = addressOne.Equals(addressTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithIdenticalValues_ReturnsTrue()
    {
        // Arrange
        var addressOne = TestAddressBuilder.Create().Build();
        var addressTwo = TestAddressBuilder.Create().Build();
        
        // Act
        var equalsResult = addressOne.Equals(addressTwo);
        var getHashCodeAddressOneResult = addressOne.GetHashCode();
        var getHashCodeAddressTwoResult = addressTwo.GetHashCode();
        
        // Assert
        equalsResult.Should().BeTrue();
        getHashCodeAddressOneResult.Should().Be(getHashCodeAddressTwoResult);
    }
}