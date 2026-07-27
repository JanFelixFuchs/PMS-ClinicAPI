using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace Utils.Exceptions.CustomExceptions;

public class ValidationException(
    string logMessage,
    ICollection<FieldError> fieldErrors)
    : CustomExceptionBase(
        logMessage,
        HttpStatusCode.UnprocessableEntity, 
        ErrorType.VALIDATION_ERROR, 
        fieldErrors);