using Application.Common.Contexts;
using Application.Common.Logging;
using Application.Repositories.IdentityRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Utils.Authentication;
using Utils.Exceptions.CustomExceptions;

namespace Application.Common.Behaviours.RequestContextBehaviour;

public class RequestContextBehaviour<TRequest, TResponse>(
    ILogger<RequestContextBehaviour<TRequest, TResponse>> logger,
    IRequestContext requestContext,
    IClinicRepository clinicRepository,
    IUserRepository userRepository)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Checking if request requires request context
        if (request is not IRequireRequestContext requestWithRequestContext)
            return await next(cancellationToken);
        
        // Validating clinic id
        var clinic = await clinicRepository.GetByIdAsync(requestContext.ClinicId, cancellationToken);
        if (clinic == null)
        {
            logger.LogWarning(LogMessages.InvalidMandatoryClaim, ClaimNames.ClinicId);
            throw new AuthorizationFailedException();
        }
        
        // Validating user id
        var user = await userRepository.GetByClinicIdAndUserIdAsync(
            requestContext.ClinicId, 
            requestContext.UserId, 
            cancellationToken);
        if (user == null || user.IsArchived || user.IsDeleted)
        {
            logger.LogWarning(LogMessages.InvalidMandatoryClaim, ClaimNames.UserId);
            throw new AuthorizationFailedException();
        }
        
        // Populating entities
        requestWithRequestContext.Clinic = clinic;
        requestWithRequestContext.User = user;
        
        // Returning next action
        return await next(cancellationToken);
    }
}