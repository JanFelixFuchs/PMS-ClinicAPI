using Domain.Commons.Base;
using Domain.Entities.IdentityEntities;

namespace Domain.Entities.DeviceEntities;

public class DeviceCategory : CategoryBase<Device>
{
    // Properties
    public ICollection<Device> Devices
    {
        get => Items;
        private set => Items = value;
    }
    protected override string ItemsPropertyName => nameof(Devices);
    
    // Constructor used by ef core and tests to initialize objects
    protected DeviceCategory() { }
    
    // Standard constructor used to initialize objects 
    public DeviceCategory(
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