using Domain.Common.Enums;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using Claim = Domain.Entities.IdentityEntities.Claim;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.IdentityEntities;

public class ClaimTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullRole_ThrowsValidationException()
    {
        // Act
        var act = () => TestClaimBuilder
            .Create()
            .WithRole(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Claim.Role),
            ErrorCode = ErrorCode.MISSING_VALUE
        });  
    }

    [Fact]
    public void Constructor_WithDeletedRole_ThrowsInvalidOperationException()
    {
        // Arrange
        var role = TestRoleBuilder.Create().Build();
        role.Delete([], []);
        
        // Act
        var act = () => TestClaimBuilder
            .Create()
            .WithRole(role)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Claim.Role),
            ErrorCode = ErrorCode.DELETED_ENTITY
        }); 
    }

    [Fact]
    public void Constructor_WithUndefinedType_ThrowsValidationException()
    {
        // Act
        var act = () => TestClaimBuilder
            .Create()
            .WithType((ClaimType)999)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Claim.Type),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithUndefinedValue_ThrowsValidationException()
    {
        // Act
        var act = () => TestClaimBuilder
            .Create()
            .WithValue((ClaimValue)999)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Claim.Value),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var claimBuilder = TestClaimBuilder.Create();

        // Act
        var sut = claimBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Role.Should().Be(claimBuilder.Role);
        sut.RoleId.Should().Be(claimBuilder.Role!.Id);
        sut.Type.Should().Be(claimBuilder.Type);
        sut.Value.Should().Be(claimBuilder.Value);
        sut.IsDeleted.Should().BeFalse();
    }
    
    
    /* - - - Method: Delete - - - */
    [Fact]
    public void Delete_WithDeletedClaim_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClaimBuilder.Create().Build();
        sut.Delete();
        
        // Act
        var act = () => sut.Delete();
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Delete_WithDeletableClaim_DeletesClaim()
    {
        // Arrange
        var sut = TestClaimBuilder.Create().Build();
        
        // Act
        sut.Delete();
        
        // Assert
        sut.IsDeleted.Should().BeTrue();
    }
}