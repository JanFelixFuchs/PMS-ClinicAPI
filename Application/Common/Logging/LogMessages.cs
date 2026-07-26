namespace Application.Common.Logging;

public static class LogMessages
{
    public const string RequestStarted = "{Request} request started";
    public const string RequestCompleted = "{Request} request completed successfully";
    public const string RequestFailed = "{Request} request failed";
    
    public const string ValidationFailed = "Failed to validate request {Request} with messages {Messages}";
}
