using Application.Common.OutputModels.SharedOutputModels;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.SharedOutputModels;

public class ClaimOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var claim = TestClaimBuilder.Create().Build();
        
        // Act
        var result = new ClaimOutputModel(claim);
        
        // Assert
        result.Id.Should().Be(claim.Id);
        result.Type.Should().Be(claim.Type);
        result.Value.Should().Be(claim.Value);
    }
}