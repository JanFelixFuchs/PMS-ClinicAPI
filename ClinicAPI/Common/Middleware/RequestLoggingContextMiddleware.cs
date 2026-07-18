using Serilog.Context;

namespace PMS_ClinicAPI.Common.Middleware;

public class RequestLoggingContextMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
        {
            await next(context);
        }
    }
}