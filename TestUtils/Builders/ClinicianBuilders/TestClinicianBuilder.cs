using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;
using TestUtils.Builders.AuthBuilders;

namespace TestUtils.Builders.ClinicianBuilders;

public class TestClinicianBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public ICollection<ClinicianCategory>? ClinicianCategories { get; private set; }
    
    // Constructor
    private TestClinicianBuilder(Clinic defaultClinic)
    {
        // Setting default values
        Clinic = defaultClinic;
        FirstName = "test-first-name";
        LastName = "test-last-name";
        ClinicianCategories = new List<ClinicianCategory>{ TestClinicianCategoryBuilder.Create(defaultClinic).Build() };
    }
    
    
    /* - - - Factory methods - - - */
    public static TestClinicianBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestClinicianBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public Clinician Build()
    {
        return new Clinician(
            Clinic!,
            FirstName!,
            LastName!,
            ClinicianCategories!);
    }

    public TestClinicianBuilder AsUpdate()
    {
        // Setting default values
        LastName = "test-updated-last-name";
        ClinicianCategories = new List<ClinicianCategory>{ TestClinicianCategoryBuilder.Create(Clinic).Build() };
        
        // Returning instance
        return this;
    }

    public Action Apply(Clinician clinician) =>
        () => clinician.Update(
            LastName!,
            ClinicianCategories!);
    
    
    /* - - - Override methods - - - */
    public TestClinicianBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestClinicianBuilder WithFirstName(string? firstName)
    {
        FirstName = firstName;
        return this;
    }
    
    public TestClinicianBuilder WithLastName(string? lastName)
    {
        LastName = lastName;
        return this;
    }
    
    public TestClinicianBuilder WithClinicianCategories(ICollection<ClinicianCategory>? clinicianCategories)
    {
        ClinicianCategories = clinicianCategories;
        return this;
    }
}