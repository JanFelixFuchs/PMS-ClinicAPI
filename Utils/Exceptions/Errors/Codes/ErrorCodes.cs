namespace Utils.Exceptions.Errors.Codes;

// Resharper disable InconsistentNaming
public enum ErrorCode
{
    // Errors related to input model validation
    INVALID_INPUT_MODEL_FORMAT,
    
    // Errors related to required and optional properties
    MISSING_VALUE,
    EMPTY_VALUE,
    
    // Errors related to lengths
    MAX_LENGTH_EXCEEDED,
    MIN_LENGTH_NOT_REACHED,
    
    // Errors related to regex patterns
    PATTERN_MISMATCH,
    
    // Errors related to enum values
    INVALID_ENUM_VALUE,
    
    // Errors related to collections
    CONTAINS_DUPLICATE_ELEMENTS,
    
    // Errors related to dates and times
    DATETIME_NOT_IN_PAST,
    DATETIME_NOT_IN_FUTURE,
    DATETIME_OUT_OF_RANGE,
    
    // Errors related to file uploads
    UNSUPPORTED_FILE_TYPE,
    
    // Errors related to property changes
    INCORRECT_VALUE,
    VALUE_ALREADY_IN_USE,
    UNCHANGED_VALUE,
    
    // Errors related to entity state invariants
    DELETED_ENTITY,
    ARCHIVED_ENTITY,
    CLINIC_MISMATCH,
    UNEXPECTED_STATUS,
    
    // Errors related to unknown errors
    UNKNOWN_ERROR
}