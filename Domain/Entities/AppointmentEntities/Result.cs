using Domain.Common.Enums;
using Domain.Common.Interfaces;
using Domain.Common.Utils.Constants;
using Domain.Common.Utils.Helper;
using Domain.Common.Utils.InvariantValidation;
using Domain.Common.Utils.Validation;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.PatientEntities;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

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
    public AppendixContentType AppendixContentType { get; private set; }
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
        Device? device,
        DateTime currentDateTime)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetTitle(title);
        ValidateAndSetDateOfCreation(dateOfCreation, currentDateTime);
        ValidateAndSetAppendixAndAppendixContentType(appendix);
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
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(clinic, nameof(Clinic)));
        
        // Setting properties
        Clinic = clinic;
        ClinicId = clinic.Id;
    }
    
    // Method to validate and set the title
    private void ValidateAndSetTitle(string title)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(title, nameof(Title)),
            PropertyValidationConditions.HasMaximumLength(title, Lengths.ResultTitle, nameof(Title)));
        
        // Setting property
        Title = title;
    }
    
    // Method to validate and set the date of creation
    private void ValidateAndSetDateOfCreation(DateTime dateOfCreation, DateTime currentDateTime)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(dateOfCreation, nameof(DateOfCreation)),
            PropertyValidationConditions.IsDateTimeInThePast(dateOfCreation, currentDateTime, nameof(DateOfCreation)));
        
        // Setting property
        DateOfCreation = dateOfCreation.Date;
    }
    
    // Method to validate and set the appendix and appendix content type
    private void ValidateAndSetAppendixAndAppendixContentType(byte[] appendix)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(appendix, nameof(Appendix)),
            PropertyValidationConditions.IsNotEmpty(appendix, nameof(Appendix)),
            PropertyValidationConditions.HasMaximumLength(appendix, Lengths.Appendix, nameof(Appendix)));

        // Inferring appendix content type
        var appendixContentType = FileHelper.InferFileContentType(appendix, nameof(Appendix));
        
        // Setting properties
        AppendixContentType = appendixContentType;
        Appendix = appendix;
    }
    
    // Method to validate and set the remarks
    private void ValidateAndSetRemarks(string? remarks)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNullNotEmptyOrWhitespace(remarks, nameof(Remarks)),
            PropertyValidationConditions.IsNullOrHasMaximumLength(remarks, Lengths.ResultRemarks, nameof(Remarks)));

        // Setting property
        Remarks = remarks;
    }
    
    // Method to validate and set the patient
    private void ValidateAndSetPatient(Patient patient)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(patient, nameof(Patient)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            InvariantValidationConditions.IsNotDeleted(patient, nameof(Patient)),
            InvariantValidationConditions.IsNotArchived(patient, nameof(Patient)));
        
        // Setting properties
        Patient = patient;
        PatientId = patient.Id;
    }
    
    // Method to validate and set the clinician
    private void ValidateAndSetClinician(Clinician clinician)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(clinician, nameof(Clinician)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            InvariantValidationConditions.IsNotDeleted(clinician, nameof(Clinician)),
            InvariantValidationConditions.IsNotArchived(clinician, nameof(Clinician)));
        
        // Setting properties
        Clinician = clinician;
        ClinicianId = clinician.Id;
    } 
    
    // Method to validate and set the device
    private void ValidateAndSetDevice(Device? device)
    {
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            InvariantValidationConditions.IsNullOrNotDeleted(device, nameof(Device)),
            InvariantValidationConditions.IsNullOrNotArchived(device, nameof(Device)));
        
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