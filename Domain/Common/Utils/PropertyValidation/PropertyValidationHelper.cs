using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Field;

namespace Domain.Common.Utils.PropertyValidation;

public static class PropertyValidationHelper
{
    public static void ConstructPropertyValidation(params Func<PropertyValidationResult>[] fieldValidations)
    {
        // Checking conditions
        foreach (var fieldValidation in fieldValidations)
        {
            // Calling validation condition
            var validationResult = fieldValidation();
            
            // Continue if no validation error occured
            if (validationResult.IsValid) continue;
            
            // Throwing exception
            throw new ValidationException(
                validationResult.ValidationMessage, 
                [new FieldError(validationResult.Field, validationResult.ErrorCode)]);
        }
    }
}