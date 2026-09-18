using Domain.Entities.IdentityEntities;
using Domain.Entities.RoomEntities;
using TestUtils.Builders.AuthBuilders;

namespace TestUtils.Builders.RoomBuilders;

public class TestRoomCategoryBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public string? Color { get; private set; }
    
    // Constructor
    private TestRoomCategoryBuilder(Clinic defaultClinic)
    {
        // Setting default values
        Clinic = defaultClinic;
        Name = "test-name";
        Abbreviation = "test-abbreviation";
        Color = "#000000";
    }
    
    
    /* - - - Factory methods - - - */
    public static TestRoomCategoryBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestRoomCategoryBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public RoomCategory Build()
    {
        return new RoomCategory(
            Clinic!,
            Name!,
            Abbreviation!,
            Color!);
    }
    
    
    /* - - - Override methods - - - */
    public TestRoomCategoryBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }
}