using FluentAssertions;
using TestUtils.Builders.DeviceBuilders;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.DeviceEntities;

public class DeviceCategoryTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var deviceCategoryBuilder = TestDeviceCategoryBuilder.Create();
        
        // Act
        var sut = deviceCategoryBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(deviceCategoryBuilder.Clinic);
        sut.ClinicId.Should().Be(deviceCategoryBuilder.Clinic!.Id);
        sut.Name.Should().Be(deviceCategoryBuilder.Name);
        sut.Abbreviation.Should().Be(deviceCategoryBuilder.Abbreviation);
        sut.Color.Should().Be(deviceCategoryBuilder.Color);
        sut.IsDeleted.Should().BeFalse();
        sut.Devices.Should().BeEmpty();
    }
    
    
    /* - - - Method: Delete - - - */
    [Fact]
    public void Delete_WithExistingRooms_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestDeviceCategoryBuilder.Create().Build();
        
        // Act
        var act = () => sut.Delete([TestDeviceBuilder.Create().Build()]);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}