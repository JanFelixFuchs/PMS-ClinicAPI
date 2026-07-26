namespace Utils.Exceptions.Errors.Codes;

// Resharper disable InconsistentNaming
public enum ErrorCode
{
    INVALID_INPUT_MODEL_FORMAT,
    
    MISSING_VALUE,
    EMPTY_VALUE,
    
    MAX_LENGTH_EXCEEDED,
    MIN_LENGTH_NOT_REACHED,
    
    PATTERN_MISMATCH,
    
    INVALID_ENUM_VALUE,
    
    CONTAINS_DUPLICATE_ELEMENTS,
    
    DATETIME_NOT_IN_PAST,
    DATETIME_NOT_IN_FUTURE,
    DATETIME_OUT_OF_RANGE,
    
    INCORRECT_VALUE,
    VALUE_ALREADY_IN_USE,
    UNCHANGED_VALUE,
}