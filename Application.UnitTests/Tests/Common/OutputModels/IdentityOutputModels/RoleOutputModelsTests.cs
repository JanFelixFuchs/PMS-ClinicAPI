using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.OutputModels.SharedOutputModels;
using Domain.Entities.IdentityEntities;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.IdentityOutputModels;

public class RoleOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var role = TestRoleBuilder.Create().Build();
        
        // Act
        var result = new RoleOverviewOutputModel(role);
        
        // Assert
        result.Id.Should().Be(role.Id);
        result.Name.Should().Be(role.Name);
        result.IsSystemRole.Should().Be(role.IsSystemRole);
    }
}

public class RoleDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var role = TestRoleBuilder.Create().Build();
        var users = new List<User> { TestUserBuilder.Create().Build() };
        var claims = new List<Claim> { TestClaimBuilder.Create().Build() };
        
        // Act
        var result = new RoleDetailedOutputModel(
            role,
            users,
            claims);
        
        // Assert
        result.Id.Should().Be(role.Id);
        result.Name.Should().Be(role.Name);
        result.IsSystemRole.Should().Be(role.IsSystemRole);
        result.Users.Should().BeEquivalentTo(users.Select(user => new UserOverviewOutputModel(user)));
        result.Claims.Should().BeEquivalentTo(claims.Select(claim => new ClaimOutputModel(claim)));
    }
}