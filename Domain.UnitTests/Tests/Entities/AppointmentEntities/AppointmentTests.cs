using System.Globalization;
using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Entities.AppointmentEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Builders.DeviceBuilders;
using TestUtils.Builders.PatientBuilders;
using TestUtils.Builders.RoomBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.AppointmentEntities;

public class AppointmentTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceTitle_ThrowsValidationException(string? title)
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithTitle(title)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Title),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithTitleExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var title = new string('*', Lengths.AppointmentTitle + 1);
        
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithTitle(title)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Title),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }

    [Fact]
    public void Constructor_WithStartTimeAndEndTimeOnDifferentDates_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddDays(1))
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.StartTime),
            ErrorCode = ErrorCode.DATETIME_OUT_OF_RANGE
        });
    }

    [Fact]
    public void Constructor_WithPastStartTime_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime.AddMinutes(-15))
            .WithEndTime(currentDateTime)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.StartTime),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_FUTURE
        });
    }
    
    [Fact]
    public void Constructor_WithPastEndTime_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddMinutes(-15))
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.EndTime),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_FUTURE
        });
    }

    [Fact]
    public void Constructor_WithStartTimeAfterEndTime_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime.AddMinutes(30))
            .WithEndTime(currentDateTime.AddMinutes(15))
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.StartTime),
            ErrorCode = ErrorCode.DATETIME_OUT_OF_RANGE
        });
    }
    
    [Fact]
    public void Constructor_WithNullAppointmentCategoriesCollection_ThrowsValidationException()
    {
        // Act
        var act  = () => TestAppointmentBuilder
            .Create()
            .WithAppointmentCategories(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNullAppointmentCategoryElement_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithAppointmentCategories([null!])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithDuplicateAppointmentCategories_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var appointmentCategory = TestAppointmentCategoryBuilder.Create(appointmentBuilder.Clinic).Build();
        
        // Act
        var act  = () => appointmentBuilder
            .WithAppointmentCategories([appointmentCategory,  appointmentCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void Constructor_WithAppointmentCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithAppointmentCategories([TestAppointmentCategoryBuilder.Create().Build()])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Constructor_WithDeletedAppointmentCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var appointmentCategory = TestAppointmentCategoryBuilder.Create(appointmentBuilder.Clinic).Build();
        appointmentCategory.Delete([]);
        
        // Act
        var act  = () => appointmentBuilder
            .WithAppointmentCategories([appointmentCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithNullPatient_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithPatient(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithPatientOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithPatient(TestPatientBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }

    [Fact]
    public void Constructor_WithDeletedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var patient = TestPatientBuilder.Create(appointmentBuilder.Clinic).Build();
        patient.Archive([], []);
        patient.Delete([], [], []);
        
        // Act
        var act = () => appointmentBuilder
            .WithPatient(patient)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithArchivedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var patient = TestPatientBuilder.Create(appointmentBuilder.Clinic).Build();
        patient.Archive([], []);
        
        // Act
        var act = () => appointmentBuilder
            .WithPatient(patient)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithNullRoom_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithRoom(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithRoomOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithRoom(TestRoomBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }

    [Fact]
    public void Constructor_WithDeletedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var room = TestRoomBuilder.Create(appointmentBuilder.Clinic).Build();
        room.Archive([], []);
        room.Delete([], []);
        
        // Act
        var act = () => appointmentBuilder
            .WithRoom(room)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithArchivedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var room = TestRoomBuilder.Create(appointmentBuilder.Clinic).Build();
        room.Archive([], []);
        
        // Act
        var act = () => appointmentBuilder
            .WithRoom(room)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithNullDevicesCollection_ThrowsValidationException()
    {
        // Act
        var act  = () => TestAppointmentBuilder
            .Create()
            .WithDevices(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNullDeviceElement_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithDevices([null!])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithDuplicateDevices_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentBuilder.Clinic).Build();
        
        // Act
        var act  = () => appointmentBuilder
            .WithDevices([device,  device])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void Constructor_WithDeviceOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithDevices([TestDeviceBuilder.Create().Build()])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Constructor_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentBuilder.Clinic).Build();
        device.Archive([], []);
        device.Delete([], [], []);
        
        // Act
        var act  = () => appointmentBuilder
            .WithDevices([device])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentBuilder.Clinic).Build();
        device.Archive([], []);
        
        // Act
        var act  = () => appointmentBuilder
            .WithDevices([device])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithNonOperationalDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder
            .Create(appointmentBuilder.Clinic)
            .WithStatus(DeviceStatus.InMaintenance)
            .Build();
        
        // Act
        var act  = () => appointmentBuilder
            .WithDevices([device])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.UNEXPECTED_STATUS
        });
    }
    
    [Fact]
    public void Constructor_WithNullCliniciansCollection_ThrowsValidationException()
    {
        // Act
        var act  = () => TestAppointmentBuilder
            .Create()
            .WithClinicians(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithEmptyCliniciansCollection_ThrowsValidationException()
    {
        // Act
        var act  = () => TestAppointmentBuilder
            .Create()
            .WithClinicians([])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithNullClinicianElement_ThrowsValidationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithClinicians([null!])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithDuplicateClinicians_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentBuilder.Clinic).Build();
        
        // Act
        var act  = () => appointmentBuilder
            .WithClinicians([clinician,  clinician])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void Constructor_WithClinicianOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestAppointmentBuilder
            .Create()
            .WithClinicians([TestClinicianBuilder.Create().Build()])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Constructor_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentBuilder.Clinic).Build();
        clinician.Archive([], []);
        clinician.Delete(null, [], [], []);
        
        // Act
        var act  = () => appointmentBuilder
            .WithClinicians([clinician])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentBuilder.Clinic).Build();
        clinician.Archive([], []);
        
        // Act
        var act  = () => appointmentBuilder
            .WithClinicians([clinician])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        
        // Act
        var sut = appointmentBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(appointmentBuilder.Clinic);
        sut.ClinicId.Should().Be(appointmentBuilder.Clinic!.Id);
        sut.Title.Should().Be(appointmentBuilder.Title);
        sut.StartTime.Should().Be(appointmentBuilder.StartTime);
        sut.EndTime.Should().Be(appointmentBuilder.EndTime);
        sut.Status.Should().Be(AppointmentStatus.Planned);
        sut.IsDeleted.Should().BeFalse();
        sut.AppointmentCategories.Should().Equal(appointmentBuilder.AppointmentCategories);
        sut.Patient.Should().Be(appointmentBuilder.Patient);
        sut.PatientId.Should().Be(appointmentBuilder.Patient!.Id);
        sut.Room.Should().Be(appointmentBuilder.Room);
        sut.RoomId.Should().Be(appointmentBuilder.Room!.Id);
        sut.Clinicians.Should().Equal(appointmentBuilder.Clinicians);
        sut.Devices.Should().Equal(appointmentBuilder.Devices);
    }
    
    
    /* - - - Update - - - */
    [Fact]
    public void Update_WithDeletedAppointment_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        sut.Delete();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Update_WithAttendedAppointment_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create(currentDateTime: currentDateTime);
        var sut = appointmentBuilder.Build();
        sut.MarkAsAttended(currentDateTime);
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceTitle_ThrowsValidationException(string? title)
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithTitle(title)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Title),
            ErrorCode = ErrorCode.MISSING_VALUE
        });   
    }

    [Fact]
    public void Update_WithTitleExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var title = new string('*', Lengths.AppointmentTitle + 1);
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithTitle(title)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Title),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Fact]
    public void Update_WithStartTimeAndEndTimeOnDifferentDates_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate(currentDateTime)
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddDays(1))
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.StartTime),
            ErrorCode = ErrorCode.DATETIME_OUT_OF_RANGE
        });
    }
    
    [Fact]
    public void Update_WithPastStartTime_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate(currentDateTime)
            .WithStartTime(currentDateTime.AddMinutes(-15))
            .WithEndTime(currentDateTime)
            .Apply(sut);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.StartTime),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_FUTURE
        });
    }

    [Fact]
    public void Update_WithPastEndTime_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate(currentDateTime)
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddMinutes(-15))
            .Apply(sut);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.EndTime),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_FUTURE
        });
    }

    [Fact]
    public void Update_WithStartTimeAfterEndTime_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate(currentDateTime)
            .WithStartTime(currentDateTime.AddMinutes(30))
            .WithEndTime(currentDateTime.AddMinutes(15))
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.StartTime),
            ErrorCode = ErrorCode.DATETIME_OUT_OF_RANGE
        });
    }
    
    [Fact]
    public void Update_WithNullAppointmentCategoriesCollection_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithAppointmentCategories(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithNullAppointmentCategoryElement_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithAppointmentCategories([null!])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithDuplicateAppointmentCategories_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var appointmentCategory = TestAppointmentCategoryBuilder .Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithAppointmentCategories([appointmentCategory,  appointmentCategory])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }
    
    [Fact]
    public void Update_WithAppointmentCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithAppointmentCategories([TestAppointmentCategoryBuilder.Create().Build()])
            .Apply(sut);
        
        // Assert
       var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedAppointmentCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var appointmentCategory = TestAppointmentCategoryBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        appointmentCategory.Delete([]);
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithAppointmentCategories([appointmentCategory])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.AppointmentCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void Update_WithNullPatient_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithPatient(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithPatientOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithPatient(TestPatientBuilder.Create().Build())
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var patient = TestPatientBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        patient.Archive([], []);
        patient.Delete([], [], []);
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithPatient(patient)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void Update_WithArchivedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var patient = TestPatientBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        patient.Archive([], []);
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithPatient(patient)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Patient),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithNullRoom_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithRoom(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithRoomOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithRoom(TestRoomBuilder.Create().Build())
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var room = TestRoomBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        room.Archive([], []);
        room.Delete([], []);
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithRoom(room)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void Update_WithArchivedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var room = TestRoomBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        room.Archive([], []);
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithRoom(room)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Room),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithNullDevicesCollection_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithDevices(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithNullDeviceElement_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithDevices([null!])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithDuplicateDevices_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithDevices([device,  device])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }
    
    [Fact]
    public void Update_WithDeviceOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithDevices([TestDeviceBuilder.Create().Build()])
            .Apply(sut);
        
        // Assert
       var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        device.Archive([], []);
        device.Delete([], [], []);
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithDevices([device])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        device.Archive([], []);
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithDevices([device])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithNonOperationalDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var device = TestDeviceBuilder
            .Create(appointmentBuilder.Clinic)
            .WithStatus(DeviceStatus.InMaintenance)
            .Build();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithDevices([device])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Devices),
            ErrorCode = ErrorCode.UNEXPECTED_STATUS
        });
    }
    
    [Fact]
    public void Update_WithNullCliniciansCollection_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithClinicians(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithEmptyCliniciansCollection_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithClinicians([])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithNullClinicianElement_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithClinicians([null!])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithDuplicateClinicians_ThrowsValidationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithClinicians([clinician,  clinician])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }
    
    [Fact]
    public void Update_WithClinicianOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        var act = appointmentBuilder
            .AsUpdate()
            .WithClinicians([TestClinicianBuilder.Create().Build()])
            .Apply(sut);
        
        // Assert
       var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        clinician.Archive([], []);
        clinician.Delete(null, [], [], []);
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithClinicians([clinician])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var clinician = TestClinicianBuilder.Create(appointmentBuilder.Clinic).Build();
        var sut = appointmentBuilder.Build();
        clinician.Archive([], []);
        
        // Act
        var act  = appointmentBuilder
            .AsUpdate()
            .WithClinicians([clinician])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Appointment.Clinicians),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithValidArguments_UpdatesAllProperties()
    {
        // Arrange
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        
        // Act
        appointmentBuilder.AsUpdate().Apply(sut)();
        
        // Assert
        sut.Title.Should().Be(appointmentBuilder.Title);
        sut.StartTime.Should().Be(appointmentBuilder.StartTime);
        sut.EndTime.Should().Be(appointmentBuilder.EndTime);
        sut.AppointmentCategories.Should().Equal(appointmentBuilder.AppointmentCategories);
        sut.Patient.Should().Be(appointmentBuilder.Patient);
        sut.PatientId.Should().Be(appointmentBuilder.Patient!.Id);
        sut.Room.Should().Be(appointmentBuilder.Room);
        sut.RoomId.Should().Be(appointmentBuilder.Room!.Id);
        sut.Clinicians.Should().Equal(appointmentBuilder.Clinicians);
        sut.Devices.Should().Equal(appointmentBuilder.Devices);
    }
    
    
    /* - - - MarkAsAttended - - - */
    [Fact]
    public void MarkAsAttended_WithDeletedAppointment_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder.Build();
        sut.Delete();
        
        // Act
        var act = () => sut.MarkAsAttended(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void MarkAsAttended_WithAttendedAppointment_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create(currentDateTime: currentDateTime.AddMinutes(-15));
        var sut = appointmentBuilder.Build();
        sut.MarkAsAttended(currentDateTime);
        
        // Act
        var act = () => sut.MarkAsAttended(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void MarkAsAttended_WithFutureStartTime_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create();
        var sut = appointmentBuilder
            .WithStartTime(currentDateTime.AddMinutes(15))
            .WithEndTime(currentDateTime.AddMinutes(30))
            .Build();
        
        // Act
        var act = () => sut.MarkAsAttended(currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkAsAttended_WithStartedAppointment_MarksAppointmentAsAttended()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointmentBuilder = TestAppointmentBuilder.Create(currentDateTime: currentDateTime);
        var sut = appointmentBuilder.Build();
        
        // Act
        sut.MarkAsAttended(currentDateTime);
        
        // Assert
        sut.Status.Should().Be(AppointmentStatus.Attended);
    }
}