using System.Globalization;
using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Entities.AppointmentEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Builders.DeviceBuilders;
using TestUtils.Builders.PatientBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.AppointmentEntities;

public class ResultTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceTitle_ThrowsValidationException(string? title)
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithTitle(title)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Title),
            ErrorCode = ErrorCode.MISSING_VALUE
        });  
    }

    [Fact]
    public void Constructor_WithTitleExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var title = new string('*', Lengths.ResultTitle + 1);
        
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithTitle(title)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Title),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }

    [Fact]
    public void Constructor_WithFutureDateOfCreation_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithDateOfCreation(currentDateTime.AddDays(1))
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.DateOfCreation),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_PAST
        }); 
    }

    [Fact]
    public void Constructor_WithNullAppendix_ThrowsValidationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithAppendix(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Appendix),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithEmptyAppendix_ThrowsValidationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithAppendix([])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Appendix),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }
    
    [Fact]
    public void Constructor_WithAppendixExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var appendix = new byte[Lengths.Appendix + 1];
        
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithAppendix(appendix)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Appendix),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Fact]
    public void Constructor_WithUndefinedAppendixContentType_ThrowsValidationException()
    {
        // Arrange
        byte[] appendix = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05];
        
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithAppendix(appendix)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Appendix),
            ErrorCode = ErrorCode.UNSUPPORTED_FILE_TYPE
        }); 
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithEmptyOrWhitespaceRemarks_ThrowsValidationException(string? remarks)
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithRemarks(remarks)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Remarks),
            ErrorCode = ErrorCode.EMPTY_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithRemarksExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var remarks = new string('*', Lengths.ResultRemarks + 1);
        
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithRemarks(remarks)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Remarks),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        }); 
    }
    
    [Fact]
    public void Constructor_WithNullPatient_ThrowsValidationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithPatient(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Patient),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithPatientOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithPatient(TestPatientBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Patient),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        }); 
    }

    [Fact]
    public void Constructor_WithDeletedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var resultBuilder = TestResultBuilder.Create();
        var patient = TestPatientBuilder.Create(resultBuilder.Clinic).Build();
        patient.Archive([], []);
        patient.Delete([], [], []);
        
        // Act
        var act = () => resultBuilder
            .WithPatient(patient)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Patient),
            ErrorCode = ErrorCode.DELETED_ENTITY
        }); 
    }
    
    [Fact]
    public void Constructor_WithArchivedPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var resultBuilder = TestResultBuilder.Create();
        var patient = TestPatientBuilder.Create(resultBuilder.Clinic).Build();
        patient.Archive([], []);
        
        // Act
        var act = () => resultBuilder
            .WithPatient(patient)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Patient),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        }); 
    }
    
    [Fact]
    public void Constructor_WithNullClinician_ThrowsValidationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithClinician(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Clinician),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithClinicianOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithClinician(TestClinicianBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Clinician),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        }); 
    }

    [Fact]
    public void Constructor_WithDeletedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var resultBuilder = TestResultBuilder.Create();
        var clinician = TestClinicianBuilder.Create(resultBuilder.Clinic).Build();
        clinician.Archive([], []);
        clinician.Delete(null, [], [], []);
        
        // Act
        var act = () => resultBuilder
            .WithClinician(clinician)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Clinician),
            ErrorCode = ErrorCode.DELETED_ENTITY
        }); 
    }
    
    [Fact]
    public void Constructor_WithArchivedClinician_ThrowsInvalidOperationException()
    {
        // Arrange
        var resultBuilder = TestResultBuilder.Create();
        var clinician = TestClinicianBuilder.Create(resultBuilder.Clinic).Build();
        clinician.Archive([], []);
        
        // Act
        var act = () => resultBuilder
            .WithClinician(clinician)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Clinician),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        }); 
    }
    
    [Fact]
    public void Constructor_WithDeviceOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestResultBuilder
            .Create()
            .WithDevice(TestDeviceBuilder.Create().Build())
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Device),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        }); 
    }

    [Fact]
    public void Constructor_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var resultBuilder = TestResultBuilder.Create();
        var device = TestDeviceBuilder.Create(resultBuilder.Clinic).Build();
        device.Archive([], []);
        device.Delete([], [], []);
        
        // Act
        var act = () => resultBuilder
            .WithDevice(device)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Device),
            ErrorCode = ErrorCode.DELETED_ENTITY
        }); 
    }
    
    [Fact]
    public void Constructor_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var resultBuilder = TestResultBuilder.Create();
        var device = TestDeviceBuilder.Create(resultBuilder.Clinic).Build();
        device.Archive([], []);
        
        // Act
        var act = () => resultBuilder
            .WithDevice(device)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Result.Device),
            ErrorCode = ErrorCode.ARCHIVED_ENTITY
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.ValidAppendixContentTypes), MemberType = typeof(TestConstants))]
    public void Constructor_WithValidArguments_SetsAllProperties(AppendixContentType appendixContentType, byte[] bytes)
    {
        // Arrange
        var resultBuilder = TestResultBuilder.Create();
        
        // Act
        var sut = resultBuilder
            .WithAppendix(bytes)
            .Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(resultBuilder.Clinic);
        sut.ClinicId.Should().Be(resultBuilder.Clinic!.Id);
        sut.Title.Should().Be(resultBuilder.Title);
        sut.DateOfCreation.Should().Be(resultBuilder.DateOfCreation.Date);
        sut.Appendix.Should().BeEquivalentTo(resultBuilder.Appendix);
        sut.AppendixContentType.Should().Be(appendixContentType);
        sut.IsDeleted.Should().BeFalse();
        sut.Remarks.Should().Be(resultBuilder.Remarks);
        sut.Patient.Should().Be(resultBuilder.Patient);
        sut.PatientId.Should().Be(resultBuilder.Patient!.Id);
        sut.Clinician.Should().Be(resultBuilder.Clinician);
        sut.ClinicianId.Should().Be(resultBuilder.Clinician!.Id);
        sut.Device.Should().Be(resultBuilder.Device);
        sut.DeviceId.Should().Be(resultBuilder.Device!.Id);
    }
}