using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using LogMessages = Application.Common.Logging.LogMessages;
using ValidationException = Utils.Exceptions.CustomExceptions.ValidationException;

namespace Application.Common.Behaviours.Validation;

public class ValidationBehaviour<TRequest, TResponse>(
    ILogger<ValidationBehaviour<TRequest, TResponse>> logger,
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        // Checking validators for existence
        if (!validators.Any())
            return await next(cancellationToken);
        
        // Executing validation and collecting failure messages
        var validationFailureMessages = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null)
            .Select(validationFailure => validationFailure.ErrorMessage)
            .ToList();
        
        // Checking validation
        if (validationFailureMessages.Count > 0)
        {
            logger.LogWarning(LogMessages.ValidationFailed, typeof(TRequest).Name, string.Join(", ", validationFailureMessages));
            throw new ValidationException(validationFailureMessages);
        }
        
        // Returning next action
        return await next(cancellationToken);
    }
}