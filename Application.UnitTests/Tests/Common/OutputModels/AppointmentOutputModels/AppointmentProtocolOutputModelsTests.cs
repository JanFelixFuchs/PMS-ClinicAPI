using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Common.OutputModels.RoomOutputModels;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.AppointmentOutputModels;

public class AppointmentProtocolOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointmentProtocol = TestAppointmentProtocolBuilder.Create().Build();
        
        // Act
        var result = new AppointmentProtocolOverviewOutputModel(appointmentProtocol);
        
        // Assert
        result.Id.Should().Be(appointmentProtocol.Id);
        result.DateOfAppointment.Should().Be(appointmentProtocol.DateOfAppointment);
        result.Status.Should().Be(appointmentProtocol.Status);
    }
}

public class AppointmentProtocolDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointmentProtocol = TestAppointmentProtocolBuilder.Create().Build();

        // Act
        var result = new AppointmentProtocolDetailedOutputModel(
            appointmentProtocol,
            appointmentProtocol.Appointment,
            appointmentProtocol.Patient,
            appointmentProtocol.Clinician,
            appointmentProtocol.Room,
            appointmentProtocol.Devices);
        
        // Assert
        result.Id.Should().Be(appointmentProtocol.Id);
        result.DateOfAppointment.Should().Be(appointmentProtocol.DateOfAppointment);
        result.Status.Should().Be(appointmentProtocol.Status);
        result.DateOfProcessingStart.Should().Be(appointmentProtocol.DateOfProcessingStart);
        result.DateOfProcessingCompletion.Should().Be(appointmentProtocol.DateOfProcessingCompletion);
        result.Symptoms.Should().Be(appointmentProtocol.Symptoms);
        result.Diagnosis.Should().Be(appointmentProtocol.Diagnosis);
        result.Treatment.Should().Be(appointmentProtocol.Treatment);
        result.Remarks.Should().Be(appointmentProtocol.Remarks);
        result.Appointment.Should().BeEquivalentTo(new AppointmentOverviewOutputModel(appointmentProtocol.Appointment));
        result.Patient.Should().BeEquivalentTo(new PatientOverviewOutputModel(appointmentProtocol.Patient));
        result.Clinician.Should().BeEquivalentTo(new ClinicianOverviewOutputModel(appointmentProtocol.Clinician));
        result.Room.Should().BeEquivalentTo(new RoomOverviewOutputModel(appointmentProtocol.Room));
        result.Devices.Should().BeEquivalentTo(appointmentProtocol.Devices.Select(device => new DeviceOverviewOutputModel(device)));
    }
}