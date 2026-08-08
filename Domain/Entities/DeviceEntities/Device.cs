using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Invariants;
using Domain.Commons.Utils.Validation;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.IdentityEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.DeviceEntities;

public class Device : IEntity, IDeletable, IArchivable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Abbreviation { get; private set; } = string.Empty;
    public string SerialNumber { get; private set; } = string.Empty;
    public DeviceStatus Status { get; private set; }
    public string Producer { get; private set; } = string.Empty;
    public bool IsArchived { get; private set; }
    public bool IsDeleted { get; private set; }
    public ICollection<DeviceCategory> DeviceCategories { get; private set; } = new List<DeviceCategory>();
    public DateTime? DateOfPurchase { get; private set; }
    public DateTime? DateOfLastMaintenance { get; private set; }
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>(); 
    public ICollection<AppointmentProtocol> AppointmentProtocols { get; private set; } = new List<AppointmentProtocol>();
    public ICollection<Result> Results { get; private set; } = new List<Result>();
    
    // Constructor used by ef core and tests to initialize objects
    protected Device() { }
    
    // Standard constructor used to initialize objects 
    public Device(
        Clinic clinic,
        string name,
        string abbreviation,
        string serialNumber,
        DeviceStatus status,
        string producer,
        ICollection<DeviceCategory> deviceCategories,
        DateTime? dateOfPurchase,
        DateTime? dateOfLastMaintenance,
        DateTime currentDateTime)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        ValidateAndSetSerialNumber(serialNumber);
        ValidateAndSetStatus(status);
        ValidateAndSetProducer(producer);
        IsArchived = false;
        IsDeleted = false;
        ValidateAndSetDeviceCategories(deviceCategories);
        ValidateAndSetDateOfPurchase(dateOfPurchase, currentDateTime);
        ValidateAndSetDateOfLastMaintenance(dateOfLastMaintenance, currentDateTime);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string name,
        string abbreviation,
        ICollection<DeviceCategory> deviceCategories,
        DateTime? dateOfLastMaintenance,
        DateTime currentDateTime)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Device)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Device)}");
        
        // Updating properties
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        ValidateAndSetDeviceCategories(deviceCategories);
        ValidateAndSetDateOfLastMaintenance(dateOfLastMaintenance, currentDateTime);
    }
    
    // Method to add a device category
    public void AddDeviceCategory(DeviceCategory deviceCategory)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Device)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Device)}");
        
        // Setting property
        ValidateAndSetDeviceCategories(DeviceCategories.Append(deviceCategory).ToList());
    }
    
    // Method to remove a device category
    public void RemoveDeviceCategory(DeviceCategory deviceCategory)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Device)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Device)}");
        
        // Setting property
        ValidateAndSetDeviceCategories(DeviceCategories.Except(new List<DeviceCategory> { deviceCategory }).ToList());
    }
    
    // Method to change the status
    public void ChangeStatus(DeviceStatus status, ICollection<Appointment> appointments, DateTime currentDateTime)
    {
        // Checking archive and deletion flag and future appointments
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Device)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {nameof(Device)}");
        if (status != DeviceStatus.Operational && appointments.Any(appointment => appointment.EndTime > currentDateTime))
            throw new InvalidOperationException($"Cannot change the status of a {nameof(Device)} that has future {nameof(Appointments)}");
        
        // Setting property
        ValidateAndSetStatus(status);
    }
    
    // Method to archive the entity
    public void Archive(ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot archive an already archived {nameof(Device)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot archive a deleted {nameof(Device)}");
        if (appointments.Any(appointment => appointment.Status != AppointmentStatus.Attended))
            throw new InvalidOperationException($"Cannot archive a {nameof(Device)} that has unattended {nameof(Appointments)}");
        if (appointmentProtocols.Any(appointmentProtocol => appointmentProtocol.Status != AppointmentProtocolStatus.Completed))
            throw new InvalidOperationException($"Cannot archive a {nameof(Device)} that has uncompleted {nameof(AppointmentProtocols)}");
            
        // Setting property
        IsArchived = true;
    }
    
    // Method to unarchive the entity
    public void Unarchive()
    {
        // Validating
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot unarchive an unarchived {nameof(Device)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot unarchive a deleted {nameof(Device)}");
        
        // Setting property
        IsArchived = false;
    }
    
    // Method to delete the entity
    public void Delete(ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols, ICollection<Result> results)
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Device)}");
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot delete an unarchived {nameof(Device)}");
        if (appointments.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Device)} that has {nameof(Appointments)}");
        if (appointmentProtocols.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Device)} that has {nameof(AppointmentProtocols)}");
        if (results.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Device)} that has {nameof(Results)}");
        
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
    
    // Method to validate and set the name
    private void ValidateAndSetName(string name)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(name, nameof(Name)),
            PropertyValidationConditions.HasMaximumLength(name, Lengths.DeviceName, nameof(Name)));
        
        // Setting property
        Name = name;
    }
    
    // Method to validate and set the abbreviation
    private void ValidateAndSetAbbreviation(string abbreviation)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(abbreviation, nameof(Abbreviation)),
            PropertyValidationConditions.HasMaximumLength(abbreviation, Lengths.Abbreviation, nameof(Abbreviation)));
        
        // Setting property
        Abbreviation = abbreviation;
    }
    
    // Method to validate and set the serial number
    private void ValidateAndSetSerialNumber(string serialNumber)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(serialNumber, nameof(SerialNumber)),
            PropertyValidationConditions.HasMaximumLength(serialNumber, Lengths.SerialNumber, nameof(SerialNumber)));
        
        // Setting property
        SerialNumber = serialNumber;
    }
    
    // Method to validate and set the status
    private void ValidateAndSetStatus(DeviceStatus status)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsDefinedEnum(status, nameof(Status)));
        
        // Setting property
        Status = status;
    }
    
    // Method to validate and set the producer
    private void ValidateAndSetProducer(string producer)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNullEmptyOrWhitespace(producer, nameof(Producer)),
            PropertyValidationConditions.HasMaximumLength(producer, Lengths.Producer, nameof(Producer)));
        
        // Setting property
        Producer = producer;
    }
    
    // Method to validate and set the device categories
    private void ValidateAndSetDeviceCategories(ICollection<DeviceCategory> deviceCategories)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNotNull(deviceCategories, nameof(DeviceCategories)),
            PropertyValidationConditions.IsNotContainingDuplicates(deviceCategories, nameof(DeviceCategories)));
        
        // Invariant validation
        InvariantValidationHelper.ConstructionInvariantValidation(
            InvariantValidationConditions.IsNotContainingDeletedElements(deviceCategories, nameof(DeviceCategories)));
        
        // Setting property
        DeviceCategories = deviceCategories;
    }
    
    // Method to validate and set the date of purchase
    private void ValidateAndSetDateOfPurchase(DateTime? dateOfPurchase, DateTime currentDateTime)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNullOrDateTimeInThePast(dateOfPurchase, currentDateTime, nameof(DateOfPurchase)));
        
        // Setting property
        DateOfPurchase = dateOfPurchase?.Date;
    }
        
    // Method to validate and set the date of last maintenance
    private void ValidateAndSetDateOfLastMaintenance(DateTime? dateOfLastMaintenance, DateTime currentDateTime)
    {
        // Property validation
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsNullOrDateTimeInThePast(dateOfLastMaintenance, currentDateTime, nameof(DateOfLastMaintenance)));
        
        // Setting property
        DateOfLastMaintenance = dateOfLastMaintenance?.Date;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(Name)}: {Name}, " +
               $"{nameof(Abbreviation)}: {Abbreviation}, " +
               $"{nameof(SerialNumber)}: {SerialNumber}, " +
               $"{nameof(Status)}: {Status}, " +
               $"{nameof(IsArchived)}: {IsArchived}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(DeviceCategories)}: [{string.Join("| ", DeviceCategories.Select(deviceCategory => deviceCategory.Id))}], " +
               $"{nameof(Producer)}: {Producer}, " +
               $"{nameof(DateOfPurchase)}: {DateOfPurchase}, " +
               $"{nameof(DateOfLastMaintenance)}: {DateOfLastMaintenance}, " +
               $"{nameof(Appointments)}: [{string.Join("| ", Appointments.Select(appointment => appointment.Id))}], " +
               $"{nameof(AppointmentProtocols)}: [{string.Join("| ", AppointmentProtocols.Select(appointmentProtocol => appointmentProtocol.Id))}], " +
               $"{nameof(Results)}: [{string.Join("| ", Results.Select(result => result.Id))}]";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Device other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}