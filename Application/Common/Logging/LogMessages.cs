namespace Application.Common.Logging;

public static class LogMessages
{
    public const string RequestStarted = "Started request {Request}";
    public const string RequestCompleted = "Completed request {Request}";
    public const string RequestFailed = "Failed request {Request}";
    
    public const string EntityNotFound = "Failed to find {Entity} with id {Id}";
    public const string EntitiesNotFound = "Failed to find {Entities} with ids {Ids}";
    public const string EntityPropertyAlreadyInUse = "Provided {Property} of {Entity} is already in use";
    public const string EntityPropertyInvalid = "Provided {Property} of {Entity} is invalid";
    public const string EntityPropertyUnchanged = "Provided {Property} of {Entity} is unchanged";
    
    public const string ValidationFailed = "Failed to validate request {Request} with messages {Messages}";
    
    public const string InvalidMandatoryClaim = "Failed to authorize due to invalid mandatory claim {Claim}";
    public const string InvalidRefreshToken = "Failed to authorize due to invalid or expired refresh token for user with id {UserId}";
    
    public const string InvalidPassword = "Failed to authorize due to invalid password for user with id {UserId}";
}
