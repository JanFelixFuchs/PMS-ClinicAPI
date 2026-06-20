namespace PMS_ClinicAPI.Common.Logging;

public static class LogMessages
{
    public const string EndpointCallStarted = "Started endpoint call {Endpoint}";
    public const string EndpointCallSucceeded = "Sucessfully completed endpoint call {Endpoint} with status code {StatusCode}";
    public const string EndpointCallFailed = "Failed to process endpoint call with status code {StatusCode}";
    
    public const string AuthorizationSucceeded = "Completed authorization for resource {Resource} with access level {AccessLevel} >= required {MinimumAccessLevel}";
    
    public const string DatabaseMigrationsSucceeded = "Successfully applied database migrations";
    
    public const string InputModelValidationFailed = "Failed to validate input model with messages {Messages}";
    
    public const string MissingMandatoryClaim = "Failed to authorize due to missing mandatory claims {ClinicIdClaim} or {UserIdClaim}";
    public const string MissingPolicyClaim = "Failed to authorize due to missing claim for resource {Resource}";
    public const string InvalidPolicyClaim = "Failed to authorize due to invalid claim value {Value} for resource {Resource}";
    public const string InsufficientAccessLevel = "Failed to authorize due to insufficient access level {AccessLevel} < required {MinimumAccessLevel} for resource {Resource}";
    
    public const string UnexpectedException = "Unexpected exception occurred";
    
    public const string DatabaseConnectionFailed = "Establishing a database connection failed. Application will not start";
}