namespace Utils.Exceptions.Errors.Codes;

// Resharper disable InconsistentNaming
public enum ErrorCode
{
    INVALID_INPUT_MODEL_FORMAT,
    
    MISSING_VALUE,
    EMPTY_VALUE,
    
    DATETIME_NOT_IN_PAST,
    DATETIME_NOT_IN_FUTURE,
    DATETIME_OUT_OF_RANGE,
    
    INCORRECT_VALUE,
    VALUE_ALREADY_IN_USE,
    UNCHANGED_VALUE,
}