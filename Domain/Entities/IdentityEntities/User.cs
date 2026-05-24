using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Helper;
using Domain.Commons.Utils.Validation;
using Domain.Entities.ClinicianEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.IdentityEntities;

public class User : IEntity, IDeletable, IArchivable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string Username { get; private set; } = string.Empty;
    public string NormalizedUsername { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsAdmin { get; private set; }
    public bool IsArchived { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenExpirationTime { get; set; }
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public Guid? ClinicianId { get; private set; }
    public Clinician? Clinician { get; private set; }
    
    // Constructor used by ef core and tests to initialize objects
    protected User() { }
    
    // Standard constructor used to initialize objects
    public User(
        Clinic clinic, 
        string username,
        string passwordHash,
        bool isAdmin, 
        Role role,
        Clinician? clinician)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetUsername(username);
        ValidateAndSetPasswordHash(passwordHash);
        ValidateAndSetIsAdmin(isAdmin);
        IsArchived = false;
        IsDeleted = false;
        ValidateAndSetRefreshTokenHash(null);
        ValidateAndSetRefreshTokenExpirationTime(null);
        ValidateAndSetRole(role);
        ValidateAndSetClinician(clinician, isAdmin);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the role
    public void UpdateRole(Role role)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        
        // Updating property
        ValidateAndSetRole(role);
    }
    
    // Method to update the username
    public void UpdateUsername(string username)
    {
        // Validating 
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        
        // Update property
        ValidateAndSetUsername(username);
    }
    
    // Method to update the password hash
    public void UpdatePasswordHash(string passwordHash)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        
        // Updating property
        ValidateAndSetPasswordHash(passwordHash);
    }
    
    // Method to update the refresh token hash and refresh token expiration time
    public void UpdateRefreshTokenHashAndExpirationTime(string refreshTokenHash, DateTime refreshTokenExpirationTime)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        
        // Updating properties
        ValidateAndSetRefreshTokenHash(refreshTokenHash);
        ValidateAndSetRefreshTokenExpirationTime(refreshTokenExpirationTime);
    }
    
    // Method to mark the refresh token as expired
    public void MarkRefreshTokenHashAsExpired()
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        
        // Updating properties
        ValidateAndSetRefreshTokenHash(null);
        ValidateAndSetRefreshTokenExpirationTime(null);
    }
    
    // Method to archive the entity
    public void Archive()
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot archive an already archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot archive a deleted {nameof(User)}");
        if (IsAdmin)
            throw new InvalidOperationException($"Cannot archive an admin {nameof(User)}");
        
        // Invalidating refresh token
        ValidateAndSetRefreshTokenHash(null);
        ValidateAndSetRefreshTokenExpirationTime(null);
        
        // Setting property
        IsArchived = true;
    }
    
    // Method to unarchive the entity
    public void Unarchive(Clinician? clinician)
    {
        // Validating
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot unarchive an already unarchived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot unarchive a deleted {nameof(User)}");
        if (clinician != null && clinician.IsArchived)
            throw new InvalidOperationException($"Cannot unarchive a {nameof(User)} with an archived {nameof(Clinician)}");
    
        // Setting property
        IsArchived = false;   
    }
    
    // Method to delete the entity
    public void Delete()
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(User)}");
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot delete an unarchived {nameof(User)}");
        
        // Detaching clinician
        ClinicianId = null;
        Clinician = null;
        
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
    
    // Method to validate and set the username
    private void ValidateAndSetUsername(string username)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(username, nameof(Username)),
            ValidationConditions.IsMatchingRegex(username, RegexPatterns.Username, nameof(Username)));
        
        // Setting properties
        Username = username;
        NormalizedUsername = StringHelper.Normalize(username);
    }
    
    // Method to validate and set the password hash
    private void ValidateAndSetPasswordHash(string passwordHash)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(passwordHash, nameof(PasswordHash)),
            ValidationConditions.HasMaximumLength(passwordHash, Lengths.PasswordHash, nameof(PasswordHash)));
        
        // Setting property
        PasswordHash = passwordHash;
    }
    
    // Method to validate and set the admin flag
    private void ValidateAndSetIsAdmin(bool isAdmin)
    {
        // Setting property
        IsAdmin = isAdmin;
    }
    
    // Method to validate and set the refresh token hash
    private void ValidateAndSetRefreshTokenHash(string? refreshTokenHash)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullNotEmptyOrWhitespace(refreshTokenHash, nameof(RefreshTokenHash)),
            ValidationConditions.IsNullOrHasMaximumLength(refreshTokenHash, Lengths.RefreshTokenHash, nameof(RefreshTokenHash)));
        
        // Setting property
        RefreshTokenHash = refreshTokenHash;
    }
    
    // Method to validate and set the refresh token expiration time
    private void ValidateAndSetRefreshTokenExpirationTime(DateTime? refreshTokenExpirationTime)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullOrDateTimeInTheFuture(refreshTokenExpirationTime, nameof(RefreshTokenExpirationTime)));
        
        // Setting property
        RefreshTokenExpirationTime = refreshTokenExpirationTime;
    }
        
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
    
    // Method to validate and set the clinician
    private void ValidateAndSetClinician(Clinician? clinician, bool isAdmin)
    {
        // Validating
        if (!isAdmin)
        {
            ValidationHelper.ConstructPropertyValidation(
                ValidationConditions.IsNotNull(clinician, nameof(Clinician)),
                ValidationConditions.IsNullOrNotArchived(clinician, nameof(Clinician)),
                ValidationConditions.IsNullOrNotDeleted(clinician, nameof(Clinician)));
        }
        
        // Setting properties
        Clinician = clinician;
        ClinicianId = clinician?.Id;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(Username)}: <omitted>, " +
               $"{nameof(NormalizedUsername)}: <omitted>, " +
               $"{nameof(PasswordHash)}: <omitted>, " +
               $"{nameof(IsAdmin)}: {IsAdmin}, " +
               $"{nameof(IsArchived)}: {IsArchived}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(RefreshTokenHash)}: <omitted>, " +
               $"{nameof(RefreshTokenExpirationTime)}: <omitted>, " +
               $"{nameof(Role)}: {RoleId}, " +
               $"{nameof(Clinician)}: {ClinicianId}";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is User other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}