using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.OutputModels.IdentityOutputModels;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.IdentityOutputModels;

public class UserOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var user = TestUserBuilder.Create().Build();
        
        // Act
        var result = new UserOverviewOutputModel(user);
        
        // Assert
        result.Id.Should().Be(user.Id);
        result.Username.Should().Be(user.Username);
        result.IsAdmin.Should().Be(user.IsAdmin);
        result.IsArchived.Should().Be(user.IsArchived);
    }
}

public class UserDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var user = TestUserBuilder.Create().Build();
        var role = TestRoleBuilder.Create().Build();
        var clinician = TestClinicianBuilder.Create().Build();
        
        // Act
        var result = new UserDetailedOutputModel(
            user,
            role,
            clinician);
        
        // Assert
        result.Id.Should().Be(user.Id);
        result.Username.Should().Be(user.Username);
        result.IsAdmin.Should().Be(user.IsAdmin);
        result.IsArchived.Should().Be(user.IsArchived);
        result.Role.Should().BeEquivalentTo(new RoleOverviewOutputModel(role));
        result.Clinician.Should().BeEquivalentTo(new ClinicianOverviewOutputModel(clinician));
    }
}