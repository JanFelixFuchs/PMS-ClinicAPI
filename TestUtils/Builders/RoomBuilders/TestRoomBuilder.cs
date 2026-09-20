using Domain.Entities.IdentityEntities;
using Domain.Entities.RoomEntities;
using TestUtils.Builders.AuthBuilders;

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
        Name = "test-name";
        Abbreviation = "test-abbreviation";
        RoomCategories = new List<RoomCategory>{ TestRoomCategoryBuilder.Create(defaultClinic).Build() };
        RoomNumber = "123";
        Floor = "test-floor";
        Building = "test-building";
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
        Name = "test-updated-name";
        Abbreviation = "test-updated-abbreviation";
        RoomCategories = new List<RoomCategory>{ TestRoomCategoryBuilder.Create(Clinic).Build() };
        RoomNumber = "456";
        Floor = "test-updated-floor";
        Building = "test-updated-building";
        
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