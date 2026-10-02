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
    
    
    /* - - - Method: Update - - - */
    [Fact]
    public void Update_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        sut.Archive([], []);
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Update_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        sut.Archive([], []);
        sut.Delete([], [], []);
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });   
    }

    [Fact]
    public void Update_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.DeviceName + 1);
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
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
    public void Update_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Update_WithNullDeviceCategoriesCollection_ThrowsValidationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act  = deviceBuilder
            .AsUpdate()
            .WithDeviceCategories(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithNullDeviceCategoryElement_ThrowsValidationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .WithDeviceCategories([null!])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithDuplicateDeviceCategories_ThrowsValidationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder .Create(deviceBuilder.Clinic).Build();
        var sut = deviceBuilder.Build();
        
        // Act
        var act  = deviceBuilder
            .AsUpdate()
            .WithDeviceCategories([deviceCategory,  deviceCategory])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }
    
    [Fact]
    public void Update_WithDeviceCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .WithDeviceCategories([TestDeviceCategoryBuilder.Create().Build()])
            .Apply(sut);
        
        // Assert
       var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedDeviceCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        var sut = deviceBuilder.Build();
        deviceCategory.Delete([]);
        
        // Act
        var act  = deviceBuilder
            .AsUpdate()
            .WithDeviceCategories([deviceCategory])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void Update_WithFutureDateOfLastMaintenance_ThrowsValidationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = deviceBuilder
            .AsUpdate()
            .WithDateOfLastMaintenance(currentDateTime.AddDays(1))
            .Apply(sut);

        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DateOfLastMaintenance),
            ErrorCode = ErrorCode.DATETIME_NOT_IN_PAST
        });
    }
    
    [Fact]
    public void Update_WithValidArguments_UpdatesAllProperties()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        deviceBuilder.AsUpdate().Apply(sut)();
        
        // Assert
        sut.Name.Should().Be(deviceBuilder.Name);
        sut.Abbreviation.Should().Be(deviceBuilder.Abbreviation);
        sut.DeviceCategories.Should().Equal(deviceBuilder.DeviceCategories);
        sut.DateOfLastMaintenance.Should().Be(deviceBuilder.DateOfLastMaintenance!.Value.Date);
    }
    
    
    /* - - - Method: AddDeviceCategory - - - */
    [Fact]
    public void AddDeviceCategory_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestDeviceBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.AddDeviceCategory(TestDeviceCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddDeviceCategory_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestDeviceBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete([], [], []);
        
        // Act
        var act = () => sut.AddDeviceCategory(TestDeviceCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddDeviceCategory_WithNull_ThrowsValidationException()
    {
        // Arrange
        var sut = TestDeviceBuilder.Create().Build();
        
        // Act
        var act = () => sut.AddDeviceCategory(null!);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void AddDeviceCategory_WithDuplicateDeviceCategories_ThrowsValidationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        var sut = deviceBuilder
            .WithDeviceCategories([deviceCategory])
            .Build();
        
        // Act
        var act = () => sut.AddDeviceCategory(deviceCategory);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void AddDeviceCategory_WithDeviceCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceCategory = TestDeviceCategoryBuilder.Create().Build();
        var sut = TestDeviceBuilder.Create().Build();
        
        // Act
        var act = () => sut.AddDeviceCategory(deviceCategory);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void AddDeviceCategory_WithDeletedDeviceCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        var sut = deviceBuilder.Build();
        deviceCategory.Delete([]);
        
        // Act
        var act = () => sut.AddDeviceCategory(deviceCategory);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.DeviceCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void AddDeviceCategory_WithValidDeviceCategory_AddsDeviceCategory()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        var sut = deviceBuilder.Build();
        
        // Act
        sut.AddDeviceCategory(deviceCategory);
        
        // Assert
        sut.DeviceCategories.Should().Contain(deviceCategory);
    }
    
    
    /* - - - Method: RemoveDeviceCategory - - - */
    [Fact]
    public void RemoveDeviceCategory_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestDeviceBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.RemoveDeviceCategory(TestDeviceCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveDeviceCategory_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestDeviceBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete([], [], []);
        
        // Act
        var act = () => sut.RemoveDeviceCategory(TestDeviceCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void RemoveDeviceCategory_WithNull_DoesNotChangeDeviceCategories()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        sut.RemoveDeviceCategory(null!);
        
        // Assert
        sut.DeviceCategories.Should().BeEquivalentTo(deviceBuilder.DeviceCategories);
    }
    
    [Fact]
    public void RemoveDeviceCategory_WithNonExistingDeviceCategory_DoesNotChangeDeviceCategories()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        var sut = deviceBuilder.Build();
        
        // Act
        sut.RemoveDeviceCategory(deviceCategory);
        
        // Assert
        sut.DeviceCategories.Should().BeEquivalentTo(deviceBuilder.DeviceCategories);
    }
    
    [Fact]
    public void RemoveDeviceCategory_WithExistingDeviceCategory_RemovesDeviceCategory()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var deviceCategory = TestDeviceCategoryBuilder.Create(deviceBuilder.Clinic).Build();
        var sut = deviceBuilder.Build();
        sut.AddDeviceCategory(deviceCategory);
        
        // Act
        sut.RemoveDeviceCategory(deviceCategory);
        
        // Assert
        sut.DeviceCategories.Should().NotContain(deviceCategory);
    }
    
    
    /* - - - Method: ChangeStatus - - - */
    [Fact]
    public void ChangeStatus_WithArchivedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.ChangeStatus(DeviceStatus.InMaintenance, [], deviceBuilder.CreationDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ChangeStatus_WithDeletedDevice_ThrowsInvalidOperationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        sut.Archive([], []);
        sut.Delete([], [], []);
        
        // Act
        var act = () => sut.ChangeStatus(DeviceStatus.InMaintenance, [], deviceBuilder.CreationDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void ChangeStatus_WithUndefinedStatus_ThrowsValidationException()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        var act = () => sut.ChangeStatus((DeviceStatus)999, [],  deviceBuilder.CreationDateTime);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Device.Status),
            ErrorCode = ErrorCode.INVALID_ENUM_VALUE
        });
    }
    
    [Fact]
    public void ChangeStatus_WithNonOperationalStatusAndFutureAppointments_ThrowsInvalidOperationException()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointment = TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddHours(1))
            .Build();
        var sut = TestDeviceBuilder.Create().Build();
        
        // Act
        var act = () => sut.ChangeStatus(DeviceStatus.InMaintenance, [appointment], currentDateTime);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
        
    [Fact]
    public void ChangeStatus_WithNonOperationalStatusAndPastAppointments_ChangesStatus()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointment = TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddHours(1))
            .Build();
        var sut = TestDeviceBuilder.Create().Build();
        
        // Act
        sut.ChangeStatus(DeviceStatus.InMaintenance, [appointment], currentDateTime.AddHours(1));
        
        // Assert
        sut.Status.Should().Be(DeviceStatus.InMaintenance);
    }
    
    [Fact]
    public void ChangeStatus_WithNonOperationalStatusAndNoAppointments_ChangesStatus()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        sut.ChangeStatus(DeviceStatus.InMaintenance, [],  deviceBuilder.CreationDateTime);
        
        // Assert
        sut.Status.Should().Be(DeviceStatus.InMaintenance);
    }
    
    [Fact]
    public void ChangeStatus_WithOperationalStatusAndFutureAppointments_ChangesStatus()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointment = TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddHours(1))
            .Build();
        var sut = TestDeviceBuilder.Create().Build();
        
        // Act
        sut.ChangeStatus(DeviceStatus.Operational, [appointment], currentDateTime);
        
        // Assert
        sut.Status.Should().Be(DeviceStatus.Operational);
    }
    
    [Fact]
    public void ChangeStatus_WithOperationalStatusAndPastAppointments_ChangesStatus()
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var appointment = TestAppointmentBuilder
            .Create()
            .WithStartTime(currentDateTime)
            .WithEndTime(currentDateTime.AddHours(1))
            .Build();
        var sut = TestDeviceBuilder.Create().Build();
        
        // Act
        sut.ChangeStatus(DeviceStatus.Operational, [appointment], currentDateTime.AddHours(1));
        
        // Assert
        sut.Status.Should().Be(DeviceStatus.Operational);
    }
    
    [Fact]
    public void ChangeStatus_WithOperationalStatusAndNoAppointments_ChangesStatus()
    {
        // Arrange
        var deviceBuilder = TestDeviceBuilder.Create();
        var sut = deviceBuilder.Build();
        
        // Act
        sut.ChangeStatus(DeviceStatus.Operational, [],  deviceBuilder.CreationDateTime);
        
        // Assert
        sut.Status.Should().Be(DeviceStatus.Operational);
    }
}