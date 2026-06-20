using System.Net;
using Application.Common.Exceptions;
using PMS_ClinicAPI.Common.Logging;
using PMS_ClinicAPI.Common.Utils.Returns;
using Utils.Exceptions.Base;

namespace PMS_ClinicAPI.Common.Middleware;

public class GlobalExceptionHandlerMiddleware(
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Executing next action
            await next(context);
        }
        catch (Exception exception)
        {
            // Handling authorization failures without any response body
            if (exception is AuthorizationFailedException)
            {
                logger.LogInformation(LogMessages.EndpointCallFailed, HttpStatusCode.Unauthorized);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            
            // Creating http result
            var httpResult = new HttpResult<EmptyPayload>(exception);
            
            // Logging exception
            if (exception is CustomExceptionBase)
            { 
                logger.LogInformation(LogMessages.EndpointCallFailed, httpResult.HttpStatusCode);
            }
            else
            { 
                logger.LogCritical(exception, LogMessages.UnexpectedException);   
            }
            
            // Setting response status code
            context.Response.StatusCode = httpResult.HttpStatusCode;
            
            // Setting response body
            await context.Response.WriteAsJsonAsync(httpResult);
        }
    }
}