using Application.Common.OutputModels.IdentityOutputModels;
using FluentAssertions;

namespace Application.UnitTests.Tests.Common.OutputModels.IdentityOutputModels;

public class UpdatePasswordOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        const string accessToken = "test-access-token";
        
        // Act
        var result = new UpdatePasswordOutputModel(accessToken);
        
        // Assert
        result.AccessToken.Should().Be(accessToken);
    }
}