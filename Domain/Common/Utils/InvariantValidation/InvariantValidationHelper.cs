using Utils.Exceptions.Errors.Field;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Common.Utils.InvariantValidation;

public static class InvariantValidationHelper
{
    public static void ConstructInvariantValidation(params Func<InvariantValidationResult>[] invariantValidations)
    {
        // Checking conditions
        foreach (var invariantValidation in invariantValidations)
        {
            // Calling validation condition
            var validationResult = invariantValidation();
            
            // Continue if no validation error occurred
            if (validationResult.IsValid) continue;
            
            // Throwing exception
            throw new InvalidOperationException(
                validationResult.ValidationMessage,
                [new FieldError(validationResult.Field, validationResult.ErrorCode)]);
        }
    }
}