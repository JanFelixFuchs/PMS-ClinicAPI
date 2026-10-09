using Domain.Common.Utils.Constants;
using Domain.Entities.RoomEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.RoomBuilders;
using TestUtils.Constants;
using TestUtils.Helper;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.RoomEntities;

public class RoomTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.RoomName + 1);
        
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }

    [Fact]
    public void Constructor_WithNullRoomCategoriesCollection_ThrowsValidationException()
    {
        // Act
        var act  = () => TestRoomBuilder
            .Create()
            .WithRoomCategories(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNullRoomCategoryElement_ThrowsValidationException()
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithRoomCategories([null!])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Constructor_WithDuplicateRoomCategories_ThrowsValidationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        
        // Act
        var act  = () => roomBuilder
            .WithRoomCategories([roomCategory,  roomCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void Constructor_WithRoomCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithRoomCategories([TestRoomCategoryBuilder.Create().Build()])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Constructor_WithDeletedRoomCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        roomCategory.Delete([]);
        
        // Act
        var act  = () => roomBuilder
            .WithRoomCategories([roomCategory])
            .Build();
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithEmptyOrWhitespaceRoomNumber_ThrowsValidationException(string? roomNumber)
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithRoomNumber(roomNumber)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomNumber),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Constructor_WithRoomNumberExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var roomNumber = new string('*', Lengths.RoomNumber + 1);
        
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithRoomNumber(roomNumber)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomNumber),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithEmptyOrWhitespaceFloor_ThrowsValidationException(string? floor)
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithFloor(floor)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Floor),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Constructor_WithFloorExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var floor = new string('*', Lengths.Floor + 1);
        
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithFloor(floor)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Floor),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithEmptyOrWhitespaceBuilding_ThrowsValidationException(string? building)
    {
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithBuilding(building)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Building),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Constructor_WithBuildingExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var building = new string('*', Lengths.Building + 1);
        
        // Act
        var act = () => TestRoomBuilder
            .Create()
            .WithBuilding(building)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Building),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        
        // Act
        var sut = roomBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(roomBuilder.Clinic);
        sut.ClinicId.Should().Be(roomBuilder.Clinic!.Id);
        sut.Name.Should().Be(roomBuilder.Name);
        sut.Abbreviation.Should().Be(roomBuilder.Abbreviation);
        sut.IsArchived.Should().BeFalse();
        sut.IsDeleted.Should().BeFalse();
        sut.RoomCategories.Should().Equal(roomBuilder.RoomCategories);
        sut.RoomNumber.Should().Be(roomBuilder.RoomNumber);
        sut.Floor.Should().Be(roomBuilder.Floor);
        sut.Building.Should().Be(roomBuilder.Building);
    }
}