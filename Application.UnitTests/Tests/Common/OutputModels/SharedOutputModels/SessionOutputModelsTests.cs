using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.OutputModels.SharedOutputModels;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.SharedOutputModels;

public class SessionOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinic = TestClinicBuilder.Create().Build();
        var user = TestUserBuilder.Create().Build();
        var role = TestRoleBuilder.Create().Build();
        var clinician = TestClinicianBuilder.Create().Build();
        const string accessToken = "test-access-token";
        
        // Act
        var result = new SessionOutputModel(
            clinic,
            user,
            role,
            clinician,
            accessToken);
        
        // Assert
        result.Clinic.Should().BeEquivalentTo(new ClinicOutputModel(clinic));
        result.User.Should().BeEquivalentTo(new CurrentUserOutputModel(user, role, clinician));
        result.AccessToken.Should().Be(accessToken);
    }
}