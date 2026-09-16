using Domain.Common.Interfaces;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Common.Utils.InvariantValidation;

public static class InvariantValidationConditions
{
    // Generic conditions
    public static InvariantValidationResult IsNotDeleted<T>(T input, string propertyName) where T : IDeletable => new(
        !input.IsDeleted, 
        propertyName,
        ErrorCode.DELETED_ENTITY,
        $"{propertyName} must not be deleted");
    
    public static InvariantValidationResult IsNullOrNotDeleted<T>(T? input, string propertyName) where T : IDeletable => new(
        input == null || !input.IsDeleted, 
        propertyName,
        ErrorCode.DELETED_ENTITY,
        $"{propertyName} must not be deleted");
    
    public static InvariantValidationResult IsNotArchived<T>(T input, string propertyName) where T: IArchivable => new(
        !input.IsArchived,
        propertyName,
        ErrorCode.ARCHIVED_ENTITY,
        $"{propertyName} must not be archived");
    
    public static InvariantValidationResult IsNullOrNotArchived<T>(T? input, string propertyName) where T : IArchivable => new(
        input == null || !input.IsArchived,
        propertyName,
        ErrorCode.ARCHIVED_ENTITY,
        $"{propertyName} must not be archived");

    
    // Enum conditions
    public static InvariantValidationResult IsExactEnumValue<T>(T enumInput, T expectedValue, string propertyName) where T : struct, Enum => new(
        EqualityComparer<T>.Default.Equals(enumInput, expectedValue),  
        propertyName,
        ErrorCode.UNEXPECTED_STATUS,
        $"{propertyName} must be {expectedValue}");

    
    // Guid conditions
    public static InvariantValidationResult IsBelongingToSameClinic(Guid clinicId, Guid expectedClinicId, string propertyName) => new(
        clinicId == expectedClinicId,
        propertyName,
        ErrorCode.CLINIC_MISMATCH,
        $"{propertyName} must be {expectedClinicId}");
    
    public static InvariantValidationResult IsNullOrBelongingToSameClinic(Guid? clinicId, Guid expectedClinicId, string propertyName) => new(
        clinicId == null || clinicId == expectedClinicId,
        propertyName,
        ErrorCode.CLINIC_MISMATCH,
        $"{propertyName} must be {expectedClinicId}");

    
    // Collection conditions
    public static InvariantValidationResult IsNotContainingDeletedElements<T>(ICollection<T> collectionInput, string propertyName) where T : IDeletable => new(
        collectionInput.All(element => !element.IsDeleted),  
        propertyName,
        ErrorCode.DELETED_ENTITY,
        $"{propertyName} must not contain deleted elements");
    
    public static InvariantValidationResult IsNotContainingArchivedElements<T>(ICollection<T> collectionInput, string propertyName) where T : IArchivable => new(
        collectionInput.All(element => !element.IsArchived), 
        propertyName,
        ErrorCode.ARCHIVED_ENTITY,
        $"{propertyName} must not contain archived elements");
    
    public static InvariantValidationResult IsContainingOnlyElementsWithExactEnumValue<TElement, TEnum>(ICollection<TElement> collectionInput, Func<TElement, TEnum> enumSelector, TEnum expectedValue, string propertyName) where TEnum : struct, Enum => new(
        collectionInput.All(element => EqualityComparer<TEnum>.Default.Equals(enumSelector(element), expectedValue)),  
        propertyName,
        ErrorCode.UNEXPECTED_STATUS,
        $"{propertyName} must be {expectedValue}");

    public static InvariantValidationResult IsContainingOnlyElementsBelongingToSameClinic<T>(ICollection<T> collectionInput, Func<T, Guid> clinicIdSelector, Guid expectedClinicId, string propertyName) => new(
        collectionInput.All(element => clinicIdSelector(element) == expectedClinicId),
        propertyName,
        ErrorCode.CLINIC_MISMATCH,
        $"{propertyName} must be {expectedClinicId}");
}