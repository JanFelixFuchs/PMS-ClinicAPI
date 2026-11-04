using Application.Common.OutputModels.SharedOutputModels;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.SharedOutputModels;

public class CurrentUserOutputModelTests
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
        var result = new CurrentUserOutputModel(
            user,
            role,
            clinician);
        
        // Assert
        result.Username.Should().Be(user.Username);
        result.IsAdmin.Should().Be(user.IsAdmin);
        result.RoleName.Should().Be(role.Name);
        result.FirstName.Should().Be(clinician.FirstName);
        result.LastName.Should().Be(clinician.LastName);
    }
}