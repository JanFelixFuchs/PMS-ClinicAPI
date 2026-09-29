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
    

    /* - - - Update - - - */
    [Fact]
    public void Update_WithUndealtAppointmentProtocol_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Update_WithCompletedAppointmentProtocol_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        sut.Complete(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithEmptyOrWhitespaceSymptoms_ThrowsValidationException(string? symptoms)
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithSymptoms(symptoms)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Symptoms),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Update_WithSymptomsExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var symptoms = new string('*', Lengths.Symptoms + 1);
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithSymptoms(symptoms)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Symptoms),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithEmptyOrWhitespaceDiagnosis_ThrowsValidationException(string? diagnosis)
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithDiagnosis(diagnosis)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Diagnosis),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Update_WithDiagnosisExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var diagnosis = new string('*', Lengths.Diagnosis + 1);
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithDiagnosis(diagnosis)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Diagnosis),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithEmptyOrWhitespaceTreatment_ThrowsValidationException(string? treatment)
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithTreatment(treatment)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Treatment),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Update_WithTreatmentExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var treatment = new string('*', Lengths.Treatment + 1);
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithTreatment(treatment)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Treatment),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithEmptyOrWhitespaceRemarks_ThrowsValidationException(string? remarks)
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithRemarks(remarks)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Remarks),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Update_WithRemarksExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var remarks = new string('*', Lengths.AppointmentProtocolRemarks + 1);
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithRemarks(remarks)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Remarks),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Update_WithNullClinician_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithClinician(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Clinician),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithClinicianOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithClinician(TestClinicianBuilder.Create().Build())
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Clinician),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        clinician.Archive([], []);
        clinician.Delete(null, [], [], []);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithClinician(clinician)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Clinician),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void Update_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        clinician.Archive([], []);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithClinician(clinician)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Clinician),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithNullRoom_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithRoom(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Room),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithRoomOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithRoom(TestRoomBuilder.Create().Build())
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Room),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var room = TestRoomBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        room.Archive([], []);
        room.Delete([], []);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithRoom(room)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Room),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void Update_WithArchivedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var room = TestRoomBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        room.Archive([], []);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithRoom(room)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Room),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithNullDevicesCollection_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act  = appointmentProtocolBuilder
            .AsUpdate()
            .WithDevices(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Devices),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithNullDeviceElement_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithDevices([null!])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Devices),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithDuplicateDevices_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act  = appointmentProtocolBuilder
            .AsUpdate()
            .WithDevices([device,  device])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Devices),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }
    
    [Fact]
    public void Update_WithDeviceOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = appointmentProtocolBuilder
            .AsUpdate()
            .WithDevices([TestDeviceBuilder.Create().Build()])
            .Apply(sut);
        
        // Assert
       var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Devices),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        device.Archive([], []);
        device.Delete([], [], []);
        
        // Act
        var act  = appointmentProtocolBuilder
            .AsUpdate()
            .WithDevices([device])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Devices),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentProtocolBuilder.Clinic).Build();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        device.Archive([], []);
        
        // Act
        var act  = appointmentProtocolBuilder
            .AsUpdate()
            .WithDevices([device])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(AppointmentProtocol.Devices),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithValidArguments_UpdatesAllProperties()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        appointmentProtocolBuilder.AsUpdate().Apply(sut)();
        
        // Assert
        sut.Symptoms.Should().Be(appointmentProtocolBuilder.Symptoms);
        sut.Diagnosis.Should().Be(appointmentProtocolBuilder.Diagnosis);
        sut.Treatment.Should().Be(appointmentProtocolBuilder.Treatment);
        sut.Remarks.Should().Be(appointmentProtocolBuilder.Remarks);
        sut.Clinician.Should().Be(appointmentProtocolBuilder.Clinician);
        sut.ClinicianId.Should().Be(appointmentProtocolBuilder.Clinician!.Id);
        sut.Room.Should().Be(appointmentProtocolBuilder.Room);
        sut.RoomId.Should().Be(appointmentProtocolBuilder.Room!.Id);
        sut.Devices.Should().Equal(appointmentProtocolBuilder.Devices);
    }
    
    
    /* - - - Start - - - */
    [Fact]
    public void Start_WithStartedAppointmentProtocol_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        var act = () => sut.Start(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Start_WithCompletedAppointmentProtocol_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        sut.Complete(currentDateTime);
        
        // Act
        var act = () => sut.Start(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Start_WithUndealtAppointmentProtocol_SetsDateOfProcessingStartAndStartsAppointmentProtocol()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        
        // Act
        sut.Start(currentDateTime);
        
        // Assert
        sut.DateOfProcessingStart.Should().Be(currentDateTime);
        sut.Status.Should().Be(AppointmentProtocolStatus.Started);
    }
    
    
    /* - - - Complete - - - */
    [Fact]
    public void Complete_WithUndealtAppointmentProtocol_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        
        // Act
        var act = () => sut.Complete(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Complete_WithCompletedAppointmentProtocol_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        sut.Complete(currentDateTime);
        
        // Act
        var act = () => sut.Complete(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Complete_WithStartedAppointmentProtocol_SetsDateOfProcessingCompletionAndCompletesAppointmentProtocol()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentProtocolBuilder = TestAppointmentProtocolBuilder.Create();
        var sut = appointmentProtocolBuilder.Build();
        sut.Start(currentDateTime);
        
        // Act
        sut.Complete(currentDateTime);
        
        // Assert
        sut.DateOfProcessingCompletion.Should().Be(currentDateTime);
        sut.Status.Should().Be(AppointmentProtocolStatus.Completed);
    }
}