using FluentValidation;
using MediatR;
using Utils.Exceptions.Errors.Codes;
using Utils.Exceptions.Errors.Field;
using ValidationException = Utils.Exceptions.CustomExceptions.ValidationException;

namespace Application.Common.Behaviours.ValidationBehaviour;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
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
        var validationErrors = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null)
            .Select(validationFailure =>
            {
                // Constructing field error
                var fieldError = new FieldError(
                    validationFailure.PropertyName,
                    validationFailure.CustomState is ErrorCode errorCode ? errorCode : ErrorCode.UNKNOWN_ERROR);

                // Returning tuple
                return (FieldError: fieldError, LogMessage: validationFailure.ErrorMessage);
            })
            .ToList();
        
        // Checking validation
        if (validationErrors.Count > 0)
            throw new ValidationException(
                string.Join(", ", validationErrors.Select(validationError => validationError.LogMessage).ToList()),
                validationErrors.Select(validationError => validationError.FieldError).ToList());
        
        // Returning next action
        return await next(cancellationToken);
    }
}