using Domain.Common.Utils.Constants;
using Domain.Common.Utils.Helper;
using Domain.Entities.IdentityEntities;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.IdentityEntities;

public class UserTests
{
    /* - - - Preparation - - - */
    private const string ValidUsernameMatchingRegex = "test_updated_username";
    private const string ValidRefreshTokenHash = "test-updated-refresh-token-hash";
    private const string ValidPasswordHash = "test-updated-password-hash";
    public static TheoryData<string> InvalidUsernameNotMatchingRegex =>
    [
        new string('A', 7),
        new string('A', 65),
        "test-username",
        "test+username",
        "test!username",
        "test#username",
        "test@username",
        "test username"
    ];
    
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceUsername_ThrowsValidationException(string? username)
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithUsername(username)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Username),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Theory]
    [MemberData(nameof(InvalidUsernameNotMatchingRegex))]
    public void Constructor_WithUsernameNotMatchingRegex_ThrowsValidationException(string username)
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithUsername(username)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Username),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespacePasswordHash_ThrowsValidationException(string? passwordHash)
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithPasswordHash(passwordHash)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.PasswordHash),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithPasswordHashExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var passwordHash = new string('*', Lengths.PasswordHash + 1);
        
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithPasswordHash(passwordHash)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.PasswordHash),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }

    [Fact]
    public void Constructor_WithNullRole_ThrowsValidationException()
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithRole(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Role),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithRoleOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithRole(TestRoleBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Role),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }

    [Fact]
    public void Constructor_WithDeletedRole_ThrowsInvalidOperationException()
    {
        // Arrange
        var userBuilder = TestUserBuilder.Create();
        var role = TestRoleBuilder.Create(userBuilder.Clinic).Build();
        role.Delete([], []);
        
        // Act
        var act = () => userBuilder
            .WithRole(role)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Role),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void Constructor_WithNonAdminUserAndNullClinician_ThrowsValidationException()
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithClinician(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Clinician),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNonAdminUserAndClinicianOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestUserBuilder
            .Create()
            .WithClinician(TestClinicianBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Clinician),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }

    [Fact]
    public void Constructor_WithNonAdminUserAndArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var userBuilder = TestUserBuilder.Create();
        var clinician = TestClinicianBuilder.Create(userBuilder.Clinic).Build();
        clinician.Archive([], []);
        
        // Act
        var act = () => userBuilder
            .WithClinician(clinician)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Clinician),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithNonAdminUserAndDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var userBuilder = TestUserBuilder.Create();
        var clinician = TestClinicianBuilder.Create(userBuilder.Clinic).Build();
        clinician.Archive([], []);
        clinician.Delete(null, [], [], []);
        
        // Act
        var act = () => userBuilder
            .WithClinician(clinician)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Clinician),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Constructor_WithValidArguments_SetsAllProperties(bool isAdmin)
    {
        // Arrange
        var userBuilder = TestUserBuilder.Create();
        var clinician = isAdmin 
            ? null 
            : TestClinicianBuilder.Create(userBuilder.Clinic).Build();
        
        // Act
        var sut = userBuilder
            .WithIsAdmin(isAdmin)
            .WithClinician(clinician)
            .Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(userBuilder.Clinic);
        sut.ClinicId.Should().Be(userBuilder.Clinic!.Id);
        sut.Username.Should().Be(userBuilder.Username);
        sut.NormalizedUsername.Should().Be(StringHelper.Normalize(userBuilder.Username!));
        sut.PasswordHash.Should().Be(userBuilder.PasswordHash);
        sut.IsAdmin.Should().Be(userBuilder.IsAdmin);
        sut.IsArchived.Should().BeFalse();
        sut.IsDeleted.Should().BeFalse();
        sut.RefreshTokenHash.Should().BeNull();
        sut.RefreshTokenExpirationTime.Should().BeNull();
        sut.Role.Should().Be(userBuilder.Role);
        sut.RoleId.Should().Be(userBuilder.Role!.Id);
        sut.Clinician.Should().Be(clinician);
        sut.ClinicianId.Should().Be(clinician?.Id);
    }
}