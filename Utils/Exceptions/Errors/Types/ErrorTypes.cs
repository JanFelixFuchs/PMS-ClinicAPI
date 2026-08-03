namespace Utils.Exceptions.Errors.Types;

// Resharper disable InconsistentNaming
public enum ErrorType
{
    // Success message
    SUCCESS,
    
    // Errors related to invalid operation
    INVALID_OPERATION,
    
    // Errors related to validation
    VALIDATION_ERROR,
    
    // Errors related to non-existent resources
    NOT_FOUND,
    
    // Errors related to property changes
    INCORRECT_PROPERTY_VALUE,
    PROPERTY_VALUE_ALREADY_IN_USE,
    PROPERTY_VALUE_UNCHANGED,
    
    // Errors related to authorization
    AUTHORIZATION_FAILED,
    
    // Errors related to unknown errors
    INTERNAL_ERROR
}