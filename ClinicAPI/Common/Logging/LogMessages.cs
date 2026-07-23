namespace PMS_ClinicAPI.Common.Logging;

public static class LogMessages
{
    // Endpoints
    public const string EndpointCallStarted = "{Endpoint} endpoint call started";
    public const string EndpointCallSucceeded = "{Endpoint} endpoint call completed successfully with status code {StatusCode}";
    public const string EndpointCallFailed = "{Endpoint} endpoint call failed with status code {StatusCode}, error type {ErrorType} and message '{Message}'";
    
    // Jwt bearer validation
    public const string MissingMandatoryClaim = "Authorization failed: missing mandatory claims {ClinicIdClaim} and/or {UserIdClaim}";
    
    // Policy handler
    public const string AuthorizationSucceeded = "Authorization succeeded: resource {Resource} requires access level {MinimumAccessLevel}, user has access level {AccessLevel}";
    public const string MissingPolicyClaim = "Authorization failed: resource {Resource} is missing the required claim";
    public const string InvalidPolicyClaimValue = "Authorization failed: resource {Resource} has invalid claim value {Value}";
    public const string InsufficientAccessLevel = "Authorization failed: resource {Resource} requires access level {MinimumAccessLevel}, user has access level {AccessLevel}";
    
    // Database
    public const string DatabaseMigrationsSucceeded = "Database migration succeeded";
    public const string DatabaseConnectionFailed = "Database connection failed: application startup aborted";
    
    // Unexpected exception
    public const string UnexpectedException = "Unexpected exception occurred";
}