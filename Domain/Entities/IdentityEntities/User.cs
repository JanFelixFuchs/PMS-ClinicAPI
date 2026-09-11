using Domain.Common.Interfaces;
using Domain.Common.Utils.Constants;
using Domain.Common.Utils.Helper;
using Domain.Common.Utils.InvariantValidation;
using Domain.Common.Utils.PropertyValidation;
using Domain.Entities.ClinicianEntities;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

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
        Clinician? clinician,
        DateTime currentDateTime)
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
        ValidateAndSetRefreshTokenExpirationTime(null, currentDateTime);
        ValidateAndSetRole(role);
        ValidateAndSetClinician(clinician);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the role
    public void UpdateRole(Role role)
    {
        // Checking archive, deletion and admin flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        if (IsAdmin)
            throw new InvalidOperationException($"Cannot update an admin {nameof(User)}");
        
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
    public void UpdateRefreshTokenHashAndExpirationTime(string refreshTokenHash, DateTime refreshTokenExpirationTime, DateTime currentDateTime)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        
        // Updating properties
        ValidateAndSetRefreshTokenHash(refreshTokenHash);
        ValidateAndSetRefreshTokenExpirationTime(refreshTokenExpirationTime, currentDateTime);
    }
    
    // Method to mark the refresh token as expired
    public void MarkRefreshTokenHashAsExpired(DateTime currentDateTime)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(User)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(User)}");
        
        // Updating properties
        ValidateAndSetRefreshTokenHash(null);
        ValidateAndSetRefreshTokenExpirationTime(null, currentDateTime);
    }
    
    // Method to archive the entity
    public void Archive(DateTime currentDateTime)
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
        ValidateAndSetRefreshTokenExpirationTime(null, currentDateTime);
        
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
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(clinic, nameof(Clinic)));
        
        // Setting properties
        Clinic = clinic;
        ClinicId = clinic.Id;
    }
    
    // Method to validate and set the username
    private void ValidateAndSetUsername(string username)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNullEmptyOrWhitespace(username, nameof(Username)),
            () => PropertyValidationConditions.IsMatchingRegex(username, RegexPatterns.Username, nameof(Username)));
        
        // Setting properties
        Username = username;
        NormalizedUsername = StringHelper.Normalize(username);
    }
    
    // Method to validate and set the password hash
    private void ValidateAndSetPasswordHash(string passwordHash)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNullEmptyOrWhitespace(passwordHash, nameof(PasswordHash)),
            () => PropertyValidationConditions.HasMaximumLength(passwordHash, Lengths.PasswordHash, nameof(PasswordHash)));
        
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
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNullNotEmptyOrWhitespace(refreshTokenHash, nameof(RefreshTokenHash)),
            () => PropertyValidationConditions.IsNullOrHasMaximumLength(refreshTokenHash, Lengths.RefreshTokenHash, nameof(RefreshTokenHash)));
        
        // Setting property
        RefreshTokenHash = refreshTokenHash;
    }
    
    // Method to validate and set the refresh token expiration time
    private void ValidateAndSetRefreshTokenExpirationTime(DateTime? refreshTokenExpirationTime, DateTime currentDateTime)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNullOrDateTimeInTheFuture(refreshTokenExpirationTime, currentDateTime, nameof(RefreshTokenExpirationTime)));
        
        // Setting property
        RefreshTokenExpirationTime = refreshTokenExpirationTime;
    }
        
    // Method to validate and set the role
    private void ValidateAndSetRole(Role role)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(role, nameof(Role)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            () => InvariantValidationConditions.IsExactGuidValue(role.ClinicId, ClinicId, nameof(Role)),
            () => InvariantValidationConditions.IsNotDeleted(role, nameof(Role)));
        
        // Setting properties
        Role = role;
        RoleId = role.Id;
    }
    
    // Method to validate and set the clinician
    private void ValidateAndSetClinician(Clinician? clinician)
    {
        if (!IsAdmin)
        {
            // Property validation
            PropertyValidationHelper.ConstructPropertyValidation(
                () => PropertyValidationConditions.IsNotNull(clinician, nameof(Clinician)));
            
            // Invariant validation
            InvariantValidationHelper.ConstructInvariantValidation(
                () => InvariantValidationConditions.IsNullOrExactGuidValue(clinician?.ClinicId, ClinicId, nameof(Clinician)),
                () => InvariantValidationConditions.IsNullOrNotArchived(clinician, nameof(Clinician)),
                () => InvariantValidationConditions.IsNullOrNotDeleted(clinician, nameof(Clinician)));
        }
        
        // Setting properties
        Clinician = IsAdmin ? null : clinician;
        ClinicianId = IsAdmin ? null : clinician?.Id;
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