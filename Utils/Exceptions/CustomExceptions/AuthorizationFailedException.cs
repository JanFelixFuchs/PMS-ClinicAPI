using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Types;

namespace Utils.Exceptions.CustomExceptions;

public class AuthorizationFailedException : CustomExceptionBase
{
    private AuthorizationFailedException(string logMessage)
        : base(logMessage, HttpStatusCode.Unauthorized, ErrorType.AUTHORIZATION_FAILED) { }
    
    public static AuthorizationFailedException DueToInvalidMandatoryClaim(string claimName) =>
        new ($"Invalid mandatory claim {claimName}");
}