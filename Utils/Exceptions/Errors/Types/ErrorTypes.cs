namespace Utils.Exceptions.Errors.Types;

// Resharper disable InconsistentNaming
public enum ErrorType
{
    SUCCESS,
    
    INVALID_OPERATION,
    
    AUTHORIZATION_FAILED,

    VALIDATION_ERROR,
    
    NOT_FOUND,
    
    INCORRECT_PROPERTY_VALUE,
    PROPERTY_VALUE_ALREADY_IN_USE,
    PROPERTY_VALUE_UNCHANGED,
    
    INTERNAL_ERROR
}