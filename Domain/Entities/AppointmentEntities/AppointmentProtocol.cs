using Domain.Common.Enums;
using Domain.Common.Interfaces;
using Domain.Common.Utils.Constants;
using Domain.Common.Utils.InvariantValidation;
using Domain.Common.Utils.PropertyValidation;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.PatientEntities;
using Domain.Entities.RoomEntities;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Entities.AppointmentEntities;

public class AppointmentProtocol : IEntity
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public DateTime DateOfAppointment { get; private set; }
    public DateTime? DateOfProcessingStart { get; private set; }
    public DateTime? DateOfProcessingCompletion { get; private set; }
    public string? Symptoms { get; private set; }
    public string? Diagnosis { get; private set; }
    public string? Treatment { get; private set; }
    public string? Remarks { get; private set; }
    public AppointmentProtocolStatus Status { get; private set; }
    public Guid AppointmentId { get; private set; }
    public Appointment Appointment { get; private set; } = null!;
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; } = null!;
    public Guid ClinicianId { get; private set; }
    public Clinician Clinician { get; private set; } = null!;
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; } = null!;
    public ICollection<Device> Devices { get; private set; } = new List<Device>();
    
    // Constructor used by ef core and tests to initialize objects
    protected AppointmentProtocol() { }
    
    // Standard constructor used to initialize objects
    public AppointmentProtocol(
        Clinic clinic,
        Appointment appointment,
        DateTime currentDateTime)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        DateOfProcessingStart = null;
        DateOfProcessingCompletion = null;
        Symptoms = null;
        Diagnosis = null;
        Treatment = null;
        Remarks = null;
        Status = AppointmentProtocolStatus.Undealt;
        ValidateAndSetAppointment(appointment);
        ValidateAndSetPatient(appointment.Patient);
        ValidateAndSetClinician(appointment.Clinicians.First());
        ValidateAndSetRoom(appointment.Room);
        ValidateAndSetDevices(appointment.Devices);
        ValidateAndSetDateOfAppointment(appointment.EndTime, currentDateTime);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string? symptoms,
        string? diagnosis,
        string? treatment,
        string? remarks,
        Clinician clinician,
        Room room,
        ICollection<Device> devices)
    {
        // Checking status
        if (Status != AppointmentProtocolStatus.Started)
            throw new InvalidOperationException($"Cannot update an {nameof(AppointmentProtocol)} that is not {nameof(AppointmentProtocolStatus.Started)}");
        
        // Updating properties
        ValidateAndSetSymptoms(symptoms);
        ValidateAndSetDiagnosis(diagnosis);
        ValidateAndSetTreatment(treatment);
        ValidateAndSetRemarks(remarks);
        ValidateAndSetClinician(clinician);
        ValidateAndSetRoom(room);
        ValidateAndSetDevices(devices);
    }
    
    // Method to set the status to started
    public void Start(DateTime currentDateTime)
    {
        // Checking status
        if (Status != AppointmentProtocolStatus.Undealt)
            throw new InvalidOperationException($"Cannot start an {nameof(AppointmentProtocol)} that is not {nameof(AppointmentProtocolStatus.Undealt)}");
            
        // Setting properties
        Status = AppointmentProtocolStatus.Started;
        DateOfProcessingStart = currentDateTime;
    }
    
    // Method to set the status to completed
    public void Complete(DateTime currentDateTime)
    {
        // Checking status
        if (Status != AppointmentProtocolStatus.Started)
            throw new InvalidOperationException($"Cannot complete an {nameof(AppointmentProtocol)} that is not {nameof(AppointmentProtocolStatus.Started)}");
        
        // Setting properties
        DateOfProcessingCompletion = currentDateTime;
        Status = AppointmentProtocolStatus.Completed;
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
    
    // Method to validate and set the date of appointment
    private void ValidateAndSetDateOfAppointment(DateTime appointmentDate, DateTime currentDateTime)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(appointmentDate, nameof(DateOfAppointment)),
            () => PropertyValidationConditions.IsDateTimeInThePast(appointmentDate, currentDateTime, nameof(DateOfAppointment)));
        
        // Setting property
        DateOfAppointment = appointmentDate.Date;
    }
    
    // Method to validate and set the symptoms
    private void ValidateAndSetSymptoms(string? symptoms)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNullNotEmptyOrWhitespace(symptoms, nameof(Symptoms)),
            () => PropertyValidationConditions.IsNullOrHasMaximumLength(symptoms, Lengths.Symptoms, nameof(Symptoms)));

        // Setting property
        Symptoms = symptoms;
    }
    
    // Method to validate and set the diagnosis
    private void ValidateAndSetDiagnosis(string? diagnosis)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNullNotEmptyOrWhitespace(diagnosis, nameof(Diagnosis)),
            () => PropertyValidationConditions.IsNullOrHasMaximumLength(diagnosis, Lengths.Diagnosis, nameof(Diagnosis)));

        // Setting property
        Diagnosis = diagnosis;
    }
    
    // Method to validate and set the treatment
    private void ValidateAndSetTreatment(string? treatment)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNullNotEmptyOrWhitespace(treatment, nameof(Treatment)),
            () => PropertyValidationConditions.IsNullOrHasMaximumLength(treatment, Lengths.Treatment, nameof(Treatment)));

        // Setting property
        Treatment = treatment;
    }
    
    // Method to validate and set the remarks
    private void ValidateAndSetRemarks(string? remarks)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNullNotEmptyOrWhitespace(remarks, nameof(Remarks)),
            () => PropertyValidationConditions.IsNullOrHasMaximumLength(remarks, Lengths.AppointmentProtocolRemarks, nameof(Remarks)));

        // Setting property
        Remarks = remarks;
    }
    
    // Method to validate and set the appointment
    private void ValidateAndSetAppointment(Appointment appointment)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(appointment, nameof(Appointment)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            () => InvariantValidationConditions.IsNotDeleted(appointment, nameof(Appointment)),
            () => InvariantValidationConditions.IsExactEnumValue(appointment.Status, AppointmentStatus.Attended, nameof(Appointment)));
        
        // Setting properties
        Appointment = appointment;
        AppointmentId = appointment.Id;
    }
    
    // Method to validate and set the patient
    private void ValidateAndSetPatient(Patient patient)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(patient, nameof(Patient)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            () => InvariantValidationConditions.IsNotDeleted(patient, nameof(Patient)),
            () => InvariantValidationConditions.IsNotArchived(patient, nameof(Patient)));
        
        // Setting properties
        Patient = patient;
        PatientId = patient.Id;
    }
    
    // Method to validate and set the clinician
    private void ValidateAndSetClinician(Clinician clinician)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(clinician, nameof(Clinician)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            () => InvariantValidationConditions.IsNotDeleted(clinician, nameof(Clinician)),
            () => InvariantValidationConditions.IsNotArchived(clinician, nameof(Clinician)));
        
        // Setting properties
        Clinician = clinician;
        ClinicianId = clinician.Id;
    }
    
    // Method to validate and set the room
    private void ValidateAndSetRoom(Room room)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(room, nameof(Room)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            () => InvariantValidationConditions.IsNotDeleted(room, nameof(Room)),
            () => InvariantValidationConditions.IsNotArchived(room, nameof(Room)));
        
        // Setting properties
        Room = room;
        RoomId = room.Id;
    }
    
    // Method to validate and set the devices
    private void ValidateAndSetDevices(ICollection<Device> devices)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            () => PropertyValidationConditions.IsNotNull(devices, nameof(Devices)),
            () => PropertyValidationConditions.IsNotContainingDuplicates(devices, nameof(Devices)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructInvariantValidation(
            () => InvariantValidationConditions.IsNotContainingDeletedElements(devices, nameof(Devices)),
            () => InvariantValidationConditions.IsNotContainingArchivedElements(devices, nameof(Devices)));
        
        // Setting property
        Devices = devices;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(DateOfAppointment)}: {DateOfAppointment}, " +
               $"{nameof(DateOfProcessingStart)}: {DateOfProcessingStart}, " +
               $"{nameof(DateOfProcessingCompletion)}: {DateOfProcessingCompletion}, " +
               $"{nameof(Symptoms)}: {Symptoms}, " +
               $"{nameof(Diagnosis)}: {Diagnosis}, " +
               $"{nameof(Treatment)}: {Treatment}, " +
               $"{nameof(Remarks)}: {Remarks}, " +               
               $"{nameof(Status)}: {Status}, " +
               $"{nameof(Appointment)}: {AppointmentId}, " +
               $"{nameof(Patient)}: {PatientId}, " +
               $"{nameof(Clinician)}: {ClinicianId}, " +
               $"{nameof(Room)}: {RoomId}, " +
               $"{nameof(Devices)}: [{string.Join("| ", Devices.Select(device => device.Id))}]";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is AppointmentProtocol other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}