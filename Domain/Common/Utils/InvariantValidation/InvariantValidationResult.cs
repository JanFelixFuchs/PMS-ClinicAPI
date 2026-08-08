namespace Domain.Common.Utils.InvariantValidation;

public record InvariantValidationResult(
    bool IsValid,
    string ValidationMessage);