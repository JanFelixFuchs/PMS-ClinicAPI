using Domain.Commons.Interfaces;
using Domain.Commons.Utils.Constants;
using Domain.Commons.Utils.Validation;
using Domain.Entities.IdentityEntities;
using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Commons.Base;

public abstract class CategoryBase<T> : IEntity, IDeletable where T : class, IEntity
{
    // Properties
    public Guid Id { get; }
    public Guid ClinicId { get; private set; }
    public Clinic Clinic { get; private set; } = null!;
    public string Name { get; protected set; } = string.Empty;
    public string Abbreviation { get; protected set; } = string.Empty;
    public string Color { get; protected set; } = string.Empty;
    public bool IsDeleted { get; protected set; }
    protected ICollection<T> Items { get; set; } = new List<T>();
    protected abstract string ItemsPropertyName { get; }
    
    // Constructor used by ef core and tests to initialize objects
    protected CategoryBase() { }
    
    // Standard constructor used to initialize objects 
    protected CategoryBase(
        Clinic clinic,
        string name,
        string abbreviation,
        string color)
    {
        // Initializing properties
        Id = Guid.NewGuid();
        ValidateAndSetClinic(clinic);
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        ValidateAndSetColor(color);
        IsDeleted = false;
    }
    
    
    /* - - - Behaviour methods - - - */
    // Method to update the state of the entity
    public void Update(
        string name,
        string abbreviation,
        string color)
    {
        // Checking deletion flag
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot update a deleted {GetType().Name}");
        
        // Updating properties
        ValidateAndSetName(name);
        ValidateAndSetAbbreviation(abbreviation);
        ValidateAndSetColor(color);
    }
    
    // Method to delete the entity
    public void Delete(ICollection<T> items)
    {
        // Validating
        if (IsDeleted)
            throw new InvalidOperationException($"Cannot delete an already deleted {GetType().Name}");
        if (items.Count > 0)
            throw new InvalidOperationException($"Cannot delete a {GetType().Name} that has {ItemsPropertyName}");
        
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
            ValidationConditions.HasMaximumLength(name, Lengths.CategoryName, nameof(Name)));
        
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
    
    // Method to validate and set the color
    private void ValidateAndSetColor(string color)
    {
        // Validating
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsNotNullEmptyOrWhitespace(color, nameof(Color)),
            ValidationConditions.IsMatchingRegex(color, RegexPatterns.Color, nameof(Color)));
        
        // Setting property
        Color = color;
    }
    
    
    /* - - - Object overrides - - - */
    // Method to convert the entity into a string
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, " +
               $"{nameof(Clinic)}: {ClinicId}, " +
               $"{nameof(Name)}: {Name}, " +
               $"{nameof(Abbreviation)}: {Abbreviation}, " +
               $"{nameof(Color)}: {Color}, " +
               $"{nameof(IsDeleted)}: {IsDeleted}, " + 
               $"{nameof(ItemsPropertyName)}: [{string.Join("| ", Items.Select(item => item.Id))}]";
    }
    
    // Method to compare the entity with another object
    public override bool Equals(object? comparisonObject)
    {
        return comparisonObject is CategoryBase<T> other && 
               GetType() == other.GetType() &&
               Id == other.Id;
    }
    
    // Method to generate a hash code for the entity
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }
}