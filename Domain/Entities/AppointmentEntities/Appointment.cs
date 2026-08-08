using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Invariants;
using Domain.Commons.Utils.Validation;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.PatientEntities;
using Domain.Entities.RoomEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.AppointmentEntities;

public class Appointment : IEntity, IDeletable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public bool IsDeleted { get; private set; }
    public ICollection<AppointmentCategory> AppointmentCategories { get; private set; } = new List<AppointmentCategory>();
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; } = null!;
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; } = null!;
    public ICollection<Device> Devices { get; private set; } = new List<Device>();
    public ICollection<Clinician> Clinicians { get; private set; } = new List<Clinician>();
    public AppointmentProtocol? AppointmentProtocol { get; private set; }

    // Constructor used by ef core and tests to initialize objects
    protected Appointment() { }
    
    // Standard constructor used to initialize objects 
    public Appointment(
        Clinic clinic,
        string title,
        DateTime startTime,
        DateTime endTime,
        ICollection<AppointmentCategory> appointmentCategories,
        Patient patient,
        Room room,
        ICollection<Device> devices,
        ICollection<Clinician> clinicians,
        DateTime currentDateTime)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetTitle(title);
        ValidateAndSetDateTimes(startTime, endTime, currentDateTime);
        Status = AppointmentStatus.Planned;
        IsDeleted = false;
        ValidateAndSetAppointmentCategories(appointmentCategories);
        ValidateAndSetPatient(patient);
        ValidateAndSetRoom(room);
        ValidateAndSetDevices(devices);
        ValidateAndSetClinicians(clinicians);
    }
    

    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string title,
        DateTime startTime,
        DateTime endTime,
        ICollection<AppointmentCategory> appointmentCategories,
        Patient patient,
        Room room,
        ICollection<Device> devices,
        ICollection<Clinician> clinicians,
        DateTime currentDateTime)
    {
        // Checking deletion flag and status
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Appointment)}");
        if (Status == AppointmentStatus.Attended)
            throw new InvalidOperationException($"Cannot update an {nameof(Appointment)} that is {nameof(AppointmentStatus.Attended)}");

        // Updating properties
        ValidateAndSetTitle(title);
        ValidateAndSetDateTimes(startTime, endTime, currentDateTime);
        ValidateAndSetAppointmentCategories(appointmentCategories);
        ValidateAndSetPatient(patient);
        ValidateAndSetRoom(room);
        ValidateAndSetDevices(devices);
        ValidateAndSetClinicians(clinicians);
    }
    
    // Method to set the status to attended
    public void MarkAsAttended(DateTime currentDateTime)
    {
        // Checking deletion flag status and time
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot complete a deleted {nameof(Appointment)}");
        if (Status == AppointmentStatus.Attended)
            throw new InvalidOperationException($"Cannot mark an {nameof(Appointment)} as attended that is {nameof(AppointmentStatus.Attended)}");
        if (StartTime > currentDateTime)
            throw new InvalidOperationException($"Cannot mark an {nameof(Appointment)} as attended that has not started");

        // Setting property
        Status = AppointmentStatus.Attended;
    }
    
    // Method to delete the entity
    public void Delete()
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Appointment)}");
        if (Status is AppointmentStatus.Attended)
            throw new InvalidOperationException($"Cannot delete an {nameof(Appointment)} that is {nameof(AppointmentStatus.Attended)}");
        
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
            PropertyValidationConditions.HasMaximumLength(title, Lengths.AppointmentTitle, nameof(Title)));
        
        // Setting property
        Title = title;
    }
    
    // Method to validate and set the start time and end time
    private void ValidateAndSetDateTimes(DateTime startTime, DateTime endTime, DateTime currentDateTime)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(startTime, nameof(StartTime)),
            PropertyValidationConditions.IsNotNull(endTime, nameof(EndTime)),
            PropertyValidationConditions.AreIdenticalDates(startTime, endTime, nameof(StartTime), nameof(EndTime)),
            PropertyValidationConditions.IsDateTimeInTheFuture(startTime, currentDateTime, nameof(StartTime)),
            PropertyValidationConditions.IsDateTimeInTheFuture(endTime, currentDateTime, nameof(EndTime)),
            PropertyValidationConditions.AreDateTimesInOrder(startTime, endTime, nameof(StartTime), nameof(EndTime)));

        // Setting properties
        StartTime = startTime;
        EndTime = endTime;
    }
    
    // Method to validate and set the appointment categories
    private void ValidateAndSetAppointmentCategories(ICollection<AppointmentCategory> appointmentCategories)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(appointmentCategories, nameof(AppointmentCategories)),
            PropertyValidationConditions.IsNotContainingDuplicates(appointmentCategories, nameof(AppointmentCategories)));

        // Invariant validation
        InvariantValidationHelper.ConstructionInvariantValidation(
            InvariantValidationConditions.IsNotContainingDeletedElements(appointmentCategories, nameof(AppointmentCategories)));

        // Setting property
        AppointmentCategories = appointmentCategories;
    }
    
    // Method to validate and set the patient
    private void ValidateAndSetPatient(Patient patient)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(patient, nameof(Patient)));

        // Invariant validation
        InvariantValidationHelper.ConstructionInvariantValidation(
            InvariantValidationConditions.IsNotDeleted(patient, nameof(Patient)),
            InvariantValidationConditions.IsNotArchived(patient, nameof(Patient)));

        // Setting properties
        Patient = patient;
        PatientId = patient.Id;
    }
    
    // Method to validate and set the room
    private void ValidateAndSetRoom(Room room)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(room, nameof(Room)));

        // Invariant validation
        InvariantValidationHelper.ConstructionInvariantValidation(
            InvariantValidationConditions.IsNotDeleted(room, nameof(Room)),
            InvariantValidationConditions.IsNotArchived(room, nameof(Room)));

        // Setting properties
        Room = room;
        RoomId = room.Id;
    }

    // Method to validate and set the devices
    private void ValidateAndSetDevices(ICollection<Device> devices)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(devices, nameof(Devices)),
            PropertyValidationConditions.IsNotContainingDuplicates(devices, nameof(Devices)));

        // Invariant validation
        InvariantValidationHelper.ConstructionInvariantValidation(
            InvariantValidationConditions.IsNotContainingDeletedElements(devices, nameof(Devices)),
            InvariantValidationConditions.IsNotContainingArchivedElements(devices, nameof(Devices)),
            InvariantValidationConditions.IsContainingElementsWithExactEnumValue(devices, device => device.Status, DeviceStatus.Operational, nameof(Devices)));

        // Setting property
        Devices = devices;
    }

    // Method to validate and set the clinicians
    private void ValidateAndSetClinicians(ICollection<Clinician> clinicians)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(clinicians, nameof(Clinicians)),
            PropertyValidationConditions.IsNotEmpty(clinicians, nameof(Clinicians)),
            PropertyValidationConditions.IsNotContainingDuplicates(clinicians, nameof(Clinicians)));

        // Invariant validation
        InvariantValidationHelper.ConstructionInvariantValidation(
            InvariantValidationConditions.IsNotContainingDeletedElements(clinicians, nameof(Clinicians)),
            InvariantValidationConditions.IsNotContainingArchivedElements(clinicians, nameof(Clinicians)));

        // Setting property
        Clinicians = clinicians;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(Title)}: {Title}, " +
               $"{nameof(StartTime)}: {StartTime}, " +
               $"{nameof(EndTime)}: {EndTime}, " +
               $"{nameof(Status)}: {Status}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(AppointmentCategories)}: [{string.Join("| ", AppointmentCategories.Select(appointmentCategory => appointmentCategory.Id))}], " +
               $"{nameof(Patient)}: {PatientId}, " +
               $"{nameof(Room)}: {RoomId}, " +
               $"{nameof(Devices)}: [{string.Join("| ", Devices.Select(device => device.Id))}], " +
               $"{nameof(Clinicians)}: [{string.Join("| ", Clinicians.Select(clinician => clinician.Id))}], " +
               $"{nameof(AppointmentProtocol)}: {AppointmentProtocol?.Id}";
    }

    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Appointment other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}