using Domain.Commons.Base;
using Domain.Entities.IdentityEntities;

namespace Domain.Entities.AppointmentEntities;

public class AppointmentCategory : CategoryBase<Appointment>
{
    // Properties
    public ICollection<Appointment> Appointments
    {
        get => Items;
        private set => Items = value;
    }
    protected override string ItemsPropertyName => nameof(Appointments);
    
    // Constructor used by ef core and tests to initialize objects
    protected AppointmentCategory() { }

    // Standard constructor used to initialize objects 
    public AppointmentCategory(
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