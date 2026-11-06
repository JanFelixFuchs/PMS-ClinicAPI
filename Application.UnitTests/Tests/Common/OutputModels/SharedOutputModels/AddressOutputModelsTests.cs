using Application.Common.OutputModels.SharedOutputModels;
using FluentAssertions;
using TestUtils.Builders.ValueObjectBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.SharedOutputModels;

public class AddressOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var address = TestAddressBuilder.Create().Build();
        
        // Act
        var result = new AddressOutputModel(address);
        
        // Assert
        result.Street.Should().Be(address.Street);
        result.HouseNumber.Should().Be(address.HouseNumber);
        result.City.Should().Be(address.City);
        result.ZipCode.Should().Be(address.ZipCode);
        result.Country.Should().Be(address.Country);
    }
}