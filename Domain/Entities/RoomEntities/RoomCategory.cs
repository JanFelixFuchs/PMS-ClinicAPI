using Domain.Commons.Base;
using Domain.Entities.IdentityEntities;

namespace Domain.Entities.RoomEntities;

public class RoomCategory : CategoryBase<Room>
{
    // Properties
    public ICollection<Room> Rooms
    {
        get => Items;
        private set => Items = value;
    }
    protected override string ItemsPropertyName => nameof(Rooms);

    // Constructor used by ef core and tests to initialize objects
    protected RoomCategory() { }
    
    // Standard constructor used to initialize objects 
    public RoomCategory(
        Clinic clinic,
        string name,
        string abbreviation,
        string color) : base(
        clinic,
        name,
        abbreviation,
        color)
    {
        
    }
}