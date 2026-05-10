using Utils.Exceptions.CustomExceptions;

namespace Domain.Commons.Utils.Validation;

public static class ValidationHelper
{
    public static void ConstructPropertyValidation(params (bool condition, string errorMessage)[] validations)
    {
        // Checking conditions and collecting error messages
        var validationErrors = validations.
            Where(validation => !validation.condition).
            Select(validation => validation.errorMessage)
            .ToList();

        // Throwing exception
        if (validationErrors.Count > 0)
            throw new ValidationException(validationErrors);
    }
}