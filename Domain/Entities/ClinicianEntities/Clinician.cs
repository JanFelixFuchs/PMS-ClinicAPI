using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Validation;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.IdentityEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.ClinicianEntities;

public class Clinician : IEntity, IDeletable, IArchivable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public bool IsArchived { get; private set; }
    public bool IsDeleted { get; private set; }
    public ICollection<ClinicianCategory> ClinicianCategories { get; private set; } = new List<ClinicianCategory>();
    public User? User { get; private set; }
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
    public ICollection<AppointmentProtocol> AppointmentProtocols { get; private set; } = new List<AppointmentProtocol>();
    public ICollection<Result> Results { get; private set; } = new List<Result>();
    
    // Constructor used by ef core and tests to initialize objects
    protected Clinician() { }
    
    // Standard constructor used to initialize objects
    public Clinician(
        Clinic clinic,
        string firstName,
        string lastName,
        ICollection<ClinicianCategory> clinicianCategories)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetFirstName(firstName);
        ValidateAndSetLastName(lastName);
        IsArchived = false;
        IsDeleted = false;
        ValidateAndSetClinicianCategories(clinicianCategories);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string lastName,
        ICollection<ClinicianCategory> clinicianCategories)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Clinician)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Clinician)}");
        
        // Updating properties
        ValidateAndSetLastName(lastName);
        ValidateAndSetClinicianCategories(clinicianCategories);
    }
    
    // Method to add a clinician category
    public void AddClinicianCategory(ClinicianCategory clinicianCategory)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Clinician)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Clinician)}");
        
        // Setting property
        ValidateAndSetClinicianCategories(new List<ClinicianCategory>(ClinicianCategories) { clinicianCategory });
    }
    
    // Method to remove a clinician category
    public void RemoveClinicianCategory(ClinicianCategory clinicianCategory)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Clinician)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Clinician)}");
        
        // Setting property
        ValidateAndSetClinicianCategories(ClinicianCategories.Except(new List<ClinicianCategory> { clinicianCategory }).ToList());
    }
    
    // Method to archive the entity
    public void Archive(ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot archive an already archived {nameof(Clinician)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot archive a deleted {nameof(Clinician)}");
        if (appointments.Any(appointment => appointment.Status != AppointmentStatus.Completed))
            throw new InvalidOperationException($"Cannot archive a {nameof(Clinician)} that has uncompleted {nameof(Appointments)}");
        if (appointmentProtocols.Any(appointmentProtocol => appointmentProtocol.Status != AppointmentProtocolStatus.Completed))
            throw new InvalidOperationException($"Cannot archive a {nameof(Clinician)} that has uncompleted {nameof(AppointmentProtocols)}");
        
        // Setting properties
        IsArchived = true;
    }
    
    // Method to unarchive the entity
    public void Unarchive()
    {
        // Validating
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot unarchive an unarchived {nameof(Clinician)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot unarchive a deleted {nameof(Clinician)}");
        
        // Setting property
        IsArchived = false;
    }
    
    // Method to delete the entity
    public void Delete(User? user, ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols, ICollection<Result> results)
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Clinician)}");
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot delete an unarchived {nameof(Clinician)}");
        if (user != null)
            throw new InvalidOperationException($"Cannot delete a {nameof(Clinician)} that is assigned to a {nameof(User)}");
        if (appointments.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Clinician)} that has {nameof(Appointments)}");
        if (appointmentProtocols.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Clinician)} that has {nameof(AppointmentProtocols)}");
        if (results.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Clinician)} that has {nameof(Results)}");
        
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
    
    // Method to validate and set the first name
    private void ValidateAndSetFirstName(string firstName)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(firstName, nameof(FirstName)),
            ValidationConditions.HasMaximumLength(firstName, Lengths.FirstName, nameof(FirstName)));
        
        // Setting property
        FirstName = firstName;
    }
    
    // Method to validate and set the last name
    private void ValidateAndSetLastName(string lastName)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(lastName, nameof(LastName)),
            ValidationConditions.HasMaximumLength(lastName, Lengths.LastName, nameof(LastName)));
        
        // Setting property
        LastName = lastName;
    }
    
    // Method to validate and set the clinician categories
    private void ValidateAndSetClinicianCategories(ICollection<ClinicianCategory> clinicianCategories)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(clinicianCategories, nameof(ClinicianCategories)),
            ValidationConditions.IsNotContainingDuplicates(clinicianCategories, nameof(ClinicianCategories)),
            ValidationConditions.IsNotContainingDeletedElements(clinicianCategories, nameof(ClinicianCategories)));
        
        // Setting property
        ClinicianCategories = clinicianCategories;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(FirstName)}: {FirstName}, " +
               $"{nameof(LastName)}: {LastName}, " +
               $"{nameof(IsArchived)}: {IsArchived}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(ClinicianCategories)}: [{string.Join("| ", ClinicianCategories.Select(clinicianCategory => clinicianCategory.Id))}], " +
               $"{nameof(User)}: {User?.Id}, " +
               $"{nameof(Appointments)}: [{string.Join("| ", Appointments.Select(appointment => appointment.Id))}], " +
               $"{nameof(AppointmentProtocols)}: [{string.Join("| ", AppointmentProtocols.Select(appointmentProtocol => appointmentProtocol.Id))}], " +
               $"{nameof(Results)}: [{string.Join("| ", Results.Select(result => result.Id))}] ";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Clinician other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}