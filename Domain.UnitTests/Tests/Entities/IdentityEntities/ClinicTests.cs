using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Common.Utils.Helper;
using Domain.Common.ValueObjects;
using Domain.Entities.IdentityEntities;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Entities.IdentityEntities;

public class ClinicTests
{
    /* - - - Preparation - - - */
    private const string ValidCodeMatchingRegex = "TESTUPDATEDCODE1";
    public static TheoryData<string> InvalidCodesNotMatchingRegex =>
    [
        new string('A', 7),
        new string('A', 65),
        "abcdefgh",
        "ABCDEFG!",
        "ABCD EFGH"
    ];
    
    
    /* - - - Constructor - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceCode_ThrowsValidationException(string? code)
    {
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithCode(code)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Code),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Theory]
    [MemberData(nameof(InvalidCodesNotMatchingRegex))]
    public void Constructor_WithCodeNotMatchingRegex_ThrowsValidationException(string? code)
    {
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithCode(code)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Code),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.ClinicName + 1);
        
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });  
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceOwner_ThrowsValidationException(string? owner)
    {
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithOwner(owner)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Owner),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithOwnerExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var owner = new string('*', Lengths.Owner + 1);
        
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithOwner(owner)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Owner),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }

    [Fact]
    public void Constructor_WithUndefinedMedicalField_ThrowsValidationException()
    {
        // Act
        var act = () => TestClinicBuilder
            .Create()
            .WithMedicalField((MedicalField)999)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.MedicalField),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceStreet_ThrowsValidationException(string? street)
    {
        // Act
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceEmail_ThrowsValidationException(string? email)
    {
        // Act
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
        var act = () => TestClinicBuilder
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
    
    [Theory]
    [MemberData(nameof(TestConstants.ValidCountryRelatedInformation), MemberType = typeof(TestConstants))]
    public void Constructor_WithValidArguments_SetsAllProperties(string zipCode, string phoneNumber, Country country)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        
        // Act
        var sut = clinicBuilder
            .WithZipCode(zipCode)
            .WithCountry(country)
            .WithPhoneNumber(phoneNumber)
            .Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Code.Should().Be(clinicBuilder.Code);
        sut.NormalizedCode.Should().Be(StringHelper.Normalize(clinicBuilder.Code!));
        sut.Name.Should().Be(clinicBuilder.Name);
        sut.Abbreviation.Should().Be(clinicBuilder.Abbreviation);
        sut.Owner.Should().Be(clinicBuilder.Owner);
        sut.MedicalField.Should().Be(clinicBuilder.MedicalField);
        sut.Address.Should().Be(
            new Address(
                clinicBuilder.Street!,
                clinicBuilder.HouseNumber!,
                clinicBuilder.City!,
                clinicBuilder.ZipCode!,
                clinicBuilder.Country));
        sut.ContactInformation.Should().Be(
            new ContactInformation(
                clinicBuilder.Email!,
                clinicBuilder.PhoneNumber!,
                clinicBuilder.Country));
    }
    
    
    /* - - - Method: Update - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.ClinicName + 1);
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceOwner_ThrowsValidationException(string? owner)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithOwner(owner)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Owner),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithOwnerExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var owner = new string('*', Lengths.Owner + 1);
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithOwner(owner)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Owner),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }

    [Fact]
    public void Update_WithUndefinedMedicalField_ThrowsValidationException()
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithMedicalField((MedicalField)999)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.MedicalField),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceStreet_ThrowsValidationException(string? street)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithStreet(street)
            .Apply(sut);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.Street),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithStreetExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var street = new string('*', Lengths.Street + 1);
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithStreet(street)
            .Apply(sut);
        
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
    public void Update_WithNullEmptyOrWhitespaceHouseNumber_ThrowsValidationException(string? houseNumber)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithHouseNumber(houseNumber)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.HouseNumber),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithHouseNumberExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var houseNumber = new string('*', Lengths.HouseNumber + 1);
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithHouseNumber(houseNumber)
            .Apply(sut);
        
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
    public void Update_WithNullEmptyOrWhitespaceCity_ThrowsValidationException(string? city)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithCity(city)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.City),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithCityExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var city = new string('*', Lengths.City + 1);
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithCity(city)
            .Apply(sut);
        
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
    public void Update_WithNullEmptyOrWhitespaceZipCode_ThrowsValidationException(string? zipCode)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithZipCode(zipCode)
            .Apply(sut);

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
    public void Update_WithZipCodeNotMatchingRegex_ThrowsValidationException(string zipCode, Country country)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithZipCode(zipCode)
            .WithCountry(country)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.ZipCode),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithUndefinedCountry_ThrowsValidationException()
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithCountry((Country)999)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Address.Country),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceEmail_ThrowsValidationException(string? email)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithEmail(email)
            .Apply(sut);
        
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
    public void Update_WithEmailNotMatchingRegex_ThrowsValidationException(string email)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithEmail(email)
            .Apply(sut);
        
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
    public void Update_WithNullEmptyOrWhitespacePhoneNumber_ThrowsValidationException(string? phoneNumber)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithPhoneNumber(phoneNumber)
            .Apply(sut);
        
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
    public void Update_WithPhoneNumberNotMatchingRegex_ThrowsValidationException(string phoneNumber, Country country)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();
        
        // Act
        var act = clinicBuilder
            .AsUpdate()
            .WithPhoneNumber(phoneNumber)
            .WithCountry(country)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(ContactInformation.PhoneNumber),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.ValidCountryRelatedInformation), MemberType = typeof(TestConstants))]
    public void Update_WithValidArguments_UpdatesAllProperties(string zipCode, string phoneNumber, Country country)
    {
        // Arrange
        var clinicBuilder = TestClinicBuilder.Create();
        var sut = clinicBuilder.Build();

        // Act
        clinicBuilder
            .AsUpdate()
            .WithZipCode(zipCode)
            .WithPhoneNumber(phoneNumber)
            .WithCountry(country)
            .Apply(sut)();
        
        // Assert
        sut.Name.Should().Be(clinicBuilder.Name);
        sut.Abbreviation.Should().Be(clinicBuilder.Abbreviation);
        sut.Owner.Should().Be(clinicBuilder.Owner);
        sut.MedicalField.Should().Be(clinicBuilder.MedicalField);
        sut.Address.Should().Be(
            new Address(
                clinicBuilder.Street!,
                clinicBuilder.HouseNumber!,
                clinicBuilder.City!,
                clinicBuilder.ZipCode!,
                clinicBuilder.Country));
        sut.ContactInformation.Should().Be(
            new ContactInformation(
                clinicBuilder.Email!,
                clinicBuilder.PhoneNumber!,
                clinicBuilder.Country));
    }
    
    
    /* - - - Method: UpdateCode - - - */
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void UpdateCode_WithNullEmptyOrWhitespaceCode_ThrowsValidationException(string? code)
    {
        // Arrange
        var sut = TestClinicBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateCode(code!);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Code),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Theory]
    [MemberData(nameof(InvalidCodesNotMatchingRegex))]
    public void UpdateCode_WithCodeNotMatchingRegex_ThrowsValidationException(string code)
    {
        // Arrange
        var sut = TestClinicBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateCode(code);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinic.Code),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }

    [Fact]
    public void UpdateCode_WithValidCode_UpdatesCodeAndNormalizedCode()
    {
        // Arrange
        var sut = TestClinicBuilder.Create().Build();
        
        // Act
        sut.UpdateCode(ValidCodeMatchingRegex);
        
        // Assert
        sut.Code.Should().Be(ValidCodeMatchingRegex);
        sut.NormalizedCode.Should().Be(StringHelper.Normalize(ValidCodeMatchingRegex));
    }
}