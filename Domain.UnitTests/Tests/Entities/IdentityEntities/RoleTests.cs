using Domain.Common.Utils.Constants;
using Domain.Common.Utils.Helper;
using Domain.Entities.IdentityEntities;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.IdentityEntities;

public class RoleTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestRoleBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Role.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Act
        var act = () => TestRoleBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Role.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });  
    }

    [Fact]
    public void Constructor_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.RoleName + 1);
        
        // Act
        var act = () => TestRoleBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Role.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_WithValidArguments_SetsAllProperties(bool isSystemRole)
    {
        // Arrange
        var roleBuilder = TestRoleBuilder.Create();
        
        // Act
        var sut = roleBuilder
            .WithIsSystemRole(isSystemRole)
            .Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(roleBuilder.Clinic);
        sut.ClinicId.Should().Be(roleBuilder.Clinic!.Id);
        sut.Name.Should().Be(roleBuilder.Name);
        sut.NormalizedName.Should().Be(StringHelper.Normalize(roleBuilder.Name!));
        sut.IsSystemRole.Should().Be(roleBuilder.IsSystemRole);
        sut.IsDeleted.Should().BeFalse();
    }
    
    
    /* - - - Method: Update - - - */
    [Fact]
    public void Update_WithDeletedRole_ThrowsInvalidOperationException()
    {
        // Arrange
        var roleBuilder = TestRoleBuilder.Create();
        var sut = roleBuilder.Build();
        sut.Delete([], []);
        
        // Act
        var act = roleBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Arrange
        var roleBuilder = TestRoleBuilder.Create();
        var sut = roleBuilder.Build();
        
        // Act
        var act = roleBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Role.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });  
    }

    [Fact]
    public void Update_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.RoleName + 1);
        var roleBuilder = TestRoleBuilder.Create();
        var sut = roleBuilder.Build();
        
        // Act
        var act = roleBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Role.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Fact]
    public void Update_WithValidArguments_UpdatesAllProperties()
    {
        // Arrange
        var roleBuilder = TestRoleBuilder.Create();
        var sut = roleBuilder.Build();
        
        // Act
        roleBuilder.AsUpdate().Apply(sut)();
        
        // Assert
        sut.Name.Should().Be(roleBuilder.Name);
        sut.NormalizedName.Should().Be(StringHelper.Normalize(roleBuilder.Name!));
    }
}