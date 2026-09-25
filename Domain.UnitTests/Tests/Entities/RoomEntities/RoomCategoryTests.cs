using FluentAssertions;
using TestUtils.Builders.RoomBuilders;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.RoomEntities;

public class RoomCategoryTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var roomCategoryBuilder = TestRoomCategoryBuilder.Create();
        
        // Act
        var sut = roomCategoryBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(roomCategoryBuilder.Clinic);
        sut.ClinicId.Should().Be(roomCategoryBuilder.Clinic!.Id);
        sut.Name.Should().Be(roomCategoryBuilder.Name);
        sut.Abbreviation.Should().Be(roomCategoryBuilder.Abbreviation);
        sut.Color.Should().Be(roomCategoryBuilder.Color);
        sut.IsDeleted.Should().BeFalse();
        sut.Rooms.Should().BeEmpty();
    }
    
    
    /* - - - Method: Delete - - - */
    [Fact]
    public void Delete_WithExistingRooms_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestRoomCategoryBuilder.Create().Build();
        
        // Act
        var act = () => sut.Delete([TestRoomBuilder.Create().Build()]);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}