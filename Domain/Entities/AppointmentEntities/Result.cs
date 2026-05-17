using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Validation;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.PatientEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.AppointmentEntities;

public class Result : IEntity, IDeletable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string Title { get; private set; } = string.Empty;
    public DateTime DateOfCreation { get; private set; }
    public byte[] Appendix { get; private set; } = [];
    public bool IsDeleted { get; private set; }
    public string? Remarks { get; private set; }
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; } = null!;
    public Guid ClinicianId { get; private set; }
    public Clinician Clinician { get; private set; } = null!;
    public Guid? DeviceId { get; private set; }
    public Device? Device { get; private set; }
    
    // Constructor used by ef core and test to initialize objects
    protected Result() { }
    
    // Standard constructor used to initialize objects
    public Result(
        Clinic clinic,
        string title,
        DateTime dateOfCreation,
        byte[] appendix,
        string? remarks,
        Patient patient,
        Clinician clinician,
        Device? device)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetTitle(title);
        ValidateAndSetDateOfCreation(dateOfCreation);
        ValidateAndSetAppendix(appendix);
        IsDeleted = false;
        ValidateAndSetRemarks(remarks);
        ValidateAndSetPatient(patient);
        ValidateAndSetClinician(clinician);
        ValidateAndSetDevice(device);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to delete the entity
    public void Delete(Patient patient)
    {
        // Validating
        if (IsDeleted) 
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Result)}");
        if (patient.IsArchived)
            throw new InvalidOperationException($"Cannot delete an {nameof(Result)} that is associated with an archived {nameof(Patient)}");
        
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
    
    // Method to validate and set the title
    private void ValidateAndSetTitle(string title)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(title, nameof(Title)),
            ValidationConditions.HasMaximumLength(title, Lengths.Title, nameof(Title)));
        
        // Setting property
        Title = title;
    }
    
    // Method to validate and set the date of creation
    private void ValidateAndSetDateOfCreation(DateTime dateOfCreation)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(dateOfCreation, nameof(DateOfCreation)),
            ValidationConditions.IsDateInThePast(dateOfCreation, nameof(DateOfCreation)));
        
        // Setting property
        DateOfCreation = dateOfCreation;
    }
    
    // Method to validate and set the appendix
    private void ValidateAndSetAppendix(byte[] appendix)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(appendix, nameof(Appendix)),
            ValidationConditions.IsNotEmpty(appendix, nameof(Appendix)),
            ValidationConditions.HasMaximumLength(appendix, Lengths.Appendix, nameof(Appendix), $"{nameof(Appendix)} must not exceed maximum file size"));

        // Setting property
        Appendix = appendix;
    }
    
    // Method to validate and set the remarks
    private void ValidateAndSetRemarks(string? remarks)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullNotEmptyOrWhitespace(remarks, nameof(Remarks)),
            ValidationConditions.IsNullOrHasMaximumLength(remarks, Lengths.ResultRemarks, nameof(Remarks)));

        // Setting property
        Remarks = remarks;
    }
    
    // Method to validate and set the patient
    private void ValidateAndSetPatient(Patient patient)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(patient, nameof(Patient)),
            ValidationConditions.IsNotDeleted(patient, nameof(Patient)),
            ValidationConditions.IsNotArchived(patient, nameof(Patient)));
        
        // Setting properties
        Patient = patient;
        PatientId = patient.Id;
    }
    
    // Method to validate and set the clinician
    private void ValidateAndSetClinician(Clinician clinician)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(clinician, nameof(Clinician)),
            ValidationConditions.IsNotDeleted(clinician, nameof(Clinician)),
            ValidationConditions.IsNotArchived(clinician, nameof(Clinician)));
        
        // Setting properties
        Clinician = clinician;
        ClinicianId = clinician.Id;
    } 
    
    // Method to validate and set the device
    private void ValidateAndSetDevice(Device? device)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullOrNotDeleted(device, nameof(Device)),
            ValidationConditions.IsNullOrNotArchived(device, nameof(Device)));
        
        // Setting properties
        Device = device;
        DeviceId = device?.Id;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(Title)}: {Title}, " +
               $"{nameof(DateOfCreation)}: {DateOfCreation}, " +
               $"{nameof(Appendix)}: <omitted>, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(Remarks)}: {Remarks}, " +
               $"{nameof(Patient)}: {PatientId}, " +
               $"{nameof(Clinician)}: {ClinicianId}, " +
               $"{nameof(Device)}: {DeviceId}";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Result other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}