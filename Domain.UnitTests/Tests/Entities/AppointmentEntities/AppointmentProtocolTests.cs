using System.Globalization;
using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Entities.AppointmentEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Builders.DeviceBuilders;
using TestUtils.Builders.RoomBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;


namespace Domain.Tests.Tests.Entities.AppointmentEntities;

public class AppointmentProtocolTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentProtocolBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithNullAppointment_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentProtocolBuilder
            .Create()
            .WithAppointment(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Appointment),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithAppointmentOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestAppointmentProtocolBuilder
            .Create()
            .WithAppointment(TestAppointmentBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Appointment),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }

    [Fact]
    public void Constructor_WithDeletedAppointment_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var appointment = TestAppointmentBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        appointment.Delete();
        
        // Act
        var act = () => appointmentProtocolBuilder
            .WithAppointment(appointment)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Appointment),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithNonAttendedAppointment_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var appointment = TestAppointmentBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        
        // Act
        var act = () => appointmentProtocolBuilder
            .WithAppointment(appointment)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Appointment),
            ErrorCode = ErrorCode.UNEXPECTED_STATUS
        });
    }
    
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        
        // Act
        var sut = appointmentProtocolBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(appointmentProtocolBuilder.Clinic);
        sut.ClinicId.Should().Be(appointmentProtocolBuilder.Clinic!.Id);
        sut.DateOfProcessingStart.Should().BeNull();
        sut.DateOfProcessingCompletion.Should().BeNull();
        sut.Symptoms.Should().BeNull();
        sut.Diagnosis.Should().BeNull();
        sut.Treatment.Should().BeNull();
        sut.Remarks.Should().BeNull();
        sut.Status.Should().Be(AppointmentProtocolStatus.Undealt);
        sut.Patient.Should().Be(appointmentProtocolBuilder.Appointment!.Patient);
        sut.PatientId.Should().Be(appointmentProtocolBuilder.Appointment!.PatientId);
        sut.Clinician.Should().Be(appointmentProtocolBuilder.Appointment!.Clinicians.First());
        sut.ClinicianId.Should().Be(appointmentProtocolBuilder.Appointment!.Clinicians.First().Id);
        sut.Room.Should().Be(appointmentProtocolBuilder.Appointment!.Room);
        sut.RoomId.Should().Be(appointmentProtocolBuilder.Appointment!.RoomId);
        sut.Devices.Should().Equal(appointmentProtocolBuilder.Appointment!.Devices);
    }
}