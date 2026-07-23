using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Types;

namespace Utils.Exceptions.CustomExceptions;

public class AuthorizationFailedException(string? logMessage = null)
    : CustomExceptionBase(
        logMessage ?? "Authorization failed", 
        HttpStatusCode.Unauthorized,
        ErrorType.AUTHORIZATION_FAILED);