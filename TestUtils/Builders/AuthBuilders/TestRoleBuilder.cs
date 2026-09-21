using Domain.Entities.IdentityEntities;

namespace TestUtils.Builders.AuthBuilders;

public class TestRoleBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Name { get; private set; }
    public bool IsSystemRole { get; private set; }

    // Constructor
    private TestRoleBuilder(Clinic defaultClinic)
    {
        // Generating and setting default values
        Clinic = defaultClinic;
        Name = "test-name";
        IsSystemRole = false;
    }


    /* - - - Factory methods - - - */
    public static TestRoleBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestRoleBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public Role Build()
    {
        return new Role(
            Clinic!,
            Name!,
            IsSystemRole);
    }

    public TestRoleBuilder AsUpdate()
    {
        // Setting default values
        Name = "test-updated-name";
        
        // Returning instance
        return this;
    }

    public Action Apply(Role role) =>
        () => role.Update(Name!);
    
    
    /* - - - Override methods - - - */
    public TestRoleBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestRoleBuilder WithName(string? name)
    {
        Name = name;
        return this;
    }

    public TestRoleBuilder WithIsSystemRole(bool isSystemRole)
    {
        IsSystemRole = isSystemRole;
        return this;
    }
}