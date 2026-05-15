using Domain.Commons.Enums;
using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Validation;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.IdentityEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Entities.RoomEntities;

public class Room : IEntity, IDeletable, IArchivable
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Abbreviation { get; private set; } = string.Empty;
    public bool IsArchived { get; private set; }
    public bool IsDeleted { get; private set; }
    public ICollection<RoomCategory> RoomCategories { get; private set; } = new List<RoomCategory>(); 
    public string? RoomNumber { get; private set; }
    public string? Floor { get; private set; }
    public string? Building { get; private set; }
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
    public ICollection<AppointmentProtocol> AppointmentProtocols { get; private set; } = new List<AppointmentProtocol>();
    
    // Constructor used by ef core and tests to initialize objects
    protected Room() { }
    
    // Standard constructor used to initialize objects
    public Room(
        Clinic clinic,
        string name,
        string abbreviation,
        ICollection<RoomCategory> roomCategories,
        string? roomNumber,
        string? floor,
        string? building)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        IsArchived = false;
        IsDeleted = false;
        ValidateAndSetRoomCategories(roomCategories);
        ValidateAndSetRoomNumber(roomNumber);
        ValidateAndSetFloor(floor);
        ValidateAndSetBuilding(building);
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string name,
        string abbreviation,
        ICollection<RoomCategory> roomCategories,
        string? roomNumber,
        string? floor,
        string? building)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Room)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update an deleted {nameof(Room)}");
 
        // Updating properties
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        ValidateAndSetRoomCategories(roomCategories);
        ValidateAndSetRoomNumber(roomNumber);
        ValidateAndSetFloor(floor);
        ValidateAndSetBuilding(building);
    }
    
    // Method to add a room category
    public void AddRoomCategory(RoomCategory roomCategory)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Room)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update an deleted {nameof(Room)}");
        
        // Setting property
        ValidateAndSetRoomCategories(RoomCategories.Append(roomCategory).ToList());
    }

    // Method to remove a room category
    public void RemoveRoomCategory(RoomCategory roomCategory)
    {
        // Checking archive and deletion flag
        if (IsArchived)
            throw new InvalidOperationException($"Cannot update an archived {nameof(Room)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update an deleted {nameof(Room)}");
        
        // Setting property
        ValidateAndSetRoomCategories(RoomCategories.Except(new List<RoomCategory> { roomCategory }).ToList());
    }
    
    // Method to archive the entity
    public void Archive(ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols)
    {
        // Validating
        if (IsArchived)
            throw new InvalidOperationException($"Cannot archive an already archived {nameof(Room)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot archive a deleted {nameof(Room)}");
        if (appointments.Any(appointment => appointment.Status != AppointmentStatus.Completed))
            throw new InvalidOperationException($"Cannot archive a {nameof(Room)} that has uncompleted {nameof(Appointments)}");
        if (appointmentProtocols.Any(appointmentProtocol => appointmentProtocol.Status != AppointmentProtocolStatus.Completed))
            throw new InvalidOperationException($"Cannot archive a {nameof(Room)} that has uncompleted {nameof(AppointmentProtocols)}");
        
        // Setting property
        IsArchived = true;
    }
    
    // Method to unarchive the entity
    public void Unarchive()
    {
        // Validating
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot unarchive an unarchived {nameof(Room)}");
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot unarchive a deleted {nameof(Room)}");
        
        // Setting property
        IsArchived = false;
    }
    
    // Method to delete the entity
    public void Delete(ICollection<Appointment> appointments, ICollection<AppointmentProtocol> appointmentProtocols)
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {nameof(Room)}");
        if (!IsArchived)
            throw new InvalidOperationException($"Cannot delete an unarchived {nameof(Room)}");
        if (appointments.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Room)} that has {nameof(Appointments)}");
        if (appointmentProtocols.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {nameof(Room)} that has {nameof(AppointmentProtocols)}");
        
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

    // Method to validate and set the name
    private void ValidateAndSetName(string name)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(name, nameof(Name)),
            ValidationConditions.HasMaximumLength(name, Lengths.RoomName, nameof(Name)));
        
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

        // Setting abbreviation
        Abbreviation = abbreviation;
    }

    // Method to validate and set the room categories
    private void ValidateAndSetRoomCategories(ICollection<RoomCategory> roomCategories)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNull(roomCategories, nameof(RoomCategories)),
            ValidationConditions.IsNotContainingDuplicates(roomCategories, nameof(RoomCategories)),
            ValidationConditions.IsNotContainingDeletedElements(roomCategories, nameof(RoomCategories)));
        
        // Setting property
        RoomCategories = roomCategories;
    }

    // Method to validate and set the room number
    private void ValidateAndSetRoomNumber(string? roomNumber)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullNotEmptyOrWhitespace(roomNumber, nameof(RoomNumber)),
            ValidationConditions.IsNullOrHasMaximumLength(roomNumber, Lengths.RoomNumber, nameof(RoomNumber)));
        
        // Setting property
        RoomNumber = roomNumber;
    }

    // Method to validate and set the floor
    private void ValidateAndSetFloor(string? floor)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullNotEmptyOrWhitespace(floor, nameof(Floor)),
            ValidationConditions.IsNullOrHasMaximumLength(floor, Lengths.Floor, nameof(Floor)));
        
        // Setting property
        Floor = floor;
    }

    // Method to validate and set the building
    private void ValidateAndSetBuilding(string? building)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNullNotEmptyOrWhitespace(building, nameof(Building)),
            ValidationConditions.IsNullOrHasMaximumLength(building, Lengths.Building, nameof(Building)));
        
        // Setting property
        Building = building;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(Name)}: {Name}, " +
               $"{nameof(Abbreviation)}: {Abbreviation}, " +
               $"{nameof(IsArchived)}: {IsArchived}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " +
               $"{nameof(RoomCategories)}: [{string.Join("| ", RoomCategories.Select(roomCategory => roomCategory.Id))}], " +
               $"{nameof(RoomNumber)}: {RoomNumber}, " +
               $"{nameof(Floor)}: {Floor}, " +
               $"{nameof(Building)}: {Building}" +
               $"{nameof(Appointments)}: [{string.Join("| ", Appointments.Select(appointment => appointment.Id))}], " + 
               $"{nameof(AppointmentProtocols)}: [{string.Join("| ", AppointmentProtocols.Select(appointmentProtocol => appointmentProtocol.Id))}]";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is Room other && Id == other.Id;
    }

    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}