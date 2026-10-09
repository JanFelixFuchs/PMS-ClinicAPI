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
    
    
    /* - - - Method: Update - - - */
    [Fact]
    public void Update_WithArchivedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        sut.Archive([], []);
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Update_WithDeletedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        sut.Archive([], []);
        sut.Delete([], []);
        
        // Act
        var act = roomBuilder
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
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.RoomName + 1);
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
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
    public void Update_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Update_WithNullRoomCategoriesCollection_ThrowsValidationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act  = roomBuilder
            .AsUpdate()
            .WithRoomCategories(null)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithNullRoomCategoryElement_ThrowsValidationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithRoomCategories([null!])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void Update_WithDuplicateRoomCategories_ThrowsValidationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        var sut = roomBuilder.Build();
        
        // Act
        var act  = roomBuilder
            .AsUpdate()
            .WithRoomCategories([roomCategory,  roomCategory])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }
    
    [Fact]
    public void Update_WithRoomCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithRoomCategories([TestRoomCategoryBuilder.Create().Build()])
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithDeletedRoomCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        var sut = roomBuilder.Build();
        roomCategory.Delete([]);
        
        // Act
        var act  = roomBuilder
            .AsUpdate()
            .WithRoomCategories([roomCategory])
            .Apply(sut);
        
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
    public void Update_WithEmptyOrWhitespaceRoomNumber_ThrowsValidationException(string? roomNumber)
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithRoomNumber(roomNumber)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomNumber),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Update_WithRoomNumberExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var roomNumber = new string('*', Lengths.RoomNumber + 1);
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithRoomNumber(roomNumber)
            .Apply(sut);
        
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
    public void Update_WithEmptyOrWhitespaceFloor_ThrowsValidationException(string? floor)
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithFloor(floor)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Floor),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Update_WithFloorExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var floor = new string('*', Lengths.Floor + 1);
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithFloor(floor)
            .Apply(sut);
        
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
    public void Update_WithEmptyOrWhitespaceBuilding_ThrowsValidationException(string? building)
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithBuilding(building)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Building),
            ErrorCode = ErrorCode.EMPTY_VALUE
        });
    }

    [Fact]
    public void Update_WithBuildingExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var building = new string('*', Lengths.Building + 1);
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        var act = roomBuilder
            .AsUpdate()
            .WithBuilding(building)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.Building),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Fact]
    public void Update_WithValidArguments_UpdatesAllProperties()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        roomBuilder.AsUpdate().Apply(sut)();
        
        // Assert
        sut.Name.Should().Be(roomBuilder.Name);
        sut.Abbreviation.Should().Be(roomBuilder.Abbreviation);
        sut.RoomCategories.Should().Equal(roomBuilder.RoomCategories);
        sut.RoomNumber.Should().Be(roomBuilder.RoomNumber);
        sut.Floor.Should().Be(roomBuilder.Floor);
        sut.Building.Should().Be(roomBuilder.Building);
    }

    
    /* - - - Method: AddRoomCategory - - - */
    [Fact]
    public void AddRoomCategory_WithArchivedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestRoomBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.AddRoomCategory(TestRoomCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddRoomCategory_WithDeletedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestRoomBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete([], []);
        
        // Act
        var act = () => sut.AddRoomCategory(TestRoomCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddRoomCategory_WithNull_ThrowsValidationException()
    {
        // Arrange
        var sut = TestRoomBuilder.Create().Build();
        
        // Act
        var act = () => sut.AddRoomCategory(null!);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }
    
    [Fact]
    public void AddRoomCategory_WithDuplicateRoomCategories_ThrowsValidationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        var sut = roomBuilder
            .WithRoomCategories([roomCategory])
            .Build();
        
        // Act
        var act = () => sut.AddRoomCategory(roomCategory);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.CONTAINS_DUPLICATE_ELEMENTS
        });
    }

    [Fact]
    public void AddRoomCategory_WithRoomCategoryOfDifferentClinic_ThrowsInvalidOperationException()
    {
        // Arrange
        var roomCategory = TestRoomCategoryBuilder.Create().Build();
        var sut = TestRoomBuilder.Create().Build();
        
        // Act
        var act = () => sut.AddRoomCategory(roomCategory);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.CLINIC_MISMATCH
        });
    }
    
    [Fact]
    public void AddRoomCategory_WithDeletedRoomCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        var sut = roomBuilder.Build();
        roomCategory.Delete([]);
        
        // Act
        var act = () => sut.AddRoomCategory(roomCategory);
        
        // Assert
        var exception = act.Should().Throw<InvalidOperationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(Room.RoomCategories),
            ErrorCode = ErrorCode.DELETED_ENTITY
        });
    }
    
    [Fact]
    public void AddRoomCategory_WithValidRoomCategory_AddsRoomCategory()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        var sut = roomBuilder.Build();
        
        // Act
        sut.AddRoomCategory(roomCategory);
        
        // Assert
        sut.RoomCategories.Should().Contain(roomCategory);
    }
    
    
    /* - - - Method: RemoveRoomCategory - - - */
    [Fact]
    public void RemoveRoomCategory_WithArchivedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestRoomBuilder.Create().Build();
        sut.Archive([], []);
        
        // Act
        var act = () => sut.RemoveRoomCategory(TestRoomCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveRoomCategory_WithDeletedRoom_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestRoomBuilder.Create().Build();
        sut.Archive([], []);
        sut.Delete([], []);
        
        // Act
        var act = () => sut.RemoveRoomCategory(TestRoomCategoryBuilder.Create().Build());
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void RemoveRoomCategory_WithNull_DoesNotChangeRoomCategories()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var sut = roomBuilder.Build();
        
        // Act
        sut.RemoveRoomCategory(null!);
        
        // Assert
        sut.RoomCategories.Should().BeEquivalentTo(roomBuilder.RoomCategories);
    }
    
    [Fact]
    public void RemoveRoomCategory_WithNonExistingRoomCategory_DoesNotChangeRoomCategories()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        var sut = roomBuilder.Build();
        
        // Act
        sut.RemoveRoomCategory(roomCategory);
        
        // Assert
        sut.RoomCategories.Should().BeEquivalentTo(roomBuilder.RoomCategories);
    }
    
    [Fact]
    public void RemoveRoomCategory_WithExistingRoomCategory_RemovesRoomCategory()
    {
        // Arrange
        var roomBuilder = TestRoomBuilder.Create();
        var roomCategory = TestRoomCategoryBuilder.Create(roomBuilder.Clinic).Build();
        var sut = roomBuilder.Build();
        sut.AddRoomCategory(roomCategory);
        
        // Act
        sut.RemoveRoomCategory(roomCategory);
        
        // Assert
        sut.RoomCategories.Should().NotContain(roomCategory);
    }
}