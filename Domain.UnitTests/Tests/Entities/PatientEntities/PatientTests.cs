using System.Globalization;
using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Common.ValueObjects;
using Domain.Entities.PatientEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.PatientBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.PatientEntities;

public class PatientTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceFirstName_ThrowsValidationException(string? firstName)
    {
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithFirstName(firstName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.FirstName),
            ErrorCode = ErrorCode.MISSING_VALUE
        });  
    }

    [Fact]
    public void Constructor_WithFirstNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var firstName = new string('*', Lengths.FirstName + 1);
        
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithFirstName(firstName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.FirstName),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceLastName_ThrowsValidationException(string? lastName)
    {
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithLastName(lastName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.LastName),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithLastNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var lastName = new string('*', Lengths.LastName + 1);
        
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithLastName(lastName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.LastName),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Constructor_WithFutureDateOfBirth_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithDateOfBirth(currentDateTime.AddDays(1))
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.DateOfBirth),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_PAST
        });
    }
    
    [Fact]
    public void Constructor_WithUndefinedGender_ThrowsValidationException()
    {
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithGender((Gender)999)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Gender),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceStreet_ThrowsValidationException(string? street)
    {
        // Act
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
        var act = () => TestPatientBuilder
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
    public void Constructor_WithUndefinedInsuranceStatus_ThrowsValidationException()
    {
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithInsuranceStatus((InsuranceStatus)999)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.InsuranceStatus),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithEmptyOrWhitespaceAllergies_ThrowsValidationException(string? allergies)
    {
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithAllergies(allergies)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Allergies),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Constructor_WithAllergiesExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var allergies = new string('*', Lengths.Allergies + 1);
        
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithAllergies(allergies)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Allergies),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithEmptyOrWhitespaceRemarks_ThrowsValidationException(string? remarks)
    {
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithRemarks(remarks)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Remarks),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Constructor_WithRemarksExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var remarks = new string('*', Lengths.PatientRemarks + 1);
        
        // Act
        var act = () => TestPatientBuilder
            .Create()
            .WithRemarks(remarks)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Remarks),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.ValidCountryRelatedInformation), MemberType = typeof(TestConstants))]
    public void Constructor_WithValidArguments_SetsAllProperties(string zipCode, string phoneNumber, Country country)
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        
        // Act
        var sut = patientBuilder
            .WithZipCode(zipCode)
            .WithPhoneNumber(phoneNumber)
            .WithCountry(country)
            .Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(patientBuilder.Clinic);
        sut.ClinicId.Should().Be(patientBuilder.Clinic!.Id);
        sut.DateOfCreation.Should().Be(patientBuilder.CreationDateTime);
        sut.FirstName.Should().Be(patientBuilder.FirstName);
        sut.LastName.Should().Be(patientBuilder.LastName);
        sut.DateOfBirth.Should().Be(patientBuilder.DateOfBirth.Date);
        sut.Gender.Should().Be(patientBuilder.Gender);
        sut.Address.Should().Be(
            new Address(
                patientBuilder.Street!,
                patientBuilder.HouseNumber!,
                patientBuilder.City!,
                patientBuilder.ZipCode!,
                patientBuilder.Country));
        sut.ContactInformation.Should().Be(
            new ContactInformation(
                patientBuilder.Email!,
                patientBuilder.PhoneNumber!,
                patientBuilder.Country));
        sut.InsuranceStatus.Should().Be(patientBuilder.InsuranceStatus);
        sut.IsArchived.Should().BeFalse();
        sut.IsDeleted.Should().BeFalse();
        sut.Allergies.Should().Be(patientBuilder.Allergies);
        sut.Remarks.Should().Be(patientBuilder.Remarks);
    }
    
    
    /* - - - Method: Update - - - */
    [Fact]
    public void Update_WithArchivedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        sut.Archive([], []);
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Update_WithDeletedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        sut.Archive([], []);
        sut.Delete([], [], []);
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceLastName_ThrowsValidationException(string? lastName)
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .WithLastName(lastName)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.LastName),
            ErrorCode = ErrorCode.MISSING_VALUE
        });  
    }

    [Fact]
    public void Update_WithLastNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var lastName = new string('*', Lengths.LastName + 1);
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .WithLastName(lastName)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.LastName),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });  
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceStreet_ThrowsValidationException(string? street)
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
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
    
    [Fact]
    public void Update_WithUndefinedInsuranceStatus_ThrowsValidationException()
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .WithInsuranceStatus((InsuranceStatus)999)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.InsuranceStatus),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });  
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithEmptyOrWhitespaceAllergies_ThrowsValidationException(string? allergies)
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .WithAllergies(allergies)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Allergies),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });  
    }

    [Fact]
    public void Update_WithAllergiesExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var allergies = new string('*', Lengths.Allergies + 1);
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .WithAllergies(allergies)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Allergies),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });  
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithEmptyOrWhitespaceRemarks_ThrowsValidationException(string? remarks)
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .WithRemarks(remarks)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Remarks),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });  
    }

    [Fact]
    public void Update_WithRemarksExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var remarks = new string('*', Lengths.PatientRemarks + 1);
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        var act = patientBuilder
            .AsUpdate()
            .WithRemarks(remarks)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Patient.Remarks),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });  
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.ValidCountryRelatedInformation), MemberType = typeof(TestConstants))]
    public void Update_WithValidArguments_UpdatesAllProperties(string zipCode, string phoneNumber, Country country)
    {
        // Arrange
        var patientBuilder = TestPatientBuilder.Create();
        var sut = patientBuilder.Build();
        
        // Act
        patientBuilder
            .AsUpdate()
            .WithZipCode(zipCode)
            .WithPhoneNumber(phoneNumber)
            .WithCountry(country)
            .Apply(sut)();
        
        // Assert
        sut.LastName.Should().Be(patientBuilder.LastName);
        sut.Address.Should().Be(
            new Address(
                patientBuilder.Street!,
                patientBuilder.HouseNumber!,
                patientBuilder.City!,
                patientBuilder.ZipCode!,
                patientBuilder.Country));
        sut.ContactInformation.Should().Be(
            new ContactInformation(
                patientBuilder.Email!,
                patientBuilder.PhoneNumber!,
                patientBuilder.Country));
        sut.InsuranceStatus.Should().Be(patientBuilder.InsuranceStatus);
        sut.Allergies.Should().Be(patientBuilder.Allergies);
        sut.Remarks.Should().Be(patientBuilder.Remarks);
    }
}