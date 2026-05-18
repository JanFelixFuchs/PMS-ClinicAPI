using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Validation;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.IdentityEntities;

public class Claim : IEntity, IDeletable
{
    // Properties
    public Guid Id { get; }
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public ClaimType Type { get; private set; }
    public ClaimValue Value { get; private set; }
    public bool IsDeleted { get; private set; }
    
    // Constructor used by ef core and tests to initialize objects
    protected Claim() { }
    
    // Standard constructor used to initialize objects
    public Claim(
        Role role,
        ClaimType type,
        ClaimValue value)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetRole(role);
        ValidateAndSetType(type);
        ValidateAndSetValue(value);
        IsDeleted = false;
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to delete the entity
    public void Delete()
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Claim)}");
        
        // Setting property
        IsDeleted = true;
    }
    
    
    /* - - - Validation methods - - - */
    // Method to validate and set the role
    private void ValidateAndSetRole(Role role)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(role, nameof(Role)),
            ValidationConditions.IsNotDeleted(role, nameof(Role)));
        
        // Setting properties
        Role = role;
        RoleId = role.Id;
    } 
    
    // Method to validate and set the type
    private void ValidateAndSetType(ClaimType type)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsDefinedEnum(type, nameof(Type)));
        
        // Setting property
        Type = type;
    }
    
    // Method to validate and set the value
    private void ValidateAndSetValue(ClaimValue value)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsDefinedEnum(value, nameof(Value)));
        
        // Setting property
        Value = value;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Role)}: {RoleId}, " +
               $"{nameof(Type)}: {Type}, " +
               $"{nameof(Value)}: {Value}," + 
               $"{nameof(IsDeleted)}: {IsDeleted}";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Claim other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}