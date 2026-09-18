using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using TestUtils.Builders.AuthBuilders;

namespace TestUtils.Builders.DeviceBuilders;

public class TestDeviceCategoryBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public string? Color { get; private set; }
    
    // Constructor
    private TestDeviceCategoryBuilder(Clinic defaultClinic)
    {
        // Setting default values
        Clinic = defaultClinic;
        Name = "test-name";
        Abbreviation = "test-abbreviation";
        Color = "#000000";
    }
    
    
    /* - - - Factory methods - - - */
    public static TestDeviceCategoryBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestDeviceCategoryBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public DeviceCategory Build()
    {
        return new DeviceCategory(
            Clinic!,
            Name!,
            Abbreviation!,
            Color!);
    }
    
    
    /* - - - Override methods - - - */
    public TestDeviceCategoryBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }
}