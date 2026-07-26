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
    
    public static AuthorizationFailedException DueToInvalidCredentials(Guid? userId = null) => 
        new ($"Invalid credentials for user with id {userId?.ToString() ?? "unknown"}");
    
    public static AuthorizationFailedException DueToInvalidRefreshToken(Guid? userId = null) =>
        new ($"Invalid or expired refresh token for user with id {userId?.ToString() ?? "unknown"}");
    
    public static AuthorizationFailedException DueToMissingRefreshTokenCookie() =>
        new ("Missing refresh token cookie");
}