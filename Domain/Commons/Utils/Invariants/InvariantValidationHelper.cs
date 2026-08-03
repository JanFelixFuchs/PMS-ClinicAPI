using InvalidOperationException = Domain.Commons.Exceptions.InvalidOperationException;

namespace Domain.Commons.Utils.Invariants;

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