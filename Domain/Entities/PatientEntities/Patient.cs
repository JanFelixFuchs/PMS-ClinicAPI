using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Validation;
using Domain.Commons.Value_Objects;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.IdentityEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.PatientEntities;

public class Patient : IEntity, IDeletable, IArchivable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public DateTime DateOfCreation { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public Address Address { get; private set; } = null!;
    public ContactInformation ContactInformation { get; private set; } = null!;
    public InsuranceStatus InsuranceStatus { get; private set; }
    public bool IsArchived { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? Allergies { get; private set; }
    public string? Remarks { get; private set; }
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
    public ICollection<AppointmentProtocol> AppointmentProtocols { get; private set; } = new List<AppointmentProtocol>();
    public ICollection<Result> Results { get; private set; } = new List<Result>();
    
    // Constructor used by ef core and tests to initialize objects
    protected Patient() { }
    
    // Standard constructor used to initialize objects 
    public Patient(
        Clinic clinic,
        string firstName, 
        string lastName,
        DateTime dateOfBirth,
        Gender gender,
        string street,
        string houseNumber,
        string city,
        string zipCode,
        Country country,
        string email,
        string phoneNumber,
        InsuranceStatus insuranceStatus,
        string? allergies,
        string? remarks)
    {
        // Constructing value objects
        var address = new Address(street, houseNumber, city, zipCode, country);
        var contactInformation = new ContactInformation(email, phoneNumber, country);
        
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        DateOfCreation = DateTime.UtcNow;
        ValidateAndSetFirstName(firstName);
        ValidateAndSetLastName(lastName);
        ValidateAndSetDateOfBirth(dateOfBirth);
        ValidateAndSetGender(gender);
        ValidateAndSetAddress(address);
        ValidateAndSetContactInformation(contactInformation);
        ValidateAndSetInsuranceStatus(insuranceStatus);
        IsArchived = false;
        IsDeleted = false;
        ValidateAndSetAllergies(allergies);
        ValidateAndSetRemarks(remarks);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string lastName,
        string street,
        string houseNumber,
        string city,
        string zipCode,
        Country country,
        string email,
        string phoneNumber,
        InsuranceStatus insuranceStatus,
        string? allergies,
        string? remarks)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Patient)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Patient)}");
        
        // Constructing value objects
        var address = new Address(street, houseNumber, city, zipCode, country);
        var contactInformation = new ContactInformation(email, phoneNumber, country);
        
        // Updating properties
        ValidateAndSetLastName(lastName);
        ValidateAndSetAddress(address);
        ValidateAndSetContactInformation(contactInformation);
        ValidateAndSetInsuranceStatus(insuranceStatus);
        ValidateAndSetAllergies(allergies);
        ValidateAndSetRemarks(remarks);
    }
    
    // Method to archive the entity
    public void Archive(ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot archive an already archived {nameof(Patient)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot archive a deleted {nameof(Patient)}");
        if (appointments.Any(appointment => appointment.Status != AppointmentStatus.Attended))
            throw new InvalidOperationException($"Cannot archive a {nameof(Patient)} that has unattended {nameof(Appointments)}");
        if (appointmentProtocols.Any(appointmentProtocol => appointmentProtocol.Status != AppointmentProtocolStatus.Completed))
            throw new InvalidOperationException($"Cannot archive a {nameof(Patient)} that has uncompleted {nameof(AppointmentProtocols)}");
        
        // Setting property
        IsArchived = true;
    }
    
    // Method to unarchive the entity
    public void Unarchive()
    {
        // Validating
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot unarchive an unarchived {nameof(Patient)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot unarchive a deleted {nameof(Patient)}");
        
        // Setting property
        IsArchived = false;
    }
    
    // Method to delete the entity
    public void Delete(ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols, ICollection<Result> results)
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Patient)}");
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot delete an unarchived {nameof(Patient)}");
        if (appointments.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Patient)} that has {nameof(Appointments)}");
        if (appointmentProtocols.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Patient)} that has {nameof(AppointmentProtocols)}");
        if (results.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Patient)} that has {nameof(Results)}");
        
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
    
    // Method to validate and set the date of birth
    private void ValidateAndSetDateOfBirth(DateTime dateOfBirth)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(dateOfBirth, nameof(DateOfBirth)),
            ValidationConditions.IsDateTimeInThePast(dateOfBirth, nameof(DateOfBirth)));
        
        // Setting property
        DateOfBirth = dateOfBirth.Date;
    }
    
    // Method to validate and set the gender
    private void ValidateAndSetGender(Gender gender)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsDefinedEnum(gender, nameof(Gender)));
        
        // Setting property
        Gender = gender;
    }  
    
    // Method to validate and set the address
    private void ValidateAndSetAddress(Address address)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(address, nameof(Address)));
        
        // Setting property
        Address = address;
    }
    
    // Method to validate and set the contact information
    private void ValidateAndSetContactInformation(ContactInformation contactInformation)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(contactInformation, nameof(ContactInformation)));
        
        // Setting property
        ContactInformation = contactInformation;
    }
    
    // Method to validate and set the insurance status
    private void ValidateAndSetInsuranceStatus(InsuranceStatus insuranceStatus)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsDefinedEnum(insuranceStatus, nameof(InsuranceStatus)));
        
        // Setting property
        InsuranceStatus = insuranceStatus;
    }
    
    // Method to validate and set the allergies
    private void ValidateAndSetAllergies(string? allergies)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullNotEmptyOrWhitespace(allergies, nameof(Allergies)),
            ValidationConditions.IsNullOrHasMaximumLength(allergies, Lengths.Allergies, nameof(Allergies)));
        
        // Setting property
        Allergies = allergies;
    }
    
    // Method to validate and set the remarks
    private void ValidateAndSetRemarks(string? remarks)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullNotEmptyOrWhitespace(remarks, nameof(Remarks)),
            ValidationConditions.IsNullOrHasMaximumLength(remarks, Lengths.PatientRemarks, nameof(Remarks)));
        
        // Setting property
        Remarks = remarks;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(DateOfCreation)}: {DateOfCreation}, " +
               $"{nameof(FirstName)}: {FirstName}, " +
               $"{nameof(LastName)}: {LastName}, " +
               $"{nameof(DateOfBirth)}: {DateOfBirth}, " +
               $"{nameof(Gender)}: {Gender}, " +
               $"{nameof(Address)}: {Address}, " +
               $"{nameof(ContactInformation)}: {ContactInformation}, " +
               $"{nameof(InsuranceStatus)}: {InsuranceStatus}, " +
               $"{nameof(IsArchived)}: {IsArchived}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(Allergies)}: {Allergies}, " +
               $"{nameof(Remarks)}: {Remarks}" +
               $"{nameof(Appointments)}: [{string.Join("| ", Appointments.Select(appointment => appointment.Id))}], " +
               $"{nameof(AppointmentProtocols)}: [{string.Join("| ", AppointmentProtocols.Select(appointmentProtocol => appointmentProtocol.Id))}], " +
               $"{nameof(Results)}: [{string.Join("| ", Results.Select(result => result.Id))}]";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Patient other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}