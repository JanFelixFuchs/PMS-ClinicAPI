using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace Domain.Common.Exceptions;

public class InvalidOperationException : CustomExceptionBase
{ 
    public InvalidOperationException(string logMessage)
        : base(
            logMessage, 
            HttpStatusCode.Conflict,
            ErrorType.INVALID_OPERATION) { }
    
    public InvalidOperationException(string logMessage, ICollection<FieldError> fieldErrors)
        : base(
            logMessage, 
            HttpStatusCode.Conflict,
            ErrorType.INVALID_OPERATION,
            fieldErrors) { }
}