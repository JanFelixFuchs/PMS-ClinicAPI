using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Field;

namespace Domain.Common.Utils.Validation;

public static class PropertyValidationHelper
{
    public static void ConstructPropertyValidation(params PropertyValidationResult[] fieldValidations)
    {
        // Checking conditions
        var validationErrors = fieldValidations
            .Where(fieldValidation => !fieldValidation.IsValid)
            .Select(fieldValidation =>
            {
                // Constructing field error
                var fieldError = new FieldError(
                    fieldValidation.Field,
                    fieldValidation.ErrorCode);
                
                // Returning tuple
                return (FieldError: fieldError, LogMessage: fieldValidation.ValidationMessage);
            })
            .ToList();
        
        // Throwing exception
        if (validationErrors.Count > 0)
            throw new ValidationException(
                string.Join(", ", validationErrors.Select(validationError => validationError.LogMessage).ToList()),
                validationErrors.Select(validationError => validationError.FieldError).ToList());
    }
}