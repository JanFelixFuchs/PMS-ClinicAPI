using Domain.Common.Utils.Constants;
using Domain.Entities.ClinicianEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.ClinicianEntities;

public class ClinicianTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceFirstName_ThrowsValidationException(string? firstName)
    {
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithFirstName(firstName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.FirstName),
            ErrorCode = ErrorCode.MISSING_VALUE
        });  
    }

    [Fact]
    public void Constructor_WithFirstNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var firstName = new string('*', Lengths.FirstName + 1);
        
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithFirstName(firstName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.FirstName),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceLastName_ThrowsValidationException(string? lastName)
    {
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithLastName(lastName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.LastName),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithLastNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var lastName = new string('*', Lengths.LastName + 1);
        
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithLastName(lastName)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.LastName),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }

    [Fact]
    public void Constructor_WithNullClinicianCategoriesCollection_ThrowsValidationException()
    {
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithClinicianCategories(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNullClinicianCategoryElement_ThrowsValidationException()
    {
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithClinicianCategories([null!])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithDuplicateClinicianCategories_ThrowsValidationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        
        // Act
        var act = () => clinicianBuilder
            .WithClinicianCategories([clinicianCategory,  clinicianCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void Constructor_WithClinicianCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestClinicianBuilder
            .Create()
            .WithClinicianCategories([TestClinicianCategoryBuilder.Create().Build()])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Constructor_WithDeletedClinicianCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        clinicianCategory.Delete([]);
        
        // Act
        var act = () => clinicianBuilder
            .WithClinicianCategories([clinicianCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        
        // Act
        var sut = clinicianBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(clinicianBuilder.Clinic);
        sut.ClinicId.Should().Be(clinicianBuilder.Clinic!.Id);
        sut.FirstName.Should().Be(clinicianBuilder.FirstName);
        sut.LastName.Should().Be(clinicianBuilder.LastName);
        sut.IsArchived.Should().BeFalse();
        sut.IsDeleted.Should().BeFalse();
        sut.ClinicianCategories.Should().Equal(clinicianBuilder.ClinicianCategories);
    }
    
    
     /* - - - Method: Update - - - */
    [Fact]
    public void Update_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        sut.Archive([], []);
        
        // Act
        var act = clinicianBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Update_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        sut.Archive([], []);
        sut.Delete(null, [], [], []);
        
        // Act
        var act = clinicianBuilder
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
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        
        // Act
        var act = clinicianBuilder
            .AsUpdate()
            .WithLastName(lastName)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.LastName),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Fact]
    public void Update_WithLastNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var lastName = new string('*', Lengths.LastName + 1);
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        
        // Act
        var act = clinicianBuilder
            .AsUpdate()
            .WithLastName(lastName)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.LastName),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Update_WithNullClinicianCategoriesCollection_ThrowsValidationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        
        // Act
        var act  = clinicianBuilder
            .AsUpdate()
            .WithClinicianCategories(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithNullClinicianCategoryElement_ThrowsValidationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        
        // Act
        var act = clinicianBuilder
            .AsUpdate()
            .WithClinicianCategories([null!])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithDuplicateClinicianCategories_ThrowsValidationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        var sut = clinicianBuilder.Build();
        
        // Act
        var act  = clinicianBuilder
            .AsUpdate()
            .WithClinicianCategories([clinicianCategory,  clinicianCategory])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }
    
    [Fact]
    public void Update_WithClinicianCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        
        // Act
        var act = clinicianBuilder
            .AsUpdate()
            .WithClinicianCategories([TestClinicianCategoryBuilder.Create().Build()])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedClinicianCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        var sut = clinicianBuilder.Build();
        clinicianCategory.Delete([]);
        
        // Act
        var act  = clinicianBuilder
            .AsUpdate()
            .WithClinicianCategories([clinicianCategory])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithValidArguments_UpdatesAllProperties()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        
        // Act
        clinicianBuilder.AsUpdate().Apply(sut)();
        
        // Assert
        sut.LastName.Should().Be(clinicianBuilder.LastName);
        sut.ClinicianCategories.Should().Equal(clinicianBuilder.ClinicianCategories);
    }
    
    
    /* - - - Method: AddClinicianCategory - - - */
    [Fact]
    public void AddClinicianCategory_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.AddClinicianCategory(TestClinicianCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddClinicianCategory_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete(null, [], [], []);
        
        // Act
        var act = () => sut.AddClinicianCategory(TestClinicianCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddClinicianCategory_WithNull_ThrowsValidationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var act = () => sut.AddClinicianCategory(null!);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void AddClinicianCategory_WithDuplicateClinicianCategories_ThrowsValidationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        var sut = clinicianBuilder
            .WithClinicianCategories([clinicianCategory])
            .Build();
        
        // Act
        var act = () => sut.AddClinicianCategory(clinicianCategory);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void AddClinicianCategory_WithClinicianCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var clinicianCategory = TestClinicianCategoryBuilder.Create().Build();
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var act = () => sut.AddClinicianCategory(clinicianCategory);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void AddClinicianCategory_WithDeletedClinicianCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        var sut = clinicianBuilder.Build();
        clinicianCategory.Delete([]);
        
        // Act
        var act = () => sut.AddClinicianCategory(clinicianCategory);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Clinician.ClinicianCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void AddClinicianCategory_WithValidClinicianCategory_AddsClinicianCategory()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        var sut = clinicianBuilder.Build();
        
        // Act
        sut.AddClinicianCategory(clinicianCategory);
        
        // Assert
        sut.ClinicianCategories.Should().Contain(clinicianCategory);
    }
    
    
    /* - - - Method: RemoveClinicianCategory - - - */
    [Fact]
    public void RemoveClinicianCategory_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.RemoveClinicianCategory(TestClinicianCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveClinicianCategory_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete(null, [], [], []);
        
        // Act
        var act = () => sut.RemoveClinicianCategory(TestClinicianCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void RemoveClinicianCategory_WithNull_DoesNotChangeClinicianCategories()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var sut = clinicianBuilder.Build();
        
        // Act
        sut.RemoveClinicianCategory(null!);
        
        // Assert
        sut.ClinicianCategories.Should().BeEquivalentTo(clinicianBuilder.ClinicianCategories);
    }
    
    [Fact]
    public void RemoveClinicianCategory_WithNonExistingClinicianCategory_DoesNotChangeClinicianCategories()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        var sut = clinicianBuilder.Build();
        
        // Act
        sut.RemoveClinicianCategory(clinicianCategory);
        
        // Assert
        sut.ClinicianCategories.Should().BeEquivalentTo(clinicianBuilder.ClinicianCategories);
    }
    
    [Fact]
    public void RemoveClinicianCategory_WithExistingClinicianCategory_RemovesClinicianCategory()
    {
        // Arrange
        var clinicianBuilder = TestClinicianBuilder.Create();
        var clinicianCategory = TestClinicianCategoryBuilder.Create(clinicianBuilder.Clinic).Build();
        var sut = clinicianBuilder.Build();
        sut.AddClinicianCategory(clinicianCategory);
        
        // Act
        sut.RemoveClinicianCategory(clinicianCategory);
        
        // Assert
        sut.ClinicianCategories.Should().NotContain(clinicianCategory);
    }
    
    
    /* - - - Method: Archive - - - */
    [Fact]
    public void Archive_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.Archive([], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Archive_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete(null, [], [], []);
        
        // Act
        var act = () => sut.Archive([], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Archive_WithNonAttendedAppointments_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var act = () => sut.Archive([TestAppointmentBuilder.Create().Build()], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Archive_WithNonCompletedAppointmentProtocols_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var act = () => sut.Archive([], [TestAppointmentProtocolBuilder.Create().Build()]);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Archive_WithArchivableClinician_ArchivesClinician()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        sut.Archive([], []);
        
        // Assert
        sut.IsArchived.Should().BeTrue();
    }
    
    
    /* - - - Method: Unarchive - - - */
    [Fact]
    public void Unarchive_WithNonArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var act = () => sut.Unarchive();
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Unarchive_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete(null, [], [], []);
        
        // Act
        var act = () => sut.Unarchive();
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Unarchive_WithUnarchivableClinician_UnarchivesClinician()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        sut.Unarchive();
        
        // Assert
        sut.IsArchived.Should().BeFalse();
    }
    
    
    /* - - - Method: Delete - - - */
    [Fact]
    public void Delete_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete(null, [], [], []);
        
        // Act
        var act = () => sut.Delete(null, [], [], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Delete_WithNonArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var act = () => sut.Delete(null, [], [], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Delete_WithUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.Delete(TestUserBuilder.Create().Build(), [], [], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Delete_WithAppointments_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.Delete(null, [TestAppointmentBuilder.Create().Build()], [], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Delete_WithAppointmentProtocols_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.Delete(null, [], [TestAppointmentProtocolBuilder.Create().Build()], []);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Delete_WithDeletableClinician_DeletesClinician()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        sut.Delete(null, [], [], []);
        
        // Assert
        sut.IsDeleted.Should().BeTrue();
    }
    
    
    /* - - - Method: ToString - - - */
    [Fact]
    public void ToString_ReturnsStringContainingAllProperties()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
    
        // Act
        var result = sut.ToString();
        
        // Assert
        result.Should()
            .Contain(sut.Id.ToString()).And
            .Contain(sut.ClinicId.ToString()).And
            .Contain(sut.FirstName).And
            .Contain(sut.LastName).And
            .Contain(sut.IsArchived.ToString()).And
            .Contain(sut.IsDeleted.ToString()).And
            .ContainAll(sut.ClinicianCategories.Select(clinicianCategory => clinicianCategory.Id.ToString()).ToArray());
    }
    
    
    /* - - - Method: Equals and GetHashCode - - - */
    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(null);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        // Arrange
        var sut = TestClinicianBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(new object());
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentId_ReturnsFalse()
    {
        // Arrange
        var clinicianOne = TestClinicianBuilder.Create().Build();
        var clinicianTwo = TestClinicianBuilder.Create().Build();
        
        // Act
        var result = clinicianOne.Equals(clinicianTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithSameId_ReturnsTrue()
    {
        // Arrange
        var clinicianOne = TestClinicianBuilder.Create().Build();
        var clinicianTwo = TestClinicianBuilder.Create().Build();
        TestEntityHelper.SetId(clinicianTwo, clinicianOne.Id);
        
        // Act
        var equalsResult = clinicianOne.Equals(clinicianTwo);
        var getHashCodeClinicianOneResult = clinicianOne.GetHashCode();
        var getHashCodeClinicianTwoResult = clinicianTwo.GetHashCode();
        
        // Assert
        equalsResult.Should().BeTrue();
        getHashCodeClinicianOneResult.Should().Be(getHashCodeClinicianTwoResult);
    }
}