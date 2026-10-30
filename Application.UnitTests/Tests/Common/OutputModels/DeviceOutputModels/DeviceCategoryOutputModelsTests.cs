using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Entities.DeviceEntities;
using FluentAssertions;
using TestUtils.Builders.DeviceBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.DeviceOutputModels;

public class DeviceCategoryOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var deviceCategory = TestDeviceCategoryBuilder.Create().Build();
        
        // Act
        var result = new DeviceCategoryOverviewOutputModel(deviceCategory);
        
        // Assert
        result.Id.Should().Be(deviceCategory.Id);
        result.Name.Should().Be(deviceCategory.Name);
        result.Abbreviation.Should().Be(deviceCategory.Abbreviation);
        result.Color.Should().Be(deviceCategory.Color);
    }
}

public class DeviceCategoryDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var deviceCategory = TestDeviceCategoryBuilder.Create().Build();
        var devices = new List<Device> { TestDeviceBuilder.Create().Build() };
        
        // Act
        var result = new DeviceCategoryDetailedOutputModel(
            deviceCategory,
            devices);
        
        // Assert
        result.Id.Should().Be(deviceCategory.Id);
        result.Name.Should().Be(deviceCategory.Name);
        result.Abbreviation.Should().Be(deviceCategory.Abbreviation);
        result.Color.Should().Be(deviceCategory.Color);
        result.Devices.Should().BeEquivalentTo(devices.Select(device => new DeviceOverviewOutputModel(device)));
    }
}