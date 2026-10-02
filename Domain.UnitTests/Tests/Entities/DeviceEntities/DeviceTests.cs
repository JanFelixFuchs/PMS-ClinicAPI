using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Entities.DeviceEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.DeviceBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.DeviceEntities;

public class DeviceTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        }); 
    }

    [Fact]
    public void Constructor_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.DeviceName + 1);
        
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceSerialNumber_ThrowsValidationException(string? serialNumber)
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithSerialNumber(serialNumber)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.SerialNumber),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithSerialNumberExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var serialNumber = new string('*', Lengths.SerialNumber + 1);
        
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithSerialNumber(serialNumber)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.SerialNumber),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Constructor_WithUndefinedStatus_ThrowsValidationException()
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithStatus((DeviceStatus)999)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Status),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceProducer_ThrowsValidationException(string? producer)
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithProducer(producer)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Producer),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithProducerExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var producer = new string('*', Lengths.Producer + 1);
        
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithProducer(producer)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Producer),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Constructor_WithNullDeviceCategoriesCollection_ThrowsValidationException()
    {
        // Act
        var act  = () => TestDeviceBuilder
            .Create()
            .WithDeviceCategories(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNullDeviceCategoryElement_ThrowsValidationException()
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithDeviceCategories([null!])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithDuplicateDeviceCategories_ThrowsValidationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        
        // Act
        var act  = () => deviceBuilder
            .WithDeviceCategories([deviceCategory,  deviceCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void Constructor_WithDeviceCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithDeviceCategories([TestDeviceCategoryBuilder.Create().Build()])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Constructor_WithDeletedDeviceCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        deviceCategory.Delete([]);
        
        // Act
        var act  = () => deviceBuilder
            .WithDeviceCategories([deviceCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }

    [Fact]
    public void Constructor_WithFutureDateOfPurchase_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithDateOfPurchase(currentDateTime.AddDays(1))
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DateOfPurchase),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_PAST
        });
    }
    
    [Fact]
    public void Constructor_WithFutureDateOfLastMaintenance_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        
        // Act
        var act = () => TestDeviceBuilder
            .Create()
            .WithDateOfLastMaintenance(currentDateTime.AddDays(1))
            .Build();

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DateOfLastMaintenance),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_PAST
        });
    }
    
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        
        // Act
        var sut = deviceBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(deviceBuilder.Clinic);
        sut.ClinicId.Should().Be(deviceBuilder.Clinic!.Id);
        sut.Name.Should().Be(deviceBuilder.Name);
        sut.Abbreviation.Should().Be(deviceBuilder.Abbreviation);
        sut.SerialNumber.Should().Be(deviceBuilder.SerialNumber);
        sut.Status.Should().Be(deviceBuilder.Status);
        sut.Producer.Should().Be(deviceBuilder.Producer);
        sut.IsArchived.Should().BeFalse();
        sut.IsDeleted.Should().BeFalse();
        sut.DeviceCategories.Should().Equal(deviceBuilder.DeviceCategories);
        sut.DateOfPurchase.Should().Be(deviceBuilder.DateOfPurchase!.Value.Date);
        sut.DateOfLastMaintenance.Should().Be(deviceBuilder.DateOfLastMaintenance!.Value.Date);
    }
}