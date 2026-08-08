namespace Domain.Common.Utils.Invariants;

public record InvariantValidationResult(
    bool IsValid,
    string ValidationMessage);