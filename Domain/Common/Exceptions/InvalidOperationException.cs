using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Types;

namespace Domain.Common.Exceptions;

public class InvalidOperationException(string logMessage)
    : CustomExceptionBase(
        logMessage, 
        HttpStatusCode.Conflict,
        ErrorType.INVALID_OPERATION);