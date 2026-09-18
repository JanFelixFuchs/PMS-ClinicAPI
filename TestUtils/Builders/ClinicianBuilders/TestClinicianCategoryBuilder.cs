using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;
using TestUtils.Builders.AuthBuilders;

namespace TestUtils.Builders.ClinicianBuilders;

public class TestClinicianCategoryBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public string? Color { get; private set; }
    
    // Constructor
    private TestClinicianCategoryBuilder(Clinic defaultClinic)
    {
        // Setting default values
        Clinic = defaultClinic;
        Name = "test-name";
        Abbreviation = "test-abbreviation";
        Color = "#000000";
    }
    
    
    /* - - - Factory methods - - - */
    public static TestClinicianCategoryBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestClinicianCategoryBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public ClinicianCategory Build()
    {
        return new ClinicianCategory(
            Clinic!,
            Name!,
            Abbreviation!,
            Color!);
    }
    
    
    /* - - - Override methods - - - */
    public TestClinicianCategoryBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }
}