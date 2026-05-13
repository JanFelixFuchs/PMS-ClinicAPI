using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Helper;
using Domain.Commons.Utils.Validation;
using Domain.Commons.Value_Objects;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.PatientEntities;
using Domain.Entities.RoomEntities;

namespace Domain.Entities.IdentityEntities;

public class Clinic : IEntity
{
    // Properties
    public Guid Id { get; }
    public string Code { get; private set; } = string.Empty;
    public string NormalizedCode { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Abbreviation { get; private set; } = string.Empty;
    public string Owner { get; private set; } = string.Empty;
    public MedicalField MedicalField { get; private set; }
    public Address Address { get; private set; } = null!;
    public ContactInformation ContactInformation { get; private set; } = null!;
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Role> Roles { get; private set; } = new List<Role>();
    public ICollection<Device> Devices { get; private set; } = new List<Device>();
    public ICollection<DeviceCategory> DeviceCategories { get; private set; } = new List<DeviceCategory>();
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();
    public ICollection<RoomCategory> RoomCategories { get; private set; } = new List<RoomCategory>();
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
    public ICollection<AppointmentCategory> AppointmentCategories { get; private set; } = new List<AppointmentCategory>();
    public ICollection<Clinician> Clinicians { get; private set; } = new List<Clinician>();
    public ICollection<ClinicianCategory> ClinicianCategories { get; private set; } = new List<ClinicianCategory>();
    public ICollection<Patient> Patients { get; private set; } = new List<Patient>();
    public ICollection<AppointmentProtocol> AppointmentProtocols { get; private set; } = new List<AppointmentProtocol>();
    public ICollection<Result> Results { get; private set; } = new List<Result>();
    
    // Constructor used by ef core and tests to initialize objects
    protected Clinic() { }
    
    // Standard constructor used to initialize objects
    public Clinic(
        string code,
        string name,
        string abbreviation,
        string owner,
        MedicalField medicalField, 
        string street,
        string houseNumber,
        string city,
        string zipCode,
        Country country,
        string email,
        string phoneNumber)
    {
        // Constructing value objects
        var address = new Address(street, houseNumber, city, zipCode, country);
        var contactInformation = new ContactInformation(email, phoneNumber, country);
        
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetCode(code);
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        ValidateAndSetOwner(owner);
        ValidateAndSetMedicalField(medicalField);
        ValidateAndSetAddress(address);
        ValidateAndSetContactInformation(contactInformation);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string name, 
        string abbreviation,
        string owner,
        MedicalField medicalField,
        string street,
        string houseNumber,
        string city,
        string zipCode,
        Country country,
        string email,
        string phoneNumber)
    {
        // Constructing value objects
        var address = new Address(street, houseNumber, city, zipCode, country);
        var contactInformation = new ContactInformation(email, phoneNumber, country);
        
        // Updating properties
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        ValidateAndSetOwner(owner);
        ValidateAndSetMedicalField(medicalField);
        ValidateAndSetAddress(address);
        ValidateAndSetContactInformation(contactInformation);
    }
    
    // Method to update the code
    public void UpdateCode(string code)
    {
        // Updating property
        ValidateAndSetCode(code);
    }
    
    
    /*  - - - Validation methods - - - */
    // Method to validate and set the code 
    private void ValidateAndSetCode(string code)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(code, nameof(Code)),
            ValidationConditions.IsMatchingRegex(code, RegexPatterns.Code, nameof(Code)));
        
        // Setting properties
        Code = code;
        NormalizedCode = StringHelper.Normalize(Code);
    }
    
    // Method to validate and set the name
    private void ValidateAndSetName(string name)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(name, nameof(Name)),
            ValidationConditions.HasMaximumLength(name, Lengths.ClinicName, nameof(Name)));
        
        // Setting property
        Name = name;
    }
    
    // Method to validate and set the abbreviation
    private void ValidateAndSetAbbreviation(string abbreviation)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(abbreviation, nameof(Abbreviation)),
            ValidationConditions.HasMaximumLength(abbreviation, Lengths.Abbreviation, nameof(Abbreviation)));
        
        // Setting property
        Abbreviation = abbreviation;
    }
    
    // Method to validate and set the owner
    private void ValidateAndSetOwner(string owner)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(owner, nameof(Owner)),
            ValidationConditions.HasMaximumLength(owner, Lengths.Owner, nameof(Owner)));
        
        // Setting property
        Owner = owner;
    }
    
    // Method to validate and set the medical field
    private void ValidateAndSetMedicalField(MedicalField medicalField)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsDefinedEnum(medicalField, nameof(MedicalField)));
        
        // Setting property
        MedicalField = medicalField;
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
    
    // Method to validate and set contact information
    private void ValidateAndSetContactInformation(ContactInformation contactInformation)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(contactInformation, nameof(ContactInformation)));
        
        // Setting property
        ContactInformation = contactInformation;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Code)}: <omitted>, " +
               $"{nameof(NormalizedCode)}: <omitted>, " +
               $"{nameof(Name)}: {Name}, " +
               $"{nameof(Abbreviation)}: {Abbreviation}, " +
               $"{nameof(Owner)}: {Owner}, " +
               $"{nameof(MedicalField)}: {MedicalField}, " +
               $"{nameof(ContactInformation)}: {ContactInformation}, " +
               $"{nameof(Address)}: {Address}, " +
               $"{nameof(Users)}: [{string.Join("| ", Users.Select(user => user.Id))}], " + 
               $"{nameof(Roles)}: [{string.Join("| ", Roles.Select(role => role.Id))}], " + 
               $"{nameof(Devices)}: [{string.Join("| ", Devices.Select(device => device.Id))}], " +
               $"{nameof(DeviceCategories)}: [{string.Join("| ", DeviceCategories.Select(deviceCategory => deviceCategory.Id))}], " +
               $"{nameof(Rooms)}: [{string.Join("| ", Rooms.Select(room => room.Id))}], " +
               $"{nameof(RoomCategories)}: [{string.Join("| ", RoomCategories.Select(roomCategory => roomCategory.Id))}], " +
               $"{nameof(Appointments)}: [{string.Join("| ", Appointments.Select(appointment => appointment.Id))}], " +
               $"{nameof(AppointmentCategories)}: [{string.Join("| ", AppointmentCategories.Select(appointmentCategory => appointmentCategory.Id))}], " +
               $"{nameof(Clinicians)}: [{string.Join("| ", Clinicians.Select(clinician => clinician.Id))}], " +
               $"{nameof(ClinicianCategories)}: [{string.Join("| ", ClinicianCategories.Select(clinicianCategory => clinicianCategory.Id))}], " +
               $"{nameof(Patients)}: [{string.Join("| ", Patients.Select(patient => patient.Id))}], " +
               $"{nameof(AppointmentProtocols)}: [{string.Join("| ", AppointmentProtocols.Select(appointmentProtocol => appointmentProtocol.Id))}], " +
               $"{nameof(Results)}: [{string.Join("| ", Results.Select(result => result.Id))}]";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Clinic other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}