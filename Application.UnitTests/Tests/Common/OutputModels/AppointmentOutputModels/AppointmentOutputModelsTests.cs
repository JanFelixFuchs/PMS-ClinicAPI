using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Common.OutputModels.RoomOutputModels;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.AppointmentOutputModels;

public class AppointmentOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointment = TestAppointmentBuilder.Create().Build();
        
        // Act
        var result = new AppointmentOverviewOutputModel(appointment);
        
        // Assert
        result.Id.Should().Be(appointment.Id);
        result.Title.Should().Be(appointment.Title);
        result.StartTime.Should().Be(appointment.StartTime);
        result.EndTime.Should().Be(appointment.EndTime);
        result.Status.Should().Be(appointment.Status);
    }
}

public class AppointmentDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointment = TestAppointmentBuilder.Create().Build();
        var appointmentProtocol = TestAppointmentProtocolBuilder.Create().Build();
        
        // Act
        var result = new AppointmentDetailedOutputModel(
            appointment,
            appointment.AppointmentCategories,
            appointment.Patient,
            appointment.Room,
            appointment.Devices,
            appointment.Clinicians,
            appointmentProtocol);
        
        // Assert
        result.Id.Should().Be(appointment.Id);
        result.Title.Should().Be(appointment.Title);
        result.StartTime.Should().Be(appointment.StartTime);
        result.EndTime.Should().Be(appointment.EndTime);
        result.Status.Should().Be(appointment.Status);
        result.AppointmentCategories.Should().BeEquivalentTo(appointment.AppointmentCategories.Select(appointmentCategory => new AppointmentCategoryOverviewOutputModel(appointmentCategory)));
        result.Patient.Should().BeEquivalentTo(new PatientOverviewOutputModel(appointment.Patient));
        result.Room.Should().BeEquivalentTo(new RoomOverviewOutputModel(appointment.Room));
        result.Devices.Should().BeEquivalentTo(appointment.Devices.Select(device => new DeviceOverviewOutputModel(device)));
        result.Clinicians.Should().BeEquivalentTo(appointment.Clinicians.Select(clinician => new ClinicianOverviewOutputModel(clinician)));
        result.AppointmentProtocol.Should().BeEquivalentTo(new AppointmentProtocolOverviewOutputModel(appointmentProtocol));
    }
}