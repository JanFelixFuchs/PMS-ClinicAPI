using Domain.Entities.IdentityEntities;
using Domain.Entities.RoomEntities;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Constants;

namespace TestUtils.Builders.RoomBuilders;

public class TestRoomBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public ICollection<RoomCategory>? RoomCategories { get; private set; }
    public string? RoomNumber { get; private set; }
    public string? Floor { get; private set; }
    public string? Building { get; private set; }

    // Constructor
    private TestRoomBuilder(Clinic defaultClinic)
    {
        // Setting default values
        Clinic = defaultClinic;
        Name = TestConstants.ValidInitialRoomName;
        Abbreviation = TestConstants.ValidInitialAbbreviation;
        RoomCategories = new List<RoomCategory>{ TestRoomCategoryBuilder.Create(defaultClinic).Build() };
        RoomNumber = TestConstants.ValidInitialRoomNumber;
        Floor = TestConstants.ValidInitialFloor;
        Building = TestConstants.ValidInitialBuilding;
    }


    /* - - - Factory methods - - - */
    public static TestRoomBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestRoomBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public Room Build()
    {
        return new Room(
            Clinic!,
            Name!,
            Abbreviation!,
            RoomCategories!,
            RoomNumber,
            Floor,
            Building);
    }

    public TestRoomBuilder AsUpdate()
    {
        // Setting default values
        Name = TestConstants.ValidUpdatedRoomName;
        Abbreviation = TestConstants.ValidUpdatedAbbreviation;
        RoomCategories = new List<RoomCategory>{ TestRoomCategoryBuilder.Create(Clinic).Build() };
        RoomNumber = TestConstants.ValidUpdatedRoomNumber;
        Floor = TestConstants.ValidUpdatedFloor;
        Building = TestConstants.ValidUpdatedBuilding;
        
        // Returning instance
        return this;
    }

    public Action Apply(Room room) =>
        () => room.Update(
            Name!,
            Abbreviation!,
            RoomCategories!,
            RoomNumber,
            Floor,
            Building);
    

    /* - - - Override methods - - - */
    public TestRoomBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestRoomBuilder WithName(string? name)
    {
        Name = name;
        return this;
    }

    public TestRoomBuilder WithAbbreviation(string? abbreviation)
    {
        Abbreviation = abbreviation;
        return this;
    }

    public TestRoomBuilder WithRoomCategories(ICollection<RoomCategory>? roomCategories)
    {
        RoomCategories = roomCategories;
        return this;
    }

    public TestRoomBuilder WithRoomNumber(string? roomNumber)
    {
        RoomNumber = roomNumber;
        return this;
    }

    public TestRoomBuilder WithFloor(string? floor)
    {
        Floor = floor;
        return this;
    }

    public TestRoomBuilder WithBuilding(string? building)
    {
        Building = building;
        return this;
    }
}