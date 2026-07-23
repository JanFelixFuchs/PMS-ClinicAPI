using Application.Common.Contexts;
using Application.Repositories.IdentityRepositories;
using MediatR;
using Utils.Authentication;
using Utils.Exceptions.CustomExceptions;

namespace Application.Common.Behaviours.RequestContextBehaviour;

public class RequestContextBehaviour<TRequest, TResponse>(
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
            throw AuthorizationFailedException.DueToInvalidMandatoryClaim(ClaimNames.ClinicId);
        
        // Validating user id
        var user = await userRepository.GetByClinicIdAndUserIdAsync(
            requestContext.ClinicId, 
            requestContext.UserId, 
            cancellationToken);
        if (user == null || user.IsArchived || user.IsDeleted)
            throw AuthorizationFailedException.DueToInvalidMandatoryClaim(ClaimNames.UserId);
        
        // Populating entities
        requestWithRequestContext.Clinic = clinic;
        requestWithRequestContext.User = user;
        
        // Returning next action
        return await next(cancellationToken);
    }
}