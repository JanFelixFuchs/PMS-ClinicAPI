using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Field;

namespace Domain.Common.Utils.PropertyValidation;

public static class PropertyValidationHelper
{
    public static void ConstructPropertyValidation(params Func<PropertyValidationResult>[] propertyValidations)
    {
        // Checking conditions
        foreach (var fieldValidation in propertyValidations)
        {
            // Calling validation condition
            var validationResult = fieldValidation();
            
            // Continue if no validation error occurred
            if (validationResult.IsValid) continue;
            
            // Throwing exception
            throw new ValidationException(
                validationResult.ValidationMessage, 
                [new FieldError(validationResult.Field, validationResult.ErrorCode)]);
        }
    }
}