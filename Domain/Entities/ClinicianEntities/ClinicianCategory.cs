using Domain.Commons.Base;
using Domain.Entities.IdentityEntities;

namespace Domain.Entities.ClinicianEntities;

public class ClinicianCategory : CategoryBase<Clinician>
{
    // Properties
    public ICollection<Clinician> Clinicians
    {
        get => Items;
        private set => Items = value;
    }
    protected override string ItemsPropertyName => nameof(Clinicians);
    
    // Constructor used by ef core and tests to initialize objects
    protected ClinicianCategory() { }
    
    // Standard constructor used to initialize objects
    public ClinicianCategory(
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