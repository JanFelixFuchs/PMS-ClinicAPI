using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Logging;
using PMS_ClinicAPI.Common.Utils.Helper;
using PMS_ClinicAPI.Common.Utils.Returns;
using Utils.Exceptions.Base;
using Utils.Exceptions.CustomExceptions;

namespace PMS_ClinicAPI.Common.Middleware;

public class GlobalExceptionHandlerMiddleware(
    IOptions<JsonOptions> jsonOptions,
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
            // Getting endpoint name
            var endpointName = EndpointHelper.GetEndpointName(context);
            
            // Creating http result
            var httpResult = new HttpResult<EmptyPayload>(exception);
            
            // Logging exception
            if (exception is CustomExceptionBase customExceptionBase)
            {
                // Choosing log level based on whether exception has inner exception
                var logLevel = customExceptionBase.InnerException != null ? LogLevel.Error : LogLevel.Warning;
                
                // Logging custom exception
                logger.Log(
                    logLevel,
                    customExceptionBase.InnerException,
                    LogMessages.EndpointCallFailed,
                    endpointName,
                    httpResult.HttpStatusCode,
                    httpResult.ErrorType,
                    customExceptionBase.Message);
            }
            else
            { 
                // Logging unexpected exception
                logger.LogCritical(exception, LogMessages.UnexpectedException);   
            }
            
            // Setting response status code
            context.Response.StatusCode = httpResult.HttpStatusCode;
            
            // Handling authorization failures without any response body
            if (exception is AuthorizationFailedException )
                return;
            
            // Setting response body
            await context.Response.WriteAsJsonAsync(httpResult, jsonOptions.Value.JsonSerializerOptions);
        }
    }
}