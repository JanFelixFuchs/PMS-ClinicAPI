using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
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
        ICollection<Clinician> clinicians)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetTitle(title);
        ValidateAndSetDateTimes(startTime, endTime);
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
        ICollection<Clinician> clinicians)
    {
        // Checking deletion flag and status
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Appointment)}");
        if (Status == AppointmentStatus.Completed)
            throw new InvalidOperationException($"Cannot update an {nameof(Appointment)} that is {nameof(AppointmentStatus.Completed)}");

        // Updating properties
        ValidateAndSetTitle(title);
        ValidateAndSetDateTimes(startTime, endTime);
        ValidateAndSetAppointmentCategories(appointmentCategories);
        ValidateAndSetPatient(patient);
        ValidateAndSetRoom(room);
        ValidateAndSetDevices(devices);
        ValidateAndSetClinicians(clinicians);
    }
    
    // Method to set the status to held
    public void Complete()
    {
        // Checking deletion flag status and time
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot complete a deleted {nameof(Appointment)}");
        if (Status == AppointmentStatus.Completed)
            throw new InvalidOperationException($"Cannot complete an {nameof(Appointment)} that is {nameof(AppointmentStatus.Completed)}");
        if (EndTime > DateTime.UtcNow)
            throw new InvalidOperationException($"Cannot complete an {nameof(Appointment)} that has not ended");

        // Setting property
        Status = AppointmentStatus.Completed;
    }
    
    // Method to delete the entity
    public void Delete()
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Appointment)}");
        if (Status is AppointmentStatus.Completed)
            throw new InvalidOperationException($"Cannot delete an {nameof(Appointment)} that is {nameof(AppointmentStatus.Completed)}");
        
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
            ValidationConditions.HasMaximumLength(title, Lengths.AppointmentTitle, nameof(Title)));
        
        // Setting property
        Title = title;
    }
    
    // Method to validate and set the start time and end time
    private void ValidateAndSetDateTimes(DateTime startTime, DateTime endTime)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(startTime, nameof(StartTime)),
            ValidationConditions.IsNotNull(endTime, nameof(EndTime)),
            ValidationConditions.AreIdenticalDates(startTime, endTime, nameof(StartTime), nameof(EndTime)),
            ValidationConditions.IsDateTimeInTheFuture(startTime, nameof(StartTime)),
            ValidationConditions.IsDateTimeInTheFuture(endTime, nameof(EndTime)),
            ValidationConditions.AreDatesInOrder(startTime, endTime, nameof(StartTime), nameof(EndTime)));

        // Setting properties
        StartTime = startTime;
        EndTime = endTime;
    }
    
    // Method to validate and set the appointment categories
    private void ValidateAndSetAppointmentCategories(ICollection<AppointmentCategory> appointmentCategories)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(appointmentCategories, nameof(AppointmentCategories)),
            ValidationConditions.IsNotContainingDuplicates(appointmentCategories, nameof(AppointmentCategories)),
            ValidationConditions.IsNotContainingDeletedElements(appointmentCategories, nameof(AppointmentCategories)));

        // Setting property
        AppointmentCategories = appointmentCategories;
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
    
    // Method to validate and set the room
    private void ValidateAndSetRoom(Room room)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(room, nameof(Room)),
            ValidationConditions.IsNotDeleted(room, nameof(Room)),
            ValidationConditions.IsNotArchived(room, nameof(Room)));

        // Setting properties
        Room = room;
        RoomId = room.Id;
    }

    // Method to validate and set the devices
    private void ValidateAndSetDevices(ICollection<Device> devices)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(devices, nameof(Devices)),
            ValidationConditions.IsNotContainingDuplicates(devices, nameof(Devices)),
            ValidationConditions.IsNotContainingDeletedElements(devices, nameof(Devices)),
            ValidationConditions.IsNotContainingArchivedElements(devices, nameof(Devices)),
            ValidationConditions.IsContainingElementsWithExactEnumValue(devices, device => device.Status, DeviceStatus.Operational, nameof(Devices)));

        // Setting property
        Devices = devices;
    }

    // Method to validate and set the clinicians
    private void ValidateAndSetClinicians(ICollection<Clinician> clinicians)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(clinicians, nameof(Clinicians)),
            ValidationConditions.IsNotEmpty(clinicians, nameof(Clinicians)),
            ValidationConditions.IsNotContainingDuplicates(clinicians, nameof(Clinicians)),
            ValidationConditions.IsNotContainingDeletedElements(clinicians, nameof(Clinicians)),
            ValidationConditions.IsNotContainingArchivedElements(clinicians, nameof(Clinicians)));

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