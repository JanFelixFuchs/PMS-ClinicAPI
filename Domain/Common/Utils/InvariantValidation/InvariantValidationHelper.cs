using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Common.Utils.InvariantValidation;

public static class InvariantValidationHelper
{
    public static void ConstructionInvariantValidation(params InvariantValidationResult[] invariantValidations)
    {
        // Checking conditions
        var validationError = invariantValidations.FirstOrDefault(invariantValidation => !invariantValidation.IsValid);

        // Throwing exception
        if (validationError != null)
            throw new InvalidOperationException(validationError.ValidationMessage);
    }
}