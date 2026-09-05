using Domain.Common.Interfaces;

namespace Domain.Common.Utils.InvariantValidation;

public static class InvariantValidationConditions
{
    // Generic conditions
    public static InvariantValidationResult IsNotDeleted<T>(T input, string propertyName) where T : IDeletable => new(
        !input.IsDeleted, 
        $"{propertyName} must not be deleted");
    
    public static InvariantValidationResult IsNullOrNotDeleted<T>(T? input, string propertyName) where T : IDeletable => new(
        input == null || !input.IsDeleted, 
        $"{propertyName} must not be deleted");
    
    public static InvariantValidationResult IsNotArchived<T>(T input, string propertyName) where T: IArchivable => new(
        !input.IsArchived,
        $"{propertyName} must not be archived");
    
    public static InvariantValidationResult IsNullOrNotArchived<T>(T? input, string propertyName) where T : IArchivable => new(
        input == null || !input.IsArchived,
        $"{propertyName} must not be archived");

    
    // Enum conditions
    public static InvariantValidationResult IsExactEnumValue<T>(T enumInput, T expectedValue, string propertyName) where T : struct, Enum => new(
        EqualityComparer<T>.Default.Equals(enumInput, expectedValue),  
        $"{propertyName} must be {expectedValue}");

    
    // Guid conditions
    public static InvariantValidationResult IsExactGuidValue(Guid guidInput, Guid expectedValue, string propertyName) => new(
        guidInput == expectedValue,
        $"{propertyName} must be {expectedValue}");

    
    // Collection conditions
    public static InvariantValidationResult IsNotContainingDeletedElements<T>(ICollection<T> collectionInput, string propertyName) where T : IDeletable => new(
        collectionInput.All(element => !element.IsDeleted),  
        $"{propertyName} must not contain deleted elements");
    
    public static InvariantValidationResult IsNotContainingArchivedElements<T>(ICollection<T> collectionInput, string propertyName) where T : IArchivable => new(
        collectionInput.All(element => !element.IsArchived), 
        $"{propertyName} must not contain archived elements");
    
    public static InvariantValidationResult IsContainingElementsWithExactEnumValue<TElement, TEnum>(ICollection<TElement> collectionInput, Func<TElement, TEnum> enumSelector, TEnum expectedValue, string propertyName) where TEnum : struct, Enum => new(
        collectionInput.All(element => EqualityComparer<TEnum>.Default.Equals(enumSelector(element), expectedValue)),  
        $"{propertyName} must be {expectedValue}");
}