using Domain.Common.Enums;
using Domain.Entities.IdentityEntities;

namespace TestUtils.Builders.AuthBuilders;

public class TestClaimBuilder
{
    // Properties
    public Role? Role { get; private set; }
    public ClaimType Type { get; private set; }
    public ClaimValue Value { get; private set; }
    
    // Constructor
    private TestClaimBuilder()
    {
        // Setting default values
        Role = TestRoleBuilder.Create().Build();
        Type = ClaimType.Appointment;
        Value = ClaimValue.None;
    }
    
    
    /* - - - Factory methods - - - */
    public static TestClaimBuilder Create()
    {
        return new TestClaimBuilder();
    }

    public Claim Build()
    {
        return new Claim(
            Role!,
            Type,
            Value);
    }
    
    
    /* - - - Override methods - - - */
    public TestClaimBuilder WithRole(Role? role)
    {
        Role = role;
        return this;
    }

    public TestClaimBuilder WithType(ClaimType type)
    {
        Type = type;
        return this;
    }

    public TestClaimBuilder WithValue(ClaimValue value)
    {
        Value = value;
        return this;
    }
}