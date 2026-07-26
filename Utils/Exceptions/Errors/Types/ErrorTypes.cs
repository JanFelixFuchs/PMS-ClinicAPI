namespace Utils.Exceptions.Errors.Types;

// Resharper disable InconsistentNaming
public enum ErrorType
{
    SUCCESS,
    
    AUTHORIZATION_FAILED,

    VALIDATION_ERROR,
    
    NOT_FOUND,
    
    INCORRECT_PROPERTY_VALUE,
    PROPERTY_VALUE_ALREADY_IN_USE,
    
    INTERNAL_ERROR
}