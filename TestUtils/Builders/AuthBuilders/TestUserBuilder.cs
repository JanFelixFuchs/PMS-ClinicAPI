using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Constants;

namespace TestUtils.Builders.AuthBuilders;

public class TestUserBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Username { get; private set; }
    public string? PasswordHash { get; private set; }
    public bool IsAdmin { get; private set; }
    public Role? Role { get; private set; }
    public Clinician? Clinician { get; private set; }
    public readonly DateTime CreationDateTime;
    
    // Constructor
    private TestUserBuilder(
        Clinic defaultClinic,
        DateTime creationDateTime)
    {
        // Setting default values
        Clinic = defaultClinic;
        Username = "test_username";
        PasswordHash = "test-password-hash";
        IsAdmin = false;
        Role = TestRoleBuilder.Create(defaultClinic).Build();
        Clinician = TestClinicianBuilder.Create(defaultClinic).Build();
        CreationDateTime = creationDateTime;
    }
    
    
    /* - - - Factory methods - - - */
    public static TestUserBuilder Create(
        Clinic? defaultClinic = null,
        DateTime? currentDateTime = null)
    {
        return new TestUserBuilder(
            defaultClinic ?? TestClinicBuilder.Create().Build(),
            currentDateTime ?? TestConstants.DefaultCurrentDateTime);
    }

    public User Build()
    {
        return new User(
            Clinic!,
            Username!,
            PasswordHash!,
            IsAdmin,
            Role!,
            Clinician,
            CreationDateTime);
    }
    
    
    /* - - - Override methods - - - */
    public TestUserBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestUserBuilder WithUsername(string? username)
    {
        Username = username;
        return this;
    }

    public TestUserBuilder WithPasswordHash(string? passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }

    public TestUserBuilder WithIsAdmin(bool isAdmin)
    {
        IsAdmin = isAdmin;
        return this;
    }

    public TestUserBuilder WithRole(Role? role)
    {
        Role = role;
        return this;
    }

    public TestUserBuilder WithClinician(Clinician? clinician)
    {
        Clinician = clinician;
        return this;
    }
}