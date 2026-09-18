using Domain.Entities.AppointmentEntities;
using Domain.Entities.IdentityEntities;
using TestUtils.Builders.AuthBuilders;

namespace TestUtils.Builders.AppointmentBuilders;

public class TestAppointmentCategoryBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public string? Color { get; private set; }
    
    // Constructor
    private TestAppointmentCategoryBuilder(Clinic defaultClinic)
    {
        // Setting default values
        Clinic = defaultClinic;
        Name = "test-name";
        Abbreviation = "test-abbreviation";
        Color = "#000000";
    }
    
    
    /* - - - Factory methods - - - */
    public static TestAppointmentCategoryBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestAppointmentCategoryBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public AppointmentCategory Build()
    {
        return new AppointmentCategory(
            Clinic!,
            Name!,
            Abbreviation!,
            Color!);
    }
    
    
    /* - - - Override methods - - - */
    public TestAppointmentCategoryBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }
}