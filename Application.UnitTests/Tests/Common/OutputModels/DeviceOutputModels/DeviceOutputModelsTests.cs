using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.DeviceEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.DeviceBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.DeviceOutputModels;

public class DeviceOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var device = TestDeviceBuilder.Create().Build();
        
        // Act
        var result = new DeviceOverviewOutputModel(device);
        
        // Assert
        result.Id.Should().Be(device.Id);
        result.Name.Should().Be(device.Name);
        result.Abbreviation.Should().Be(device.Abbreviation);
        result.SerialNumber.Should().Be(device.SerialNumber);
        result.Status.Should().Be(device.Status);
        result.Producer.Should().Be(device.Producer);
        result.IsArchived.Should().Be(device.IsArchived);
    }
}

public class DeviceDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var device = TestDeviceBuilder.Create().Build();
        var deviceCategories = new List<DeviceCategory> { TestDeviceCategoryBuilder.Create().Build() };
        var appointments = new List<Appointment> { TestAppointmentBuilder.Create().Build() };
        var appointmentProtocols = new List<AppointmentProtocol> { TestAppointmentProtocolBuilder.Create().Build() };
        var results = new List<Result> { TestResultBuilder.Create().Build() };
        
        // Act
        var result = new DeviceDetailedOutputModel(
            device,
            deviceCategories,
            appointments,
            appointmentProtocols,
            results);
        
        // Assert
        result.Id.Should().Be(device.Id);
        result.Name.Should().Be(device.Name);
        result.Abbreviation.Should().Be(device.Abbreviation);
        result.SerialNumber.Should().Be(device.SerialNumber);
        result.Status.Should().Be(device.Status);
        result.Producer.Should().Be(device.Producer);
        result.IsArchived.Should().Be(device.IsArchived);
        result.DeviceCategories.Should().BeEquivalentTo(deviceCategories.Select(deviceCategory => new DeviceCategoryOverviewOutputModel(deviceCategory)));
        result.Appointments.Should().BeEquivalentTo(appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)));
        result.AppointmentProtocols.Should().BeEquivalentTo(appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel(appointmentProtocol)));
        result.Results.Should().BeEquivalentTo(results.Select(resultValue => new ResultOverviewOutputModel(resultValue)));
    }
}