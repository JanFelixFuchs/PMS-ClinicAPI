using Application.Common.OutputModels.SharedOutputModels;
using FluentAssertions;
using TestUtils.Builders.ValueObjectBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.SharedOutputModels;

public class ContactInformationOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var contactInformation = TestContactInformationBuilder.Create().Build();
        
        // Act
        var result = new ContactInformationOutputModel(contactInformation);
        
        // Assert
        result.Email.Should().Be(contactInformation.Email);
        result.PhoneNumber.Should().Be(contactInformation.PhoneNumber);
    }
}