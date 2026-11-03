using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.OutputModels.RoomOutputModels;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.RoomEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.RoomBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.RoomOutputModels;

public class RoomOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var room = TestRoomBuilder.Create().Build();
        
        // Act
        var result = new RoomOverviewOutputModel(room);
        
        // Assert
        result.Id.Should().Be(room.Id);
        result.Name.Should().Be(room.Name);
        result.Abbreviation.Should().Be(room.Abbreviation);
        result.IsArchived.Should().Be(room.IsArchived);
        result.RoomNumber.Should().Be(room.RoomNumber);
        result.Floor.Should().Be(room.Floor);
        result.Building.Should().Be(room.Building);
    }
}

public class RoomDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var room = TestRoomBuilder.Create().Build();
        var roomCategories = new List<RoomCategory> { TestRoomCategoryBuilder.Create().Build() };
        var appointments = new List<Appointment> { TestAppointmentBuilder.Create().Build() };
        var appointmentProtocols = new List<AppointmentProtocol> { TestAppointmentProtocolBuilder.Create().Build() };
        
        // Act
        var result = new RoomDetailedOutputModel(
            room,
            roomCategories,
            appointments,
            appointmentProtocols);
        
        // Assert
        result.Id.Should().Be(room.Id);
        result.Name.Should().Be(room.Name);
        result.Abbreviation.Should().Be(room.Abbreviation);
        result.IsArchived.Should().Be(room.IsArchived);
        result.RoomNumber.Should().Be(room.RoomNumber);
        result.Floor.Should().Be(room.Floor);
        result.Building.Should().Be(room.Building);
        result.RoomCategories.Should().BeEquivalentTo(roomCategories.Select(roomCategory => new RoomCategoryOverviewOutputModel(roomCategory)));
        result.Appointments.Should().BeEquivalentTo(appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)));
        result.AppointmentProtocols.Should().BeEquivalentTo(appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel(appointmentProtocol)));
    }
}