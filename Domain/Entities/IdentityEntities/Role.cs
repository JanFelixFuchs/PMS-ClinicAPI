using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Helper;
using Domain.Commons.Utils.Validation;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.IdentityEntities;

public class Role : IEntity, IDeletable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public bool IsSystemRole { get; private set; }
    public bool IsDeleted { get; private set; }
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Claim> Claims { get; private set; } = new List<Claim>();
    
    // Constructor used by ef core and tests to initialize objects
    protected Role() { }
    
    // Standard constructor used to initialize objects
    public Role(
        Clinic clinic, 
        string name, 
        bool isSystemRole)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetName(name);
        ValidateAndSetIsSystemRole(isSystemRole);
        IsDeleted = false;
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(string name)
    {
        // Checking deletion flag
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Role)}");
        
        // Updating property
        ValidateAndSetName(name);
    }
    
    // Method to delete the entity
    public void Delete(ICollection<User> users, ICollection<Claim> claims)
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Role)}");
        if (IsSystemRole)
            throw new InvalidOperationException($"Cannot delete a system {nameof(Role)}");
        if (users.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Role)} that has {nameof(Users)}");
        
        // Deleting claims
        foreach (var claim in claims)
            claim.Delete();
        
        // Setting property
        IsDeleted = true;
    }
    
    
    /* - - - Validation methods - - - */
    // Method to validate and set the clinic
    private void ValidateAndSetClinic(Clinic clinic)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(clinic, nameof(Clinic)));
        
        // Setting properties
        Clinic = clinic;
        ClinicId = clinic.Id;
    }
    
    // Method to validate and set name
    private void ValidateAndSetName(string name)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(name, nameof(Name)),
            ValidationConditions.HasMaximumLength(name, Lengths.RoleName, nameof(Name)));
            
        // Setting properties
        Name = name;
        NormalizedName = StringHelper.Normalize(name);
    }
    
    // Method to validate and set the system role flag
    private void ValidateAndSetIsSystemRole(bool isSystemRole)
    {
        // Setting property
        IsSystemRole = isSystemRole;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(Name)}: {Name}, " +
               $"{nameof(NormalizedName)}: {NormalizedName}, " +
               $"{nameof(IsSystemRole)}: {IsSystemRole}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(Users)}: [{string.Join("| ", Users.Select(user => user.Id))}], " +
               $"{nameof(Claims)}: [{string.Join("| ", Claims.Select(claim => claim.Id))}]";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Role other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}