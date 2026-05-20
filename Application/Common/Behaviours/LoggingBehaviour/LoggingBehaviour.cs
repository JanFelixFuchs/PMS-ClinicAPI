using Application.Common.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours.LoggingBehaviour;

public class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        // Logging request start
        logger.LogInformation(LogMessages.RequestStarted, typeof(TRequest).Name);
        
        // Executing request
        var response = await next(cancellationToken);
        
        // Logging request completion
        logger.LogInformation(LogMessages.RequestCompleted, typeof(TRequest).Name);
        
        // Returning response
        return response;
    }
}