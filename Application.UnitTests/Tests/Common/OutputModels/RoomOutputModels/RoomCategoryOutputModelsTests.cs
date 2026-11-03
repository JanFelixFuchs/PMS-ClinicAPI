using Application.Common.OutputModels.RoomOutputModels;
using Domain.Entities.RoomEntities;
using FluentAssertions;
using TestUtils.Builders.RoomBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.RoomOutputModels;

public class RoomCategoryOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var roomCategory = TestRoomCategoryBuilder.Create().Build();
        
        // Act
        var result = new RoomCategoryOverviewOutputModel(roomCategory);
        
        // Assert
        result.Id.Should().Be(roomCategory.Id);
        result.Name.Should().Be(roomCategory.Name);
        result.Abbreviation.Should().Be(roomCategory.Abbreviation);
        result.Color.Should().Be(roomCategory.Color);
    }
}

public class RoomCategoryDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var roomCategory = TestRoomCategoryBuilder.Create().Build();
        var rooms = new List<Room> { TestRoomBuilder.Create().Build() };
        
        // Act
        var result = new RoomCategoryDetailedOutputModel(
            roomCategory,
            rooms);
        
        // Assert
        result.Id.Should().Be(roomCategory.Id);
        result.Name.Should().Be(roomCategory.Name);
        result.Abbreviation.Should().Be(roomCategory.Abbreviation);
        result.Color.Should().Be(roomCategory.Color);
        result.Rooms.Should().BeEquivalentTo(rooms.Select(room => new RoomOverviewOutputModel(room)));
    }
}