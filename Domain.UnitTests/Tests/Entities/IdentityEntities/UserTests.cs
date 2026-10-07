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

    
    /* - - - Method: UpdateRole - - - */
    [Fact]
    public void UpdateRole_WithArchivedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut  = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        
        // Act
        var act = () => sut.UpdateRole(TestRoleBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void UpdateRole_WithDeletedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut  = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        sut.Delete();
        
        // Act
        var act = () => sut.UpdateRole(TestRoleBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void UpdateRole_WithAdminUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut  = TestUserBuilder
            .Create()
            .WithIsAdmin(true)
            .Build();
        
        // Act
        var act = () => sut.UpdateRole(TestRoleBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void UpdateRole_WithNullRole_ThrowsValidationException()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateRole(null!);
        
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Role),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void UpdateRole_WithRoleOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateRole(TestRoleBuilder.Create().Build());
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Role),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }

    [Fact]
    public void UpdateRole_WithDeletedRole_ThrowsInvalidOperationException()
    {
        // Arrange
        var userBuilder = TestUserBuilder.Create();
        var role = TestRoleBuilder.Create(userBuilder.Clinic).Build();
        role.Delete([], []);
        var sut = userBuilder.Build();
        
        // Act
        var act = () => sut.UpdateRole(role);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Role),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void UpdateRole_WithValidRole_UpdatesRole()
    {
        // Arrange
        var userBuilder = TestUserBuilder.Create();
        var role = TestRoleBuilder.Create(userBuilder.Clinic).Build();
        var sut = userBuilder.Build();
        
        // Act
        sut.UpdateRole(role);

        // Assert
        sut.Role.Should().Be(role);
        sut.RoleId.Should().Be(role.Id);
    }
    
    
    /* - - - Method: UpdateUsername - - - */
    [Fact]
    public void UpdateUsername_WithArchivedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        
        // Act
        var act = () => sut.UpdateUsername(ValidUsernameMatchingRegex);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void UpdateUsername_WithDeletedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        sut.Delete();
        
        // Act
        var act = () => sut.UpdateUsername(ValidUsernameMatchingRegex);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void UpdateUsername_WithNullEmptyOrWhitespaceUsername_ThrowsValidationException(string? username)
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateUsername(username!);

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
    public void UpdateUsername_WithUsernameNotMatchingRegex_ThrowsValidationException(string username)
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateUsername(username);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.Username),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }

    [Fact]
    public void UpdateUsername_WithValidUsername_UpdatesUsernameAndNormalizedUsername()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        sut.UpdateUsername(ValidUsernameMatchingRegex);
        
        // Assert
        sut.Username.Should().Be(ValidUsernameMatchingRegex);
        sut.NormalizedUsername.Should().Be(StringHelper.Normalize(ValidUsernameMatchingRegex));
    }
    
    
    /* - - - Method: UpdatePasswordHash - - - */
    [Fact]
    public void UpdatePasswordHash_WithArchivedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        
        // Act
        var act = () => sut.UpdatePasswordHash(ValidPasswordHash);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void UpdatePasswordHash_WithDeletedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        sut.Delete();
        
        // Act
        var act = () => sut.UpdatePasswordHash(ValidPasswordHash);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void UpdatePasswordHash_WithNullEmptyOrWhitespacePasswordHash_ThrowsValidationException(string? passwordHash)
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdatePasswordHash(passwordHash!);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.PasswordHash),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void UpdatePasswordHash_WithPasswordHashExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var passwordHash = new string('*', Lengths.PasswordHash + 1);
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdatePasswordHash(passwordHash);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.PasswordHash),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });  
    }
    
    [Fact]
    public void UpdatePasswordHash_WithValidPasswordHash_UpdatesPasswordHash()
    {
        // Arrange
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        sut.UpdatePasswordHash(ValidPasswordHash);
        
        // Assert
        sut.PasswordHash.Should().Be(ValidPasswordHash);
    }
    
    
    /* - - - Method: UpdateRefreshTokenHashAndExpirationTime - - - */
    [Fact]
    public void UpdateRefreshTokenHashAndExpirationTime_WithArchivedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        
        // Act
        var act = () => sut.UpdateRefreshTokenHashAndExpirationTime(ValidRefreshTokenHash, currentDateTime.AddMinutes(15), currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void UpdateRefreshTokenHashAndExpirationTime_WithDeletedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(TestConstants.DefaultCurrentDateTime);
        sut.Delete();
        
        // Act
        var act = () => sut.UpdateRefreshTokenHashAndExpirationTime(ValidRefreshTokenHash, currentDateTime.AddMinutes(15), currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void UpdateRefreshTokenHashAndExpirationTime_WithNullEmptyOrWhitespaceRefreshTokenHash_ThrowsValidationException(string? refreshTokenHash)
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateRefreshTokenHashAndExpirationTime(refreshTokenHash!, currentDateTime.AddMinutes(15), currentDateTime);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.RefreshTokenHash),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });  
    }
    
    [Fact]
    public void UpdateRefreshTokenHashAndExpirationTime_WithRefreshTokenHashExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var refreshTokenHash = new string('*', Lengths.RefreshTokenHash + 1);
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateRefreshTokenHashAndExpirationTime(refreshTokenHash, currentDateTime.AddMinutes(15), currentDateTime);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.RefreshTokenHash),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });  
    }

    [Fact]
    public void UpdateRefreshTokenHashAndExpirationTime_WithPastRefreshTokenExpirationTime_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        var act = () => sut.UpdateRefreshTokenHashAndExpirationTime(ValidRefreshTokenHash, currentDateTime.AddMinutes(-15), currentDateTime);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(User.RefreshTokenExpirationTime),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_FUTURE
        }); 
    }
    
    [Fact]
    public void UpdateRefreshTokenHashAndExpirationTime_WithValidArguments_UpdatesRefreshTokenHashAndExpirationTime()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var refreshTokenHashExpirationTime = currentDateTime.AddMinutes(15);
        var sut = TestUserBuilder.Create().Build();
        
        // Act
        sut.UpdateRefreshTokenHashAndExpirationTime(ValidRefreshTokenHash, refreshTokenHashExpirationTime, currentDateTime);
        
        // Assert
        sut.RefreshTokenHash.Should().Be(ValidRefreshTokenHash);
        sut.RefreshTokenExpirationTime.Should().Be(refreshTokenHashExpirationTime);
    }
    
    
    /* - - - Method: MarkRefreshTokenHashAsExpired - - - */
    [Fact]
    public void MarkRefreshTokenHashAsExpired_WithArchivedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(currentDateTime);
        
        // Act
        var act = () => sut.MarkRefreshTokenHashAsExpired(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void MarkRefreshTokenHashAsExpired_WithDeletedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = TestUserBuilder.Create().Build();
        sut.Archive(currentDateTime);
        sut.Delete();
        
        // Act
        var act = () => sut.MarkRefreshTokenHashAsExpired(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkRefreshTokenHashAsExpired_WithExistingRefreshTokenHash_MarksRefreshTokenHashAsExpired()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var sut = TestUserBuilder.Create().Build();
        sut.UpdateRefreshTokenHashAndExpirationTime(ValidRefreshTokenHash, currentDateTime.AddMinutes(15), currentDateTime);
        
        // Act
        sut.MarkRefreshTokenHashAsExpired(currentDateTime);
        
        // Assert
        sut.RefreshTokenHash.Should().BeNull();
        sut.RefreshTokenExpirationTime.Should().BeNull();
    }
}